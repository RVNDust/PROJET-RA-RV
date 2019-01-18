using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ControllerInput : NetworkBehaviour
{
    public SteamVR_Input_Sources inputSource;
    public SphereCollider HoverSphere;
    public float AuthorityRemovalTimer = 0.1f;
    private SteamVR_Behaviour_Pose behaviourPose;
    private GameObject GrabbedObject;


    void Awake()
    {
        behaviourPose = GetComponent<SteamVR_Behaviour_Pose>();
        inputSource = behaviourPose.inputSource;
        if(HoverSphere == null)
        {
            HoverSphere = gameObject.AddComponent<SphereCollider>();
            HoverSphere.radius = 0.1f;
            HoverSphere.isTrigger = true;
        }
    }

    void Update()
    {
        if (SteamVR_Input.Swift.inActions.Touchpad.GetStateDown(inputSource))
        {
            GrabObject();
        }
        if (SteamVR_Input.Swift.inActions.Touchpad.GetStateUp(inputSource))
        {
            ReleaseObject();
        }
        if (SteamVR_Input.Swift.inActions.Grip.GetStateDown(inputSource))
        {
            TeleportPressed();
        }
        if (SteamVR_Input.Swift.inActions.Grip.GetStateUp(inputSource))
        {
            TeleportReleased();
        }
    }

    /// <summary>
    /// Adds a pointer at the tip of the controller for teleporation
    /// </summary>
    private void TeleportPressed()
    {
        ControllerPointer ContPt = gameObject.AddComponent<ControllerPointer>();
        ContPt.RaycastMask = LayerMask.GetMask("TeleportableFloor");
    }

    /// <summary>
    /// Removes the pointer and teleports the player to the destination (if it is correct)
    /// </summary>
    private void TeleportReleased()
    {
        ControllerPointer ContPt = gameObject.GetComponent<ControllerPointer>();
        if(ContPt.CanTeleport) 
        {
            Vector3 cameraPos = GetLocalPlayer().GetComponent<VR_CameraRigMultiuser>().SteamVRCamera.transform.localPosition;
            Vector3 pointerPos = GetComponent<ControllerPointer>().TargetPosition;
            Vector3 offset = new Vector3(pointerPos.x - cameraPos.x, pointerPos.y, pointerPos.z - cameraPos.z);
            GetLocalPlayer().transform.position = offset;
        }
        ContPt.DesactivatePointer();
        Destroy(ContPt);
    }

    /// <summary>
    /// Grab the closest object (with a grabbable script) to this hand
    /// </summary>
    private void GrabObject()
    {
        LayerMask DefaultLayer = 1 << LayerMask.NameToLayer("Default");
        Collider[] CollisionResult = Physics.OverlapSphere(HoverSphere.transform.position, HoverSphere.radius, DefaultLayer);
        if(CollisionResult.Length > 0)
        {
            GrabbedObject = CollisionResult[0].gameObject;
            if(GrabbedObject.GetComponent<GrabbableObject>())
            {
                GrabbedObject.GetComponent<GrabbableObject>().SendMessage("OnGrab");
                GrabbedObject.GetComponent<VelocityEstimator>().BeginEstimatingVelocity();
                Debug.Log("Grabbed object " + GrabbedObject.name);
                GetLocalPlayer().GetComponent<VR_CameraRigMultiuser>().CmdAddAuth(GrabbedObject);
                if(!gameObject.GetComponent<FixedJoint>()){
                    FixedJoint fx = gameObject.AddComponent<FixedJoint>();
                    fx.breakForce = Mathf.Infinity;
                    fx.breakTorque = Mathf.Infinity;
                    fx.connectedBody = GrabbedObject.GetComponent<Rigidbody>();
                }
            }
        }
	}

    /// <summary>
    /// Releases any object held inside this hand
    /// </summary>
    private void ReleaseObject(){
        if(GrabbedObject != null) 
        {
            if (gameObject.GetComponent<FixedJoint>()){
                Destroy(gameObject.GetComponent<FixedJoint>());
            }
            GrabbedObject.GetComponent<GrabbableObject>().SendMessage("OnRelease");
            GrabbedObject.GetComponent<Rigidbody>().velocity = GrabbedObject.GetComponent<VelocityEstimator>().GetVelocityEstimate();
            GrabbedObject.GetComponent<Rigidbody>().angularVelocity = GrabbedObject.GetComponent<VelocityEstimator>().GetAngularVelocityEstimate();
            GrabbedObject.GetComponent<VelocityEstimator>().FinishEstimatingVelocity();
            StartCoroutine(RemoveAuthCoroutine());
        }        
    }

    /// <summary>
    /// Removes the authority after an elapsed time to ensure a smooth transition
    /// </summary>
    private IEnumerator RemoveAuthCoroutine()
    {
        yield return new WaitForSeconds(AuthorityRemovalTimer);
        GetLocalPlayer().GetComponent<VR_CameraRigMultiuser>().CmdRemoveAuth(GrabbedObject);
        GrabbedObject = null;
    }

    /// <summary>
    /// Get the local player via a tag
    /// </summary>
    private GameObject GetLocalPlayer()
    {
        return GameObject.FindGameObjectWithTag("VRLocalPlayer");
    }




}

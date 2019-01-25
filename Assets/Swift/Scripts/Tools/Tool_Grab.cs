using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.Networking;

public class Tool_Grab : ToolObject {

    [Tooltip("SphereCollider used to detect objects. (Will be created if empty)")]
    public SphereCollider HoverSphere;
    [Tooltip("The amount of time (in seconds) before an object is removed from authority")]
    public float AuthorityRemovalTimer = 0.1f;
    
    private GameObject GrabbedObject;

    /// <summary>
    /// Initalize the necessary SphereCollider and/or RigidBody used by this component.
    /// </summary>
    void Awake()
	{
		if(HoverSphere == null)
        {
            HoverSphere = gameObject.AddComponent<SphereCollider>();
            HoverSphere.radius = 0.1f;
            HoverSphere.isTrigger = true;
        }
        if(GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
        }
	}

	protected override void OnInteractDown()
	{
		GrabObject();
	}

	protected override void OnInteractUp()
	{
		ReleaseObject(false);
	}

	/// <summary>
    /// Grab the closest object (with a grabbable script) to this hand
    /// </summary>
    private void GrabObject()
    {
        LayerMask DefaultLayer = 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Machines"); // Only allow grabbing of objects from layer 'Default' or 'Machines'
        Collider[] CollisionResult = Physics.OverlapSphere(HoverSphere.transform.position, HoverSphere.radius, DefaultLayer);
        if(CollisionResult.Length > 0)
        {         
            GrabbedObject = CollisionResult[0].gameObject;
            if(GrabbedObject.GetComponent<GrabbableObject>())
            {
                GrabbedObject.GetComponent<GrabbableObject>().SendMessage("OnGrab");
                GrabbedObject.GetComponent<VelocityEstimator>().BeginEstimatingVelocity(); // Start estimating the velocity to throw the object later
                GetLocalPlayer().GetComponent<VR_CameraRigMultiuser>().CmdSetAuth(GrabbedObject.GetComponent<NetworkIdentity>().netId, GetLocalPlayer().GetComponent<NetworkIdentity>());

                GrabbedObject.transform.position = gameObject.transform.position;
                // We verify if there is no fixed joint, if there isn't then we create one
                if(!GrabbedObject.GetComponent<FixedJoint>()){
                    FixedJoint fx = GrabbedObject.AddComponent<FixedJoint>();
                    fx.breakForce = Mathf.Infinity;
                    fx.breakTorque = Mathf.Infinity;
                }
                // If there would've already been a fixed joint (another player is holding the object) then we just change the connectedBody reference
                GrabbedObject.GetComponent<FixedJoint>().connectedBody = gameObject.GetComponent<Rigidbody>();
            }
        }
	}

    /// <summary>
    /// Releases any object held inside this hand
    /// </summary>
    /// <param name="immediate">Do we need to let go of the object immediately without making use of the delayed authority removal ?</param>
    private void ReleaseObject(bool immediate){
        if(GrabbedObject != null) 
        {
            if(GrabbedObject.GetComponent<FixedJoint>())
            {
                // The following condition checks if the GrabbedObject is connected to us, if it is not then someone else picked it up from our hand
                // We therefore do not need to remove the authority and can just change it to null
                if (GrabbedObject.GetComponent<FixedJoint>().connectedBody == gameObject.GetComponent<Rigidbody>())
                {
                    Destroy(GrabbedObject.GetComponent<FixedJoint>());
                    GrabbedObject.GetComponent<GrabbableObject>().SendMessage("OnRelease");
                    GrabbedObject.GetComponent<Rigidbody>().velocity = GrabbedObject.GetComponent<VelocityEstimator>().GetVelocityEstimate();
                    GrabbedObject.GetComponent<Rigidbody>().angularVelocity = GrabbedObject.GetComponent<VelocityEstimator>().GetAngularVelocityEstimate();
                    GrabbedObject.GetComponent<VelocityEstimator>().FinishEstimatingVelocity();
                    if(immediate)
                    {
                        GetLocalPlayer().GetComponent<VR_CameraRigMultiuser>().CmdRemoveAuth(GrabbedObject);
                        GrabbedObject = null;
                    } else {
                        StartCoroutine(RemoveAuthCoroutine());
                    }
                } else {
                    GrabbedObject = null;
                }
            } 
        }       
    }

    void OnDestroy()
    {
        if(GrabbedObject != null)
        {
            ReleaseObject(true);
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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Tool_Grab : ToolObject {

	public SphereCollider HoverSphere;
    public float AuthorityRemovalTimer = 0.1f;
    private GameObject GrabbedObject;

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

	public override void OnInteractDown()
	{
		GrabObject();
	}

	public override void OnInteractUp()
	{
		ReleaseObject();
	}
	
	public override void OnMenu()
	{

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

}

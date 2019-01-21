﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.Networking;

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
        LayerMask DefaultLayer = 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Machines");
        Collider[] CollisionResult = Physics.OverlapSphere(HoverSphere.transform.position, HoverSphere.radius, DefaultLayer);
        if(CollisionResult.Length > 0)
        {         
            GrabbedObject = CollisionResult[0].gameObject;
            if(GrabbedObject.GetComponent<GrabbableObject>())
            {
                GrabbedObject.GetComponent<GrabbableObject>().SendMessage("OnGrab");
                GrabbedObject.GetComponent<VelocityEstimator>().BeginEstimatingVelocity();
                Debug.Log("Grabbed object " + GrabbedObject.name);
                GetLocalPlayer().GetComponent<VR_CameraRigMultiuser>().CmdSetAuth(GrabbedObject.GetComponent<NetworkIdentity>().netId, GetLocalPlayer().GetComponent<NetworkIdentity>());
                
                GrabbedObject.transform.position = gameObject.transform.position;
                // GrabbedObject.transform.localRotation = gameObject.transform.rotation; Too much discomfort for the user when holding machines
                if(!GrabbedObject.GetComponent<FixedJoint>()){
                    FixedJoint fx = GrabbedObject.AddComponent<FixedJoint>();
                    fx.breakForce = Mathf.Infinity;
                    fx.breakTorque = Mathf.Infinity;
                }
                GrabbedObject.GetComponent<FixedJoint>().connectedBody = gameObject.GetComponent<Rigidbody>();
            }
        }
	}

    /// <summary>
    /// Releases any object held inside this hand
    /// </summary>
    private void ReleaseObject(bool immediate){
        if(GrabbedObject != null) 
        {
            if(GrabbedObject.GetComponent<FixedJoint>())
            {
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

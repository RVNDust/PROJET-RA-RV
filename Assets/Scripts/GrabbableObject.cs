using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(VelocityEstimator))]
public class GrabbableObject : MonoBehaviour {

	public bool ShrinkOnGrab = false;
	public float ShrinkFactor = 2.0f;
	public float ShrinkTime = 0.5f;
	private bool grabbed;
	private Vector3 originalTransform;
	private Vector3 shrinkTransform;
	private Vector3 shrinkVelocity;

	void Awake()
	{
		originalTransform = gameObject.transform.localScale;
		shrinkTransform = originalTransform / ShrinkFactor;
	}

	void Update()
	{
		if(ShrinkOnGrab && grabbed)
		{
			Vector3 SmoothedScale = Vector3.SmoothDamp(gameObject.transform.localScale, shrinkTransform, ref shrinkVelocity, ShrinkTime);
			gameObject.transform.localScale = SmoothedScale;
		} else if (ShrinkOnGrab && !grabbed)
		{
			Vector3 SmoothedScale = Vector3.SmoothDamp(gameObject.transform.localScale, originalTransform, ref shrinkVelocity, ShrinkTime);
			gameObject.transform.localScale = SmoothedScale;
		}
	}
	void OnGrab()
	{
		grabbed = true;
	}

	void OnRelease()
	{
		grabbed = false;
	}
}

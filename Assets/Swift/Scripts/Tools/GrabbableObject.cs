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

	private Vector3 repositionTransform;
	private Vector3 repositionVelocity;

	public GameObject boundingBoxPrefab;
	private GameObject boundingBox;

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
			
			repositionTransform = GetComponent<FixedJoint>().connectedBody.transform.position;
			//Vector3 SmoothedTranslation = Vector3.SmoothDamp(gameObject.transform.position, repositionTransform, ref repositionVelocity, ShrinkTime);
			//gameObject.transform.position = SmoothedTranslation;
			
			if(boundingBox == null)
			{
				boundingBox = Instantiate(boundingBoxPrefab);
				Vector3 meshSize = CalculateLocalBounds().extents;
				Vector3 boxSize = new Vector3(meshSize.x * 2, 1, meshSize.z *2 );
				boundingBox.transform.localScale = boxSize;
			}

			LayerMask layerMask = LayerMask.GetMask("Ground"); // Put as private variable
			RaycastHit hit;
			Debug.DrawRay(transform.position, Vector3.down, Color.red, 50.0f);
			if(Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
			{
				boundingBox.transform.position = hit.point + new Vector3(0, 0.1f, 0);
				boundingBox.transform.rotation = Quaternion.Euler(boundingBox.transform.rotation.eulerAngles.x, 
																  gameObject.transform.rotation.eulerAngles.y,
																  boundingBox.transform.rotation.eulerAngles.z);
			}

		} else if (ShrinkOnGrab && !grabbed)
		{
			Vector3 SmoothedScale = Vector3.SmoothDamp(gameObject.transform.localScale, originalTransform, ref shrinkVelocity, ShrinkTime);
			gameObject.transform.localScale = SmoothedScale;

			Destroy(boundingBox);
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

	private Bounds CalculateLocalBounds()
    {
         Quaternion currentRotation = this.transform.rotation;
         this.transform.rotation = Quaternion.Euler(0f,0f,0f);
         Bounds bounds = new Bounds(this.transform.position, Vector3.zero);
         foreach(Renderer renderer in GetComponentsInChildren<Renderer>())
         {
             bounds.Encapsulate(renderer.bounds);
         }
         Debug.Log("The local bounds of this model is " + bounds);
		 return bounds;
    }
}

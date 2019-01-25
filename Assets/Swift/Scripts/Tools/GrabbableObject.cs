using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(VelocityEstimator))]
public class GrabbableObject : MonoBehaviour {

    [Tooltip("Does the object shrink when the user grabs it?")]
    public bool ShrinkOnGrab = false;
    [Tooltip("By how much does the object shrink when the user grabs it.")]
    public float ShrinkFactor = 2.0f;
    [Tooltip("How long does it take for the object to shrink fully when it is grabbed.")]
    public float ShrinkTime = 0.5f;

	private bool grabbed;
	private Vector3 originalTransform; // The size of the object at its original size
	private Vector3 shrinkTransform; // The size of the shrinked object, calculated on Awake()
	private Vector3 shrinkVelocity;

    [Tooltip("Prefab of the bounding box.")]
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
            // Shrink the object by shrinking it linearly
			Vector3 SmoothedScale = Vector3.SmoothDamp(gameObject.transform.localScale, shrinkTransform, ref shrinkVelocity, ShrinkTime);
			gameObject.transform.localScale = SmoothedScale;
			
            // Create a bounding box of the original size (at the time of grab)
			if(boundingBox == null)
			{
				boundingBox = Instantiate(boundingBoxPrefab);
				Vector3 meshSize = CalculateLocalBounds().extents;
				Vector3 boxSize = new Vector3(meshSize.x * 2, 1, meshSize.z *2 );
				boundingBox.transform.localScale = boxSize;
			}

            // Create a raycast below the grabbed object to show the original bounding box of it
			LayerMask layerMask = LayerMask.GetMask("BoundingBoxCollider");
			RaycastHit hit;
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

    /// <summary>
    /// Add up every child collider bounds to get the object's local bounds
    /// </summary>
    private Bounds CalculateLocalBounds()
    {
         Quaternion currentRotation = this.transform.rotation;
         this.transform.rotation = Quaternion.Euler(0f,0f,0f);
         Bounds bounds = new Bounds(this.transform.position, Vector3.zero);
         foreach(Renderer renderer in GetComponentsInChildren<Renderer>())
         {
             bounds.Encapsulate(renderer.bounds);
         }
		 return bounds;
    }
}

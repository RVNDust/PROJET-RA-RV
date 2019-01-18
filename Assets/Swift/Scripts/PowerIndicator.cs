using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerIndicator : MonoBehaviour {

	public GameObject ElectricityIconPrefab;
	public string ElectricityTag;
	public float hoverSpeed = 1.0f;
	public float hoverDistance = 2.0f;

	private GameObject ElectricityIconObject;
	
	void OnCollisionStay(Collision other)
	{
		if(other.gameObject.CompareTag(ElectricityTag)){
			if(ElectricityIconObject == null)
			{
				ElectricityIconObject = Instantiate(ElectricityIconPrefab, gameObject.transform);
				ElectricityIconObject.transform.localPosition = new Vector3( 0, hoverDistance, 0);
			}
			Vector3 tempVector = ElectricityIconObject.transform.position;
			tempVector.y += Mathf.Sin(Time.fixedTime * Mathf.PI * hoverSpeed);
			ElectricityIconObject.transform.position = tempVector;
		}
	}

	void OnCollisionExit(Collision other)
	{

	}
}

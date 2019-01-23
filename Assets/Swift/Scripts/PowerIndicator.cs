using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerIndicator : MonoBehaviour {

	public GameObject ElectricityIconPrefab;
    public GameObject NonElectricityPrefab;
    //public GameObject NonElectricityIconPrefab;
	public string ElectricityTag;
	public float hoverSpeed = 0.5f;
    public float hoverDistance = 2.0f;
    public float hoverAmplitude = 0.25f;

	private GameObject ElectricityIconObject;

    private void Start()
    {
        ElectricityIconObject = Instantiate(NonElectricityPrefab, gameObject.transform);
        ElectricityIconObject.transform.localPosition = new Vector3(0, hoverDistance, 0);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag(ElectricityTag))
        {
            Destroy(ElectricityIconObject);
            ElectricityIconObject = Instantiate(ElectricityIconPrefab, gameObject.transform);
            ElectricityIconObject.transform.localPosition = new Vector3(0, hoverDistance, 0);
            var scale = ElectricityIconObject.transform.localScale;
            /*ElectricityIconObject.transform.localScale = new Vector3(scale.x / other.gameObject.transform.localScale.x,
                scale.y / other.gameObject.transform.localScale.y,
                scale.z / other.gameObject.transform.localScale.z);*/
        }

    }

    private void Update()
    {
        Vector3 tempVector = ElectricityIconObject.transform.localPosition;
        tempVector.y += Mathf.Sin(Time.fixedTime * Mathf.PI * hoverSpeed) * hoverAmplitude;
        ElectricityIconObject.transform.localPosition = tempVector;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(ElectricityTag))
        {
            Destroy(ElectricityIconObject);
            ElectricityIconObject = Instantiate(NonElectricityPrefab, gameObject.transform);
            ElectricityIconObject.transform.localPosition = new Vector3(0, hoverDistance, 0);
        }
    }


}

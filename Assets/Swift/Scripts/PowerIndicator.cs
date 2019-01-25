using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerIndicator : MonoBehaviour {

    [Tooltip("Prefab of the electricity icon")]
    public GameObject ElectricityIconPrefab;
    [Tooltip("Prefab of the non-powered icon")]
    public GameObject NonElectricityPrefab;
    [Tooltip("Tag used to detect if a collider gives electricity")]
    public string ElectricityTag;
    [Tooltip("How high does the icon stand over the object")]
    public float hoverDistance = 2.0f;
    [Tooltip("How fast does the icon hover over the object")]
    public float hoverSpeed = 0.5f;
    [Tooltip("How far does the icon hover reach")]
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

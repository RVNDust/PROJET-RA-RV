using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowUI : MonoBehaviour {

    public Toggle EnableButton;
    public Text ProductName;
    public Text Distance;
    public Text AnnualVolume;
    public Text AnnualDistance;

    public GameObject AssociatedFlow;

    private void Start()
    {
        EnableButton.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(EnableButton);
        });

        ToggleValueChanged(EnableButton);
    }

    private void Update()
    {
        Distance.text = CalculateDistance().ToString();
    }

    private void ToggleValueChanged(Toggle change)
    {
         AssociatedFlow.SetActive(EnableButton.isOn);
    }

    private void OnDestroy()
    {
        Destroy(AssociatedFlow);
    }

    private float CalculateDistance()
    {
        float totalDistance = 0.0f;
        foreach(Flow f in AssociatedFlow.GetComponentsInChildren<Flow>())
        {
            totalDistance += f.GetDistance();
        }
        return totalDistance;
    }

}

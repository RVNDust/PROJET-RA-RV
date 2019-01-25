using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowUI : MonoBehaviour {

    [Tooltip("The button to enable the AssociatedFlow.")]
    public Toggle EnableButton;
    [Tooltip("Text that will contain the product name.")]
    public Text ProductName;
    [Tooltip("Text that will contain the total distance.")]
    public Text Distance;
    [Tooltip("Text that will contain the annual volume.")]
    public Text AnnualVolume;
    [Tooltip("Text that will contain the annual distance.")]
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

    /// <summary>
    /// Add up the distances of every Flow elements inside the AssociatedFlow component
    /// </summary>
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

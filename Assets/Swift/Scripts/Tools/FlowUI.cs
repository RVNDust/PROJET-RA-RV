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

    public List<Flow> Flow;

    private void Start()
    {
        EnableButton.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(EnableButton);
        });
    }

    void ToggleValueChanged(Toggle change)
    {
        foreach(Flow f in Flow)
        {
            f.gameObject.SetActive(EnableButton.isOn);
        }
    }

}

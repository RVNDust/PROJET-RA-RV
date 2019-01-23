using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using UnityEngine.UI;
using System.Globalization;

public class Tool_FlowManager : ToolObject_UI {

    public GameObject UI_Scrollrect;
    public GameObject FlowGroupUIPrefab;
    public GameObject FlowPrefab;
    public Text TotalAnnualDistanceText;

    private List<FlowUI> FlowGroupsUI = new List<FlowUI>();

    public string DataPath;
    private string FullDataPath;
    private FlowPathsList flowPathsList = new FlowPathsList();

    new void Start()
    {
        base.Start();

        FullDataPath = Application.dataPath + "/StreamingAssets/FlowLayout/FlowPaths.json";
        flowPathsList = LoadData(FullDataPath);

        for(int i = 0 ; i < flowPathsList.FlowPaths.Count ; i++)
        {
            FlowGroupsUI.Add(Instantiate(FlowGroupUIPrefab, UI_Scrollrect.transform).GetComponent<FlowUI>());
            var lastUI = FlowGroupsUI.Last();
            lastUI.ProductName.text = flowPathsList.FlowPaths[i].name;
            GameObject flowContainer = new GameObject("FlowPath_" + flowPathsList.FlowPaths[i].name);
            lastUI.AssociatedFlow = flowContainer;

            Color flowColor;
            ColorUtility.TryParseHtmlString(flowPathsList.FlowPaths[i].color, out flowColor);
            lastUI.EnableButton.GetComponent<Image>().color = flowColor;

            for (int x = 0 ; x < flowPathsList.FlowPaths[i].path.Length + 1 ; x++ )
            {
                Flow lastFlow = Instantiate(FlowPrefab, flowContainer.transform).GetComponent<Flow>();
                string startPt = (x == 0) ? "FlowStart" : flowPathsList.FlowPaths[i].path[x - 1];
                string endPt = (x == flowPathsList.FlowPaths[i].path.Length ) ? "FlowEnd" : flowPathsList.FlowPaths[i].path[x];
                lastFlow.gameObject.name = startPt + " to " + endPt;
                lastFlow.InitLine(startPt, endPt);
                lastFlow.GetComponent<LineRenderer>().startColor = flowColor;
                lastFlow.GetComponent<LineRenderer>().endColor = flowColor;
            }
        }
    }

    private void Update()
    {
        float totalAnnualDistance = 0.0f;
        foreach(FlowUI f in FlowGroupsUI)
        {
            totalAnnualDistance += float.Parse(f.AnnualDistance.text.Trim(), CultureInfo.InvariantCulture.NumberFormat);
        }
        TotalAnnualDistanceText.text = totalAnnualDistance.ToString();
    }

    public FlowPathsList LoadData(string myPath)
    {
        string dataAsJSON = File.ReadAllText(myPath);
        return flowPathsList = JsonUtility.FromJson<FlowPathsList>(dataAsJSON);
    }
}

[Serializable]
public class FlowPath
{
    public string name;
    public string[] path;
    public string color;
}


[Serializable]
public class FlowPathsList
{
    public List<FlowPath> FlowPaths = new List<FlowPath>();
}

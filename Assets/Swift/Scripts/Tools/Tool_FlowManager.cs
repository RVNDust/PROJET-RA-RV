using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;

public class Tool_FlowManager : ToolObject_UI {

    public GameObject UI_Scrollrect;
    public GameObject FlowGroupUIPrefab;
    public GameObject FlowPrefab;

    public string[] FlowGroup_A;
    private List<FlowUI> FlowGroupsUI = new List<FlowUI>();
    private List<Flow> temp_Flow = new List<Flow>();

    public string DataPath;
    private string FullDataPath;

    private void Start()
    {
       
        FullDataPath = Application.dataPath + "/StreamingAssets/FlowLayout/FlowPaths.json";
        LoadData(FullDataPath);

        FlowGroup_A = new string[3];
        FlowGroup_A[0] = "F1";
        FlowGroup_A[1] = "F2";
        FlowGroup_A[2] = "F3";

        var newFlow = Instantiate(new GameObject("Flow " + "A"));
        temp_Flow.Add(Instantiate(FlowPrefab, newFlow.transform).GetComponent<Flow>());
        temp_Flow.Last().gameObject.name = "Start to " + FlowGroup_A[0];

        for (int i=0; i < FlowGroup_A.Length-1; i++)
        {
            temp_Flow.Add(Instantiate(FlowPrefab, newFlow.transform).GetComponent<Flow>());
            if (i > FlowGroup_A.Length - 1)
            {
                temp_Flow.Last().gameObject.name = FlowGroup_A[FlowGroup_A.Length] + " to End";
            }
            else
            {
                temp_Flow.Last().gameObject.name = FlowGroup_A[i] + " to " + FlowGroup_A[i + 1];
            }
        }
        

        FlowGroupsUI.Add(Instantiate(FlowGroupUIPrefab, UI_Scrollrect.transform).GetComponent<FlowUI>());
        FlowGroupsUI.Last().Flow = temp_Flow;

    }

    public void LoadData(string myPath)
    {
        FlowPathsList flowPathsList = new FlowPathsList();

        string dataAsJSON = File.ReadAllText(myPath);
        flowPathsList = JsonUtility.FromJson<FlowPathsList>(dataAsJSON);

        foreach (var element in flowPathsList.FlowPath)
        {
            Debug.Log(element.name);
            Debug.Log(element.path.Length);

        }
    }
}

[Serializable]
public class FlowPath
{
    public string name;
    public string[] path;
}


[Serializable]
public class FlowPathsList
{
    public List<FlowPath> FlowPath = new List<FlowPath>();
}

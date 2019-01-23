using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tool_Radar : ToolObject {

    public GameObject ArrowPrefab;
    public Material OrangeMat;
    public float SwitchDistance = 10.0f;
    public Camera RadarCamera;

    private List<GameObject> currentPlayers;
    private List<GameObject> arrowsList = new List<GameObject>();


    private Vector3 arrowPos = new Vector3(0, 10, 0);

    private void Start()
    {
        RadarCamera.transform.parent = GetLocalPlayer().transform;
        RadarCamera.transform.localPosition = new Vector3(0, 30, 0);
        RadarCamera.transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    private void Update()
    {
        UnityEngine.Profiling.Profiler.BeginSample("Code radar");
        if(arrowsList != null)
        {
            ClearList(arrowsList);
        }


        Vector3 cameraRot_Euler = transform.rotation.eulerAngles;
        Quaternion cameraRot = Quaternion.Euler(90, cameraRot_Euler.y, 0);
        RadarCamera.transform.localRotation = cameraRot;

        currentPlayers = GameObject.FindGameObjectsWithTag("Player").ToList();
        foreach(GameObject go in currentPlayers)
        {
            GameObject playerArrow = Instantiate(ArrowPrefab, go.transform.position + arrowPos, go.transform.rotation, go.transform);
            arrowsList.Add(playerArrow);

            if(Vector3.Magnitude(go.transform.position - transform.position) > SwitchDistance)
            {
                playerArrow.transform.position = GetLocalPlayer().transform.position + SwitchDistance * (go.transform.position - GetLocalPlayer().transform.position).normalized;
                playerArrow.transform.position += arrowPos;
                playerArrow.GetComponent<Renderer>().material = OrangeMat;
            }
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }

    private void ClearList(List<GameObject> list)
    {
        foreach(GameObject go in list)
        {
            Destroy(go);
        }
        list.Clear();
    }

    private void OnDestroy()
    {
        Destroy(RadarCamera.gameObject);
    }

}

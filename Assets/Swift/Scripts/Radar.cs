using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public GameObject[] trackedObjects;
    IList<GameObject> radarObjects;
    public GameObject radarPrefab;
    List<GameObject> borderObjects;
    public float switchDistance;
    public Transform Blip;

    // Use this for initialization
    void Start()
    {
        createRadarObjects();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < radarObjects.Count; i++)
        {
            if (Vector3.Distance(radarObjects[i].transform.position, transform.position) > switchDistance)
            {
                Blip.LookAt(radarObjects[i].transform);
                borderObjects[i].transform.position = transform.position + switchDistance * Blip.forward;
                //borderObjects[i].layer = LayerMask.NameToLayer("RadarBlips");
                radarObjects[i].layer = LayerMask.NameToLayer("Invisible");
            }
            else
            {
                radarObjects[i].layer = LayerMask.NameToLayer("RadarBlips");
                //borderObjects[i].layer = LayerMask.NameToLayer("Invisible");
            }
        }
    }

    void createRadarObjects()
    {
        radarObjects = new List<GameObject>();
        borderObjects = new List<GameObject>();
        foreach (GameObject o in trackedObjects)
        {
            GameObject k = Instantiate(radarPrefab, o.transform.position, Quaternion.identity) as GameObject;
            k.transform.parent = o.transform;
            radarObjects.Add(k);
            GameObject j = Instantiate(radarPrefab, o.transform.position, Quaternion.identity) as GameObject;
            j.transform.parent = o.transform;
            borderObjects.Add(j);
        }
    }


}

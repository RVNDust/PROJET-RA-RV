using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using UnityEngine;
using System.Linq;

public class Radar : MonoBehaviour
{
    public List<GameObject> trackedObjects;
    public List<GameObject> radarObjects;
    public GameObject radarPrefab;
    public List<GameObject> borderObjects;
    public float switchDistance;
    public Transform Blip;
    public Material myMat;
    public int previousNum;

    // Use this for initialization
    void Start()
    {
        previousNum = 1;
        getRadarObjects();
    }


    void getRadarObjects()
    {
        previousNum = GameObject.FindGameObjectsWithTag("Players").Length;
        foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("Players"))
        {
            Debug.Log(GameObject.FindGameObjectsWithTag("Players").Length);
            int num = 1;
            if (fooObj.GetComponent<NetworkIdentity>().isLocalPlayer == true)
            {
                Debug.Log("Detected Local Player");
                fooObj.name = "PlayerMe";
            }
            else
            {
                fooObj.name = "PLayerOther";
                trackedObjects.Add(fooObj);
                Debug.Log("Detected player:" + fooObj);
            }

        }
        radarObjects = new List<GameObject>();
        borderObjects = new List<GameObject>();
        foreach (GameObject o in trackedObjects)
        {
            int name = 1;
            GameObject k;
            k = Instantiate(radarPrefab, o.transform.position, o.transform.rotation * Quaternion.Euler(90, 0, 0));
            k.name = k.name + name;
            k.transform.parent = o.transform;
            string TargetName = k.name;
            
            radarObjects.Add(k);

            GameObject j;
            j = Instantiate(radarPrefab, o.transform.position, o.transform.rotation * Quaternion.Euler(90, 0, 0));
            j.name = j.name + name;
            j.transform.parent = o.transform;
            j.GetComponent<Renderer>().material = myMat;
            string TargetName1 = j.name;

            borderObjects.Add(j);
            name++;
        }
    }
    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < radarObjects.ToArray().Length; i++)
        {
            if (Vector3.Distance(radarObjects.ToArray()[i].transform.position, transform.position) > switchDistance)
            {
                Blip.LookAt(radarObjects[i].transform);
                borderObjects[i].transform.position = transform.position + switchDistance * Blip.forward;
                Debug.Log("il est passé par ici!");
                borderObjects[i].layer = LayerMask.NameToLayer("RadarBlips");
                radarObjects[i].layer = LayerMask.NameToLayer("Invisible");
            }
            else
            {
                radarObjects[i].layer = LayerMask.NameToLayer("RadarBlips");
                borderObjects[i].layer = LayerMask.NameToLayer("Invisible");
                Debug.Log("Et repassera par la!");
            }
        }
    }

}

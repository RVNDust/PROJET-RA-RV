using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using UnityEngine;
using System.Linq;

public class Radar : NetworkBehaviour
{
    public List<GameObject> trackedObjects;
    public List<GameObject> radarObjects;
    public GameObject radarPrefab;
    public List<GameObject> borderObjects;
    public float switchDistance;
    public Transform UserMe;
    public Material orange;
    public Material green;
    public int previousNum;
    public GameObject ArrowExist;
    public GameObject BorderExist;
    private int frames;

    // Use this for initialization
    void Start()
    {
        frames = 0;
        previousNum = 0;
        if (GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            gameObject.tag = "UserMe";
        }
        else
        {
            gameObject.tag = "UserOther";
        }

    }

    void ClearAllArraysForMe()
    {
        if (GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            if (frames > GetComponent<FPSDisplay>().fps / 10)
            {
                for (int i = 0; i < borderObjects.Count; i++)
                {
                    Destroy(borderObjects[i]);
                    Debug.Log("Deleted");
                }
                frames = 0;
                borderObjects.Clear();
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            // ClearAllArraysForMe();
            previousNum = GameObject.FindGameObjectsWithTag("UserOther").Length;
            foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("UserOther"))
            {
                fooObj.name = "UserOther";
                if (gameObject.tag == "UserOther")
                {
                    BorderExist.SetActive(true);
                    // ArrowExist.SetActive(true);
                }
                else
                {
                    Destroy(BorderExist);
                    Destroy(ArrowExist);
                }
                ClearAllArraysForMe();
                GameObject a = Instantiate(radarPrefab, fooObj.GetComponent<Radar>().ArrowExist.transform.position, fooObj.GetComponent<Radar>().ArrowExist.transform.rotation);
                a.SetActive(true);
                if (Vector3.Distance(fooObj.transform.position, transform.position) > switchDistance)
                {
                    UserMe.LookAt(fooObj.GetComponent<Radar>().ArrowExist.transform);
                    a.transform.transform.position = transform.position + switchDistance * UserMe.forward;
                    a.transform.rotation = fooObj.GetComponent<Radar>().ArrowExist.transform.rotation;
                    a.GetComponent<Renderer>().material = orange;
                    //BorderExist.transform.position = transform.position + switchDistance * UserMe.forward;
                    //BorderExist.transform.rotation = fooObj.GetComponent<Radar>().ArrowExist.transform.rotation;
                    //BorderExist.GetComponent<Renderer>().material = orange;
                    Debug.Log(fooObj.name + " Orange");
                }
                else
                {

                    a.transform.position = fooObj.GetComponent<Radar>().ArrowExist.transform.position;
                    a.transform.rotation = fooObj.GetComponent<Radar>().ArrowExist.transform.rotation;
                    a.GetComponent<Renderer>().material = green;
                    //BorderExist.transform.position = fooObj.GetComponent<Radar>().ArrowExist.transform.position;
                    //BorderExist.transform.rotation = fooObj.GetComponent<Radar>().ArrowExist.transform.rotation;
                    //BorderExist.GetComponent<Renderer>().material = green;
                    Debug.Log(fooObj.name + "Green");
                }
                borderObjects.Add(a);

            }
            frames++;
        }
    }
}

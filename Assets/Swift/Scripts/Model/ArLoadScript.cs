using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ArLoadScript : MonoBehaviour
{
    private RootObject rootObject = new RootObject();
    public GameObject[] GameObjectList;

    // Use this for initialization
    void Start()
    {
        getJson();
    }

    private void getJson()
    {
        string path;
        string jsonFile;
        //path = Application.dataPath + "/StreamingAssets" + "/Machines.json";
        path = Path.Combine(Application.persistentDataPath, "Machines.json");

        /*   if (Application.platform == RuntimePlatform.Android)
           {
               WWW reader = new WWW(path);
               while (!reader.isDone) { }

               jsonFile = reader.text;
           }
           else
           {*/
        jsonFile = File.ReadAllText(path);
        //  }


        Debug.Log(path);


        rootObject = JsonUtility.FromJson<RootObject>(jsonFile);

        for (int i = 0; i < GameObjectList.Length; i++)
        {

            foreach (Machinelist item in rootObject.machinelist)
            {
                if (GameObjectList[i].name == item.Name)
                {
                    GameObjectList[i].transform.position = new Vector3((float)item.Position.x, (float)item.Position.y, (float)item.Position.z);
                    GameObjectList[i].transform.rotation = Quaternion.Euler((float)item.Rotation.x, (float)item.Rotation.y, (float)item.Rotation.z);
                    Debug.Log(GameObjectList[i].name + " " + item.Name);
                }
            }
        }
    }

}

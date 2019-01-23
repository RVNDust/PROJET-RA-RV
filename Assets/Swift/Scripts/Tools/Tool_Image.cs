using System;
using System.IO;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_Image : ToolObject_UI {

    public string JSONPath = "Default";
    public GameObject ImageGameObject;
    Texture2D myTexture;

    string finalPath;
    WWW localFile;

    new void Start()
    {
        base.Start();
        StartCoroutine(imageAssociation(LoadConfig(JSONPath)));
    }

    string LoadConfig(string JSONPath)
    {
        string fullPath = Application.dataPath + "/StreamingAssets/ImageConfigLayout/" + JSONPath + ".json";
        ConfigurationImage configurationImage = new ConfigurationImage();
        string dataAsJSON = File.ReadAllText(fullPath);
        configurationImage = JsonUtility.FromJson<ConfigurationImage>(dataAsJSON);
        return configurationImage.imagePath;
    }

    private IEnumerator imageAssociation(string myJSONPath)
    {
        localFile = new WWW(Application.dataPath + myJSONPath);

        yield return localFile;
        //myTexture = Resources.Load(myJSONPath) as Texture2D;
        myTexture = localFile.texture;
      

        //Debug.Log(myTexture);
        ImageGameObject.GetComponent<RawImage>().texture = myTexture;

    }

    [Serializable]
    public class ConfigurationImage
    {
        public string imagePath;

    }
}

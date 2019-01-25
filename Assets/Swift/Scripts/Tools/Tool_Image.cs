using System;
using System.IO;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_Image : ToolObject_UI {

    [Tooltip("File path to the JSON file containing the path to the image.")]
    public string JSONPath = "Default";
    [Tooltip("The image object on the canvas.")]
    public GameObject ImageGameObject;
    Texture2D myTexture;

    string finalPath;
    WWW localFile;

    new void Start()
    {
        base.Start();
        StartCoroutine(imageAssociation(LoadConfig(JSONPath)));
    }

    /// <summary>
    /// Search and load the JSON file that gives the path of the image
    /// </summary>
    string LoadConfig(string JSONPath)
    {
        string fullPath = Application.dataPath + "/StreamingAssets/ImageConfigLayout/" + JSONPath + ".json";
        ConfigurationImage configurationImage = new ConfigurationImage();
        string dataAsJSON = File.ReadAllText(fullPath);
        configurationImage = JsonUtility.FromJson<ConfigurationImage>(dataAsJSON);
        return configurationImage.imagePath;
    }

    /// <summary>
    /// Given a path, find the image and put it as a texture on the image component
    /// </summary>
    private IEnumerator imageAssociation(string myJSONPath)
    {
        localFile = new WWW(Application.dataPath + myJSONPath);

        yield return localFile;
        myTexture = localFile.texture;
      
        ImageGameObject.GetComponent<RawImage>().texture = myTexture;
    }

    [Serializable]
    public class ConfigurationImage
    {
        public string imagePath;

    }
}

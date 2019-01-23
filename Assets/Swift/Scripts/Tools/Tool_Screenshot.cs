using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Tool_Screenshot : ToolObject {

    private Camera myCamera;

    private void Start()
    {
        myCamera = Camera.main;
    }

    IEnumerator TakeScreenshot(int width, int height)
    {
        yield return new WaitForEndOfFrame();

        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        RenderTexture renderTexture = myCamera.targetTexture;
        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        renderResult.ReadPixels(rect, 0, 0);
        string CurrentScreenShotName = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        byte[] byteArray = renderResult.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/StreamingAssets/Screenshots/" + CurrentScreenShotName, byteArray);
        RenderTexture.ReleaseTemporary(renderTexture);
        myCamera.targetTexture = null;
    }

    protected override void OnInteractDown()
	{
        StartCoroutine(TakeScreenshot(Screen.width, Screen.height));
	}

}

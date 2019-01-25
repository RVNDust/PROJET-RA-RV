using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(AudioSource))]
public class Tool_Screenshot : ToolObject {

    [Tooltip("Audio clip that'll play when the user takes a screenshot")]
    public AudioClip ScreenshotAudio;

    private Camera myCamera;

    private void Start()
    {
        myCamera = Camera.main;
    }

    IEnumerator TakeScreenshot(int width, int height)
    {
        // Wait until the end of the current frame
        yield return new WaitForEndOfFrame();

        // Create the render texture and the resulting 2D output
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        RenderTexture renderTexture = myCamera.targetTexture;
        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);

        // Encode into a png
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        renderResult.ReadPixels(rect, 0, 0);
        string CurrentScreenShotName = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        byte[] byteArray = renderResult.EncodeToPNG();

        // Create the PNG file on the hard drive
        File.WriteAllBytes(Application.dataPath + "/StreamingAssets/Screenshots/" + CurrentScreenShotName, byteArray);

        // Release the temporary render texture, other variables
        RenderTexture.ReleaseTemporary(renderTexture);
        myCamera.targetTexture = null;
    }

    protected override void OnInteractDown()
	{
        StartCoroutine(TakeScreenshot(Screen.width, Screen.height));
        GetComponent<AudioSource>().PlayOneShot(ScreenshotAudio);
	}

}

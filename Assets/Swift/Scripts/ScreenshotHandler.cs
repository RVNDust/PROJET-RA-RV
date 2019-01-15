using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenshotHandler : MonoBehaviour {

	private static ScreenshotHandler instance;

	private Camera myCamera;
	private bool takeScreenshotNextFrame;

	private void Awake(){
		instance = this;
		myCamera=gameObject.GetComponent<Camera>();
	}

	private void OnPostRender(){
		if(takeScreenshotNextFrame) {
			Debug.Log("OnPostRender_1");
			takeScreenshotNextFrame = false;
			RenderTexture renderTexture = myCamera.targetTexture;
			Debug.Log("OnPostRender_2");
			Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
			Debug.Log("2.1");
			Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
			Debug.Log("2.2");
			renderResult.ReadPixels(rect, 0, 0);
			Debug.Log("2.3");
			string CurrentScreenShotName = ScreenShotName(renderTexture.width, renderTexture.height); 
			Debug.Log("OnPostRender_3");
			byte[] byteArray = renderResult.EncodeToPNG();
			Debug.Log("byteArray " + byteArray);			
			File.WriteAllBytes(Application.dataPath+"/SteamingAssets/Screenshots" + CurrentScreenShotName, byteArray);
			Debug.Log("Saved ScreenShot " + CurrentScreenShotName + " to " + Application.dataPath);
			Debug.Log("4");
			RenderTexture.ReleaseTemporary(renderTexture);
			myCamera.targetTexture = null;
		}
	}

	private void TakeScreenshot(int width, int height) {
		myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
		takeScreenshotNextFrame = true;

	}

	public static string ScreenShotName(int width, int height){
        return ("/screen_" + width + "x" + height + "_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png");
	}

	public static void TakeScreenshot_Static(int width, int height) {	
		instance.TakeScreenshot(width,height);
	}
}

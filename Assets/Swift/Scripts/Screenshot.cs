using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour {

	public int Width = 2550;
	public int Height = 3300;
	public bool TakeScreenShot = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update() {
        /*TakeScreenShot |= Input.GetKeyDown("k");
        if (TakeScreenShot) {
              RenderTexture rt = new RenderTexture(Width, Height, 24);
             camera.targetTexture = rt;
             Texture2D screenShot = new Texture2D(Width, Height, TextureFormat.RGB24, false);
             camera.Render();
             RenderTexture.active = rt;
             screenShot.ReadPixels(new Rect(0, 0, Width, Height), 0, 0);
             camera.targetTexture = null;
             RenderTexture.active = null; // JC: added to avoid errors
             Destroy(rt);
             byte[] bytes = screenShot.EncodeToPNG();
             string filename = ScreenShotName(Width, Height);
             System.IO.File.WriteAllBytes(filename, bytes);
             Debug.Log(string.Format("Took screenshot to: {0}", filename));
             TakeScreenShot = false;

            ScreenCapure.CaptureScreenshot(ScreenShotName);

            TakeScreenShot = false;
        }*/
    }

	public static string ScreenShotName(int width, int height){
         return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png", 
                              Application.dataPath, 
                              width, height, 
                              System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
	}

	public void TakeSC() {
        TakeScreenShot = true;
    }

}

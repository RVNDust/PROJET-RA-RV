using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {

	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("k")) {
			ScreenshotHandler.TakeScreenshot_Static(Screen.width, Screen.height);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewPosition : MonoBehaviour {

	private bool topPosition = true;

	public Vector3 pos = new Vector3();
	public Vector3 rot = new Vector3();
	//private Vector3 pos;
	//private Vector3 rot;
	private Vector3 currentPosition;

	private static TopViewPosition instance;


	// Use this for initialization
	void Start () {	
		currentPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("t")) {
			GoTopViewPosition();
		}

	}
	private void GoTopViewPosition() {
		Debug.Log("GoTopViewPosition");
		if(topPosition == true) {
			currentPosition = transform.position;
			pos = new Vector3(currentPosition.x, 21, currentPosition.z);
			transform.position = pos;
			transform.Rotate(rot* Time.deltaTime, Space.Self);
			topPosition = false;
		}
		else{
			transform.position = currentPosition;
			topPosition = true;
		}


	}
	
}

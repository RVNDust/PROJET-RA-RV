using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewPosition : MonoBehaviour {

	private bool topPosition;

    public GameObject Roof;

    public float Hover;


	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("t")) {
			GoTopViewPosition();
		}

	}
	private void GoTopViewPosition() {
		Debug.Log("GoTopViewPosition");
		if(topPosition == false) {
            transform.position = new Vector3(transform.position.x, transform.position.y + Roof.transform.position.y + Hover, transform.position.z);

			topPosition = true;
		}
		else{

            transform.position = new Vector3(transform.position.x, transform.position.y - Roof.transform.position.y + Hover, transform.position.z);
            topPosition = false;
		}


	}
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewPosition : MonoBehaviour {

    public float Height = 10.0f;

    private GameObject Ground;


    // Use this for initialization
    void Start () {
        Ground = GameObject.Find("TeleportGround");
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("TopView"))
        {
			GoTopViewPosition();
		}

	}

    private void GoTopViewPosition()
    {
        Vector3 heightPos = new Vector3(0, Height, 0);
        if (!Ground.CompareTag("isUsed"))
        {
            Ground.transform.position += heightPos;
            transform.position += heightPos;
            Ground.tag = "isUsed";
        }
        else
        {
            Ground.transform.position -= heightPos;
            transform.position -= heightPos;
            Ground.tag = "Untagged";
        }
    }

}

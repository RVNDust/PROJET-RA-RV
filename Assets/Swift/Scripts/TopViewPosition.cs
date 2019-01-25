using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    This script is similar to "Tool_TopView" but for desktop.
 */
public class TopViewPosition : MonoBehaviour {

    [Tooltip("Height of the top view")]
    public float Height = 10.0f;

    private GameObject Ground;

    void Start () {
        Ground = GameObject.Find("TeleportGround");
    }
	
	void Update () {
		if (Input.GetButtonDown("TopView"))
        {
			GoTopViewPosition();
		}

	}

    /// <summary>
    /// Teleport the teleportable area and the player upward by a given height.
    /// </summary>
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

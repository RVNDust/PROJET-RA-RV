using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_TopView : ToolObject
{

    public float Height = 10.0f;

    private GameObject Ground;

    private Vector3 currentPosition;
    private GameObject localPlayer;

    private void Start()
    {
        localPlayer = GetLocalPlayer();
        Ground = GameObject.Find("TeleportGround");
    }

    protected override void OnInteractDown()
    {
        GoTopViewPosition();
    }

    private void GoTopViewPosition()
    {
        Vector3 heightPos = new Vector3(0, Height, 0);
        if (!Ground.CompareTag("isUsed"))
        {
            Ground.transform.position += heightPos;
            localPlayer.transform.position += heightPos;
            Ground.tag = "isUsed";
        } else
        {
            Ground.transform.position -= heightPos;
            localPlayer.transform.position -= heightPos;
            Ground.tag = "Untagged";
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Tool_Teleport : ToolObject {


	void Awake()
	{

	}

	public override void OnInteractDown()
	{
        TeleportPressed();
	}

	public override void OnInteractUp()
	{
        TeleportReleased();
	}
	
	public override void OnMenu()
	{

	}

    /// <summary>
    /// Adds a pointer at the tip of the controller for teleporation
    /// </summary>
    private void TeleportPressed()
    {
        ControllerPointer ContPt = gameObject.AddComponent<ControllerPointer>();
        ContPt.RaycastMask = LayerMask.GetMask("TeleportableFloor");
    }

    /// <summary>
    /// Removes the pointer and teleports the player to the destination (if it is correct)
    /// </summary>
    private void TeleportReleased()
    {
        ControllerPointer ContPt = gameObject.GetComponent<ControllerPointer>();
        if(ContPt.CanTeleport) 
        {
            Vector3 cameraPos = GetLocalPlayer().GetComponent<VR_CameraRigMultiuser>().SteamVRCamera.transform.localPosition;
            Vector3 pointerPos = GetComponent<ControllerPointer>().TargetPosition;
            Vector3 offset = new Vector3(pointerPos.x - cameraPos.x, pointerPos.y, pointerPos.z - cameraPos.z);
            GetLocalPlayer().transform.position = offset;
        }
        ContPt.DesactivatePointer();
        Destroy(ContPt);
    }

}

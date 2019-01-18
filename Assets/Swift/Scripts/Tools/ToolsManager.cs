using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(SteamVR_Behaviour_Pose))]
public class ToolsManager : MonoBehaviour {

    public SteamVR_Input_Sources inputSource;
	public List<GameObject> ToolsList = new List<GameObject>();

	private int id_currentTool = -1;
	private GameObject currentTool;
	private bool UImode;

	private void Update()
	{
		#region Input SendMessage
		if(SteamVR_Input.Swift.inActions.Interact.GetState(inputSource))
		{
			GetComponentsInChildren<ToolObject>().ForEach(x => x.SendMessage("OnInteract"));
		}
		if(SteamVR_Input.Swift.inActions.Interact.GetStateDown(inputSource))
		{
			GetComponentsInChildren<ToolObject>().ForEach(x => x.SendMessage("OnInteractDown"));
		}
		if(SteamVR_Input.Swift.inActions.Interact.GetStateUp(inputSource))
		{
			GetComponentsInChildren<ToolObject>().ForEach(x => x.SendMessage("OnInteractUp"));
		}

		if(SteamVR_Input.Swift.inActions.Menu.GetState(inputSource))
		{
			GetComponentsInChildren<ToolObject>().ForEach(x => x.SendMessage("OnMenu"));
		}
		if(SteamVR_Input.Swift.inActions.Menu.GetStateDown(inputSource))
		{
			GetComponentsInChildren<ToolObject>().ForEach(x => x.SendMessage("OnMenuDown"));
		}
		if(SteamVR_Input.Swift.inActions.Menu.GetStateUp(inputSource))
		{
			GetComponentsInChildren<ToolObject>().ForEach(x => x.SendMessage("OnMenuUp"));
		}

		if(SteamVR_Input.Swift.inActions.Grip.GetState(inputSource))
		{
			GetComponentsInChildren<ToolObject>().ForEach(x => x.SendMessage("OnGrip"));
		}
		if(SteamVR_Input.Swift.inActions.Grip.GetStateDown(inputSource))
		{
			GetComponentsInChildren<ToolObject>().ForEach(x => x.SendMessage("OnGripDown"));
		}
		if(SteamVR_Input.Swift.inActions.Grip.GetStateUp(inputSource))
		{
			GetComponentsInChildren<ToolObject>().ForEach(x => x.SendMessage("OnGripUp"));
		}

		if(SteamVR_Input.Swift.inActions.Touchpress.GetState(inputSource))
		{
			GetComponentsInChildren<ToolObject>().ForEach(x => 
			{
				x.SendMessage("OnTouchpress");
				x.touchpadPos = SteamVR_Input.Swift.inActions.TouchPos.GetAxis(inputSource);
			});
		}
		if(SteamVR_Input.Swift.inActions.Touchpress.GetStateDown(inputSource))
		{
			GetComponentsInChildren<ToolObject>().ForEach(x => 
			{
				x.SendMessage("OnTouchpressDown");
				x.touchpadPos = SteamVR_Input.Swift.inActions.TouchPos.GetAxis(inputSource);
			});
		}
		if(SteamVR_Input.Swift.inActions.Touchpress.GetStateUp(inputSource))
		{
			GetComponentsInChildren<ToolObject>().ForEach(x => 
			{
				x.SendMessage("OnTouchpressUp");
				x.touchpadPos = SteamVR_Input.Swift.inActions.TouchPos.GetAxis(inputSource);
			});
		}

		if(SteamVR_Input.Swift.inActions.Touchpad.GetState(inputSource))
		{
			GetComponentsInChildren<ToolObject>().ForEach(x => 
			{
				x.SendMessage("OnTouchpad");
				x.touchpadPos = SteamVR_Input.Swift.inActions.TouchPos.GetAxis(inputSource);
			});
		}
		if(SteamVR_Input.Swift.inActions.Touchpad.GetStateDown(inputSource))
		{
			GetComponentsInChildren<ToolObject>().ForEach(x => 
			{
				x.SendMessage("OnTouchpadDown");
				x.touchpadPos = SteamVR_Input.Swift.inActions.TouchPos.GetAxis(inputSource);
			});
		}
		if(SteamVR_Input.Swift.inActions.Touchpad.GetStateUp(inputSource))
		{
			GetComponentsInChildren<ToolObject>().ForEach(x => 
			{
				x.SendMessage("OnTouchpadUp");
				x.touchpadPos = SteamVR_Input.Swift.inActions.TouchPos.GetAxis(inputSource);
			});
		}
		#endregion
		// Allows changing tools when we are not using any UI element
		if(SteamVR_Input.Swift.inActions.Touchpress.GetStateDown(inputSource) && !UImode)
		{
			SetCurrentTool(GetNextToolId());
			Debug.LogWarning("No UI element detected, changing tool to " + GetCurrentToolName());
		}
	}

	void Awake()
	{
		inputSource = GetComponent<SteamVR_Behaviour_Pose>().inputSource;
		SetCurrentTool(0);
		if(GetComponent<ToolsManager_UI>() != null)
		{
			UImode = true;
		}
	}

	public string GetToolName(int id)
	{
		return ToolsList[id].GetComponent<ToolObject>().toolName;
	}

	public Sprite GetToolIcon(int id)
	{
		return ToolsList[id].GetComponent<ToolObject>().toolIcon;
	}

	// Any methods that relate to the currently equipped tool

	public void SetCurrentTool(int id)
	{
		if(GetCurrentToolId() != id)
		{
			if(currentTool != null)
			{
				Destroy(currentTool);
			}
			id_currentTool = id;
			currentTool = Instantiate(ToolsList[id]);
			currentTool.transform.parent = gameObject.transform;
			currentTool.transform.localPosition = Vector3.zero;
			currentTool.transform.localRotation = Quaternion.identity;
            currentTool.GetComponent<ToolObject>().inputSource = inputSource;
		}
	}

	public string GetCurrentToolName()
	{
		return ToolsList[id_currentTool].GetComponent<ToolObject>().toolName;
	}

	public Sprite GetCurrentToolIcon()
	{
		return ToolsList[id_currentTool].GetComponent<ToolObject>().toolIcon;
	}

	public int GetCurrentToolId()
	{
		return id_currentTool;
	}

	// a ranger 

	public int GetNextToolId()
	{
		if(id_currentTool+1 > ToolsList.Count-1)
		{
			return 0;
		}
		return id_currentTool+1;
	}

}

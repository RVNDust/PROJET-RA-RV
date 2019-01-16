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

	private int id_currentTool = 0;
	private GameObject currentTool;
	private bool UImode;

	private void Update()
	{
		if(SteamVR_Input.Swift.inActions.Interact.GetStateDown(inputSource))
		{
			currentTool.SendMessage("OnInteractDown");
		}
		if(SteamVR_Input.Swift.inActions.Interact.GetStateUp(inputSource))
		{
			currentTool.SendMessage("OnInteractUp");
		}
		if(SteamVR_Input.Swift.inActions.Menu.GetStateDown(inputSource))
		{
			currentTool.SendMessage("OnMenu");
		}
		if(SteamVR_Input.Swift.inActions.Touchpress.GetStateDown(inputSource) && !UImode)
		{
			SetCurrentTool(GetNextToolId());
		}
	}

	void Awake()
	{
		inputSource = GetComponent<SteamVR_Behaviour_Pose>().inputSource;
		SetCurrentTool(id_currentTool);
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
		if(currentTool != null)
		{
			Destroy(currentTool); // Verify if the player is holding an object, if they are, drop it -> OnDestroy()
		}
		id_currentTool = id;
		currentTool = Instantiate(ToolsList[id]);
		currentTool.transform.parent = gameObject.transform;
		currentTool.transform.localPosition = Vector3.zero;
		currentTool.transform.localRotation = Quaternion.identity;
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

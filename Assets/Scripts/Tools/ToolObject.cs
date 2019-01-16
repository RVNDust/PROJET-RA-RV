using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToolObject : MonoBehaviour {

    public string toolName = "None";
    public Sprite toolIcon;

	public abstract void OnInteractDown();
	public abstract void OnInteractUp();
	public abstract void OnMenu();

	/// <summary>
    /// Get the local player via a tag
    /// </summary>
    public GameObject GetLocalPlayer()
    {
        return GameObject.FindGameObjectWithTag("VRLocalPlayer");
    }

}

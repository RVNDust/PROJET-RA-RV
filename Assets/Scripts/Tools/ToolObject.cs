using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class ToolObject : MonoBehaviour {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ToolObject : MonoBehaviour {

    [Tooltip("Name of the tool.")]
    public string toolName = "None";
    [Tooltip("Icon of the tool.")]
    public Sprite toolIcon;

    // Every methods relating to the controller input
    protected virtual void OnInteract(){}
	protected virtual void OnInteractDown(){}
	protected virtual void OnInteractUp(){}
    
    protected virtual void OnMenu(){}
	protected virtual void OnMenuDown(){}
    protected virtual void OnMenuUp(){}

    protected virtual void OnGrip(){}
	protected virtual void OnGripDown(){}
    protected virtual void OnGripUp(){}

    protected virtual void OnTouchpress(){}
	protected virtual void OnTouchpressUp(){}
    protected virtual void OnTouchpressDown(){} 

    protected virtual void OnTouchpad(){}
	protected virtual void OnTouchpadUp(){}
    protected virtual void OnTouchpadDown(){}

    [HideInInspector]
    public Vector2 touchpadPos; // Position of the finger on the touchpad

    [HideInInspector]
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any; // The hand attached to this tool (set at runtime)

    /// <summary>
    /// Get the local player via a tag
    /// </summary>
    protected GameObject GetLocalPlayer()
    {
        return GameObject.FindGameObjectWithTag("VRLocalPlayer");
    }

    /// <summary>
    /// Debug method for an input that has yet to be implemented
    /// </summary>
    protected void NotImplementedException([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
    {
        Debug.LogWarning(memberName + " has not been implemented on " + toolName);
    }

}

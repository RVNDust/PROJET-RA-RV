using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolObject : MonoBehaviour {

    public string toolName = "None";
    public Sprite toolIcon;

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
    public Vector2 touchpadPos;

	/// <summary>
    /// Get the local player via a tag
    /// </summary>
    protected GameObject GetLocalPlayer()
    {
        return GameObject.FindGameObjectWithTag("VRLocalPlayer");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolObject_UI : ToolObject {

    [Tooltip("The canvas that will be used by the tool.")]
    public GameObject UI_Canvas;
    [Tooltip("The prefab for the UI interaction tool.")]
    public GameObject Tool_UIInteractPrefab;
    [Tooltip("Should the script give the player the UI interaction tool upon placing the window?")]
    public bool GiveInteract = true;

    private bool isDown; // Boolean to keep track if the window has been placed down in the world
    private Vector3 posOrigin_UI_Canvas;
    private Quaternion rotOrigin_UI_Canvas;
    private GameObject Tool_UIInteract;

    /// <summary>
    /// IMPORTANT - If a children overrides the start, call 'base.Start()' or MoveWindow() will not work correctly
    /// You'll need 'new void Start()'
    /// </summary>
    protected void Start()
    {
        posOrigin_UI_Canvas = UI_Canvas.transform.localPosition;
        rotOrigin_UI_Canvas = UI_Canvas.transform.localRotation;
    }

    protected override void OnMenuDown()
	{
        if(!isDown)
        {
            PlaceWindow();
            isDown = true;
        } else {
            MoveWindow();
            isDown = false;
        }
	}

    /// <summary>
    /// Places the UI_Canvas window in the world space
    /// </summary>
    protected void PlaceWindow()
    {
        UI_Canvas.transform.parent = GetLocalPlayer().transform;
        if(GiveInteract)
        {
            Tool_UIInteract = Instantiate(Tool_UIInteractPrefab, gameObject.transform);
            Tool_UIInteract.GetComponent<ToolObject>().inputSource = inputSource;
        }
    }

    /// <summary>
    /// Take back the UI_Canvas window from world space (and teleport it to the hand)
    /// </summary>
    protected void MoveWindow()
    {
        UI_Canvas.transform.SetParent(gameObject.transform);
        UI_Canvas.transform.localPosition = posOrigin_UI_Canvas;
        UI_Canvas.transform.localRotation = rotOrigin_UI_Canvas;
        if(GiveInteract) Destroy(Tool_UIInteract);
    }

    void OnDestroy()
    {
        Destroy(UI_Canvas);
        if (GiveInteract) Destroy(Tool_UIInteract);
    }

}

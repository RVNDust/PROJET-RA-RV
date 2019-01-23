using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolObject_UI : ToolObject {

	public GameObject UI_Canvas;
    [Tooltip("The prefab for the UI interaction tool.")]
    public GameObject Tool_UIInteractPrefab;
    public bool GiveInteract = true;

    private bool isDown;
    private Vector3 posOrigin_UI_Canvas;
    private Quaternion rotOrigin_UI_Canvas;
    private GameObject Tool_UIInteract;

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

    protected void PlaceWindow()
    {
        UI_Canvas.transform.parent = GetLocalPlayer().transform;
        if(GiveInteract)
        {
            Tool_UIInteract = Instantiate(Tool_UIInteractPrefab, gameObject.transform);
            Tool_UIInteract.GetComponent<ToolObject>().inputSource = inputSource;
        }
    }

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

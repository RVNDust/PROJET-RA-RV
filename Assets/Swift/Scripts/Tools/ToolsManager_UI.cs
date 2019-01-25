using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(ToolsManager))]
public class ToolsManager_UI : MonoBehaviour {

    [Tooltip("Container where the ToolPanelPrefab will be instantiated")]
	public GameObject UIPanels;
    [Tooltip("Text at the center of the touchpad")]
    public Text UIToolText;
    [Tooltip("Image at the center of the touchpad (when the finger is off of it)")]
    public Image UICurrentToolIcon;
    [Tooltip("Image that shows the finger's direction")]
    public GameObject UITouchpadDirection;
    [Tooltip("Prefab for the tool panel.")]
    public GameObject ToolPanelPrefab;
    [Tooltip("The amount of white space (in %) given to the entire touchpad")]
    [Range(0,100)]public float SpaceRatio;
    [Tooltip("Color of the selected tool")]
    public Color color_selected = new Color(0.81f, 0.51f, 0.35f);
    [Tooltip("Color of an unselected tool")]
    public Color color_default = new Color(1f, 1f, 1f, 0.3921f);

	private ToolsManager toolsManager;
	private bool isActive = false;
	private List<GameObject> activePanels = new List<GameObject>();
	private float itemAngle;
	private float rawItemAngle;
	private GameObject lastActivePanel;
	private int selectedTool;

	void Awake () {
		toolsManager = GetComponent<ToolsManager>();

        // Get the raw angle (no further calculations) by dividing 360 by the number of tools
		rawItemAngle = 360 / toolsManager.ToolsList.Count;
        // Get the angle used for the UI that takes into account the white space given by the SpaceRatio variable
		itemAngle = rawItemAngle * ((100 - SpaceRatio) / 100);
		
		// Initialize every panels, their icons and their spot in the private activePanels list
		for(int i = 0; i < toolsManager.ToolsList.Count; i++)
		{
			activePanels.Add(Instantiate(ToolPanelPrefab, UIPanels.transform));
			activePanels[i].name = "Panel_" + toolsManager.GetToolName(i);
			// After instanciating,  we rotate every tool panel according to the others
			activePanels[i].transform.localRotation = Quaternion.Euler(0, 0, -(rawItemAngle * i) + itemAngle/2 );

			// After rotating the panel, we adjust the fill amount of each tool panel
			var newPanelImg = activePanels[i].GetComponent<Image>();
			newPanelImg.fillAmount = itemAngle/360;

			// We rotate the icon on a pivot to place it in the center of their tool panel
			var toolIconPivot = activePanels[i].transform.GetChild(0);
			toolIconPivot.transform.localRotation = Quaternion.Euler(0, 0, -(itemAngle/2));

			// Finally, we rotate the icon inside with the inverse rotation of the panel rotation to make it the right way up 
			// While making sure that we're not rotating the parent
			var imageComps = activePanels[i].GetComponentsInChildren<Image>();
			foreach(Image img in imageComps)
			{
				if(img.gameObject != activePanels[i])
				{
					img.transform.localRotation = Quaternion.Euler(0, 0, (rawItemAngle*i));
					img.sprite = toolsManager.GetToolIcon(i);
				}
			}
		}

		UICurrentToolIcon.sprite = toolsManager.GetCurrentToolIcon();
	}

	private void Update()
	{
        // When the finger is on the touchpad, we activate the UI and start checking position
		if(SteamVR_Input.Swift.inActions.Touchpad.GetStateDown(toolsManager.inputSource))
		{
			isActive = true;
			UIPanels.GetComponent<Animator>().SetBool("isActive", true);
			UIToolText.enabled = true;
			UICurrentToolIcon.enabled = false;
			UITouchpadDirection.SetActive(true);
		}
        // When the finger is lifted off the touchpad, we get the last position and change the tool to that one
		if(SteamVR_Input.Swift.inActions.Touchpad.GetStateUp(toolsManager.inputSource))
		{
			isActive = false;
			UIPanels.GetComponent<Animator>().SetBool("isActive", false);
			toolsManager.SetCurrentTool(selectedTool);
			UICurrentToolIcon.sprite = toolsManager.GetCurrentToolIcon();
			UICurrentToolIcon.enabled = true;
			UITouchpadDirection.SetActive(false);
			UIToolText.enabled = false;
		}
        // This is active if the finger is on the touchpad
		if(isActive)
		{
			Vector2 touchpadPos = SteamVR_Input.Swift.inActions.TouchPos.GetAxis(toolsManager.inputSource);
            // We rotate the touchpad position counter clockwise to take into account the white space given by the SpaceRatio variable
			Vector2 rotatedTouchpadPos = Quaternion.Euler(0, 0, -rawItemAngle/2) * touchpadPos;
            // The following equation allows us to get the angle of the finger on the touchpad in degrees
			float touchpadAngle = ((Mathf.Atan2(rotatedTouchpadPos.x, rotatedTouchpadPos.y) / Mathf.PI) * 180f);
			if(touchpadAngle < 0) touchpadAngle += 360f;

			selectedTool = GetPanelFromAngle(touchpadAngle);
			GameObject hoveredPanel = activePanels[selectedTool];
			if(lastActivePanel != hoveredPanel)
			{
				if(lastActivePanel != null)
				{
					lastActivePanel.GetComponent<Animator>().SetBool("isHovered", false);
					lastActivePanel.GetComponent<Image>().color = color_default;
				}
				lastActivePanel = hoveredPanel;
				hoveredPanel.GetComponent<Animator>().SetBool("isHovered", true);
				lastActivePanel.GetComponent<Image>().color = color_selected;
				UIToolText.text = toolsManager.GetToolName(selectedTool);
				SteamVR_Input.Swift.outActions.Haptics.Execute(0, 0.1f, 50, 0.5f, toolsManager.inputSource);
			}
			UITouchpadDirection.transform.localRotation = Quaternion.Euler(0, 0, -touchpadAngle+rawItemAngle/2);
		}
	}

	private int GetPanelFromAngle(float angle)
	{
		var nbPanels = activePanels.Count-1;
		return Mathf.RoundToInt((angle)/(360/nbPanels));
	}

}

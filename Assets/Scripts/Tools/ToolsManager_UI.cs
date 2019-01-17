using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(ToolsManager))]
public class ToolsManager_UI : MonoBehaviour {

	public GameObject UIPanels;
	public Text UIToolText;
	public Image UICurrentToolIcon;
	public GameObject UITouchpadDirection;
	public GameObject ToolPanelPrefab;
	[Range(0,100)]public float SpaceRatio;
	public Color color_selected = new Color(0.81f, 0.51f, 0.35f);
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

		rawItemAngle = 360 / toolsManager.ToolsList.Count;
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
		if(SteamVR_Input.Swift.inActions.Touchpad.GetStateDown(toolsManager.inputSource))
		{
			isActive = true;
			UIPanels.GetComponent<Animator>().SetBool("isActive", true);
			UIToolText.enabled = true;
			UICurrentToolIcon.enabled = false;
			UITouchpadDirection.SetActive(true);
		}
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
		if(isActive)
		{
			Vector2 touchpadPos = SteamVR_Input.Swift.inActions.TouchPos.GetAxis(toolsManager.inputSource);
			Vector2 rotatedTouchpadPos = Quaternion.Euler(0, 0, -rawItemAngle/2) * touchpadPos;
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

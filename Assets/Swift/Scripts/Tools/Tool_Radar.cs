using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tool_Radar : ToolObject {

    [Tooltip("Prefab for the arrow that will be displayed above players.")]
    public GameObject ArrowPrefab;
    [Tooltip("Orange material that the far away player arrows will be changed to.")]
    public Material OrangeMat;
    [Tooltip("Falloff distance of the radar.")]
    public float SwitchDistance = 10.0f;
    [Tooltip("Camera that will be used to display the radar.")]
    public Camera RadarCamera;

    private List<GameObject> currentPlayers;
    private List<GameObject> arrowsList = new List<GameObject>();


    private Vector3 arrowPos = new Vector3(0, 10, 0);

    private void Start()
    {
        RadarCamera.transform.parent = GetLocalPlayer().transform;
        RadarCamera.transform.localPosition = new Vector3(0, 30, 0);
        RadarCamera.transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    private void Update()
    {
        if(arrowsList != null)
        {
            ClearList(arrowsList);
        }

        // Rotate the top camera 90 degrees while making sure it only rotates on the Y axis above the user
        Vector3 cameraRot_Euler = transform.rotation.eulerAngles;
        Quaternion cameraRot = Quaternion.Euler(90, cameraRot_Euler.y, 0);
        RadarCamera.transform.localRotation = cameraRot;

        // Find every other users (our user will always be 'VRLocalPlayer') and iterate through each of them to add arrows above their heads.
        currentPlayers = GameObject.FindGameObjectsWithTag("Player").ToList();
        foreach(GameObject go in currentPlayers)
        {
            GameObject playerArrow = Instantiate(ArrowPrefab, go.transform.position + arrowPos, go.transform.rotation, go.transform);
            arrowsList.Add(playerArrow);

            // If the distance is superior to the radar falloff, change the arrow orange and lock it the extremities of the radar.
            if(Vector3.SqrMagnitude(go.transform.position - transform.position) > SwitchDistance * SwitchDistance)
            {
                playerArrow.transform.position = GetLocalPlayer().transform.position + SwitchDistance * (go.transform.position - GetLocalPlayer().transform.position).normalized;
                playerArrow.transform.position += arrowPos;
                playerArrow.GetComponent<Renderer>().material = OrangeMat;
            }
        }
    }

    /// <summary>
    /// Destroy every gameObject in the arrowslist and clear it.
    /// </summary>
    private void ClearList(List<GameObject> list)
    {
        foreach(GameObject go in list)
        {
            Destroy(go);
        }
        list.Clear();
    }

    private void OnDestroy()
    {
        Destroy(RadarCamera.gameObject);
    }

}

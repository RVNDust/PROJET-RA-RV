using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;

public class ThirdPersonControllerMultiuser : NetworkBehaviour
{
    /// <summary>
    /// The FreeLookCameraRig GameObject to configure for the UserMe
    /// </summary>
    GameObject goFreeLookCameraRig = null;
    public GameObject CameraPivot;
    public TopViewPosition topViewScript;


    // Use this for initialization
    void Start()
    {
        GetComponent<ThirdPersonUserControl>().enabled = isLocalPlayer;

        if (isLocalPlayer){
            gameObject.tag = "VRLocalPlayer";
            FindCameraRig();

            FreeLookCam freeLookCam = goFreeLookCameraRig.GetComponent<FreeLookCam>();
            freeLookCam.enabled = true;

            FollowLocalPlayer();
        } else
        {
            Destroy(topViewScript);
            Destroy(this);
        }
    }

    protected void FindCameraRig()
    {
        goFreeLookCameraRig = transform.Find("/FreeLookCameraRig").gameObject;
    }

    /// <summary>
    /// Make the CameraRig following the LocalPlayer only.
    /// </summary>
    protected void FollowLocalPlayer()
    {
        if (goFreeLookCameraRig != null)
        {
            // find Avatar EthanHips
            Transform transformFollow = CameraPivot.transform;
            // call the SetTarget on the FreeLookCam attached to the FreeLookCameraRig
            goFreeLookCameraRig.GetComponent<FreeLookCam>().SetTarget(transformFollow);
        }
    }
}

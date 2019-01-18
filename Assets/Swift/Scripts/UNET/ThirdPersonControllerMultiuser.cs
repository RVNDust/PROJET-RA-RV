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



    // Use this for initialization
    void Start()
    {
        if(isLocalPlayer){
            gameObject.tag = "VRLocalPlayer";
        }
        UpdateGoFreeLookCameraRig();
        FollowLocalPlayer();
        ActivateLocalPlayer();
    }

    /// <summary>
    /// Get the GameObject of the CameraRig
    /// </summary>
    protected void UpdateGoFreeLookCameraRig()
    {
        try
        {
            // Get the Camera to set as the followed camera
            goFreeLookCameraRig = transform.Find("/FreeLookCameraRig").gameObject;
            FreeLookCam freeLookCam = goFreeLookCameraRig.GetComponent<FreeLookCam>();
            freeLookCam.enabled = true;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Warning, no goFreeLookCameraRig found\n" + ex);
        }

    }

    /// <summary>
    /// Make the CameraRig following the LocalPlayer only.
    /// </summary>
    protected void FollowLocalPlayer()
    {
        if (isLocalPlayer)
        {
            if (goFreeLookCameraRig != null)
            {
                // find Avatar EthanHips
                Transform transformFollow = transform.Find("EthanSkeleton") != null ? transform.Find("EthanSkeleton") : transform;
                // call the SetTarget on the FreeLookCam attached to the FreeLookCameraRig
                goFreeLookCameraRig.GetComponent<FreeLookCam>().SetTarget(transformFollow);
                Debug.Log("ThirdPersonControllerMultiuser follow:" + transformFollow);
            }
        }
    }

    protected void ActivateLocalPlayer()
    {
        // enable the ThirdPersonUserControl if it is a Loacl player = UserMe
        // disable the ThirdPersonUserControl if it is not a Loacl player = UserOther
        GetComponent<ThirdPersonUserControl>().enabled = isLocalPlayer;
    }




    // Update is called once per frame
    void Update()
    {
        // Don't do anything if we are not the UserMe isLocalPlayer
        if (!isLocalPlayer) return;
    }
}

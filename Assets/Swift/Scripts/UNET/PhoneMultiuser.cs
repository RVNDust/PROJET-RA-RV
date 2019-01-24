using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Vuforia;

public class PhoneMultiuser : NetworkBehaviour {

    public GameObject ARCamera;

    void Start()
    {
        if(isLocalPlayer){
            gameObject.tag = "VRLocalPlayer";
            Destroy(FindCameraRig());
            VuforiaRuntime.Instance.InitVuforia();
            ARCamera = transform.Find("/ARCamera").gameObject;
            ARCamera.tag = "MainCamera";
            ARCamera.GetComponent<VuforiaBehaviour>().enabled = true;
        }
    }

    protected GameObject FindCameraRig()
    {
        return transform.Find("/FreeLookCameraRig").gameObject;
    }
}

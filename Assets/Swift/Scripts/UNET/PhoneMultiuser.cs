using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PhoneMultiuser : NetworkBehaviour {
    void Start()
    {
        if(isLocalPlayer){
            gameObject.tag = "VRLocalPlayer";
        } else {
            Destroy(GetComponentInChildren<Camera>());
        }
    }

}

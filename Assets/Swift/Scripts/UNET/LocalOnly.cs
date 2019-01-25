using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
    This script will remove any object in its list if the player is not local.
 */
public class LocalOnly : NetworkBehaviour {

    public List<GameObject> ObjectsToDestroy;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            foreach(GameObject go in ObjectsToDestroy)
            {
                Destroy(go);
            }
        }
    }
}

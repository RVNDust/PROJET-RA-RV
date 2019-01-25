using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;

// Make the SteamVR available in multiplayer by deactivate script for UserOther
// Supports version # SteamVR Unity Plugin - v2.0.1
public class VR_CameraRigMultiuser : NetworkBehaviour {

    [Tooltip("Container for the parent left & right hand and head.")]
    public GameObject SteamVRLeft, SteamVRRight, SteamVRCamera;
    [Tooltip("Container for the model of the left & right hand.")]
    public GameObject UserOtherLeftHandModel, UserOtherRightHandModel;
    [Tooltip("The tool canvases used by the VR player.")]
    public GameObject ToolCanvasLeft, ToolCanvasRight;
    [Tooltip("Any script that needs to be removed if the user is not the local player. (THEY WILL BE REMOVED IN ASCENDING ORDER!)")]
    public MonoBehaviour[] ScriptsToRemove;
    private GameObject goFreeLookCameraRig;

    void Start()
    {
        UpdateGoFreeLookCameraRig();
        SteamVRactivation();
        if(isLocalPlayer){
            gameObject.tag = "VRLocalPlayer";
        }
    }

    // Deactivate de FreeLookCameraRig since we are uisng the HTC version
    // Execute only in client side
    protected void UpdateGoFreeLookCameraRig()
    {
        // Client execution ONLY LOCAL
        if (!isLocalPlayer) return;
        goFreeLookCameraRig = null;
        try
        {
            // Get the Camera to set as the follow camera
            goFreeLookCameraRig = transform.Find("/FreeLookCameraRig").gameObject;
            goFreeLookCameraRig.SetActive(false);
        } catch (System.Exception ex)
        {
            Debug.LogWarning("Warning, no goFreeLookCameraRig found\n" + ex);
        }
    }

    // If we are the client who is using the HTC, activate component of SteamVR in the client using it
    // If we are not the client using this specific HTC, deactivate some scripts
    protected void SteamVRactivation()
    {
        // Client execution for ALL
        if (!isLocalPlayer)
        {
            // Left && right activation if userMe, Deactivate if UserOther
            // Left && Right SteamVR_Rendermodel activationn if USerme, deactivate if UserOther
            foreach(MonoBehaviour script in ScriptsToRemove)
            {
                Destroy(script);
            }

            Destroy(SteamVRLeft.GetComponentInChildren<SteamVR_RenderModel>());
            Destroy(SteamVRRight.GetComponentInChildren<SteamVR_RenderModel>());

            Destroy(ToolCanvasLeft);
            Destroy(ToolCanvasRight);

            // Camera activation if userme, deactivate if userother
            Destroy(GetComponentInChildren<Camera>());
        } else {
            Destroy(UserOtherLeftHandModel);
            Destroy(UserOtherRightHandModel);
        }
    }

    /// <summary>
    /// This method will give autorithy to this player after checking if the authority was already given to someone else (and removing it if that's the case).
    /// </summary>
    [Command]
     public void CmdSetAuth(NetworkInstanceId objectId, NetworkIdentity player)
     {
         var iObject = NetworkServer.FindLocalObject(objectId);
         var networkIdentity = iObject.GetComponent<NetworkIdentity>();
         var otherOwner = networkIdentity.clientAuthorityOwner;        
 
         if (otherOwner == player.connectionToClient)
         {
             Debug.Log("Authority already available on " + objectId);
             return;
         }else
         {
             if (otherOwner != null)
             {
                 networkIdentity.RemoveClientAuthority(otherOwner);
             }
             networkIdentity.AssignClientAuthority(player.connectionToClient);
             Debug.Log("Authority assigned for " + objectId);
         }        
     }

    /// <summary>
    /// Give authority over the gameObject to this player. (Dangerous, does not check for any other autorithy!)
    /// </summary>
    [Command]
     public void CmdAddAuth(GameObject go)
     {
         NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
         ni.AssignClientAuthority(base.connectionToClient);
     }

    /// <summary>
    /// Remove this player's authority over the gameObject.
    /// </summary>
    [Command]
     public void CmdRemoveAuth(GameObject go)
     {
         NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
         ni.RemoveClientAuthority(base.connectionToClient);
     }


}

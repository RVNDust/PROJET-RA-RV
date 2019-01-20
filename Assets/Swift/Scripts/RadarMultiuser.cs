
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.ThirdPerson;

public class RadarMultiuser : NetworkBehaviour
{
    public Material PlayerLocalMat;
    public GameObject GameObjectLocalPlayerColor;
    public GameObject RadarObject;
    public GameObject BlipPrefab;
    private GameObject MyBlip;
    // Use this for initialization
    void Start()
    {
        Debug.Log("Radar isLocalPlayer:" + isLocalPlayer);


        GetComponent<ThirdPersonUserControl>().enabled = isLocalPlayer;
        if (isLocalPlayer)
        {
            try
            {
                // Change the material of the Ethan Glasses

                GameObjectLocalPlayerColor.GetComponent<Renderer>().material = PlayerLocalMat;
                MyBlip = Instantiate(BlipPrefab, GameObjectLocalPlayerColor.transform.position, GameObjectLocalPlayerColor.transform.rotation);
                MyBlip.GetComponent<Renderer>().material = PlayerLocalMat;
                MyBlip.transform.SetParent(gameObject.transform);
                Destroy(GameObjectLocalPlayerColor);
                RadarObject.SetActive(true);


                /*RadarCamera.transform.parent = transform;
                RadarCamera.SetActive(true);*/
            }
            catch (System.Exception)
            {

            }
        }
    }



}


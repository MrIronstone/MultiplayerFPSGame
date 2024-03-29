﻿using UnityEngine;
using Mirror;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string dontDrawLayerName = "DontDraw";
    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject playerUIPrefab;

    [HideInInspector]
    public GameObject playerUIInstance;


    void Start()
    {
        // Disable components that should be
        // actibe on the player that we control
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                DisableComponents();
                AssingRemoteLayer();
            }
        }
        else
        {
            

            // Disable graphics for local player
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            // create PlayerUI
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            // Configure UI
            PlayerUserInterface ui = playerUIInstance.GetComponent<PlayerUserInterface>();
            if ( ui == null)
            {
                Debug.LogError("No PlayerUserInterface component on PlayerUserInterface prefab");
            }

            ui.SetController(GetComponent<PlayerController>());
            GetComponent<Player>().SetupPlayer();
        }

        
    }

    void SetLayerRecursively ( GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach ( Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }


    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(_netID, _player);
    }


    void AssingRemoteLayer ()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents ()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    private void OnDisable()
    {
        Destroy(playerUIInstance);

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraAcive(true);
        }
        GameManager.UnRegisterPlayer(transform.name); 
    }

}
 
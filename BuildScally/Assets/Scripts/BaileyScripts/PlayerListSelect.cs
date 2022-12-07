using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListSelect : MonoBehaviour
{

    public List<GameObject> playerPrefabs = new List<GameObject>();
    List<GameObject> takenPrefabs = new List<GameObject>();

    public List<GameObject> playerMesh = new List<GameObject>();
    public List<Texture> playerTextures = new List<Texture>();
    public List<Sprite> playerIcons = new List<Sprite>();
    public RawImage image;
    public RawImage icon;
    public GameObject playerPrefab;
    public GameObject displayPrefab;
    int count = 0;
    public PlayerSetupMenuController playerSetupMenuController;
    public PlayerConfigManager playerConfigManager;
    public List<PlayerConfiguration> playerConfigs;
    public Button cancel;

    List<bool> boolCheck = new List<bool>();

    bool add = true;

    private void Start() // gets all the information needed for the player select ui
    {
        playerConfigManager = FindObjectOfType<PlayerConfigManager>();
        playerPrefab = playerPrefabs[count];
        image.texture = playerTextures[count];
        icon.texture = playerIcons[count].texture;

        displayPrefab.GetComponent<MeshFilter>().mesh = playerMesh[count].GetComponent<MeshFilter>().sharedMesh;
        displayPrefab.GetComponent<MeshRenderer>().material = playerMesh[count].GetComponent<MeshRenderer>().sharedMaterial;

        ChangeCurrent();
    }

    public GameObject GetPrefab() { return playerPrefab; }
    public void SetPrefab()  // sets the prefab on the ui for the player to choose from
    { 
        playerPrefab = playerPrefabs[count]; 
        displayPrefab.GetComponent<MeshFilter>().mesh = playerMesh[count].GetComponent<MeshFilter>().sharedMesh;
        displayPrefab.GetComponent<MeshRenderer>().material = playerMesh[count].GetComponent<MeshRenderer>().sharedMaterial;
        image.texture = playerTextures[count];
        icon.texture = playerIcons[count].texture;
        ChangeCurrent();

    }
    public void NextCount() // goes right in prefabs
    { 
        count += 1;
        if(count >= 4) 
            count = 0;
        add = true;
        SetPrefab();
    }
    public void PrevCount() // goes left in prefabs
    {
        count -= 1;
        if (count <= -1)
            count = 3;
        add=false;
        SetPrefab();
    }
    public void ChangeCurrent()// changes the prefab shown to the players and will continue till a prefab is available
    {
        while(!CheckIfPossible())
        {
            if (add)
            {
                count++;
                if (count >= 4)
                    count = 0;
                SetPrefab();
            }
            else
            {
                count -= 1;
                if (count <= -1)
                    count = 3;
                SetPrefab();
            }
        }
        
    }


    bool CheckIfPossible() // checks if the prefab is avaialble to be chosen
    {
        playerConfigs = playerConfigManager.GetPlayerPrefabs();
        foreach(var prefabs in playerConfigs)
        {
            if (prefabs.playerPrefab == playerPrefab)
                return false;
        }

        return true;
    }

    public void SetPlayer() //sets the player prefab if it is applicable
    {
        if (CheckIfPossible())
        {
            playerSetupMenuController.SetPlayerPrefab(playerPrefab);
            playerSetupMenuController.ReadyPlayer();
            cancel.Select();
        }
    }
    public void ClearPlayer()// clear player prefab
    {
        playerSetupMenuController.SetPlayerPrefab(null);
    }
}


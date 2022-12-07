using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class PlayerConfigManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;
    private PlayerControls controls;

    [SerializeField]
    private int maxPlayers = 2;

    public GameObject ready;
    public GameObject back;

    public static PlayerConfigManager instance { get; private set; }

    private void Awake()
    {
        //creates a instance of a config manager
        if (instance != null)
            Debug.Log("Make New Instance of Singleton");
        else
        {
            controls = new PlayerControls();
            instance = this; 
            DontDestroyOnLoad(instance); 
            playerConfigs = new List<PlayerConfiguration>();
            PlayerInputManager inputManager = GetComponent<PlayerInputManager>();
            Debug.Log(inputManager.playerCount);
        }
    }


    //sets the player model within the config
    public void SetPlayerModel(int index, GameObject prefab)
    {
        playerConfigs[index].playerPrefab = prefab;
    }
    //isReady is set to true and when all players are ready the players can move on to the character select.
    public void ReadyPlayer(int index)
    {

        playerConfigs[index].isReady = true;
        if (playerConfigs.Count >= maxPlayers/2 && playerConfigs.All(p=>p.isReady==true))
        {
            if(ready != null)
            ready.SetActive(true);
            return;
        }
    }
    // isReady is set to false and turns off the ui till all players are ready
    public void UnReadyPlayer(int index)
    {
        playerConfigs[index].isReady = false;
        ready.SetActive(false);

        if (playerConfigs.Count >= maxPlayers / 2 && playerConfigs.All(p => p.isReady == true))
        {
            if (ready != null)
                ready.SetActive(true);
            return;
        }
    }
    // gets all the player prefabs
    public List<PlayerConfiguration> GetPlayerPrefabs()
    {
        return playerConfigs;
    }
    // sets the player index, input, parent and allows the first player to go back to the main menu and continue to the level select. 
    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Joined " + pi.playerIndex + " - " + pi.devices[0].displayName);

        if (!playerConfigs.Any(p => p.playerIndex == pi.playerIndex))
        {
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(pi));
            playerConfigs[0].Input.onActionTriggered += Input_onActionTriggered;
        }
    }

   private void Input_onActionTriggered(CallbackContext obj)
   {
        //calls the corresponding event based on input.
        Debug.Log(obj.action.name);
        if (obj.action.name == controls.Player.Cancel.name)
            QuitSelection();
        if (obj.action.name == controls.Player.Ready.name)
            StartGame();
    }

    public void QuitSelection()
    {
        //quit to the main menu
        if (back != null)
            if (back.activeSelf)
                back.GetComponent<Button>().onClick.Invoke();
    }
    public void StartGame()
    {
        // continue to the level select. 
        if (ready != null)
            if (ready.activeSelf)
            {
                ready.GetComponent<Button>().onClick.Invoke();
                GetComponent<PlayerInputManager>().DisableJoining();
            }

    }

}

public class PlayerConfiguration
{   // the player config class
    public PlayerConfiguration(PlayerInput pi)
    {
        playerIndex = pi.playerIndex;
        Input = pi;
    }

    
    public PlayerInput Input { get; set; }

    public int playerIndex { get; set; }

    public bool isReady { get; set; }

    public GameObject playerPrefab { get; set; }
}


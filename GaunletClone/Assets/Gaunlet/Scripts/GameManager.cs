using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public GameObject[] spawnPoints;
    public List<PlayerInput> playerList = new List<PlayerInput>();

    //Instances
    public static GameManager Instance = null;

    // Events
    public event System.Action<PlayerInput> PlayerJoinedGame;
    public event System.Action<PlayerInput> PlayerLeftGame;

    [SerializeField]private InputAction joinAction;
    [SerializeField]private InputAction leaveAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        //Enable join/leave actions
        joinAction.Enable();
        joinAction.performed += context => JoinAction(context);

        leaveAction.Enable();
        leaveAction.performed += context => LeaveAction(context);
    }

    private void Start()
    {
        // Index = 0; SplitScreen = -1, ControllScheme = null
        PlayerInputManager.instance.JoinPlayer(0, -1, null);
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player joined the game");
        playerList.Add(playerInput);

        if(PlayerJoinedGame != null)
        {
            PlayerJoinedGame(playerInput);
        }
    }

    void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("BYE!");
    }

    void JoinAction(InputAction.CallbackContext context)
    {
        PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
    }

    void LeaveAction(InputAction.CallbackContext context)
    {
        // If the playerlist is bigger than one
        if(playerList.Count > 1)
        {
            // get a reference for each player in the playerlist
            foreach(var player in playerList)
            {
                // get the devices registered to the player
                foreach(var device in player.devices)
                {
                    // Check if the device is with the one that was caused the action to trigger
                    if(device != null && context.control.device == device)
                    {
                        UnregisterPlayer(player);
                        return;
                    }
                }
            }
        }
    }

    void UnregisterPlayer(PlayerInput playerInput)
    {
        // Remove the player from playerlist
        playerList.Remove(playerInput);
        
        // 
        if(PlayerLeftGame != null)
        {
            PlayerLeftGame(playerInput);
        }

        Destroy(playerInput.transform.parent.gameObject);
    }
}

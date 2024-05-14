using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

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

    // Delegates
    public static event Action<PlayerInput> onPlayerJoin;
    public static event Action<PlayerInput> onPlayerLeft;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Find all the Spawnpoints in the scene
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        //Enable join/leave actions
        joinAction.Enable();
        joinAction.performed += context => JoinAction(context);

        leaveAction.Enable();
        leaveAction.performed += context => LeaveAction(context);
    }

    private void OnDisable()
    {
        joinAction.Disable();
        leaveAction.Disable();
    }

    private void Start()
    {
        // Index = 0; SplitScreen = -1, ControllScheme = null
        //PlayerInputManager.instance.JoinPlayer(0, -1, null);
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player joined the game");
        playerList.Add(playerInput);

        if(PlayerJoinedGame != null)
        {
            PlayerJoinedGame(playerInput);
        }

        // Call Delegate
        onPlayerJoin?.Invoke(playerInput);

        // Note: ? Works as "if not null".
    }

    void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("BYE!");

        // Call Delegate
        onPlayerLeft?.Invoke(playerInput);
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

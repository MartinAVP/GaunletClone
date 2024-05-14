using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Windows;

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

    // Lists
    public List<PlayerData> playerData = new List<PlayerData>();
    [SerializeField] private List<JoinOrder> playerTypeJoinOrder;

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
        //playerData[playerData.Count].input = playerInput;
        lastInput = playerInput;
    }

    public PlayerInput lastInput;

    void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("BYE!");

        // Call Delegate
        onPlayerLeft?.Invoke(playerInput);
        PlayerLeft(playerInput);
    }

    void JoinAction(InputAction.CallbackContext context)
    {
        PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
        PlayerJoin();
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
        else
        {
            // Last Player
            Debug.Log("This is the last player trying to leave");
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

    public GameObject lastSpawnPrefab;
    public GameObject GetPlayerGameObject()
    {
        /*        Debug.Log("Executed");
                for(int i = 0; i < playerTypeJoinOrder.Count; i++)
                {
                    Debug.Log(i + " Iteration");
                    if (playerTypeJoinOrder[i].used == false)
                    {
                        Debug.Log("Found at " + i + " of type " + playerTypeJoinOrder[i].player.prefab.name);
                        return playerTypeJoinOrder[i].player.prefab;
                    }
                }
                Debug.Log("Asset Not Found");
                return null;*/
        //return playerData[playerData.Count - 1].player.prefab;
        return lastSpawnPrefab;
    }

    public void setPlayerObject(GameObject player)
    {
        playerData[playerData.Count - 1].inGamePlayer = player;
        playerData[playerData.Count - 1].input = lastInput;
    }

    private void PlayerJoin()
    {
        for (int i = 0; i < playerTypeJoinOrder.Count; i++)
        {
            if (playerTypeJoinOrder[i].used == false)
            {
                // Add Player
                playerData.Add(new PlayerData(null, playerTypeJoinOrder[playerData.Count].player, null));
                lastSpawnPrefab = playerTypeJoinOrder[i].player.prefab;

                playerTypeJoinOrder[i].used = true;

                return;
            }
        }
    }

    public void PlayerLeft(PlayerInput input)
    {
        // Find the Player by Input
        for (int i = 0; i < playerData.Count; i++)
        {
            if (playerData[i].input == input)
            {
                // Make the PlayerType Slot Available
                for (int j = 0; j < playerTypeJoinOrder.Count; j++)
                {
                    if (playerData[i].player == playerTypeJoinOrder[j].player)
                    {
                        playerTypeJoinOrder[j].used = false;
                    }
                }

                // Remove The Player
                playerData.RemoveAt(i);
                return;
            }
        }
    }


    // Join Order Class
    [System.Serializable]
    public class JoinOrder
    {
        public bool used;
        public Players player;
    }

    // Player Data
    [System.Serializable]
    public class PlayerData
    {
        public PlayerInput input;
        public Players player;
        public GameObject inGamePlayer;

        public PlayerData(PlayerInput input, Players player, GameObject inGamePlayer)
        {
            this.input = input;
            this.player = player;
            this.inGamePlayer = inGamePlayer;
        }
    }
}

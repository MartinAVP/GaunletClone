using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
//using static GameManager;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] spawnPoints;
    //public List<PlayerInput> playerList = new List<PlayerInput>();

    private bool lastPlayerPreQuit = false;

    //Instances (For Singleton)
    public static PlayerManager Instance = null;

    // Events
    public event System.Action<PlayerInput> PlayerJoinedGame;
    public event System.Action<PlayerInput> PlayerLeftGame;

    [Header("Player Join / Leave Actions")]
    [SerializeField] private InputAction joinAction;
    [SerializeField] private InputAction leaveAction;

    // Lists
    public List<PlayerData> playerData = new List<PlayerData>();
    [SerializeField] private List<JoinOrder> playerTypeJoinOrder;

    // Delegates

    // Player Joined / Left
    public static event Action<Players> onPlayerJoin;
    public static event Action<Players> onPlayerLeft;

    // Player Attempt to Quit
    public static event Action onLastPlayerTryQuit;
    public static event Action onLastPlayerTryQuitAbort;

    // Add / Remove Keys to Player
    public static event Action<Players> addKey;
    public static event Action<Players> removeKey;

    // Add / Remove Potion to Player
    public static event Action<Players> addPotion;
    public static event Action<Players> removePotion;

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

    private void OnDestroy()
    {
        for (int i = 0; i < playerData.Count; i++)
        {
            playerData[i].player.score = 0;
            playerData[i].player.keys = 0;
            playerData[i].player.potions = 0;
        }
    }

    /// ==================================================
    ///             Player Related Methods
    /// ==================================================

    // Executes when a player Join action is triggered.
    // Does not check if the player succesfully joined.
    void JoinAction(InputAction.CallbackContext context)
    {
        PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
    }
    void LeaveAction(InputAction.CallbackContext context)
    {
        if(playerData.Count > 1)
        {
            for (int i = 0; i < playerData.Count; i++)
            {
                foreach(InputDevice device in playerData[i].input.devices)
                {
                    if (device != null && context.control.device == device)
                    {
                        UnregisterPlayer(playerData[i].input);
                        return;
                    }
                }
            }
        }
        else
        {
            // Last Player
            Debug.Log("This is the last player trying to leave");

            // Check if the player really wants to leave.
            if(lastPlayerPreQuit == false)
            {
                onLastPlayerTryQuit?.Invoke();
                lastPlayerPreQuit = true;
                StartCoroutine(tryQuitDelay());
            }
            else
            {
                Debug.Log("Game Quit");
            }

        }
    }

    private IEnumerator tryQuitDelay()
    {
        yield return new WaitForSeconds(5);
        lastPlayerPreQuit = false;
        onLastPlayerTryQuitAbort?.Invoke();
    }

    void UnregisterPlayer(PlayerInput playerInput)
    {
        // Remove the player from playerlist
        //playerList.Remove(playerInput);

        // 
        if (PlayerLeftGame != null)
        {
            PlayerLeftGame(playerInput);
        }

        // Destroy GameObject
        Destroy(playerInput.transform.parent.gameObject);

        // Addon
        Players foundPlayer = null;
        //int foundAt = 0;
        // Find the player based on the playerInput
        for (int i = 0; i < playerData.Count; i++)
        {
            if (playerData[i].input == playerInput)
            {
                foundPlayer = playerData[i].player;
                Debug.Log("Found " + playerData[i].player.prefab.name + " Input at " + i);
                playerData.RemoveAt(i);
                break;
            }
        }

        // Find the player type and make the slot available
        for (int i = 0; i < playerTypeJoinOrder.Count; i++)
        {
            if (playerTypeJoinOrder[i].player == foundPlayer)
            {
                Debug.Log("Found the player type at " + i);
                playerTypeJoinOrder[i].used = !playerTypeJoinOrder[i].used;
                Debug.Log(playerTypeJoinOrder[i].used);
                break;
                //playerData.RemoveAt(foundAt);
            }
        }

        onPlayerLeft?.Invoke(foundPlayer);
    }

    // Executes when a player Succesfully joins the game.
    // Receives from the Player Input Manager.
    [HideInInspector] public GameObject lastSpawnedPrefab;
    public void SetInGameObject(GameObject gameObject, playerType player)
    {
        for (int i = 0; i < playerData.Count; i++)
        {
            if (playerData[i].player.type == player)
            {
                Debug.Log("Player Found");
                playerData[i].inGamePlayer = gameObject;
                return;
            }
        }
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player joined the game");
        //playerList.Add(playerInput);
        for (int i = 0; i < playerTypeJoinOrder.Count; i++)
        {
            if (playerTypeJoinOrder[i].used == false)
            {
                playerData.Add(new PlayerData(playerInput, playerTypeJoinOrder[i].player, null));
                lastSpawnedPrefab = playerTypeJoinOrder[i].player.prefab;
                playerTypeJoinOrder[i].used = true;

                onPlayerJoin?.Invoke(playerTypeJoinOrder[i].player);
                break;
            }
        }    

        if (PlayerJoinedGame != null)
        {
            PlayerJoinedGame(playerInput);
        }
    }
    void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player Left the Game!");
    }


    /// ==================================================
    ///           Internal / External Methods
    /// ==================================================
    // External Methods
    public void AddKeyToPlayer(playerType type)
    {
        int playerToGive = FindListLocationBasedOnType(type);
        playerData[playerToGive].player.keys++;
        addKey?.Invoke(playerData[playerToGive].player);
    }
    public void RemoveKeyToPlayer(playerType type)
    {
        int playerToGive = FindListLocationBasedOnType(type);
        playerData[playerToGive].player.keys--;
        removeKey?.Invoke(playerData[playerToGive].player);
    }

    public void AddPotionToPlayer(playerType type)
    {
        int playerToGive = FindListLocationBasedOnType(type);
        playerData[playerToGive].player.potions++;
        addPotion?.Invoke(playerData[playerToGive].player);
    }
    public void RemovePotionToPlayer(playerType type)
    {
        int playerToGive = FindListLocationBasedOnType(type);
        playerData[playerToGive].player.potions--;
        removePotion?.Invoke(playerData[playerToGive].player);
    }
    public int GetPlayerKeys(playerType type)
    {
        int playerToGive = FindListLocationBasedOnType(type);
        return playerData[playerToGive].player.keys;
    }

    // Internal Methods
    private Players FindPlayerBasedOnType(playerType type)
    {
        for (int i = 0; i < playerData.Count; i++)
        {
            if (playerData[i].player.type == type)
            {
                return playerData[i].player;
            }
        }
        return null;
    }
    private int FindListLocationBasedOnType(playerType type)
    {
        for (int i = 0; i < playerData.Count; i++)
        {
            if (playerData[i].player.type == type)
            {
                return i;
            }
        }
        return -1;
    }


    /// ==================================================
    ///            Internal Classes / Structs
    /// ==================================================
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

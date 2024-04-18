using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public List<PlayerInput> playerList = new List<PlayerInput>();

    [SerializeField]public InputAction joinAction;
    [SerializeField] public InputAction leaveAction;

    //Singleton
    public static PlayerManager instance = null;

    public event System.Action<PlayerInput> playerJoinedGame;
    public event System.Action<PlayerInput> playerLeftGame;

/*    public UnityEvent events;
    public void onEventTrigger()
    {
        events.Invoke();
    }*/

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        joinAction.Enable();
        joinAction.performed += context => JoinAction(context);

        leaveAction.Enable();
        joinAction.performed += context => LeaveAction(context);
    }

    private void Start()
    {
        PlayerInputManager.instance.JoinPlayer(0, -1, null);
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        playerList.Add(playerInput);
        if(playerJoinedGame != null)
        {
            playerJoinedGame(playerInput);
        }
        Debug.Log("Player Join the game - hello!");
    }

    void OnPlayerLeft(PlayerInput playerInput)
    {

    }

    void JoinAction(InputAction.CallbackContext context)
    {
        PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
    }
    void LeaveAction(InputAction.CallbackContext context)
    {

    }
}

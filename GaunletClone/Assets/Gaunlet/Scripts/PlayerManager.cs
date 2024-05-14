using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    //Instances
    public static PlayerManager Instance = null;

    public List<PlayerData> playerData = new List<PlayerData>();
    [SerializeField]private List<JoinOrder> playerTypeJoinOrder;

    private void OnEnable()
    {
        GameManager.onPlayerJoin += PlayerJoin;
        GameManager.onPlayerLeft += PlayerLeft;
    }

    private void OnDisable()
    {
        GameManager.onPlayerJoin -= PlayerJoin;
        GameManager.onPlayerLeft -= PlayerLeft;
    }


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
    }
    private void Start()
    {
        //playerTypeJoinOrder = new Players[4];
    }

    public GameObject GetPlayerPrefabOfLastJoin()
    {
        return playerData[playerData.Count].player.prefab;
    }

    public void PlayerJoin(PlayerInput input)
    {
        for (int i = 0; i < playerTypeJoinOrder.Count; i++)
        {
            if (playerTypeJoinOrder[i].used == false)
            {
                // Add Player
                playerData.Add(new PlayerData(input, playerTypeJoinOrder[playerData.Count].player, null));
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
                for(int j = 0; j < playerTypeJoinOrder.Count; j++)
                {
                    if (playerTypeJoinOrder[j].player == playerData[i].player)
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

    [System.Serializable]
    public class JoinOrder
    {
        [HideInInspector]public bool used;
        public Players player;
    }

    [System.Serializable]
    public struct PlayerData
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

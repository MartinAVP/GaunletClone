using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerJoinScript : MonoBehaviour
{
    private PlayerInputManager inputManager;
    private GameObject playerPrefab;
    private List<GameObject> players;

    private void Start()
    {
        inputManager = this.GetComponent<PlayerInputManager>();
        playerPrefab = inputManager.playerPrefab;
    }

    public void joinAction(InputAction.CallbackContext context)
    {
        print("Mama");
        GameObject newPlayer = Instantiate(playerPrefab, Vector3.one, Quaternion.identity);
        players.Add(newPlayer);
        newPlayer.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Keyboard");
    }
}

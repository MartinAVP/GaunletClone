using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public GameObject playerPrefab;
    PlayerController playerController;

    Vector3 startPos = Vector3.zero;

    private void Awake()
    {
        if(playerPrefab != null)
        {
            playerController = GameObject.Instantiate(playerPrefab, GameManager.Instance.spawnPoints[0].transform.position, transform.rotation).GetComponent<PlayerController>();
            transform.parent = playerController.transform;

            transform.position = playerController.transform.position;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        playerController.OnMove(context);
    }
}

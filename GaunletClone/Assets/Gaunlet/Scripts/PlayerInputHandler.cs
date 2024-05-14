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
            //Debug.Log("Called");
            //GameManager.Instance.setPlayerObject(this.gameObject);

            //playerPrefab = PlayerManager.Instance.playerData[PlayerManager.Instance.playerData.Count].player.prefab;
            playerPrefab = PlayerManager.Instance.lastSpawnedPrefab;

            playerController = GameObject.Instantiate(playerPrefab, PlayerManager.Instance.spawnPoints[0].transform.position, transform.rotation).GetComponent<PlayerController>();
            transform.parent = playerController.transform;

            transform.position = playerController.transform.position;
        }
    }

    private void Start()
    {
        //Debug.Log("Called");
        PlayerManager.Instance.SetInGameObject(this.transform.parent.gameObject, this.transform.parent.GetComponent<playerTypeHolder>().type);
        //StartCoroutine(SetDelay());
    }

    private IEnumerator SetDelay()
    {
        yield return new WaitForSeconds(.5f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        playerController.OnMove(context);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        Debug.Log(name + " shoot");
        playerController.OnShoot(context);
    }
}

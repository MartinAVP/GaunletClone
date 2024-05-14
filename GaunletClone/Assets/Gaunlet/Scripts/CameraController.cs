using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    Transform playerOne;

    private void OnEnable()
    {
        PlayerManager.onPlayerJoin += CameraStart;
    }

    private void OnDisable()
    {
        PlayerManager.onPlayerJoin -= CameraStart;
    }

    public Vector3 offset;
    public float smoothSpeed = .2f;

    private void Start()
    {
        //StartCoroutine(CameraStartDelay());
    }

    private void FixedUpdate()
    {
        if (playerOne != null && PlayerManager.Instance.playerData.Count < 2)
        {
            Vector3 desiredPosition = playerOne.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        else if(PlayerManager.Instance.playerData.Count >= 2) 
        {
            Vector3 desiredPosition = FindCentroid() + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }

    Vector3 FindCentroid()
    {
        var totalX = 0f;
        var totalY = 0f;
        var totalZ = 0f;

        for (int i = 0; i < PlayerManager.Instance.playerData.Count; i++)
        {
            totalX += PlayerManager.Instance.playerData[i].inGamePlayer.transform.position.x;
            totalY += PlayerManager.Instance.playerData[i].inGamePlayer.transform.position.y;
            totalZ += PlayerManager.Instance.playerData[i].inGamePlayer.transform.position.z;
        }

        var centerX = totalX / PlayerManager.Instance.playerData.Count;
        var centerY = totalY / PlayerManager.Instance.playerData.Count;
        var centerZ = totalZ / PlayerManager.Instance.playerData.Count;

        return new Vector3 (centerX, centerY, centerZ);
    }

    private IEnumerator CameraStartDelay()
    {
        yield return new WaitForSeconds(1f);
        playerOne = PlayerManager.Instance.playerData[0].inGamePlayer.gameObject.transform;
        transform.position = playerOne.transform.position;
    }

    public void CameraStart(Players player)
    {
        StartCoroutine(CameraStartDelay());
        /*        playerOne = PlayerManager.Instance.playerData[0].inGamePlayer.gameObject.transform;
                transform.position = playerOne.transform.position;*/
    }
}

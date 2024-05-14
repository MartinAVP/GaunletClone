using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    playerTypeHolder currentPlayerType;
    private bool canInteract = true;
    private void Awake()
    {
        currentPlayerType = GetComponent<playerTypeHolder>();
        canInteract = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canInteract == true)
        {
            if (other.tag == "Key")
            {
                Destroy(other.gameObject);
                PlayerManager.Instance.AddKeyToPlayer(currentPlayerType.type);

                canInteract = false;
                StartCoroutine(interactDelay());
            }
            if (other.tag == "Potion")
            {
                Destroy(other.gameObject);
                PlayerManager.Instance.AddPotionToPlayer(currentPlayerType.type);

                canInteract = false;
                StartCoroutine(interactDelay());
            }
            if (other.tag == "Door")
            {
                if(PlayerManager.Instance.GetPlayerKeys(currentPlayerType.type) >= 1)
                {
                    PlayerManager.Instance.RemoveKeyToPlayer(currentPlayerType.type);
                    Destroy(other.gameObject);

                    canInteract=false;
                    StartCoroutine(interactDelay());
                }
                else
                {
                    //Debug.Log("Not Enough Keys");
                }
            }
        }
    }

    private IEnumerator interactDelay()
    {
        yield return new WaitForSeconds(.1f);
        canInteract = true;
    }
}

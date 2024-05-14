using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    playerTypeHolder currentPlayerType;
    private bool canInteract = true;
    HealthComponent healthComponent;
    private void Awake()
    {
        currentPlayerType = GetComponent<playerTypeHolder>();
        canInteract = true;

        HealthComponent health = GetComponent<HealthComponent>();
        if(health != null)
        {
            health.onHealthDepleted += Kill;
            health.onTakeDamage += UpdateHealthUI;
            //int index = PlayerManager.Instance.FindListLocationBasedOnType(currentPlayerType.type);
            PlayerManager.Instance.SetHealthToPlayer(currentPlayerType.type, (int)health.MaxHealth);
        }
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
            if (other.tag == "Treasure")
            {
                Destroy(other.gameObject);
                PlayerManager.Instance.AddScoreToPlayer(currentPlayerType.type, 100);

                canInteract = false;
                StartCoroutine(interactDelay());
            }
            if (other.tag == "Food")
            {
                Destroy(other.gameObject);
                PlayerManager.Instance.AddScoreToPlayer(currentPlayerType.type, 50);

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

    private void checkPossibleSpawns()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }

    protected void UpdateHealthUI(DamageInfo info)
    {
        PlayerManager.Instance.AddHealthToPlayer(currentPlayerType.type, (int)-info.value);
    }

    protected void Kill()
    {
        gameObject.SetActive(false);
    }
}

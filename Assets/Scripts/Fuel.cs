using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null && !playerMovement.hasFuelCanister)
            {
                playerMovement.PickUpFuelCanister();
                Destroy(gameObject); // Уничтожаем объект канистры после подбора
            }
        }
    }
}

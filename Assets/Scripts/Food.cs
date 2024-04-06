using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public float hungerRecoveryAmount = 35f;
    public float healthDamageBadFood = 15f;

    public FoodType foodType;

    public enum FoodType
    {
        HealthFood,
        BadFood
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpeedCar : MonoBehaviour
{
    public Rigidbody carRigidbody; // Ссылка на Rigidbody автомобиля
    public Text speedText; // Ссылка на текстовый элемент UI

    void Update()
    {
        // Рассчитываем скорость в км/ч
        float speed = carRigidbody.velocity.magnitude * 3.6f;
        // Обновляем текстовый элемент UI, чтобы отображать скорость
        speedText.text = "Скорость: " + speed.ToString("F0") + " км/ч";
    }
}

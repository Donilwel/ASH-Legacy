using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed;
    public float usialSpeed = 10f;
    public float runSpeed = 20f;
    public float sloveSpeed = 2f;

    public float stamina = 100f; // Текущий уровень стамины
    public float maxStamina = 100f; // Максимальный уровень стамины
    public float staminaDecreasePerSecond = 10f; // Скорость расхода стамины в секунду
    public float staminaRecoveryPerSecond = 5f; // Скорость восстановления стамины в секунду
    public Slider staminaBar;
    public Text staminaText;

    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;
    private bool canRun = true; // Игрок может бежать

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    void Start()
    {
        controller = GetComponent<CharacterController>();
        controller = GetComponent<CharacterController>();
        staminaBar.maxValue = maxStamina;
        staminaBar.value = stamina;
    }

    void Update()
    {
        // Проверяем, нажата ли клавиша бега (например, Left Shift)
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0 && isMoving && canRun)
        {
            stamina -= staminaDecreasePerSecond * Time.deltaTime;
            speed = runSpeed; // Переключаемся на бег
        }
        else
        {
            if (stamina < maxStamina)
            {
                stamina += staminaRecoveryPerSecond * Time.deltaTime; // Восстанавливаем стамину
                if (stamina >= 10)
                {
                    canRun = true; // Разрешаем бег, как только стамина достигает 10
                }
            }
            speed = Input.GetKey(KeyCode.Alpha5) ? sloveSpeed : usialSpeed;
        }
        stamina = Mathf.Clamp(stamina, 0, maxStamina); //проверка на 0-100 стамины (ситуация -0.2 невозможна 101 тоже)

        // Если стамина равна 0, запрещаем бег
        if (stamina <= 0)
        {
            canRun = false;
        }
        staminaBar.value = stamina; // Обновление шкалы стамины
        staminaText.text = "Уровень стамины: " + Mathf.Round(stamina * 100f / maxStamina).ToString() + "%";

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        lastPosition = gameObject.transform.position;
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Для перезагрузки сцены

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed;
    public float usualSpeed = 10f;
    public float runSpeed = 20f;
    public float slowSpeed = 2f;

    public float stamina = 100f;
    public float maxStamina = 100f;
    public float staminaDecreasePerSecond = 10f;
    public float staminaRecoveryPerSecond = 5f;
    public Slider staminaBar;
    public Text staminaText;

    public Slider hungerSlider;
    public float currentHungry;
    public float maxHungry = 100f;

    public bool hasFuelCanister = false;
    public GameObject fuelIcon;

    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool isMoving;
    private bool canRun = true;
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    // Добавляемые переменные для здоровья
    public float health = 100f;
    public float maxHealth = 100f;
    public Slider healthBar;

    private float lastYPosition;
    private bool isFalling = false;
    public float fallDamageMultiplier = 2f;
    public float safeFallDistance = 5f;

    // UI элементы для смерти
    public GameObject deathScreen;
    public Button restartButton;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        healthBar.maxValue = maxHealth;
        healthBar.value = health;

        staminaBar.maxValue = maxStamina;
        staminaBar.value = stamina;
        healthBar.value = health;

        hungerSlider.maxValue = maxHungry;
        hungerSlider.value = currentHungry;

        lastYPosition = transform.position.y;

        // Настройка UI смерти
        restartButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        Time.timeScale = 1f;
        // Система стамины

        HungrySystem();

        HandleStamina();

        // Перемещение
        Movement();

        // Проверка на дамаг при падении
        CheckFallDamage();
        // Если здоровье <= 0, показываем экран смерти
        if (health <= 0)
        {
            Die();
        }

        fuelIcon.SetActive(hasFuelCanister);
    }

    public void PickUpFuelCanister()
    {
        hasFuelCanister = true;
    }

    public bool UseFuelCanister()
    {
        if (hasFuelCanister)
        {
            hasFuelCanister = false;
            return true;
        }
        return false;
    }
    void HungrySystem()
    {
        //опционально добавить логику голода для человека который находится в машине
        currentHungry = speed == runSpeed ? Mathf.Max(currentHungry - Time.deltaTime, 0) : Mathf.Max(currentHungry - Time.deltaTime / 3, 0);
        hungerSlider.value = currentHungry;
        if(currentHungry == 0)
        {
            TakeDamage(Time.deltaTime/2);
        }
    }

    void HandleStamina()
    {
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0 && isMoving && canRun)
        {
            stamina -= staminaDecreasePerSecond * Time.deltaTime;
            speed = runSpeed;
        }
        else
        {
            if (stamina < maxStamina)
            {
                stamina += staminaRecoveryPerSecond * Time.deltaTime;
                if (stamina >= 20)
                {
                    canRun = true;
                }
            }
            speed = Input.GetKey(KeyCode.Alpha5) ? slowSpeed : usualSpeed;
        }
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        if (stamina <= 0)
        {
            canRun = false;
        }
        staminaBar.value = stamina;
        staminaText.text = "Уровень стамины: " + Mathf.Round(stamina * 100f / maxStamina).ToString() + "%";
    }

    void Movement()
    {
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

        isMoving = lastPosition != gameObject.transform.position && isGrounded;
        lastPosition = gameObject.transform.position;
    }

    void CheckFallDamage()
    {
        // Проверяем, только если персонаж не на земле (в воздухе)
        if (!isGrounded)
        {
            float currentYPosition = transform.position.y;
            // Если персонаж начинает падение
            if (currentYPosition < lastYPosition)
            {
                isFalling = true;
            }
        }
        else if (isFalling) // Если персонаж приземлился
        {
            float fallDistance = lastYPosition - transform.position.y;
            isFalling = false;

            if (fallDistance > safeFallDistance)
            {
                float damage = (fallDistance - safeFallDistance) * fallDamageMultiplier;
                TakeDamage(damage);
            }

            // Сбрасываем lastYPosition после обработки падения
            lastYPosition = transform.position.y;
        }

        // Обновляем lastYPosition, если персонаж на земле
        if (isGrounded)
        {
            lastYPosition = transform.position.y;
        }
    }

    public void Eat(float amount)
    {
        SoundManager.Instance.PlayChewingSound();
        currentHungry = Mathf.Clamp(currentHungry + amount, 0, maxHungry);
        hungerSlider.value = currentHungry; // Обновляем UI сытости
    }


    public void TakeDamage(float damage)
    {
        SoundManager.Instance.PainSound();
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthBar.value = health; // Обновляем значение слайдера здоровья
    }

    void Die()
    {
        deathScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f; // Останавливаем время
    }

    public void RestartGame()
    {
        deathScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0); // Перезагружаем текущую сцену
    }
}

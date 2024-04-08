using UnityEngine;

public class RadiationManager : MonoBehaviour
{
    public static RadiationManager Instance { get; private set; }

    public bool isRadiationActive = true;
    public float radiationDamagePerSecond = 5f;
    private PlayerMovement player;
    public float radiationCooldown = 60f; // Отсчет времени до следующей радиации
    private bool onCooldown = false;
    public GameObject radiationIcon;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        if (isRadiationActive)
        {
            ApplyRadiationDamage();
        }
        else if (onCooldown)
        {
            // Уменьшаем время отсчета до возобновления радиации
            radiationCooldown -= Time.deltaTime;
            if (radiationCooldown <= 0)
            {
                isRadiationActive = true; // Возобновляем радиацию
                onCooldown = false; // Снимаем статус ожидания
            }
        }
        radiationIcon.SetActive(isRadiationActive);
    }

    public void StartRadiation()
    {
        SoundManager.Instance.WarningSound();
        isRadiationActive = true;
        onCooldown = false; // Отключаем ожидание, если оно было включено
    }

    public void StopRadiationTemporarily()
    {
        SoundManager.Instance.sirena.Stop();
        isRadiationActive = false;
        onCooldown = true; // Включаем отсчет времени до возобновления радиации
        radiationCooldown = 60f; // Сбрасываем время до следующего возобновления радиации
    }

    private void ApplyRadiationDamage()
    {
        if (player != null)
        {
            player.TakeDamage(radiationDamagePerSecond * Time.deltaTime);
        }
    }
}

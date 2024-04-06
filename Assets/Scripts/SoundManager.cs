using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource ShootingChannel;

    public AudioClip M1911Shot;
    public AudioClip AK47Shot;

    public AudioSource reloadingSoundM1911;
    public AudioSource reloadingSoundAK47;

    public AudioSource emptyMagazineSoundM1911;

    public AudioSource engineSoundSource; // Источник звука двигателя
    public AudioSource emptyFuel;


    public Rigidbody carRigidbody; // Ссылка на Rigidbody автомобиля
    public Transform playerTransform; // Трансформ игрока
    public float minPitch = 0.5f;
    public float maxPitch = 2.0f;
    public float maxSpeed = 100f; // Максимальная скорость для расчета pitch
    public float maxHearingDistance = 50f; // Максимальное расстояние слышимости

    private bool isPlayerInCar = false;

    private void OnEnable()
    {
        EnterExitCar.OnPlayerEnterExitCar += UpdatePlayerCarState;
    }

    private void OnDisable()
    {
        EnterExitCar.OnPlayerEnterExitCar -= UpdatePlayerCarState;
    }

    private void UpdatePlayerCarState(bool isInCar)
    {
        isPlayerInCar = isInCar;
    }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayShootingSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.AK47:
                ShootingChannel.PlayOneShot(AK47Shot);
                break;
            case Weapon.WeaponModel.M1911:
                ShootingChannel.PlayOneShot(M1911Shot);
                break;
        }
    }

    public void PlayReloadSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.AK47:
                reloadingSoundAK47.Play();
                break;
            case Weapon.WeaponModel.M1911:
                reloadingSoundM1911.Play();
                break;
        }
    }

    // Метод для воспроизведения звука заглохшего двигателя
    public void PlayEngineStallSound()
    {
        emptyFuel.Play();
    }

    private void Update()
    {
        // Если игрок в машине
        if (isPlayerInCar)
        {
            // Рассчитываем и обновляем высоту тона звука двигателя в зависимости от скорости
            float speed = carRigidbody.velocity.magnitude;
            float pitch = Mathf.Lerp(minPitch, maxPitch, speed / maxSpeed);
            engineSoundSource.pitch = pitch;
            if (!engineSoundSource.isPlaying)
            {
                engineSoundSource.Play();
            }
            engineSoundSource.volume = 1.0f; // Полная громкость, когда игрок в машине
        }
        else
        {
            // Если игрок не в машине, регулируем громкость звука в зависимости от расстояния до игрока
            float distanceToPlayer = Vector3.Distance(playerTransform.position, carRigidbody.position);
            engineSoundSource.volume = Mathf.Lerp(1.0f, 0.0f, distanceToPlayer / maxHearingDistance);

            // Автоматически запускаем или останавливаем звук двигателя в зависимости от того, слышен он или нет
            if (engineSoundSource.volume > 0.01f && !engineSoundSource.isPlaying)
            {
                engineSoundSource.Play();
            }
            else if (engineSoundSource.volume <= 0.01f && engineSoundSource.isPlaying)
            {
                engineSoundSource.Stop();
            }
        }
    }
}

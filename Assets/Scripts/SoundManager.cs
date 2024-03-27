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
}

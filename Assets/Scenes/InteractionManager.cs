using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    public Weapon hoveredWeapon = null;
    public AmmoBox hoveredAmmoBox = null;
    public Car hoveredCar = null;
    public Food hoveredFood = null;
    public MedicineChest hoveredMedicineChest = null;

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

    private void Update()
    {

        if (Camera.main)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            bool hitDetected = false;


            if (Physics.Raycast(ray, out hit, 5f))
            {
                GameObject objectHitByRaycast = hit.transform.gameObject;
                Weapon weapon = objectHitByRaycast.GetComponent<Weapon>();
                Car car = objectHitByRaycast.GetComponent<Car>();
                PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

                // Проверяем, является ли объект оружием и не является ли он активным оружием
                if (weapon && !weapon.isActiveWeapon)
                {
                    print("Selected Weapon");
                    if (hoveredWeapon)
                    {
                        hoveredWeapon.GetComponent<Outline>().enabled = false;
                    }
                    hoveredWeapon = weapon;
                    hoveredWeapon.GetComponent<Outline>().enabled = true;
                    hitDetected = true;

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                    }
                }
                if (objectHitByRaycast.GetComponent<AmmoBox>())
                {
                    print("Selected Ammo");
                    hoveredAmmoBox = objectHitByRaycast.gameObject.GetComponent<AmmoBox>();
                    hoveredAmmoBox.GetComponent<Outline>().enabled = true;
                    hitDetected = true;

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
                        Destroy(objectHitByRaycast.gameObject);
                    }
                }

                if (objectHitByRaycast.GetComponent<Food>())
                {
                    print("Selected Food");
                    Food foodItem = objectHitByRaycast.GetComponent<Food>();
                    hoveredFood = foodItem;
                    Outline outline = hoveredFood.GetComponent<Outline>();
                    if (outline != null)
                    {
                        outline.enabled = true;
                    }

                    hitDetected = true;

                    if (Input.GetKeyDown(KeyCode.F) && playerMovement.currentHungry < playerMovement.maxHungry)
                    {
                        if (playerMovement != null)
                        {
                            if (foodItem.foodType == Food.FoodType.HealthFood)
                            {
                                // Увеличиваем уровень сытости игрока
                                playerMovement.Eat(foodItem.hungerRecoveryAmount);
                            }
                            else if (foodItem.foodType == Food.FoodType.BadFood)
                            {
                                playerMovement.Eat(foodItem.hungerRecoveryAmount);
                                playerMovement.TakeDamage(foodItem.healthDamageBadFood);
                            }
                            Destroy(objectHitByRaycast);
                        }
                    }
                }

                if (objectHitByRaycast.GetComponent<MedicineChest>())
                {
                    print("Selected Food");
                    MedicineChest medicineChestItem = objectHitByRaycast.GetComponent<MedicineChest>();
                    hoveredMedicineChest = medicineChestItem;
                    Outline outline = hoveredMedicineChest.GetComponent<Outline>();
                    if (outline != null)
                    {
                        outline.enabled = true;
                    }

                    hitDetected = true;

                    if (Input.GetKeyDown(KeyCode.F) && playerMovement.health < playerMovement.maxHealth)
                    {
                        if (playerMovement != null)
                        {
                            if (medicineChestItem.typeMedChest == MedicineChest.TypeMedChest.LowHelp)
                            {
                                playerMovement.health = Mathf.Min(playerMovement.health + medicineChestItem.lowMedChest, playerMovement.maxHealth);
                            }
                            else if (medicineChestItem.typeMedChest == MedicineChest.TypeMedChest.MiddleHelp)
                            {
                                playerMovement.health = Mathf.Min(playerMovement.health + medicineChestItem.middleMedChest, playerMovement.maxHealth);
                            }
                            else if (medicineChestItem.typeMedChest == MedicineChest.TypeMedChest.HightHelp)
                            {
                                playerMovement.health = Mathf.Min(playerMovement.health + medicineChestItem.hightMedChest, playerMovement.maxHealth);
                            }
                            playerMovement.healthBar.value = playerMovement.health;
                            Destroy(objectHitByRaycast);
                        }
                    }
                }

                if (car && playerMovement.hasFuelCanister)
                {
                    print("Hovered over Car within 5 meters");
                    hitDetected = true;

                    // Включаем выделение машины
                    if (hoveredCar) hoveredCar.GetComponent<Outline>().enabled = false;
                    hoveredCar = car;
                    hoveredCar.GetComponent<Outline>().enabled = true;

                    if (Input.GetKeyDown(KeyCode.Y))
                    {
                        SoundManager.Instance.PlayCanistraSound();
                        hoveredCar.Refuel(60); // Заправляем машину на 60 единиц
                        playerMovement.hasFuelCanister = false; // Удаляем канистру из "инвентаря" игрока
                    }
                }

                if (!hitDetected)
                {
                    if (hoveredWeapon)
                    {
                        hoveredWeapon.GetComponent<Outline>().enabled = false;
                        hoveredWeapon = null;
                    }
                    if (hoveredAmmoBox)
                    {
                        hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                        hoveredAmmoBox = null;
                    }
                    if (hoveredCar)
                    {
                        hoveredCar.GetComponent<Outline>().enabled = false;
                        hoveredCar = null;
                    }
                    if (hoveredFood)
                    {
                        hoveredFood.GetComponent<Outline>().enabled = false;
                        hoveredFood = null;
                    }
                    if (hoveredMedicineChest)
                    {
                        hoveredMedicineChest.GetComponent<Outline>().enabled = false;
                        hoveredMedicineChest = null;
                    }
                }
            }
        }
    }
}

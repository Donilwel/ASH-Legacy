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

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (playerMovement != null)
                        {
                            if (foodItem.foodType == Food.FoodType.HealthFood)
                            {
                                // Увеличиваем уровень сытости игрока
                                playerMovement.currentHungry = Mathf.Min(playerMovement.currentHungry + foodItem.hungerRecoveryAmount, playerMovement.maxHungry);
                            }
                            else if (foodItem.foodType == Food.FoodType.BadFood)
                            {
                                // Наносим урон здоровью игрока
                                playerMovement.currentHungry = Mathf.Min(playerMovement.currentHungry + foodItem.hungerRecoveryAmount, playerMovement.maxHungry);
                                playerMovement.health = Mathf.Max(playerMovement.health - foodItem.healthDamageBadFood, 0);
                            }
                            playerMovement.healthBar.value = playerMovement.health;
                            playerMovement.hungerSlider.value = playerMovement.currentHungry; // Обновите UI слайдера голода
                            Destroy(objectHitByRaycast); // Удалите объект еды из сцены
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
                }
            }
        }
    }
}

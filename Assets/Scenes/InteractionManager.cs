using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    public Weapon hoveredWeapon = null;
    public AmmoBox hoveredAmmoBox = null;

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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        bool hitWeapon = false;


        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;
            Weapon weapon = objectHitByRaycast.GetComponent<Weapon>();

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
                hitWeapon = true;

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
                hitWeapon = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
                    Destroy(objectHitByRaycast.gameObject);
                }
            }
        }
        if(!hitWeapon && hoveredAmmoBox)
        {
            hoveredAmmoBox.GetComponent<Outline>().enabled = false;
            hoveredAmmoBox = null; // Сбросим ссылку на последнее выделенное оружие
        }
        // Если в этом кадре луч не попал в оружие, убираем выделение с последнего выделенного оружия.
        if (!hitWeapon && hoveredWeapon)
        {
            hoveredWeapon.GetComponent<Outline>().enabled = false;
            hoveredWeapon = null; // Сбросим ссылку на последнее выделенное оружие
        }
    }

}

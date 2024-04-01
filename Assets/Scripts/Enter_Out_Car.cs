using System;
using UnityEngine;

public class EnterExitCar : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject carCamera;
 
    public GameObject car;
    public GameObject player;
    public Transform exitPoint;
    public GameObject carCameraMap;

    public bool isPlayerInCar = false;
    public static event Action<bool> OnPlayerEnterExitCar;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isPlayerInCar)
        {
            EnterCar();
        }
        else if (Input.GetKeyDown(KeyCode.E) && isPlayerInCar)
        {
            ExitCar();
        }
    }

    void EnterCar()
    {
        isPlayerInCar = true;
        OnPlayerEnterExitCar?.Invoke(isPlayerInCar);
        carCameraMap.SetActive(true);
        player.SetActive(false); // Скрываем игрока
        car.GetComponent<Car>().enabled = true; // Включаем управление машиной
        playerCamera.gameObject.SetActive(false); // Деактивируем камеру игрока
        carCamera.gameObject.SetActive(true); // Активируем камеру машины
    }

    void ExitCar()
    {
        isPlayerInCar = false;
        OnPlayerEnterExitCar?.Invoke(isPlayerInCar);
        carCameraMap.SetActive(false);
        player.SetActive(true); // Показываем игрока
        player.transform.position = exitPoint.position; // Перемещаем игрока к точке выхода
        car.GetComponent<Car>().enabled = false; // Отключаем управление машиной
        playerCamera.gameObject.SetActive(true); // Активируем камеру игрока
        carCamera.gameObject.SetActive(false); // Деактивируем камеру машины
    }

}

using UnityEngine;

public class Enter_Out_Car : MonoBehaviour
{
    public GameObject player;
    public GameObject car;
    public bool isPlayerInside = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerNearCar() && !isPlayerInside)
        {
            EnterCar();
        }
        else if (Input.GetKeyDown(KeyCode.E) && isPlayerInside)
        {
            ExitCar();
        }
    }

    bool isPlayerNearCar()
    {
        // Реализуйте логику проверки близости игрока к машине
        return Vector3.Distance(player.transform.position, car.transform.position) < 3f;
    }

    void EnterCar()
    {
        isPlayerInside = true;
        player.SetActive(false); // Скрыть игрока
        // Передача управления машине
        car.GetComponent<CarController>().enabled = true;
    }

    void ExitCar()
    {
        isPlayerInside = false;
        player.SetActive(true); // Показать игрока
        player.transform.position = car.transform.position + new Vector3(1, 0, 0); // Игрок выходит рядом с машиной
        // Возвращение управления игроку
        car.GetComponent<CarController>().enabled = false;
    }
}

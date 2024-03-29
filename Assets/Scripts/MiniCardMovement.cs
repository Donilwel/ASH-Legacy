using UnityEngine;

public class MinimapCardMovement : MonoBehaviour
{
    public Transform player; // Переменная для объекта игрока
    public float height = 20.0f; // Высота камеры над игроком

    public Vector3 offset = new Vector3(0, 0, 0);

    void LateUpdate()
    {
        if (player != null)
        {
            // Установка позиции камеры над игроком с учетом высоты и смещения
            Vector3 newPosition = player.position + offset;
            newPosition.y += height;
            transform.position = newPosition;

            // Поворот камеры для совпадения с горизонтальным направлением игрока
            // Камера всегда смотрит вертикально вниз, угол поворота вверх-вниз фиксирован
            transform.rotation = Quaternion.Euler(90, player.eulerAngles.y, 0);
        }
    }
}
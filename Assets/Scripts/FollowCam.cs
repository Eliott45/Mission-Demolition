using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // Ссылка на шар который летит
    public float easing = 0.05f; // Перемещение камеры от растояния между ее местоположением и местоположением POI
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ; // Координата Z камеры

    void Awake() {
        camZ = this.transform.position.z; // Получить координаты Z
    }
    
    void FixedUpdate() {
        /*
            if (POI == null) return; // Если нету шара
            Vector3 destination = POI.transform.position; // Получить позицию шара 
        */
        Vector3 destination;
        // Если нет шара вернуть P 0 0 0 
        if (POI == null ) {
            destination = Vector3.zero;
        } else {
            // Получить позицию интересующего объекта
            destination = POI.transform.position;
            // Если текущий объект это шар
            if (POI.tag == "Projectile") {
                // Если он стоит на месте 
                if (POI.GetComponent<Rigidbody>().IsSleeping() ) {
                    // Вернуть исходные настройки поля зрения камеры
                    POI = null;
                    return;
                }
            }
        }
        // Ограничить X и Y минимальными значениями
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        // Определить точку между текущим местоположением камеры и destination
        destination = Vector3.Lerp(transform.position, destination, easing); 
        destination.z = camZ;
        transform.position = destination;
        // Изменить размер orthographicSize чтобы земля оставалась в поле зрения (земля.y = -10)
        Camera.main.orthographicSize = destination.y + 10;
    }
}

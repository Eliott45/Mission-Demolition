using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    static public bool goalMet = false;

    void OnTriggerEnter(Collider other) {
        // Когда в область тригера попадает что то
        if(other.gameObject.tag == "Projectile") { // Если это ядро
            Goal.goalMet = true;
            // Изменить альфа канал (увелич. непрозрачность)
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
}

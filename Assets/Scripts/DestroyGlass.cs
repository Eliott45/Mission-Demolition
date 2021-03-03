using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGlass : MonoBehaviour
{   


    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Projectile") { 
            Destroy(this.gameObject);
        }
    }
}

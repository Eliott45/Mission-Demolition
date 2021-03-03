using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySleep : MonoBehaviour
{

    void Start()
    {
        /* Обеспечить стойкость (т.е что бы стена никуда не двигалась до попадания снаряда) */
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb !=null) rb.Sleep();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Camera playerCamera; 

    void Update()
    {
        transform.LookAt(playerCamera.transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Cursor Handler
 * - Attached to camera as of now
 */
public class CursorHandler : MonoBehaviour
{


    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
}

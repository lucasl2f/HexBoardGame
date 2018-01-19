using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField]
    float movement = 1;

    bool changed;

    Transform myTransform;
    Vector3 posTemp;

    void Awake()
    {
        myTransform = gameObject.transform;
    }

    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            posTemp.z += movement;
            changed = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            posTemp.x -= movement;
            changed = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            posTemp.z -= movement;
            changed = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            posTemp.x += movement;
            changed = true;
        }

        if (changed)
        {
            myTransform.position += posTemp;
            posTemp = Vector3.zero;
            changed = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZ : MonoBehaviour
{
    [SerializeField]
    private float AnglesPerSecond = 180;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 rotation = transform.localEulerAngles;
        rotation.z -= Time.deltaTime * AnglesPerSecond;
        transform.localEulerAngles = rotation;
    }
}

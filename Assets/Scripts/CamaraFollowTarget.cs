using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraFollowTarget : MonoBehaviour
{
    [Header("Target to follow")]
    public Transform targetObject; //target to follow
    private Vector3 initalOffset; //offset
    private Vector3 cameraPosition; //next position

    void Start()
    {
        initalOffset = transform.position - targetObject.position;
    }

    void LateUpdate()
    {
        cameraPosition = targetObject.position + initalOffset;
        transform.position = cameraPosition;
    }
}

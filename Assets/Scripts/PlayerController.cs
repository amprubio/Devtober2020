using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    Camera cam;

    Ray ray;
    RaycastHit hit;

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }

    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        //Click y Posicionamiento
        if (Physics.Raycast(cam.transform.position, ray.direction, out hit, 100))
        {
            if (Input.GetButtonDown("Fire1"))
                SetPosition();

            Debug.DrawRay(cam.transform.position, ray.direction * hit.distance);
        }
    }

    void SetPosition()
    {
        agent.destination = hit.point;
    }

    private void OnDrawGizmos()
    {
        if (EditorApplication.isPlaying && hit.collider)
            Gizmos.DrawWireSphere(hit.point, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


namespace Yarn.Unity.Example
{
    public class PlayerController : MonoBehaviour
    {
        Camera cam;

        Ray ray;
        RaycastHit hit;

        NavMeshAgent agent;

        Canvas invState;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            if (GameObject.Find("Inventory_UI"))
                invState = GameObject.Find("Inventory_UI").GetComponent<Canvas>();

            cam = Camera.main;
        }

        void Update()
        {
            if (FindObjectOfType<DialogueRunner>())
            {
                if (FindObjectOfType<DialogueRunner>().IsDialogueRunning == true)
                {
                    return;
                }
            }

            ray = cam.ScreenPointToRay(Input.mousePosition);

            //Click y Posicionamiento
            if (Physics.Raycast(cam.transform.position, ray.direction, out hit, 100))
            {
                if (Input.GetButtonDown("Fire1") && invState.enabled == false)
                    SetPosition();

                Debug.DrawRay(cam.transform.position, ray.direction * hit.distance);
            }

            if (Input.GetButtonDown("Inventory") && invState)
            {
                Inventory();
            }
        }

        void SetPosition()
        {
            agent.destination = hit.point;
        }

        void Inventory()
        {
            if (invState.enabled == false)
            {
                invState.enabled = true;
            }
            else if (invState.enabled)
            {
                invState.enabled = false;
            }
        }

        private void OnDrawGizmos()
        {
            if (EditorApplication.isPlaying && hit.collider)
                Gizmos.DrawWireSphere(hit.point, 1);
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Yarn.Unity.Example {
    public class PlayerCharacter : MonoBehaviour {

        public float interactionRadius = 2.0f;

        public float movementFromButtons {get;set;}
        private Rigidbody rb;
        /// Draw the range at which we'll start talking to people.
        void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;

            // Flatten the sphere into a disk, which looks nicer in 2D games
            Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(1,1,0));

            // Need to draw at position zero because we set position in the line above
            Gizmos.DrawWireSphere(Vector3.zero, interactionRadius);
        }

       void Start()
        {
            rb = this.GetComponent<Rigidbody>();
        }
        /// Update is called once per frame
        void Update () {

            // Remove all player control when we're in dialogue
            if (FindObjectOfType<DialogueRunner>())
            {
                if (FindObjectOfType<DialogueRunner>().IsDialogueRunning == true)
                {
                    return;
                }
            }
            // Detect if we want to start a conversation
            if (Input.GetKeyDown(KeyCode.Mouse0) ) {
              Invoke("CheckForNearbyNPC",0.5f);
            }
        }

        /// Find all DialogueParticipants
        /** Filter them to those that have a Yarn start node and are in range; 
         * then start a conversation with the first one
         */
        public void CheckForNearbyNPC ()
        {
            if (rb.velocity.x < 0.1 && rb.velocity.y < 0.1 && rb.velocity.z < 0.1)
            {
                var allParticipants = new List<NPC>(FindObjectsOfType<NPC>());
                var target = allParticipants.Find(delegate (NPC p)
                {
                    return string.IsNullOrEmpty(p.talkToNode) == false && // has a conversation node?
                    (p.transform.position - this.transform.position)// is in range?
                    .magnitude <= interactionRadius;
                });
                if (target != null)
                {
                    // Kick off the dialogue at this node.
                    FindObjectOfType<DialogueRunner>().StartDialogue(target.talkToNode);
                }
            }
        }
    }
}

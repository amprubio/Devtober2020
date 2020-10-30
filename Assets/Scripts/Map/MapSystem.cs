using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yarn.Unity.Example
{
    public class MapSystem : MonoBehaviour
    {
        // Start is called before the first frame update
        public Canvas map_canvas;
        void Start()
        {
            SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void SetActive(bool b) { map_canvas.GetComponent<GameObject>().SetActive(b); }
        public bool isRunning() { return map_canvas.isActiveAndEnabled; }

    }
}

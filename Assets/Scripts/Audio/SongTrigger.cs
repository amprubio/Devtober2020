// audioController.Play(AudioType.StartMenu)
// audioController.Pause(AudioType.StartMenu)
// audioController.Stop(AudioType.StartMenu)
// audioController.Restart(AudioType.StartMenu)
// audioController.IsPlaying(AudioType.StartMenu)
// audioController.FadeIn(AudioType.StartMenu, float timeInSeconds)
// audioController.FadeOut(AudioType.StartMenu, float timeInSeconds)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore {

	namespace Audio {

		public class SongTrigger : MonoBehaviour
		{
            public GameObject playerObject;
            public MusicManager manager;
            public bool debug;

			[Header("Music is set on the gameobject name, this way: 'songname_trigger' to avoid musical bugs")]

            private MeshRenderer m_rend;
            private string musicName;

            void Start() {
                m_rend = GetComponent<MeshRenderer>();

                if (!debug) {
                    m_rend.enabled=false;
                }
                musicName = gameObject.name.Split('_')[0];
            }

            void OnTriggerEnter(Collider other){
                if (debug) {
                    Debug.Log(other.gameObject.name+" entered the [audio: " + musicName + "] trigger");
                }

                if (other.gameObject.name == playerObject.name) {
					manager.AddSong(musicName);
                }
            }

        }//end of Class
	}
}

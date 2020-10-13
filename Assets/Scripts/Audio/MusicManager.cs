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

		public class MusicManager : MonoBehaviour
		{
			public AudioController audioController;
			public bool debug;

            private string songToPlay;
            private string SourceClipName;
            private bool changing;
			private bool songAdded;

            private IEnumerator coroutine;
			private Stack<string> songsStack = new Stack<string>();

            private AudioType audioType;
			private AudioType newAudioTrack;
            private AudioSource musicSource;

            void Start() {
                musicSource = GameObject.Find("MusicSource").GetComponent<AudioSource>();
				changing = false;

				UpdateClipNameFromAudioSource();
                UpdateAudioType();
				PlayInitialSong(AudioType.Song1);
            }

			void Update() {
				UpdateClipNameFromAudioSource();
				if (songAdded) {
					if (!changing) {
						StartCoroutine( ChangeClipSmoothly(1,2,2) );
					}
					songAdded = false;
				}
			}

            void UpdateAudioType() {
				audioType = AudioType.None;
                foreach (AudioType entry in AudioType.GetValues(typeof(AudioType))){
                    if (entry.ToString() == songToPlay) {
                        audioType = entry;
                    }
                }
            } //Updates audioType variable

			void UpdateClipNameFromAudioSource() {
				if (musicSource.clip == null) {
					SourceClipName = "None";
				} else {
					SourceClipName = musicSource.clip.name;
				}
			}

			void PlayInitialSong(AudioType type) {
				audioController.Play(type);
			}

			AudioType GetCurrentSong() {
				foreach (AudioType entry in AudioType.GetValues(typeof(AudioType))){
					if (entry != AudioType.None) {
						if ( audioController.IsPlaying(entry) ) {
	                        return entry;
	                    }
					}
                }
				return AudioType.None;
			}

            IEnumerator ChangeClipSmoothly(float delay, float fadeInTime, float fadeOutTime) {
				changing = true;

                yield return new WaitForSeconds(delay);
				songToPlay = songsStack.Peek();
				UpdateAudioType();

                if (audioType != AudioType.None) {
                    audioController.FadeOut( GetCurrentSong(), fadeInTime);
					yield return new WaitForSeconds(fadeInTime);
                }

				audioController.FadeIn( audioType, fadeOutTime);
				yield return new WaitForSeconds(fadeOutTime);

                changing = false;
            }

			public void AddSong(string songName){
				if (debug) {
					Debug.Log("go add the song: "+songName);
				}
				songsStack.Push(songName);
				songAdded = true;
			}

		}//end of Class
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityCore {

	namespace Audio{

		public class AudioController : MonoBehaviour
		{
			//editor members
			public static AudioController instance;
			public bool debug;
			public AudioTrack[] tracks;

			private Hashtable m_AudioTable; //relacion entre audiotype (key) y audio tracks (value)
			private Hashtable m_JobTable;  //relacion entre audiotype (key) y jobs (value) -- (Coroutine,IEnumerator)

			[System.Serializable]
			public class AudioObject {
				public AudioType type;
				public AudioClip clip;
			}

			[System.Serializable]
			public class AudioTrack {
				public AudioSource source;
				public AudioObject[] audio;
			}

			//private members
			private class AudioJob {
				public AudioAction action;
				public AudioType type;

				public AudioJob(AudioAction _action, AudioType _type) {
					action = _action;
					type = _type;
				}
			}

			private enum AudioAction {
				START,
				STOP,
				RESTART,
				PAUSE,
				FADEOUT,
				FADEIN
			}

#region Unity Functions
			private void Awake(){
				if (!instance) {
					Configure();
				}
			}

			private void OnDisable() {
				Dispose();
			}
#endregion

#region Public Functions
			//AsyncOperation
			public void Play(AudioType _type){
				AddJob(new AudioJob(AudioAction.START, _type));
			}

			public void Stop(AudioType _type){
				if (_type != AudioType.None) {
					if ( IsPlaying(_type) ) {
						AddJob(new AudioJob(AudioAction.STOP, _type));
					} else {
						Log ("Trying to stop a clip that is not playing");
					}
				} else {
					Log ("Trying to stop nothing");
				}

			}

			public void Restart(AudioType _type){
				AddJob(new AudioJob(AudioAction.RESTART, _type));
			}

			public void Pause(AudioType _type){
				if (_type != AudioType.None) {
					if ( IsPlaying(_type) ) {
						AddJob(new AudioJob(AudioAction.PAUSE, _type));
					} else {
						Log ("Trying to pause a clip that is not playing");
					}
				} else {
					Log ("Trying to pause nothing");
				}
			}

			public void FadeOut(AudioType _type, float time){
				if (_type != AudioType.None) {
					if ( IsPlaying(_type) ) {
						AddJob(new AudioJob(AudioAction.FADEOUT, _type) , time);
					} else {
						Log ("Trying to Fade-Out a clip that is not playing");
					}
				} else {
					Log ("Trying to Fade-Out nothing");
				}

			}

			public void FadeIn(AudioType _type, float time){
				if (_type != AudioType.None) {
					if ( !IsPlaying(_type) ) {
						AddJob(new AudioJob(AudioAction.FADEIN, _type) , time);
					} else {
						Log ("Trying to Fade-In a clip that is already playing");
					}
				} else {
					Log ("Trying to Fade-In nothing");
				}
			}

			//SyncOperation
			public bool IsPlaying(AudioType _type){
				AudioTrack _track = (AudioTrack)m_AudioTable[_type];
				string checkClip,currentClip;
				checkClip = _type.ToString();

				if (_track.source.isPlaying) {
					currentClip = _track.source.clip.name;
					Log ("checking for: "+checkClip+" and is playing: "+currentClip);

					if ( checkClip.Equals(currentClip) ) {
						return true;
					}
				} else {
					Log ("checking for: "+checkClip+" and is playing: None");
				}

				return false;
			}

#endregion

#region Private Functions
			private void Configure(){
				instance = this;
				m_AudioTable = new Hashtable();
				m_JobTable = new Hashtable();
				GenerateAudioTable();
			}

			private void Dispose(){
				foreach (DictionaryEntry _entry in m_JobTable){
					IEnumerator _job = (IEnumerator)_entry.Value;
					StopCoroutine(_job);
				}
			}

			private void GenerateAudioTable() {
				foreach(AudioTrack _track in tracks){
					foreach(AudioObject _obj in _track.audio){
						//do not duplicate keys
						if (m_AudioTable.ContainsKey(_obj.type)) {
							LogWarning("You are trying to register audio ["+_obj.type+"] that has already been registered.");
						} else {
							m_AudioTable.Add(_obj.type, _track);
							Log("Registering audio ["+_obj.type+"].");
						}
					}
				}
			}

			private IEnumerator RunAudioJob(AudioJob _job){
				AudioTrack _track = (AudioTrack)m_AudioTable[_job.type];
				_track.source.clip = GetAudioClipFromAudioTrack(_job.type, _track);

				switch (_job.action) {
					case AudioAction.START:
						_track.source.Play();
						_track.source.volume = 1;
					break;

					case AudioAction.STOP:
						_track.source.Stop();
					break;

					case AudioAction.RESTART:
						_track.source.Stop();
						_track.source.Play();
						_track.source.volume = 1;
					break;

					case AudioAction.PAUSE:
						_track.source.Pause();
					break;
				}

				m_JobTable.Remove(_job.type);
				Log("Job count: "+m_JobTable.Count);

				yield return null;
			}

			private IEnumerator RunAudioJob(AudioJob _job, float floatVar){
				AudioTrack _track = (AudioTrack)m_AudioTable[_job.type];
				_track.source.clip = GetAudioClipFromAudioTrack(_job.type, _track);
				float vol;

				switch (_job.action) {
					case AudioAction.FADEOUT:
						vol = _track.source.volume;
						while ( vol > 0 ) {
							vol -= Time.deltaTime/floatVar;
							//Log("vol: " + vol);
							_track.source.volume = vol;
							yield return null;
						}

						_track.source.Pause();
					break;

					case AudioAction.FADEIN:
							_track.source.Play();

							vol = 0;
							while ( vol < 1 ) {
								vol += Time.deltaTime/floatVar;
								//Log("vol: " + vol);
								_track.source.volume = vol;
								yield return null;
							}
					break;
				}

				m_JobTable.Remove(_job.type);
				Log("Job count: "+m_JobTable.Count);

				yield return null;
			}

			private void AddJob(AudioJob _job){
				//remove conflicting jobs
				RemoveConflictingJobs(_job);

				//start job
				IEnumerator _jobRunner = RunAudioJob(_job);
				m_JobTable.Add(_job.type, _jobRunner);
				StartCoroutine(_jobRunner);
				Log("Starting job on ["+_job.type+"] with operation: "+_job.action);
			}

			private void AddJob(AudioJob _job, float floatVar){
				//remove conflicting jobs
				RemoveConflictingJobs(_job);

				//start job
				IEnumerator _jobRunner = RunAudioJob(_job, floatVar);
				m_JobTable.Add(_job.type, _jobRunner);
				StartCoroutine(_jobRunner);
				Log("Starting job with float on ["+_job.type+"] with operation: "+_job.action+" and float: "+ floatVar);
			}

			private void RemoveJob(AudioType _type){
				if (!m_JobTable.ContainsKey(_type)) {
					LogWarning("Trying to stop a Job["+_type+"] that is not running.");
					return;
				}

				IEnumerator _runningJob = (IEnumerator)m_JobTable[_type];
				StopCoroutine(_runningJob);
				m_JobTable.Remove(_type);
			}

			private void RemoveConflictingJobs(AudioJob _job){
				if (!m_JobTable.ContainsKey(_job.type)) {
					RemoveJob(_job.type);
				}

				AudioType _conflictAudio = AudioType.None;
				foreach (DictionaryEntry _entry in m_JobTable) {
					AudioType _audioType = (AudioType)_entry.Key;
					AudioTrack _audioTrackInUse = (AudioTrack)m_AudioTable[_audioType];
					AudioTrack _audioTrackNeeded = (AudioTrack)m_AudioTable[_job.type];
					if (_audioTrackNeeded.source == _audioTrackInUse.source) {
						// hay conflicto
						_conflictAudio = _audioType;
					}
				}
				if (_conflictAudio != AudioType.None) {
					RemoveJob(_conflictAudio);
				}
			}

			private AudioClip GetAudioClipFromAudioTrack(AudioType _type, AudioTrack _track){
				foreach (AudioObject _obj in _track.audio) {
					if (_obj.type == _type) {
						return _obj.clip;
					}
				}
				return null;
			}

			private void Log(string _msg){
				if (!debug) return;
				Debug.Log("[Audio Controller]: " + _msg);
			}

			private void LogWarning(string _msg){
				if (!debug) return;
				Debug.LogWarning("[Audio Controller]: " + _msg);
			}

			private void LogHash(Hashtable _table){
				if (!debug) return;

				foreach(DictionaryEntry _entry in _table) {
					var output = JsonUtility.ToJson(_entry, true);
				 	Debug.Log("{ "+_entry.Key+" , "+_entry.Value+" }");
				}
			}


#endregion

		}//end of Class

	}

}

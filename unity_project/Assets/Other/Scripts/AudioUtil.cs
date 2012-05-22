using System.Collections.Generic;
using UnityEngine;

class AudioUtil {
	/// <summary>
	/// Plays sound in specific position with regard to global listener object.
	/// </summary>
	/// <param name="sound"></param>
	/// <param name="position"></param>
	public static void PlaySound(AudioClip sound, Vector3 position) {
		var listener = AudioUtil.GetListener();

		var args = new Dictionary<int, object>();
		args[(int) MultiplayerAudioArgs.position] = position;
		args[(int) MultiplayerAudioArgs.audioClip] = sound;
		listener.SendMessage("PlaySound", args, SendMessageOptions.DontRequireReceiver);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns>Global Listener object</returns>
	public static GameObject GetListener() {
		return GameObject.Find("Listener"); ;
	}
}
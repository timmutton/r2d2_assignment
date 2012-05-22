using System.Collections.Generic;
using UnityEngine;

class AudioUtil
{
	public static void PlaySound(AudioClip sound, Vector3 position) {
		var listener = AudioUtil.GetListener();

		var args = new Dictionary<int, object>();
		args[(int) MultiplayerAudioArgs.position] = position;
		args[(int) MultiplayerAudioArgs.audioClip] = sound;
		listener.SendMessage("PlaySound", args, SendMessageOptions.DontRequireReceiver);
	}

	public static GameObject GetListener() {
		return GameObject.Find("Listener"); ;
	}
}
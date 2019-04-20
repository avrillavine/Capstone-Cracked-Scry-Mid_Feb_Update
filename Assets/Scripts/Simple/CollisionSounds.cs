using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class CollisionSounds : MonoBehaviour
{
	public AudioClip[] _clipStorage;
	AudioSource _audio;
	// Start is called before the first frame update
	void Start()
    {
		_audio = GetComponent<AudioSource>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		_audio.PlayOneShot(_clipStorage[Random.Range(0, _clipStorage.Length)]);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class BearTrap : MonoBehaviour
{

	Animator anim;
	AudioSource _audio;
	public bool _isTriggered = false;
	public AudioClip shut_sound;
	public AudioClip open_sound;

	//Damage related variables
	//public GameObject _player;
	bool _setOffByPlayer;
	//Player's health script
	public Health _health;
	public int _damage = 10;
	// Start is called before the first frame update

	//Timer Values
	//public float maxMyValue = 1.1f;
	//public float minMyValue = 0;
	//public float myValue = 1.1f; // the total
	//float _changePerSecond; // modify the total, every second
	//public float _timeToChange = 15; // the total time myValue will take to go from max to min
	//public bool _startMaterializing = false;
	void Start()
    {

		anim = GetComponent<Animator>();
		_audio = GetComponent<AudioSource>();
		//_player = GameObject.FindGameObjectWithTag("Player");
		//_health = _player.GetComponent<Health>();
		//Appear
		//_changePerSecond = (minMyValue - maxMyValue) / _timeToChange;

	}

	// Update is called once per frame
	void Update()
	{
		if (!_isTriggered)
		{
			anim.SetBool("Set", true);
			anim.SetBool("Triggered", false);

		}
		if(_isTriggered)
		{
			anim.SetBool("Set", false);
			anim.SetBool("Triggered", true);
			Shader.SetGlobalFloat("_rAmount", 0);
		}
		// If the player has zero or less health...
		//if (_health.currentHealth <= 0)
		//{
		//	// ... tell the animator the player is dead.
		//	//anim.SetTrigger("PlayerDead");
		//	Debug.Log("Add player death state");
		//}




	}

	private void OnTriggerEnter(Collider other)
	{
		if(!_isTriggered)
		{
			if (other.gameObject.tag == "Rock" || other.gameObject.tag == "Enemy")
			{
				//_ss.SkeletonTakeDamage(50.0f);
				anim.SetTrigger("shut");
				_isTriggered = true;
				
			}
			if(other.gameObject.tag == "Player")
			{
			
				_health.TakeDamage(50.0f);
				anim.SetTrigger("shut");
				_isTriggered = true;

			}
		}
		else
		{
			//Debug.Log("Already triggered, stand close and press e to reset.");
		}

		//if(other.gameObject.tag == "Player")
		//{
		//	//GamePad.SetVibration(PlayerIndex.One,)
		//}
		
	}
	private void OnTriggerStay(Collider other)
	{
		if(Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Btn 1"))
		{
			anim.SetTrigger("open");
			_isTriggered = false;
		}
	}
	void OnTriggerExit(Collider other)
	{
	}
	//Animation related events 
	void On_Shut()
	{
		_audio.clip = shut_sound;
		_audio.Play();
	}

	void On_Open()
	{
		_audio.clip = open_sound;
		_audio.Play();
	}

	//Player takes damage
	//void DmgPlayer()
	//{

	//	// If the player has health to lose...
	//	if (_health.currentHealth > 0)
	//	{
	//		// ... damage the player.
	//		_health.TakeDamage(_damage);
	//	}
	//}



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(Dissolve_Monster))]
[RequireComponent(typeof(Sentry))]
public class SkeletonScript : MonoBehaviour
{
	//borrowed from https://forum.unity.com/threads/sound-array-random.25092/
	public AudioClip[] _clipStorage;
	Animator _anim;
	AudioSource _audio;
	public AudioClip _clip;
	public GameObject pieces;
	//Dissolve_Monster _dm;
	Sentry _sentry;
	public BearTrap bearTrap;
	//Totalhealth
	public float totalHealth = 100.0f;
	//Health at the moment
	public float currentHealth = 0.0f;
	//Health Bar
	public Image _hb;

	public bool isStruck = false;
	public bool isDead = false;

	public bool fadeUI = false;

	public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
		_sentry = GetComponent<Sentry>();

		_anim = GetComponent<Animator>();
		_audio = GetComponent<AudioSource>();
		_audio.clip = _clip;
		//starts off with full health
		currentHealth = totalHealth/100.0f;

	}

    // Update is called once per frame
    void Update()
    {
		// Set the health bar's value to the current health.
		_hb.fillAmount = currentHealth;
		//For debugging purposes
		//if (Input.GetKeyDown(KeyCode.I))
		//{
		//	//print("health bar fill amount is " + _hb.fillAmount);
		//	//print("Current Health is " + currentHealth);
		//	//currentHealth -= 1.0f / 100.0f;
		//	//Lethal hit to test death state
		//	currentHealth -= 1.0f;
		//	//SkeletonTakeDamage(14.0f);
			
		//}
		//Based on code from here https://docs.unity3d.com/ScriptReference/UI.Graphic.CrossFadeAlpha.html
		//The idea is to hide the health bar as the monster vanishes so the players 
		//don't get the advantage of spotting the monster by the health bar
		//if (!_dm._startDissolving)
		//{
		//	fadeUI = false;
		//}
		//else
		//{
		//	fadeUI = true;
		//}
		//If the toggle returns true, fade in the Image
		//if (fadeUI == true)
		//{
		//	//Fully fade in Image (1) with the duration of 2
		//	_hb.CrossFadeAlpha(0, 2.0f, true);
		//}
		//If the toggle is false, fade out to nothing (0) the Image with a duration of 2
		//if (fadeUI == false)
		//{
		//	_hb.CrossFadeAlpha(1, _dm._duration*10.0f, true);
		//}
	}
	public void SkeletonTakeDamage(float _amount)
	{
		//Plays wounded animation
		_anim.SetTrigger("hit");
		_audio.Play();
		_audio.PlayOneShot(_clipStorage[Random.Range(0, _clipStorage.Length)]);
		// Reduce the current health by the damage amount.
		currentHealth -= _amount/100.0f;


		// Play the hurt sound effect.
		//playerAudio.Play();

		// If they have lost all it's health and the death flag hasn't been set yet...
		if (currentHealth <= 0.0f && !isDead)
		{
			// ... it should die.
			isDead = true;
			Death();
		}
	}

	void Death()
	{
		Instantiate(pieces, transform.position, transform.rotation);
		Shader.SetGlobalFloat("_vAmount", 0);
		Destroy(gameObject);
		//_dm.enabled = false;
		_sentry.enabled = false;
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Rock" /*&& !isDead*/)
		{
			print("Hit " + gameObject.name);
			SkeletonTakeDamage(14.0f);
			isStruck = true;
		}
		if (other.tag == "Dagger" /*&& !isDead*/)
		{
			print("Hit " + gameObject.name);
			SkeletonTakeDamage(24.0f);
			isStruck = true;
		}
		if (other.tag == "Trap" && !bearTrap._isTriggered)
		{
			print("BearTrap Triggered by " + gameObject.name);
			SkeletonTakeDamage(50.0f);
			isStruck = true;
		}
	}

	//bool Struck()
	//{
	//	return isStruck;
	//}
}

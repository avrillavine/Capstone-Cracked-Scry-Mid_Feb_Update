using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Borrows code from a unity tutorial https://unity3d.com/learn/tutorials/projects/survival-shooter/player-health
//Differences will be including the anim script with the movement script and maybe implementing in an
//attack script seperately
[RequireComponent(typeof(Movement))]

public class Health : MonoBehaviour
{
	
	public AudioSource _audio;
	public AudioClip[] _clipStorage;
	public AudioClip death;
	//Total player health
	public float totalHealth = 100.0f;              
	//Health player has at the moment
	public float currentHealth = 0.0f;       
	//UI element that controls the health bars movement
	//Health Bar
	public Image _hb;
	//Does an effect with a flashing health color 
	//Not implemented yet
	public bool isStruck = false;
	public bool isDead = false;
	public Animator anim;
	//TODO Add in death/life states from Tutorial
	//Do animations for damage and death
	// Start is called before the first frame update
	Movement _move;
	
	public bool isTriggered;
	void Start()
    {
		_move = GetComponent<Movement>();
		//starts off with full health
		currentHealth = totalHealth / 100.0f;
		//isTriggered = bearTrap._isTriggered;
		//isTriggered = false;
	}

    // Update is called once per frame
    void Update()
    {
		// Set the health bar's value to the current health.
		_hb.fillAmount = currentHealth;
	}

	public void TakeDamage(float amount)
	{
		
		// Reduce the current health by the damage amount.
		currentHealth -= amount / 100.0f;
		// Set the health bar's value to the current health.


		// Play the hurt sound effect.
		//playerAudio.Play();
		anim.Play("Knockback");
		_audio.PlayOneShot(_clipStorage[Random.Range(0, _clipStorage.Length)]);
		// If the player has lost all it's health and the death flag hasn't been set yet...
		if (currentHealth <= 0 && !isDead)
		{
			// ... it should die.
			Death();
		}
	}
	void Death()
	{

		isDead = true;
		_move.enabled = false;
		anim.Play("Death");

	}
	private void OnTriggerEnter(Collider other)
	{
		//bool triggered = GetComponent<BearTrap>()._isTriggered;
		//if (other.tag == "Trap" && !isTriggered/*&& !bearTrap._isTriggered*/)
		//{
		//	print("BearTrap Triggered by " + gameObject.name);
		//	TakeDamage(50.0f);
		//	//isStruck = true;
		//}
		//else
		//{
		//	print("BearTrap already Triggered");
		//	//isStruck = false;
		//}
		if (other.tag == "E_Weapon")
		{
			TakeDamage(10.0f);
		}
		else
		{

		}
	}
	//private void OnTriggerStay(Collider other)
	//{
	//	if (other.tag == "BearTrap" && bearTrap._isTriggered)
	//	{
	//		Debug.Log("BearTrap already triggered, please press reset");
	//	}
	//}
}

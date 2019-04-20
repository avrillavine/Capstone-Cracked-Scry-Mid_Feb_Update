using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Anim : MonoBehaviour
{
	//public Health health;
	//public SkeletonScript _ss;
	//public BearTrap _bt;
	//public Dissolve_Monster _dm;
	Animator anim;
	//right palm bone slot
	public GameObject rPalmSlot;
	public bool _hasRightHandObject = false;
	public bool _daggerEquipped = false;
	//left palm bone slot
	public GameObject lPalmSlot;
	public bool _hasLeftHandObject = false;


	//Stomach bone slots
	public GameObject beltSlot;
	public GameObject beltSlot2;
	//magnifying glass
	public GameObject mag;
	//Dagger
	public GameObject dagger;
	//Pickupable items
	public GameObject _rock;
	GameObject _rockClone;
	public float _mass = 1.0f;

	//Lantern light source and mesh
	public GameObject lantern;
	GameObject _lanternClone;

	// Start is called before the first frame update

	public float _throwForce = 10.0f;
	public float turnSpeed = 5.0f;

	public bool _isInView = false;

	AudioSource _audio;
	public AudioClip _clip;
	public AudioClip _clip1;

	public bool dragging = false;
	//public CharacterController _cc;
	void Start()
    {
		_audio = GetComponent<AudioSource>();
		_audio.playOnAwake = false;
		
		_rockClone = new GameObject();
		_lanternClone = new GameObject();
		anim = GetComponent<Animator>();

	}

	// Update is called once per frame
	public void Update()
	{
		//if(health.isDead)
		//{
		//	anim.Play("Death");
		//}
		//Activates Left-Right Movement
		anim.SetFloat("hspeed", Input.GetAxis("Horizontal"));
		//Activates Walking from Idle
		anim.SetFloat("vspeed", Input.GetAxis("Vertical"));

		//Jump
		if (Input.GetKeyDown(KeyCode.Space))
		{
			anim.SetBool("jumping", true);
			Invoke("StopJumping", 0.1f);
		}
		if (Input.GetKeyDown(KeyCode.H) || Input.GetButtonDown("Btn 0"))
		{
			if(!_hasRightHandObject)
			{
				anim.Play("Look");
				Grab_Mag();
				Slot_Mag();
			}
			else
			{
				NotPossible();
				if (_daggerEquipped)
					NotPossible();
			}
		}
		if(Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Btn 2"))
		{

			if(!_hasRightHandObject)
			{
				anim.Play("PickUp");
			}
			else
			{
				NotPossible();
				if (_daggerEquipped)
					NotPossible();
			}
		}
		if (Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Btn 1"))
		{
			if(_hasRightHandObject)
			{
				anim.Play("rThrow");
				OnThrow();
			}
			else
			{
				NotPossible();
				if (_daggerEquipped)
					NotPossible();
			}
			
		}

		//if (Input.GetKeyDown(KeyCode.B))
		//{
		//	if(!_hasLeftHandObject)
		//	{
		//		//anim.Play("lHand_Grip_Raise_Arm", 2);
		//		anim.Play("lHand_Grip_Raise_Arm");
		//		TakeLantern();
		//	}
		//	else
		//	{
		//		//NotPossible();
		//		Destroy(_lanternClone);
		//		anim.SetTrigger("lowerArm");
		//		_hasLeftHandObject = false;
		//	}

		//}
		if (_hasRightHandObject)
		{
			anim.SetLayerWeight(anim.GetLayerIndex("RHand Grip Layer"), 1.0f);
		}
		else
		{
			anim.SetLayerWeight(1, 0.0f);
		}
		//if(_hasLeftHandObject)
		//{
		//	anim.SetLayerWeight(anim.GetLayerIndex("LHand Grip Raise Arm Layer"), 1.0f);
		//}
		//else
		//{
		//	anim.SetLayerWeight(2, 0.0f);
		//}
		if(_daggerEquipped)
		{
			anim.SetLayerWeight(anim.GetLayerIndex("RHand Grip Layer"), 1.0f);
		}
		else
		{
			if (!_hasRightHandObject)
				anim.SetLayerWeight(anim.GetLayerIndex("RHand Grip Layer"), 0.0f);//anim.SetLayerWeight(1, 0.0f);
			else
				anim.SetLayerWeight(anim.GetLayerIndex("RHand Grip Layer"), 0.0f);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetButtonDown("rKick"))
		{
			anim.Play("RKick");
		}
		if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetButtonDown("lKick")) 
		{
			anim.Play("LKick");
		}
		float dPad = Input.GetAxisRaw("Axis 6");
		if (Input.GetKeyDown(KeyCode.Alpha0)|| dPad == 1.0f)
		{
			Debug.Log("dPad = " + dPad);
			//Debug.Log("SwitchPos = " + dPadUp);
			if (_hasRightHandObject)
				NotPossible();
			
			if(_daggerEquipped)
			{
				//anim.SetTrigger("putback");
				//anim.ResetTrigger("putback");
				anim.Play("Put_Back");
				Put_Weapon_Back();
			}

			
		}
		if(Input.GetKeyDown(KeyCode.Alpha1) || dPad == -1.0f)
		{
			//Debug.Log("SwitchNeg = " + dPadDown);
			Debug.Log("dPad = " + dPad);
			if (!_hasRightHandObject)
			{
				anim.Play("Grab_W");
				Grab_Weapon();
			}
			else
			{
				NotPossible();
			}
		}

		if (Input.GetKeyDown(KeyCode.V) /*|| Input.GetButtonDown("Stab") ||*/ /*Input.GetButtonDown("Fire1")*/)
		{
			if (_daggerEquipped)
				anim.Play("Stab");
			else
				NotPossible();


			if (_hasRightHandObject)
				NotPossible();
		}
		//IsInFront(_bt.transform.position);
		if (Input.GetKeyDown(KeyCode.O)||Input.GetButtonDown("Start"))
		{
			Cursor.visible = true;
			SceneManager.LoadScene(0);
		}

		if(Input.GetButton("rHand"))
		{
			if (_daggerEquipped)
				anim.Play("Stab");
			else
				anim.Play("RPunch");
		}
		if (Input.GetButton("lHand"))
		{
				anim.Play("LPunch");
		}
	}

	void StopJumping()
	{
		anim.SetBool("jumping", false);
	}
	//On frame 5 of the look animation the players hand will be by their belt
	void Grab_Mag()
	{
		Debug.Log("Grab_Mag");
		//Creates a variable based on the magnifying glass's current location
		//and reparents it to the hand
		var magTransform = mag.transform;
		magTransform.parent = rPalmSlot.transform;
		magTransform.localPosition = Vector3.zero;
		magTransform.localRotation = Quaternion.AngleAxis(270.0f, Vector3.forward);
		magTransform.localScale = Vector3.one;
			
	}
	//Animation event on frame 37 places magnifying glass object back
	void Slot_Mag()
	{
		Debug.Log("Slot_Mag");
		var magTransform = mag.transform;
		magTransform.parent = beltSlot.transform;
		magTransform.localPosition = Vector3.zero;
		magTransform.localRotation = Quaternion.identity;
		magTransform.localScale = Vector3.one;
	}
	void Pickup_Obj()
	{
		Debug.Log("Got a rock from the pile");
		_rockClone = Instantiate(_rock);
		_rockClone.GetComponent<Rigidbody>().useGravity = false;
		_rockClone.GetComponent<Rigidbody>().isKinematic = true;
		var _rc = _rockClone.transform;
		_rc.parent = rPalmSlot.transform;
		_rc.localPosition = Vector3.zero;
		_rc.localRotation = Quaternion.identity;
		_rc.localScale = new Vector3(.5f, .5f, .5f);
		//Player now has a rock on hand
		_hasRightHandObject = true;

		//Prevents Player from picking up second rock
		//Note: Theoretically I could add in a second animation that is 
		//a mirror image version of the right hand pick up (as in left hand pickup)
		//but I'm planning on leaving the left hand to primarily pickup and grip lanterns

	}
	void OnThrow()
	{
		//If player is holding rock they can throw it
		var _rc = _rockClone.transform;
		_rc.parent = null;
		Rigidbody rb = _rockClone.GetComponent<Rigidbody>();
		rb.useGravity = true;
		rb.isKinematic = false;
		rb.mass = _mass;
		Vector3 fwd = _rc.up;
		//rb.AddRelativeForce(transform.forward*_throwForce, ForceMode.Impulse);
		rb.AddForceAtPosition(transform.forward*_throwForce, _rc.position, ForceMode.Impulse);
		_hasRightHandObject = false;
		//If not the character will shake their head instead
	}
	//void OnTriggerEnter(Collider other)
	//{
	//	Debug.Log(other.gameObject.name);
	//	if (other.gameObject.tag == "Trap")
	//	{
	//	}
	//}
	void Grab_Weapon()
	{
		Debug.Log("Grab_Weapon");
		//Creates a variable based on the magnifying glass's current location
		//and reparents it to the hand
		var dagTransform = dagger.transform;
		dagTransform.parent = rPalmSlot.transform;
		dagTransform.localPosition = Vector3.zero;
		dagTransform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.forward);
		dagTransform.localScale = new Vector3(0.16f, 0.1f, 0.2f);
		_daggerEquipped = true;
		
	}
	void Put_Weapon_Back()
	{
		var dagTransform = dagger.transform;
		dagTransform.parent = beltSlot2.transform;
		dagTransform.localPosition = Vector3.zero;
		dagTransform.localRotation = Quaternion.AngleAxis(270.0f, Vector3.forward);
		dagTransform.localScale = new Vector3(0.16f, 0.1f, 0.2f);
		_daggerEquipped = false;
	}
	//void TakeLantern()
	//{
	//	_lanternClone = Instantiate(lantern);
	//	var lTransform = _lanternClone.transform;
	//	lTransform.parent = lPalmSlot.transform;
	//	lTransform.localPosition = Vector3.zero;
	//	lTransform.localRotation = Quaternion.Euler(180.0f, 45.0f, 0.0f);
	//	lTransform.localScale = Vector3.one;
	//	Rigidbody childRB = lTransform.GetComponentInChildren<Rigidbody>();
	//	childRB.isKinematic = true;
	//	childRB.useGravity = false;
	//	Rigidbody rb = lantern.GetComponent<Rigidbody>();
	//	rb.isKinematic = true;
	//	rb.useGravity = false;
	//	_hasLeftHandObject = true;
	//}
	//Animation of the player character shaking their head will play
	//if player attempts an action the character cannot do based on 
	//whether the player character is holding or not holding an item
	void NotPossible()
	{
		anim.Play("No");
	}
	//based on code from here https://forum.unity.com/threads/check-if-a-gameobject-is-in-front-of-my-player-character.166432/
	//Checks whether object is in view of the player
	bool IsInFront(Vector3 _targetPos)
	{
		Vector3 directionToTarget = transform.position - _targetPos;
		float angle = Vector3.Angle(transform.forward, directionToTarget);
		float distance = directionToTarget.magnitude;
		
		if (Mathf.Abs(angle) > 90 && distance < 10)
		{
			Debug.Log("target is in front of me");
			_isInView = true;
		}
		else
		{
			Debug.Log("Target is out of view");
			_isInView = false;
		}
		return _isInView;
	}

	void Foot0()
	{
		PlayAudio(_clip);
	}
	void Foot1()
	{
		PlayAudio(_clip);
	}
	void Foot2()
	{
		PlayAudio(_clip);
	}
	void BackUp0()
	{
		PlayAudio(_clip);
	}
	void BackUp1()
	{
		PlayAudio(_clip);
	}
	void BackUp2()
	{
		PlayAudio(_clip);
	}

	void PlayAudio(AudioClip _c)
	{
		_audio.clip = _c;
		_audio.Play();
	}
}

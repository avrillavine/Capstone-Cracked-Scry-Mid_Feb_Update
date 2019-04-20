using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class AnimManager : MonoBehaviour
{

	Animator anim;

	AudioSource _audio;
	public AudioClip no;


	//right palm bone slot
	public GameObject rPalmSlot;
	//flag to determine whether any object other than a dagger is already equipped
	public bool _hasRightHandObject = false;
	public bool _daggerEquipped = false;
	//left palm bone slot
	public GameObject lPalmSlot;
	public bool _hasLeftHandObject = false;
	public bool _carryingRock = false;

	//lower-spine bone slots
	public GameObject rBeltSlot;
	public GameObject lBeltSlot;
	//OBJECTS

	//magnifying glass
	public GameObject mag;

	//dagger
	public GameObject dag;

	//amount of force specified to throw objects
	public float _throwForce = 10.0f;

	//play sound once flag 
	public bool wasPlayed = false;

	//Pickupable items
	public GameObject _rock;
	GameObject _rockClone;


	// Start is called before the first frame update
	void Start()
    {
		anim = GetComponent<Animator>();

		_audio = GetComponent<AudioSource>();
		_audio.playOnAwake = false;

		_rockClone = new GameObject();
	}

    // Update is called once per frame
    void Update()
    {
		///----------
		/// MOVEMENT
		///----------
		//Activates Movement animation blend tree
		anim.SetFloat("vspeed", Input.GetAxis("Vertical"));
		//Activates Left-Right Movement
		anim.SetFloat("hspeed", Input.GetAxis("Horizontal"));
		//Jump -->Landing animation not implemented yet
		if (Input.GetButton("Jump"))
		{
			anim.SetBool("jumping", true);
			Invoke("StopJumping", 0.1f);
		}

		///-------------------------------
		/// ACTIONS USING ANIMATION EVENTS
		///-------------------------------
		if(_hasLeftHandObject)
		{
			anim.SetLayerWeight(anim.GetLayerIndex("LGrip"), 1.0f);
		}
		else
		{
			anim.SetLayerWeight(anim.GetLayerIndex("LGrip"), 0.0f);
		}
		if (_hasRightHandObject)
		{
			anim.SetLayerWeight(anim.GetLayerIndex("RGrip"), 1.0f);
		}
		else
		{
			anim.SetLayerWeight(1, 0.0f);
		}
		//Connects object carrying flags with flags for specific objects
		if(_daggerEquipped)
		{
			_hasRightHandObject = true;
		}
		else
		{
			_hasRightHandObject = false;
		}

		if(_carryingRock)
		{
			_hasLeftHandObject = true;
		}
		else
		{
			_hasLeftHandObject = false;
		}

		if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetButtonDown("rKick"))
		{
			anim.Play("RKick");
		}
		if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetButtonDown("lKick"))
		{
			anim.Play("LKick");
		}
		if (Input.GetKeyDown(KeyCode.H) || Input.GetButtonDown("Btn 0"))
		{
			if (!_hasRightHandObject)
			{
				anim.Play("Look");
			}
			else
			{
				NotPossible();

			}
		}
		if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Btn 2"))
		{
			if (!_carryingRock)
			{
				anim.Play("LPickUp");
			}
			else
			{
				NotPossible();
			}
		}

		//if (Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Btn 1"))
		//{
		//	if (_hasLeftHandObject)
		//	{
		//		anim.Play("LThrow");
		//	}
		//	else
		//	{
		//		NotPossible();
		//	}

		//}

		float dPad = Input.GetAxisRaw("Axis 6");
		if (Input.GetKeyDown(KeyCode.Alpha0) || dPad == 1.0f)
		{
			if (_daggerEquipped)
			{
				anim.Play("UnEquip_Dagger");
			}
			else
			{
				NotPossible();
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha1) || dPad == -1.0f)
		{
			if (!_daggerEquipped)
			{
				anim.Play("Equip_Dagger");
			}
			else
			{
				NotPossible();
			}
		}
		///-------------------
		/// ATTACK ANIMATIONS
		///-------------------
		if (Input.GetButton("rHand"))
		{
			if (_daggerEquipped)
				anim.Play("Stab");
			else
				anim.Play("RPunch");
		}
		if (Input.GetButton("lHand"))
		{
			if(_carryingRock)
				anim.Play("LThrow");
			else
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
		//Creates a variable based on the magnifying glass's current location
		//and reparents it to the hand
		var magTransform = mag.transform;
		magTransform.parent = rPalmSlot.transform;
		magTransform.localPosition = Vector3.zero;
		magTransform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(90.0f, Vector3.forward);
		magTransform.localScale = Vector3.one;

	}
	//Animation event on frame 37 places magnifying glass object back
	void Slot_Mag()
	{
		var magTransform = mag.transform;
		magTransform.parent = rBeltSlot.transform;
		magTransform.localPosition = Vector3.zero;
		magTransform.localRotation = Quaternion.identity;
		magTransform.localScale = Vector3.one;
	}

	void Grab_Weapon()
	{
		var dagTransform = dag.transform;
		dagTransform.parent = rPalmSlot.transform;
		dagTransform.localPosition = Vector3.zero;
		dagTransform.localRotation = Quaternion.AngleAxis(-95,Vector3.up); 
		dagTransform.localScale = Vector3.one;
		_daggerEquipped = true;
		dag.tag = "Dagger";
	}

	void Put_Weapon_Back()
	{
		Debug.Log("Stash Dagger");
		var dagTransform = dag.transform;
		dagTransform.parent = lBeltSlot.transform;
		dagTransform.localPosition = Vector3.zero;
		dagTransform.localRotation = Quaternion.AngleAxis(-100.0f, Vector3.right) * Quaternion.AngleAxis(-1.6f,Vector3.forward);
		dagTransform.localScale = Vector3.one;
		_daggerEquipped = false;
		dag.tag = "StashedDagger";
	}

	void LPick_Up()
	{
		_rockClone = Instantiate(_rock);
		_rockClone.GetComponent<Rigidbody>().useGravity = false;
		_rockClone.GetComponent<Rigidbody>().isKinematic = true;
		var _rc = _rockClone.transform;
		_rc.parent = lPalmSlot.transform;
		_rc.localPosition = Vector3.zero;
		_rc.localRotation = Quaternion.identity;
		_rc.localScale = new Vector3(.5f, .5f, .5f);
		_carryingRock = true;
	}
	void On_LThrow()
	{
		//If player is holding rock they can throw it
		var _rc = _rockClone.transform;
		_rc.parent = null;
		Rigidbody rb = _rockClone.GetComponent<Rigidbody>();
		rb.useGravity = true;
		rb.isKinematic = false;
		rb.mass = 1.0f;
		Vector3 fwd = _rc.up;
		//rb.AddRelativeForce(transform.forward*_throwForce, ForceMode.Impulse);
		rb.AddForceAtPosition(transform.forward * _throwForce, _rc.position, ForceMode.Impulse);
		_carryingRock = false;
	}
	/// -------------------------------------
	/// NON-ANIMATION EVENT RELATED FUNCTIONS
	/// -------------------------------------
	void NotPossible()
	{
		anim.Play("No");
		//ensures clip plays once 
		//TODO randomly activate sound
		if(!wasPlayed)
		{
			_audio.clip = no;
			_audio.Play();
			wasPlayed = true;
		}

	}

	///-----
	///Audio
	///-----
	void PlayClip(AudioClip _clip)
	{
		_audio.clip = _clip;
		_audio.Play();
	}
}

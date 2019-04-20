using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Part of this script was borrowed from https://allunityscripts.blogspot.com/2019/02/unity-3d-pickup-system-with-animation.html
public class PickUp : MonoBehaviour
{
	public Transform playerTransform;
	public Animator anim;
	GameObject rock;
	Rigidbody rb;
	public bool pickedUp = false;
	//bool ObjInHand = false;
	// Start is called before the first frame update
	void Start()
	{
		rock = GameObject.FindWithTag("Rock");
		rb = rock.GetComponent<Rigidbody>();
	}

	//// Update is called once per frame
	void Update()
	{
		if(!pickedUp)
		{
			anim.ResetTrigger("pickup");
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if(other.tag == "Rock")
		{
			//if(!ObjInHand)
			//{
			//	LPick_Up(other);
			//}
			//LPick_Up(other);
			if (Input.GetKeyDown(KeyCode.R) && !pickedUp)
			{
				//LPick_Up();
				playerTransform.LookAt(rock.transform, Vector3.up);
				pickedUp = true;
				StartCoroutine("PlayAnim");
			}
			else
			{
				anim.Play("No");
				pickedUp = false;
				//anim.SetLayerWeight(anim.GetLayerIndex("LGrip"), 0.0f);
			}
		}
	}
	//void LPick_Up()
	//{

	//}
	IEnumerator PlayAnim()
	{
		anim.SetTrigger("pickup");
		//Pause animation to allow pick to seem more realistic
		yield return new WaitForSeconds(1);
		rb.isKinematic = true;
		rb.useGravity = false;
		rock.transform.parent = transform;
		rock.transform.localPosition = transform.localPosition;
		rock.transform.localRotation = Quaternion.identity;
		rock.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
	}
		//void LPick_Up(Collider other)
		//{
		//	Debug.Log("Pick Up");
		//	other.attachedRigidbody.isKinematic = true;
		//	other.attachedRigidbody.useGravity = false;
		//	var _rc = other.transform;
		//	_rc.parent = transform;
		//	_rc.localPosition = Vector3.zero;
		//	_rc.localRotation = Quaternion.identity;
		//	_rc.localScale = new Vector3(.5f, .5f, .5f);
		//	//ObjInHand = true;
		//}
	}

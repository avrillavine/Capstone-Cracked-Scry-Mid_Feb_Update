using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CharacterController))]
public class CColliderHit : MonoBehaviour
{
	//CharacterController cc;
	// this script pushes all rigidbodies that the character touches
	float PushPower = 12.0f;
	//public float knockBack = 5.0f;
	private void Start()
	{
		//cc = GetComponent<CharacterController>();
	}
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody body = hit.collider.attachedRigidbody;

		// no rigidbody
		if (body == null || body.isKinematic) { return; }

		// We dont want to push objects below us
		if (hit.moveDirection.y < -0.3) { return; }

		// Calculate push direction from move direction,
		// we only push objects to the sides never up and down
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

		// If you know how fast your character is trying to move,
		// then you can also multiply the push velocity by that.

		// Apply the push
		body.velocity = pushDir * PushPower;

		//if(body.gameObject.tag == "Enemy")
		//{
		//	cc.Move(Vector3.back *knockBack);
		//	//Vector3 pushDirInv = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, hit.collider.transform.position.z);
		//	//Vector3 playerVelocity = hit.controller.velocity;
		//	//playerVelocity = pushDirInv * PushPower / 10.0f;
		//}
	}
}

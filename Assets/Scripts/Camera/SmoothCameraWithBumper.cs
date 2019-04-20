//(Created CSharp Version) 10/2010: Daniel P. Rossi (DR9885) 

using UnityEngine;
using System.Collections;

public class SmoothCameraWithBumper : MonoBehaviour
{
	[SerializeField] private Transform target = null;
	[SerializeField] private readonly float _distance = 3.0f;
	public float _height = 1.0f;
//	[SerializeField] private readonly float _height = 1.0f;
	[SerializeField] private readonly float _damping = 5.0f;
	[SerializeField] private readonly bool _smoothRotation = true;
	[SerializeField] private readonly float _rotationDamping = 10.0f;

	[SerializeField] private Vector3 targetLookAtOffset = Vector3.zero; // allows offsetting of camera lookAt, very useful for low bumper heights

	[SerializeField] private readonly float _bumperDistanceCheck = 2.5f; // length of bumper ray
	[SerializeField] private readonly float _bumperCameraHeight = 1.0f; // adjust camera height while bumping
	[SerializeField] private Vector3 bumperRayOffset = Vector3.zero; // allows offset of the bumper ray from target origin

	/// <Summary>
	/// If the target moves, the camera should child the target to allow for smoother movement. DR
	/// </Summary>
	private void Awake()
	{
		transform.parent = target; //used to be camera.transform.parent
	}

	private void FixedUpdate()
	{
		Vector3 wantedPosition = target.TransformPoint(0, _height, -_distance);

		// check to see if there is anything behind the target
		Vector3 back = target.transform.TransformDirection(-1 * Vector3.forward);

		// cast the bumper ray out from rear and check to see if there is anything behind
		if (Physics.Raycast(target.TransformPoint(bumperRayOffset), back, out RaycastHit hit, _bumperDistanceCheck)
			&& hit.transform != target) // ignore ray-casts that hit the user. DR
		{
			// clamp wanted position to hit position
			wantedPosition.x = hit.point.x;
			wantedPosition.z = hit.point.z;
			wantedPosition.y = Mathf.Lerp(hit.point.y + _bumperCameraHeight, wantedPosition.y, Time.deltaTime * _damping);
		}

		transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * _damping);

		Vector3 lookPosition = target.TransformPoint(targetLookAtOffset);

		if (_smoothRotation)
		{
			Quaternion wantedRotation = Quaternion.LookRotation(lookPosition - transform.position, target.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * _rotationDamping);
		}
		else
			transform.rotation = Quaternion.LookRotation(lookPosition - transform.position, target.up);
	}
}
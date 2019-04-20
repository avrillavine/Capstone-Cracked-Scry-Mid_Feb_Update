using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Scry2 : MonoBehaviour
{

	//public Canvas canvas;

	public bool _isInView;
	//Timer Values
	public float max = 1.1f;
	public float min = 0.0f;
	public float rMax = 1.1f;
	public float rMin = 0;
	public float rAmount = 1.1f; // the total
	float _changePerSecond; // modify the total, every second
	public float _timeToChange = 4.0f; // the total time myValue will take to go from max to min
	public bool _startMaterializing;
	//Timer Values
	public float vMax = 1.1f;
	public float vMin = 0;
	public float vAmount = 0f; // the total
	float _timePerSecond; // modify the total, every second
	public float _rate = 4.0f; // the total time myValue will take to go from max to min
	public bool _startDissolving;
	//public bool trapInSight;
	//public bool _trapInProcess;
	//public bool _effectInProcess;
	bool isTrap;
	public Vector3 currentPosition;
	GameObject _gameObject;
	//public bool enemyInSight = false;
	//public bool _reveal = false;
	// Update is called once per frame
	bool tempIsInView = false;
	private GameObject[] _tagged;

	bool isInViewA = false;
	bool isInViewB = false;

	//bool isCurrent = false;
	public int currentVal = 0;
	void Start()
	{
		CheckTags("Trap");
		CheckTags("Enemy");
	}
	void Update()
    {

		//Based off code from here https://gamedev.stackexchange.com/questions/121749/how-do-i-change-a-value-gradually-over-time
		//The whole idea is that the flags for keeping objects visible and enemies invisible is set to false 
		//Until the player sets off the flag
		// modify at a constant rate, keep within bounds

		if (Input.GetKeyDown(KeyCode.H) || Input.GetButtonDown("Btn 0"))
		{
			//IsTagInFront("Trap", "Shader Graphs/Transparent", "Shader Graphs/Reveal");
			//if (trapInSight)
			//	_startMaterializing = true;
			//else
			//	_startMaterializing = false;
			//CheckCurrentPosition();
			//IsTagInFront("Trap");
			//if (_isInView)
			//	_startMaterializing = true;
			//else
			//	_startMaterializing = false;

			//IsTagInFront("Enemy");
			//if (_isInView)
			//	_startDissolving = true;
			//else
			//	_startDissolving = false;
			
		}

		if(Input.GetKeyDown(KeyCode.G))
		{
			// DEBUG KEY PRESS
			//Testing to establish if shader is in process
			//_startMaterializing = true;
			//CallObjectByTag("Trap", "Shader Graphs/Transparent", "Shader Graphs/Reveal", _startMaterializing);
			//Assigns current position to currentPosition variable
			//CheckCurrentPosition();
			//Debug.Log("Current position of Player is " + currentPosition);
			IsTagAInFront("Enemy");
			if (isInViewA)
				_startDissolving = true;
			else
				_startDissolving = false;


			//IsTagBInFront("Trap");
			//if (isInViewB)
			//	_startMaterializing = true;
			//else
			//	_startMaterializing = false;

		}

		if(Input.GetKeyDown(KeyCode.J))
		{
			IsTagAInFront("Enemy");
			//TagInfo("Enemy");
			//Debug.Log("Current Value is " + currentVal);
			//if (isCurrent)
			//	_startDissolving = true;
			//else
			//	_startDissolving = false;
		}
		Reveal();
		//CheckState();
		Vanish();

	}
	
	void CheckTags(string _tag)
	{
		GameObject[] _tagArray= GameObject.FindGameObjectsWithTag(_tag);
		int numOfObjs = _tagArray.Length;
		if(numOfObjs == 1)
		{
			Debug.Log("There is " + numOfObjs + " " + _tag + " in this scene.");
		}
		if(numOfObjs > 1)
		{
			if(_tag.EndsWith("y"))
			{
				Debug.Log("There are " + numOfObjs + " " + _tag.Replace("y","ie") + "s in this scene.");
			}
			else
			{
				Debug.Log("There are " + numOfObjs + " " + _tag + "'s in this scene.");
			}
			
		}
		if(numOfObjs == 0)
		{
			Debug.Log(_tag + " does not exist in scene.");
		}
	}

	bool Reveal()
	{
		rAmount = Mathf.Clamp(rAmount + _changePerSecond * Time.deltaTime, rMin, rMax);
		Shader.SetGlobalFloat("_rAmount", rAmount);
		//If player sets boolean to true, start making the object reappear 
		if (_startMaterializing)
		{
			_changePerSecond = (rMin - rMax) / _timeToChange;
		}
		else
		{
			_changePerSecond = (rMin + rMax) / _timeToChange;
		}
		if (rAmount == rMin)
		{
			_startMaterializing = false;
		}
		return _startMaterializing;
	}
	bool Vanish()
	{
		vAmount = Mathf.Clamp(vAmount + _timePerSecond * Time.deltaTime, vMin, vMax);
		Shader.SetGlobalFloat("_vAmount", vAmount);
		//If player sets boolean to true, start making the object reappear 
		if (_startDissolving)
		{
			_timePerSecond = (vMin + vMax) / _rate;
		}
		else
		{
			_timePerSecond = (vMin - vMax) / _rate;
		}
		if (vAmount == vMax)
		{
			_startDissolving = false;
		}
		return _startDissolving;
	}
	Vector3 CheckCurrentPosition()
	{
		currentPosition = transform.position;
		return currentPosition;
	}

	bool ListTagLocation(string _tag)
	{
		GameObject[] _tagArray = GameObject.FindGameObjectsWithTag(_tag);
		int numOfObjs = _tagArray.Length;
		for (int i = 0; i < numOfObjs; i++)
		{
			Vector3 _targetPos = _tagArray[i].transform.position;
			Vector3 directionToTarget = transform.position - _targetPos;
			float angle = Vector3.Angle(transform.forward, directionToTarget);
			float distance = directionToTarget.magnitude;
			var st = _tagArray[i].ToString();
			if (Mathf.Abs(angle) > 90 && distance < 20 )//original (Mathf.Abs(angle) > 90 && distance < 10)
			{
				Debug.DrawRay(_targetPos, directionToTarget, Color.blue, 2.0f);
				//Debug.Log("target is in front of me");
				Debug.Log(st + "is in view");
				tempIsInView = true;
			}
			else
			{
				Debug.DrawRay(_targetPos, Vector3.Reflect(directionToTarget,directionToTarget.normalized),Color.green,2.0f);
				Debug.DrawRay(_targetPos, directionToTarget, Color.yellow, 2.0f);
				//Debug.Log("Target is out of view");
				Debug.Log(st + "is out of view");
				tempIsInView = false;
			}
			
		}
		return tempIsInView;
	}

	void TagInfo(string tag)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
		int temp = 0;
		int past = 0;
		for(int i = 0; i < array.Length; i++)
		{
			Vector3 target = array[i].transform.position;
			Vector3 directionToTarget = transform.position - target;
			float angle = Vector3.Angle(transform.forward, directionToTarget);
			float distance = directionToTarget.magnitude;
			var st = array[i].name;
			if (Mathf.Abs(angle) > 90 && distance < 20)//original (Mathf.Abs(angle) > 90 && distance < 10)
			{
				Debug.DrawRay(target, directionToTarget, Color.green, 2.0f);
				//Debug.Log("target is in front of me");
				Debug.Log(st + " is in view and its value is " + i);
				temp = i;
			}
			else
			{
				past = array.Length.CompareTo(i);
				Debug.Log("past value is " + past);
			}
		}
		currentVal = temp;
	}
	void CheckVal(string tag)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(tag);

		//for (int i = currentVal; i < array.Length; i++)
		//{

		//}
		
	}
	bool IsTagAInFront(string tag)
	{
		GameObject target = GameObject.FindGameObjectWithTag(tag);
		Vector3 targetPos = target.transform.position;
		Vector3 directionToTarget = transform.position - targetPos;
		float angle = Vector3.Angle(transform.forward, directionToTarget);
		float distance = directionToTarget.magnitude;
		if (Mathf.Abs(angle) > 90 && distance < 20)//original (Mathf.Abs(angle) > 90 && distance < 10)
		{
			Debug.DrawRay(targetPos, directionToTarget, Color.green, 2.0f);
			Debug.Log(target.name + " is in view");
			isInViewA = true;
		}
		else
		{
			Debug.DrawRay(targetPos, directionToTarget, Color.red, 2.0f);
			Debug.Log(target.name + " is out of view");
			isInViewA = false;
		}
		return isInViewA;
	}
	//bool IsTagAInFront(string tag)
	//{
	//	GameObject[] _tagArray = GameObject.FindGameObjectsWithTag(tag);
	//	int numOfObjs = _tagArray.Length;
	//	for (int i = 0; i < numOfObjs; i++)
	//	{
	//		Vector3 _targetPos = _tagArray[i].transform.position;
	//		Vector3 directionToTarget = transform.position - _targetPos;
	//		float angle = Vector3.Angle(transform.forward, directionToTarget);
	//		float distance = directionToTarget.magnitude;
	//		var st = _tagArray[i].name;
	//		if (Mathf.Abs(angle) > 90 && distance < 20)//original (Mathf.Abs(angle) > 90 && distance < 10)
	//		{
	//			Debug.DrawRay(_targetPos, directionToTarget, Color.green, 2.0f);
	//			//Debug.Log("target is in front of me");
	//			Debug.Log(st + " is in view and its value is " + i );
	//			isInViewA = true;
	//		}
	//		else
	//		{
	//			//Debug.DrawRay(_targetPos, Vector3.Reflect(directionToTarget, directionToTarget.normalized), Color.cyan, 2.0f);
	//			//Debug.DrawRay(_targetPos, Vector3.Reflect(directionToTarget, -directionToTarget.normalized), Color.cyan, 2.0f);
	//			Debug.DrawRay(_targetPos, directionToTarget, Color.red, 2.0f);
	//			//Debug.Log("Target is out of view");
	//			Debug.Log(st + " is out of view and its value is " + i);
	//			isInViewA = false;
	//		}
	//	}
	//	return isInViewA;
	//}
	bool IsTagBInFront(string tag)
	{
		GameObject[] _tagArray = GameObject.FindGameObjectsWithTag(tag);
		int numOfObjs = _tagArray.Length;
		for (int i = 0; i < numOfObjs; i++)
		{
			Vector3 _targetPos = _tagArray[i].transform.position;
			Vector3 directionToTarget = transform.position - _targetPos;
			float angle = Vector3.Angle(transform.forward, directionToTarget);
			float distance = directionToTarget.magnitude;
			var st = _tagArray[i].name;
			if (Mathf.Abs(angle) > 90 && distance < 20)//original (Mathf.Abs(angle) > 90 && distance < 10)
			{
				Debug.DrawRay(_targetPos, directionToTarget, Color.green, 2.0f);
				//Debug.Log("target is in front of me");
				Debug.Log(st + " is in view and its value is " + _tagArray.GetValue(i));
				isInViewB = true;
			}
			else
			{
				Debug.DrawRay(_targetPos, Vector3.Reflect(directionToTarget, directionToTarget.normalized), Color.yellow, 2.0f);
				Debug.DrawRay(_targetPos, directionToTarget, Color.red, 2.0f);
				//Debug.Log("Target is out of view");
				Debug.Log(st + " is out of view and its value is " + _tagArray.GetValue(i));
				isInViewB = false;
			}

		}
		return isInViewB;
	}
	bool IsTagInFront(string _tag)
	{
		GameObject _gameObject = HasTag(_tag)/*GameObject.FindGameObjectWithTag(_tag)*/;
		Vector3 _targetPos = _gameObject.transform.position;
		Vector3 _tempPos = currentPosition;
		Vector3 directionToTarget = _tempPos - _targetPos;
		Debug.DrawRay(_targetPos, directionToTarget, Color.red, 2.0f);
		float angle = Vector3.Angle(transform.forward, directionToTarget);


		float distance = directionToTarget.magnitude;

		if (Mathf.Abs(angle) > 90 && distance < 20)//original (Mathf.Abs(angle) > 90 && distance < 10)
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

	GameObject HasTag(string _tag)
	{
		_gameObject = GameObject.FindGameObjectWithTag(_tag);
		return _gameObject;
	}
	//bool IsTagInFront(string _tag)
	//{
	//	GameObject _gameObject = GameObject.FindGameObjectWithTag(_tag);
	//	Vector3 _targetPos = _gameObject.transform.position;
	//	Vector3 _tempPos = transform.position;
	//	Vector3 directionToTarget = transform.position - _targetPos;
	//	Debug.DrawRay(_targetPos, directionToTarget, Color.red, 2.0f);
	//	float angle = Vector3.Angle(transform.forward, directionToTarget);


	//	float distance = directionToTarget.magnitude;

	//	if (Mathf.Abs(angle) > 90 && distance < 20)//original (Mathf.Abs(angle) > 90 && distance < 10)
	//	{
	//		Debug.Log("target is in front of me");
	//		_isInView = true;
	//	}
	//	else
	//	{
	//		Debug.Log("Target is out of view");
	//		_isInView = false;
	//	}
	//	return _isInView;
	//}
}

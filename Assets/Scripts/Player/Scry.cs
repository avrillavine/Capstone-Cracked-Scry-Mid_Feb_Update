using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scry : MonoBehaviour
{
	public AnimManager anim;
	//Lists for defining trap and enemy objects 
	List<GameObject> traps;
	GameObject[] trap;

	List<GameObject> enemies;
	GameObject[] enemy;

	//Timer Values for Shader effect
	public float max = 1.1f; // Fully Transparent
	public float min = 0.0f; // Opaque

	/// 
	/// TRAP VALUES
	///
	//Based off code from here https://gamedev.stackexchange.com/questions/121749/how-do-i-change-a-value-gradually-over-time
	//The whole idea is that the flags for keeping objects visible and enemies invisible is set to false 
	//Until the player sets off the flag
	//Trap Specific Timer Values
	public float rAmount = 1.1f; //Default Amount
	public float rRate = 4.0f;
	public bool materialize; //Effect trigger flag
	public bool trapSpotted;
	float revealTime; // modify the total, every second
    float timePerSecond; // modify the total, every second
	/// 
	/// ENEMY VALUES
	///
	//Enemy Specific Timer Values
	public float vAmount = 0.0f; //Default Amount
	public float vRate = 4.0f;
	public bool dissolve; //Effect trigger flag
	public bool enemySpotted;
	float vanishTime; // modify the total, every second
	int currentTrap;
	int trapInView;
	// Start is called before the first frame update
	void Start()
    {
		DefineTraps();

		//enemies = new List<GameObject>();
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.H) || Input.GetButtonDown("Btn 0"))
		{
			//TaggedTrapInView();
			//Debug.Log(traps[currentTrap].name);
			//if (trapSpotted)
			//	materialize = true;
			//else
			//	materialize = false;
			if(!anim._hasRightHandObject)
			{
				materialize = true;
				dissolve = true;
			}
			else
			{
				materialize = false;
				dissolve = false;
			}
		}

		Reveal();
		Vanish();
	}

	void DefineTraps()
	{
		traps = new List<GameObject>();
		trap = GameObject.FindGameObjectsWithTag("Trap");
		traps.AddRange(trap);
		string trapList = traps.ToString();

		for(int i = 0; i < traps.Count; i++)
		{
			Debug.Log(traps[i].name + " is " + i);
		}
	}

	bool Reveal()
	{

		rAmount = Mathf.Clamp(rAmount + revealTime * Time.deltaTime, min, max);
		Shader.SetGlobalFloat("_rAmount", rAmount); //Affects Reveal Shader
		//If player sets boolean to true, start making the object reappear 
		if (materialize)
		{
			revealTime = (min - max) / rRate;
		}
		else
		{
			revealTime = (min + max) / rRate;
		}
		if (rAmount == min)
		{
			materialize = false;
		}
		return materialize;
	}
	bool Vanish()
	{

		vAmount = Mathf.Clamp(vAmount + vanishTime * Time.deltaTime, min, max);
		Shader.SetGlobalFloat("_vAmount", vAmount); //Affects Vanish Shader
		//If player sets boolean to true, start making the object reappear 
		if (dissolve)
		{
			vanishTime = (min + max) / vRate;
		}
		else
		{
			vanishTime = (min - max) / vRate;
		}
		if (vAmount == max)
		{
			dissolve = false;
		}
		return dissolve;
	}
	bool IsEnemyInView()
	{
		//traps.
		//Vector3 targetPos = target.transform.position;
		//Vector3 directionToTarget = transform.position - targetPos;
		//float angle = Vector3.Angle(transform.forward, directionToTarget);
		//float distance = directionToTarget.magnitude;
		//if (Mathf.Abs(angle) > 90 && distance < 20)//original (Mathf.Abs(angle) > 90 && distance < 10)
		//{
		//	Debug.DrawRay(targetPos, directionToTarget, Color.green, 2.0f);
		//	Debug.Log(target.name + " is in view");
		//	isInViewA = true;
		//}
		//else
		//{
		//	Debug.DrawRay(targetPos, directionToTarget, Color.red, 2.0f);
		//	Debug.Log(target.name + " is out of view");
		//	isInViewA = false;
		//}
		//return isInViewA;
		//based on code borrowed from here https://answers.unity.com/questions/253606/find-the-closest-target.html
		//if (Vector3.Distance(target.position, thisT.position) > Vector3.Distance(thing2.position, thisT.position))
		//{
		//	target = thing2;
		//	// remove thing2
		//}
		//else
		//{
		//	// remove thing2
		//}


		return enemySpotted;
	}

	void TrapsClosestToYou()
	{
		
		for(int i = 0;i < traps.Count; i++)
		{
			float distance = Vector3.Distance(traps[i].transform.position, transform.position);
			Debug.Log(traps[i].name + " is located " + distance + "units away");
			//Debug.Log(traps[i].name + " is " + tra);
			if(distance < 8 )
			{
				currentTrap = i;
			}
		}

	}
	void TaggedTrapInView()
	{
		TrapsClosestToYou();
		for (int i = currentTrap; i < traps.Count; i++)
		{
			Vector3 target = traps[i].transform.position;
			Vector3 directionToTarget = transform.position - target;
			float angle = Vector3.Angle(transform.forward, directionToTarget);
			float distance = directionToTarget.magnitude;
			var st = traps[i].name;
			if (Mathf.Abs(angle) > 90 && distance < 20)//original (Mathf.Abs(angle) > 90 && distance < 10)
			{
				Debug.DrawRay(target, directionToTarget, Color.green, 2.0f);
				//Debug.Log("target is in front of me");
				Debug.Log(st + " is in view and its value is " + i);
				//currentTrap = i;
			}
		}
		//return currentTrap;
	}
	//bool TriggerReveal()
	//{
	//	TaggedTrapInView();

	//	for(int i = currentTrap; i < traps.Count; i++)
	//	{
	//		if(currentTrap == trapInView)
	//		{
	//			trapSpotted = true;
	//		}
	//		else if (currentTrap != trapInView)
	//		{
	//			trapSpotted = false;
	//		}
	//	}
	//	return trapSpotted;
	//}
}

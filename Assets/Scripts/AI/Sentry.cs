using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SkeletonScript))]
public class Sentry : MonoBehaviour
{

	SkeletonScript _ss;
    // Sentry moving speed
    public int speed = 5;
	Animator anim;
    NavMeshAgent agent;

    // waypoints to patrol
    public List<Vector3> waypoints;
    int curWaypointIndex = -1;

    public enum State { PATROL, CHASE, TRACK, HIT, ATTACK } // for FSM
    State state;

    // target object
    public GameObject player;

    // last position where the player was seen
    Vector3 lastPlayerPos;
	//public Renderer _renderer;

	// Use this for initialization
	void Start()
    {
		_ss = GetComponent<SkeletonScript>();
		anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();        
        state = State.PATROL; // Initial state

    }

    Vector3 GetNextWaypoint()
    {
        if (waypoints.Count < 2)
            return transform.position;

        curWaypointIndex++;
        if (curWaypointIndex >= waypoints.Count)
        {
            curWaypointIndex = 0;
        }

        return waypoints[curWaypointIndex];
    }

    // Update is called once per frame
    void Update()
    {
		//Debug.Log("Skeleton state is " + state);
		Shader.SetGlobalColor("_ecolor", Color.white);
		switch (state)
        {
            case State.PATROL:                
                {
					//Shader emission manually added in color before making private A7FFAF
					//Shader.SetGlobalColor("_ecolor", Color.green * Color.white);
					//  GetComponent<Renderer>().material.color = Color.green;

					anim.SetBool("IsWalking", true);
					Vector3 dir = player.transform.position - transform.position;

					RaycastHit hit;
					if (Physics.Raycast(transform.position, dir.normalized, out hit))
					{
						if (hit.collider.gameObject == player)
						{
							// To do: Change state to State.CHASE

							state = State.CHASE;
							// To do: Set the agent's destination to the position of the player character
							agent.SetDestination(player.transform.position);

							// To do : uncomment the following two lines
							lastPlayerPos = player.transform.position;
							break;
						}
					}

					float distToWaypoint = agent.remainingDistance;
                    if (Mathf.Approximately(distToWaypoint, 0))
                    {                        
                        agent.SetDestination(GetNextWaypoint());
                    }
					
					if (_ss.isStruck)
					{
						state = State.HIT;
					}
                }
                break;
            case State.CHASE:
                {
					// GetComponent<Renderer>().material.color = Color.red;
					Debug.DrawLine(transform.position, player.transform.position,Color.red);
					//Shader.SetGlobalColor("_ecolor", Color.red * Color.white);
					Vector3 dir = player.transform.position - transform.position;
                                                            
                    RaycastHit hit;                    
                    if (Physics.Raycast(transform.position, dir.normalized, out hit))
                    {
                        if (hit.collider.gameObject == player)
                        {
							// To do: Set the agent's destination to the position of the player and update lastPlayerPos
							agent.SetDestination(player.transform.position);
							lastPlayerPos=player.transform.position;
							break;
                        }
                    }
					
					// To do: Change state to State.TRACK
					//        Set the agent's destination to the last seen position of the player
					state=State.TRACK;
					agent.SetDestination(lastPlayerPos);
				}
                break;
            case State.TRACK:                
                {
					// GetComponent<Renderer>().material.color = Color.yellow;
					//Shader.SetGlobalColor("_ecolor", Color.yellow*Color.white);
					// To do: Track the last seen position of the player.
					//        While tracking if the player is visible then start to chase
					//        If the player is not detected, then start to patrol
					agent.SetDestination(lastPlayerPos);
					Vector3 dir = player.transform.position - transform.position;

					RaycastHit hit;
					if (Physics.Raycast(transform.position, dir.normalized, out hit))
					{
						if (hit.collider.gameObject == player)
						{
							state = State.CHASE;
							agent.SetDestination(player.transform.position);
							lastPlayerPos = player.transform.position;
							break;
						}
					}
					float targetDist = agent.remainingDistance;
					if(Mathf.Approximately(targetDist,0))
					{
						state=State.PATROL;
						agent.SetDestination(GetNextWaypoint());
						break;
					}

				}
                break;
			case State.HIT:
				{
					StartCoroutine(Stunned());

				}
				break;
			case State.ATTACK:
				{
					StartCoroutine(Attack());
				}
				break;
		}
    }
	private IEnumerator Stunned()
	{
		//Wait for 14 secs.
		yield return new WaitForSeconds(1);

		//Turn My game object that is set to false(off) to True(on).
		//_ss.isStruck = true;
		agent.isStopped = true;
		//objectToActivate.SetActive(true);

		//Turn the Game Oject back off after 1 sec.
		yield return new WaitForSeconds(2);
		anim.SetBool("IsWalking", false);
		//anim.SetTrigger("hit");
		agent.isStopped = false;
		_ss.isStruck = false;
		state = State.CHASE;
		//state = State.PATROL;
		//Game object will turn off
		//objectToActivate.SetActive(false);
	}
	private IEnumerator Attack()
	{
		//Wait for 14 secs.
		yield return new WaitForSeconds(1);

		//Turn My game object that is set to false(off) to True(on).
		//_ss.isStruck = true;
		agent.isStopped = true;
		//objectToActivate.SetActive(true);
		anim.SetBool("IsWalking", false);
		anim.SetTrigger("attack");
		//state = State.ATTACK;
		//Turn the Game Oject back off after 1 sec.
		yield return new WaitForSeconds(2);
		anim.SetBool("IsWalking", true);
		//anim.SetTrigger("hit");
		agent.isStopped = false;
		state = State.TRACK;
		//state = State.PATROL;
		//Game object will turn off
		//objectToActivate.SetActive(false);
	}
}
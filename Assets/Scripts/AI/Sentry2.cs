using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Sentry2 : MonoBehaviour
{
	// Sentry moving speed
	public int speed = 5;

	NavMeshAgent agent;

	// waypoints to patrol
	public List<GameObject> waypoints;
	//public List<Vector3> waypoints;
	int curWaypointIndex = -1;

	public enum State { PATROL, CHASE, TRACK } // for FSM
	State state;

	// target object
	public GameObject player;

	// last position where the player was seen
	Vector3 lastPlayerPos;

	// Use this for initialization
	void Start()
	{
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

		return waypoints[curWaypointIndex].transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		switch (state)
		{
			case State.PATROL:
				{
					GetComponent<Renderer>().material.color = Color.green;
					Debug.DrawLine(transform.position, player.transform.position, Color.blue);
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
				}
				break;
			case State.CHASE:
				{
					GetComponent<Renderer>().material.color = Color.red;
					Debug.DrawLine(transform.position, player.transform.position, Color.red);

					Vector3 dir = player.transform.position - transform.position;

					RaycastHit hit;
					if (Physics.Raycast(transform.position, dir.normalized, out hit))
					{
						if (hit.collider.gameObject == player)
						{
							// To do: Set the agent's destination to the position of the player and update lastPlayerPos
							agent.SetDestination(player.transform.position);
							lastPlayerPos = player.transform.position;
							break;
						}
					}

					// To do: Change state to State.TRACK
					//        Set the agent's destination to the last seen position of the player
					state = State.TRACK;
					agent.SetDestination(lastPlayerPos);
				}
				break;
			case State.TRACK:
				{
					GetComponent<Renderer>().material.color = Color.yellow;

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
					if (Mathf.Approximately(targetDist, 0))
					{
						state = State.PATROL;
						agent.SetDestination(GetNextWaypoint());
						break;
					}

				}
				break;
		}
	}
}

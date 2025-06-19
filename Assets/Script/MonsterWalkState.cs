using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterWalkState : StateMachineBehaviour
{
    float timer;
    public float walkingTime = 10f;

    Transform player;
    NavMeshAgent agent;
    public float detectionArea = 18f;
    public float walkSpeed = 2f;

    List<Transform> waypointList = new List<Transform>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
        timer = 0;

        GameObject waypointsCluster = animator.GetComponent<MonsterWaypoints>().WaypointCluster;
        foreach (Transform t in waypointsCluster.transform)
        {
            waypointList.Add(t);
        }

        if (waypointList.Count > 0)
        {
            agent.SetDestination(waypointList[Random.Range(0, waypointList.Count)].position);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Vector3 nextPos = waypointList[Random.Range(0, waypointList.Count)].position;
            agent.SetDestination(nextPos);
        }

        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < detectionArea)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isChasing", true);
        }
    }
}
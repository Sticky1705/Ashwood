using UnityEngine;
using UnityEngine.AI;

public class MonsterIdleState : StateMachineBehaviour
{
    float timer;
    public float idleTime = 2f;

    Transform player;
    public float detectionAreaRadius = 18f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer > idleTime)
        {
            animator.SetBool("isWalking", true);
        }

        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < detectionAreaRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }
}
using UnityEngine;
using UnityEngine.AI;

public class MonsterAttackState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;

    public float stopAttackingDistance = 1f;
    public float attackRate = 1f;
    private float attackTimer;
    public int damageInflict = 1;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        attackTimer = 0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(player.position, animator.transform.position);
        Vector3 directionToPlayer = (player.position - animator.transform.position).normalized;
        float angleToPlayer = Vector3.Angle(animator.transform.forward, directionToPlayer);

        bool isInFront = angleToPlayer < 60f;
        bool isAtGoodDistance = distance >= 1.5f && distance <= stopAttackingDistance;

        if (isInFront && isAtGoodDistance)
        {
            attackTimer -= Time.deltaTime;

            if (isInFront && isAtGoodDistance && attackTimer <= 0f)
            {
                PlayerState.Instance.TakeDamage(damageInflict);
                attackTimer = 1f / attackRate;
            }
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        direction.y = 0f;
        if (direction != Vector3.zero)
            agent.transform.rotation = Quaternion.LookRotation(direction);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.isStopped = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomPatrolNode : Node
{
    private NavMeshAgent agent;
    private EnemyAI ai;
    private Vector3 targetPosition;
    private float stoppingDistance;

    public RandomPatrolNode(NavMeshAgent agent, EnemyAI ai, float stoppingDistance)
    {
        this.agent = agent;
        this.ai = ai;
        this.stoppingDistance = stoppingDistance;
    }

    public override NodeState Evaluate()
    {
        if (targetPosition == Vector3.zero)
        {
            targetPosition = ai.transform.position;
        }

        if (agent.remainingDistance <= stoppingDistance && !agent.pathPending)
        {
            float radius = 10f;
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
            randomDirection += ai.transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, radius, 1);
            targetPosition = hit.position;

            return NodeState.SUCCESS;
        }

        agent.SetDestination(targetPosition);

        return NodeState.RUNNING;
    }
}


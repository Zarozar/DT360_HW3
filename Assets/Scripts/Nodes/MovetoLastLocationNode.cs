using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToLastKnownPositionNode : Node
{
    private NavMeshAgent agent;
    private EnemyAI ai;

    public MoveToLastKnownPositionNode(NavMeshAgent agent, EnemyAI ai)
    {
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        ai.SetColor(Color.white);
        if (ai.lastKnownPlayerPosition == null)
        {
            return NodeState.FAILURE;
        }

        Vector3 lastKnownPosition = ai.lastKnownPlayerPosition.position;
        agent.SetDestination(lastKnownPosition);


        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            ai.lastKnownPlayerPosition = null;

            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }
}


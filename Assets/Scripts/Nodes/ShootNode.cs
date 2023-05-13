using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class ShootNode : Node
{
    private NavMeshAgent agent;
    private EnemyAI ai;
    private Transform target;

    private Vector3 currentVelocity;
    private float smoothDamp;

    private int shotsFired = 0;
    private float timeBetweenShots = 1f;
    private float shotTimer = 0f;
    public ShootNode(NavMeshAgent agent, EnemyAI ai, Transform target)
    {
        this.agent = agent;
        this.ai = ai;
        this.target = target;
        smoothDamp = 1f;
    }

    public override NodeState Evaluate()
    {
        agent.isStopped = true;
        ai.SetColor(Color.green);
        Vector3 direction = target.position - ai.transform.position;

        Vector3 currentDirection = Vector3.SmoothDamp(ai.transform.forward, direction, ref currentVelocity, smoothDamp);
        Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
        ai.transform.rotation = rotation;

        shotTimer += Time.deltaTime;

        if (shotTimer >= timeBetweenShots && shotsFired < 3)
        {
            shotTimer = 0f;
            shotsFired++;

            if (shotsFired == 3)
            {
                return NodeState.SUCCESS;
            }
        }

        return NodeState.RUNNING;
    }

}

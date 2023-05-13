using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsRecentlySeenPlayerNode : Node
{
    private float recentTime;
    private float lastSeenTime;
    private float range;
    private Transform target;
    private Transform origin;
    private EnemyAI ai;

    public IsRecentlySeenPlayerNode(float recentTime,float range, Transform target, Transform origin,EnemyAI ai)
    {
        this.ai = ai;
        this.range = range;
        this.recentTime = recentTime;
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(target.position, origin.position);
        if (distance <= range)
        {
            lastSeenTime = Time.time;
            ai.lastKnownPlayerPosition = target;
        }
        return Time.time - lastSeenTime <= recentTime ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}

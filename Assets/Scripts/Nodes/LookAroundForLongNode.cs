using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LookAroundForTooLongNode : Node
{
    private float lookDuration;
    private float elapsedTime;
    private Transform transform;
    private EnemyAI ai;

    public LookAroundForTooLongNode(float lookDuration, Transform transform, EnemyAI ai)
    {
        this.lookDuration = lookDuration;
        this.transform = transform;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        ai.SetColor(Color.grey);
        elapsedTime += Time.deltaTime;

        if (elapsedTime < lookDuration)
        {
            transform.Rotate(0, Random.Range(0, 360) * (Time.deltaTime/10), 0);
            return NodeState.RUNNING;
        }

        elapsedTime = 0;
        return NodeState.SUCCESS;
    }
}

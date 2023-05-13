using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsSeePlayerNode : Node
{
    private EnemyAI ai;
    private Transform playerTransform;

    public IsSeePlayerNode(EnemyAI ai, Transform playerTransform)
    {
        this.ai = ai;
        this.playerTransform = playerTransform;
    }

    public override NodeState Evaluate()
    {
        Vector3 direction = playerTransform.position - ai.transform.position;
        float distance = Vector3.Distance(ai.transform.position, playerTransform.position);

        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Player");
        if (Physics.Raycast(ai.transform.position, direction.normalized, out hit, distance, mask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return NodeState.SUCCESS;
            }
        }

        return NodeState.FAILURE;
    }
}

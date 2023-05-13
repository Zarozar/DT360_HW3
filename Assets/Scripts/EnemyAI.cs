using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private float lowHealthThreshold;
    [SerializeField] private float healthRestoreRate;

    [SerializeField] private float chasingRange;
    [SerializeField] private float shootingRange;

  
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform planeTransform;
    [SerializeField] private Cover[] avaliableCovers;
    public Transform lastKnownPlayerPosition;


    private Material material;
    private Transform bestCoverSpot;
    private NavMeshAgent agent;

    private Node topNode;

    private float _currentHealth;
	public float currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        material = GetComponentInChildren<MeshRenderer>().material;
        planeTransform = GameObject.Find("Plane").GetComponent<Transform>();

    }

    private void Start()
    {
        _currentHealth = startingHealth;
        ConstructBehahaviourTree();
    }

    private void ConstructBehahaviourTree()
    {
        IsCovereAvaliableNode coverAvaliableNode = new IsCovereAvaliableNode(avaliableCovers, playerTransform, this);
        GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
        HealthNode healthNode = new HealthNode(this, lowHealthThreshold);
        IsCoveredNode isCoveredNode = new IsCoveredNode(playerTransform, transform);
        ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this);
        RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTransform, transform);
        RangeNode shootingRangeNode = new RangeNode(shootingRange, playerTransform, transform);
        ShootNode shootNode = new ShootNode(agent, this, playerTransform);
        IsRecentlySeenPlayerNode recentlySeePlayerNode = new IsRecentlySeenPlayerNode(5f, chasingRange, playerTransform, transform, this);
        LookAroundForTooLongNode lookAround = new LookAroundForTooLongNode(10f, transform, this);
        RandomPatrolNode randomPatrolNode = new RandomPatrolNode(agent, this, 10f);
        MoveToLastKnownPositionNode moveToLastKnownPosition = new MoveToLastKnownPositionNode(agent, this);

        Sequence patrolSequence = new Sequence(new List<Node> { lookAround, randomPatrolNode });
        Sequence searchingSequence = new Sequence(new List<Node> { moveToLastKnownPosition });
        Sequence returnSequence = new Sequence(new List<Node> { lookAround, searchingSequence });

        Sequence tryShootSequence = new Sequence(new List<Node> { shootingRangeNode, shootNode });
        Selector trytoShootPlayerSelector = new Selector(new List<Node> { tryShootSequence, chaseNode });
        Sequence shootSequence = new Sequence(new List<Node> { chasingRangeNode, trytoShootPlayerSelector });

        topNode = new Selector(new List<Node> { shootSequence, returnSequence, patrolSequence });


    }

    private void Update()
    {
        topNode.Evaluate();
        if(topNode.nodeState == NodeState.FAILURE)
        {
            SetColor(Color.red);
            agent.isStopped = true;
        }
        currentHealth += Time.deltaTime * healthRestoreRate;
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void SetColor(Color color)
    {
        material.color = color;
    }

    public void SetBestCoverSpot(Transform bestCoverSpot)
    {
        this.bestCoverSpot = bestCoverSpot;
    }

    public Transform GetBestCoverSpot()
    {
        return bestCoverSpot;
    }


}

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStalker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerFatigue playerFatigue;
    [SerializeField] private Transform hallwaySpawnPoint;

    [Header("Threat Settings")]
    [SerializeField] private float warningFatigue = 75f;
    [SerializeField] private float breachFatigue = 100f;
    [SerializeField] private float catchDistance = 1.5f;

    private NavMeshAgent agent;
    private bool hasWarned = false;
    private bool hasBreached = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
                playerFatigue = player.GetComponent<PlayerFatigue>();
            }
        }

        if (hallwaySpawnPoint != null)
        {
            agent.Warp(hallwaySpawnPoint.position);
        }
    }

    void Update()
    {
        if (playerFatigue == null || playerTransform == null) return;

        // 1. Warning phase outside the door (can still be prevented with coffee)
        if (!hasBreached)
        {
            if (playerFatigue.currentFatigue >= warningFatigue && !hasWarned)
            {
                hasWarned = true;
                Debug.Log("WARNING: You hear footsteps and knocking in the hallway...");
            }
            else if (playerFatigue.currentFatigue < warningFatigue)
            {
                hasWarned = false; // Reset warning if player drinks coffee in time
            }

            // 2. Point of no return: Enemy enters the apartment permanently
            if (playerFatigue.currentFatigue >= breachFatigue)
            {
                StartBreach();
            }
        }
        // 3. Once inside, the enemy hunts relentlessly until game over or task completion
        else
        {
            agent.SetDestination(playerTransform.position);

            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance <= catchDistance)
            {
                CatchPlayer();
            }
        }
    }

    void StartBreach()
    {
        hasBreached = true;
        Debug.Log("BREACH: The enemy entered the apartment! Drink coffee to regain your speed!");
    }

    void CatchPlayer()
    {
        Debug.Log("GAME OVER: Caught by the intruder!");
        // Disable agent so it stops pushing the player
        agent.isStopped = true;
    }
}

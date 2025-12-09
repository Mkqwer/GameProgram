using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public Transform Player;


    private int _lives = 3;
    public int EnemyLives
    {
        get { return _lives; }
        private set
        {
            _lives = value;
            if (_lives <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
    public Transform PatrolRoute;
    public List<Transform> Locations;

    private int _locationIndex = 0;
    private NavMeshAgent _Agent;

    void Start()
    {
        InitializePatrolRoute();
        _Agent = GetComponent<NavMeshAgent>();
        MoveToNextPatrolLocation();
        Player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (_Agent.remainingDistance < 0.2f && !_Agent.pathPending)
        {
            MoveToNextPatrolLocation();
        }   
    }

    void InitializePatrolRoute()
    {

        foreach (Transform child in PatrolRoute)
        {
            Locations.Add(child);
        }
    }

    void MoveToNextPatrolLocation()
    {
       if (Locations.Count == 0)
            return;
        _Agent.destination = Locations[_locationIndex].position;

        _locationIndex = (_locationIndex + 1) % Locations.Count;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            _Agent.destination = Player.position;

            Debug.Log("Player detected - attack!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Player out of range, resume patrol");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            EnemyLives -= 1;
            Debug.Log("Enemy hit! Lives remaining: " + EnemyLives);
        }
    }

}


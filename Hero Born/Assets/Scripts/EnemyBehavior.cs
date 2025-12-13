using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public Transform Player;
    public Transform PatrolRoute;
    public List<Transform> Locations = new List<Transform>(); 

    public float EnemyMoveSpeed = 4f; 
    public float ChaseSpeedMultiplier = 1.5f; 

    private int _lives = 3;
    private int _locationIndex = 0;
    private NavMeshAgent _Agent;

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


    void Start()
    {
        _Agent = GetComponent<NavMeshAgent>();
        if (_Agent == null) 
        {
            Debug.LogError("EnemyBehavior: NavMeshAgent 컴포넌트가 오브젝트에 없습니다!");
            return;
        }
        _Agent.speed = EnemyMoveSpeed;
        
        InitializePatrolRoute();
        
        MoveToNextPatrolLocation();
        
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            Player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("씬에서 'Player' 오브젝트를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        if (_Agent != null && _Agent.remainingDistance < 0.2f && !_Agent.pathPending)
        {
            MoveToNextPatrolLocation();
        }   
    }


    void InitializePatrolRoute()
    {
        if (PatrolRoute == null)
        {
            Debug.LogWarning("EnemyBehavior: PatrolRoute Transform이 할당되지 않았습니다. 순찰이 불가능합니다.");
            return;
        }

        Locations.Clear();

        foreach (Transform child in PatrolRoute)
        {
            Locations.Add(child);
        }
    }

    void MoveToNextPatrolLocation()
    {
       if (_Agent == null || Locations.Count == 0)
            return;
        
       if (Locations[_locationIndex] == null)
       {
            Debug.LogError("Locations 리스트의 " + _locationIndex + "번째 항목이 null입니다. PatrolRoute 자식 오브젝트를 확인하세요.");
            _locationIndex = (_locationIndex + 1) % Locations.Count;
            return;
       }

        _Agent.destination = Locations[_locationIndex].position;

        _locationIndex = (_locationIndex + 1) % Locations.Count;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && Player != null && _Agent != null)
        {
            _Agent.speed = EnemyMoveSpeed * ChaseSpeedMultiplier; 
            _Agent.destination = Player.position;

            Debug.Log("Player detected - attack! Chase Speed: " + _Agent.speed);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player" && _Agent != null)
        {
            _Agent.speed = EnemyMoveSpeed; 
            Debug.Log("Player out of range, resume patrol");
            MoveToNextPatrolLocation(); 
        }
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
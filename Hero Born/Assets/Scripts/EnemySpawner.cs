using UnityEngine;
using System.Collections; // Coroutine을 사용하기 위해 필요

public class EnemySpawner : MonoBehaviour
{
    // 인스펙터에서 연결 (EnemyBehavior가 부착된 프리팹)
    public GameObject EnemyPrefab; 
    
    // 인스펙터에서 연결 (순찰 위치들을 담고 있는 부모 오브젝트)
    public Transform PatrolRoute; 
    
    // 생성 간격 (10초)
    public float SpawnInterval = 1f; 

    void Start()
    {
        // 10초마다 적을 생성하는 코루틴 시작
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        // PatrolRoute가 없으면 경고 후 종료
        if (PatrolRoute == null)
        {
            Debug.LogError("PatrolRoute가 EnemySpawner에 설정되지 않았습니다. 스폰을 중지합니다.");
            yield break;
        }

        Transform[] spawnLocations = new Transform[PatrolRoute.childCount];
        for (int i = 0; i < PatrolRoute.childCount; i++)
        {
            spawnLocations[i] = PatrolRoute.GetChild(i);
        }

        if (spawnLocations.Length == 0)
        {
            Debug.LogError("PatrolRoute에 스폰 위치(자식 오브젝트)가 없습니다. 스폰을 중지합니다.");
            yield break;
        }

        while (true)
        {
            int randomIndex = Random.Range(0, spawnLocations.Length);
            Transform spawnPoint = spawnLocations[randomIndex];

            GameObject newEnemy = Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
            newEnemy.name = "Enemy_Spawned";

            EnemyBehavior enemyBehavior = newEnemy.GetComponent<EnemyBehavior>();
            if (enemyBehavior != null)
            {
                enemyBehavior.PatrolRoute = PatrolRoute;
            }
            
            // 설정된 시간만큼 대기
            yield return new WaitForSeconds(SpawnInterval);
        }
    }
}
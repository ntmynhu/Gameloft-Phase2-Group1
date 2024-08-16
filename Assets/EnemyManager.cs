using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class Enemy
{
    public BaseEnemy gameObject;
    public float chance;
    public float weight;
    [HideInInspector] public IObjectPool<BaseEnemy> pool;
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemyTypes;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<BaseEnemy> activeEnemies;
    [SerializeField] private float defaultTime;
    [SerializeField] private float spawnRate;

    private float elapsedPhaseTime;
    private float elapsedSpawnTime;
    private float accumulatedWeight;

    private void Awake()
    {
        ChangePhase();
        elapsedPhaseTime = defaultTime;
        elapsedSpawnTime = spawnRate;

        foreach (Enemy enemy in enemyTypes)
        {
            enemy.pool = new ObjectPool<BaseEnemy>(
                () =>
                {
                    BaseEnemy e = Instantiate(enemy.gameObject);
                    e.gameObject.name = enemy.gameObject.name;
                    return e;
                },
                (e) =>
                {
                    e.gameObject.SetActive(true);
                },
                (e) =>
                {
                    e.gameObject.SetActive(false);
                },
                (e) =>
                {
                    Destroy(e.gameObject);
            });
        }
    }

    private int GetRandomEnemyIndex()
    {
        float random = UnityEngine.Random.Range(0f, 1f);
        Debug.Log("random: " + random);
        for (int i = 0; i < enemyTypes.Count; ++i)
        {
            if (accumulatedWeight * random < enemyTypes[i].weight)
            {
                return i;
            }
        }
        return 0;
    }

    private void ChangePhase()
    {
        ClearEnemy();
        accumulatedWeight = 0f;
        foreach (Enemy enemy in enemyTypes)
        {
            accumulatedWeight += enemy.chance;
            enemy.weight = accumulatedWeight;
        }
    }

    private void ClearEnemy()
    {
        foreach (BaseEnemy activeEnemy in activeEnemies)
        {
            Enemy enemy = enemyTypes.Find(e => e.gameObject.name == activeEnemy.gameObject.name);
            enemy.pool.Release(activeEnemy);
        }
        activeEnemies.Clear();
    }

    private BaseEnemy SpawnEnemy(int index, Vector2 location)
    {
        BaseEnemy enemy = enemyTypes[index].pool.Get();
        enemy.transform.position = location;
        activeEnemies.Add(enemy);
        Debug.Log(enemy + " " + location);
        return enemy;
    }

    private void Update()
    {
        elapsedPhaseTime -= Time.deltaTime;
        if (elapsedPhaseTime < 0f)
        {
            ChangePhase();
            elapsedPhaseTime = defaultTime;
            elapsedSpawnTime = spawnRate;
        }

        elapsedSpawnTime -= Time.deltaTime;
        if (elapsedSpawnTime < 0f)
        {
            int posRand = UnityEngine.Random.Range(0, spawnPoints.Count);
            int typeRand = GetRandomEnemyIndex();
            SpawnEnemy(typeRand, spawnPoints[posRand].position);
            elapsedSpawnTime = spawnRate;
        }
    }
}

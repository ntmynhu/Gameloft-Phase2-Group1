using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class Enemy
{
    public BaseEnemy gameObject;
    public float chance;
    [HideInInspector] public float weight;
    [HideInInspector] public IObjectPool<BaseEnemy> pool;
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemyTypes;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<BaseEnemy> activeEnemies;
    [SerializeField] private float defaultTime;
    [SerializeField] private float spawnRate;
    [SerializeField] private float shiftedValue;
    [SerializeField] private Transform PlayerRef;
    private float elapsedPhaseTime;
    private float elapsedSpawnTime;
    private float accumulatedWeight;
    private int currShiftedIndex = 0; // enemy index whose chance will be shifted to another enemy when changing phase
    private int nextShiftedIndex; // point to index that is after currShiftedIndex;
    private float realShiftedValue;
    private int phaseCount = 0;

    [SerializeField]
    private FloatPublisherSO sendElapsedPhaseTimeSO;

   private void Awake()
    {
        elapsedPhaseTime = defaultTime;
        elapsedSpawnTime = spawnRate;

        if (enemyTypes.Count < 2)
        {
            enemyTypes[0].chance = 100;
        }
        else
        {
            enemyTypes[0].chance = 60;
            enemyTypes[1].chance = 40;
        }
        
        ChangePhase();

        foreach (Enemy enemy in enemyTypes)
        {
            enemy.pool = new ObjectPool<BaseEnemy>(
                () =>
                {
                    BaseEnemy e = Instantiate(enemy.gameObject);
                    e.gameObject.name = enemy.gameObject.name;
                    e.Player = PlayerRef;
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
        phaseCount++;
        if (phaseCount > 1)
            ShiftChance();

        accumulatedWeight = 0f;
        foreach (Enemy enemy in enemyTypes)
        {
            accumulatedWeight += enemy.chance;
            enemy.weight = accumulatedWeight;
        }
    }

    private void ShiftChance()
    {
        if (enemyTypes[enemyTypes.Count - 1].chance == 50f)
        {
            return;
        }

        nextShiftedIndex = currShiftedIndex;

        if (shiftedValue >= enemyTypes[currShiftedIndex].chance)
        {
            enemyTypes[currShiftedIndex].chance = 0f;
            currShiftedIndex++;
            realShiftedValue = enemyTypes[currShiftedIndex].chance;
        }
        else
        {
            enemyTypes[currShiftedIndex].chance -= shiftedValue;
            realShiftedValue = shiftedValue;
        }

        while (nextShiftedIndex < enemyTypes.Count - 1)
        {
            nextShiftedIndex++;

            if (enemyTypes[nextShiftedIndex].chance + realShiftedValue <= 50f)
            {
                Debug.Log("case 1");
                enemyTypes[nextShiftedIndex].chance += realShiftedValue;
                break;
            }
            else
            {
                Debug.Log("case 2");
                enemyTypes[nextShiftedIndex].chance += realShiftedValue;
                realShiftedValue = enemyTypes[nextShiftedIndex].chance - 50;
                enemyTypes[nextShiftedIndex].chance -= realShiftedValue;
            }
        }
        Debug.Log(enemyTypes[enemyTypes.Count - 1].chance);
        if (enemyTypes[enemyTypes.Count - 1].chance == 50f)
        {
            enemyTypes[enemyTypes.Count - 2].chance = 50f;
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
        sendElapsedPhaseTimeSO.RaiseEvent(elapsedPhaseTime);

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

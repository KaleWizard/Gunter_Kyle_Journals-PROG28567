using UnityEngine;
using System.Collections.Generic;

public class EnemyOrbitGroup : MonoBehaviour
{
    public float orbitDistance;
    public float orbitmaxSpeed;
    public Vector3 orbitPoint;
    public List<Enemy> orbitingEnemies;
    [SerializeField] EnemyRegister enemyRegister;

    [SerializeField] float joinDistance = 2;

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform playerTransform;
    [SerializeField] int initialEnemyCount = 4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int count = orbitingEnemies.Count;
        if (count > 0)
        {
            // Setup leftEnemies
            for (int i = 1; i < count; i++)
            {
                orbitingEnemies[i].leftEnemy = orbitingEnemies[i - 1];
            }
            orbitingEnemies[0].leftEnemy = orbitingEnemies[count - 1];

            // Setup rightEnemies
            for (int i = 0; i < count - 1; i++)
            {
                orbitingEnemies[i].rightEnemy = orbitingEnemies[i + 1];
            }
            orbitingEnemies[count - 1].rightEnemy = orbitingEnemies[0];

            // Add orbitController so that states switch
            for (int i = 0; i < count; i++)
            {
                orbitingEnemies[i].orbitController = this;
            }
        }

        SpawnInitialEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        AddShips();
    }

    void AddShips()
    {
        foreach (Enemy enemy in enemyRegister.enemies)
        {
            if (Vector2.Distance(orbitPoint, enemy.transform.position) < joinDistance
                && !orbitingEnemies.Contains(enemy))
            {
                AddShip(enemy);
            }
        }
    }

    void AddShip(Enemy enemy)
    {
        if (orbitingEnemies.Count == 0)
        {
            orbitingEnemies.Add(enemy);
            enemy.leftEnemy = enemy.rightEnemy = enemy;
            enemy.orbitController = this;
            return;
        }
        int insertionIndex = 0;
        float shortestDistance = Vector2.Distance(enemy.transform.position, orbitingEnemies[0].transform.position);
        for (int i = 1; i < orbitingEnemies.Count; i++)
        {
            float nextDist = Vector2.Distance(enemy.transform.position, orbitingEnemies[i].transform.position);
            if (nextDist < shortestDistance)
            {
                shortestDistance = nextDist;
                insertionIndex = i;
            }
        }
        Enemy closestEnemy = orbitingEnemies[insertionIndex];

        float prevDistance = Vector2.Distance(enemy.transform.position, closestEnemy.leftEnemy.transform.position);
        float nextDistance = Vector2.Distance(enemy.transform.position, closestEnemy.rightEnemy.transform.position);

        Enemy prevEnemy;
        Enemy nextEnemy;

        if (prevDistance < nextDistance)
        {
            prevEnemy = closestEnemy.leftEnemy;
            nextEnemy = closestEnemy;
        } else
        {
            prevEnemy = closestEnemy;
            nextEnemy = closestEnemy.rightEnemy;
        }
        orbitingEnemies.Add(enemy);

        prevEnemy.rightEnemy = nextEnemy.leftEnemy = enemy;
        enemy.leftEnemy = prevEnemy;
        enemy.rightEnemy = nextEnemy;
        enemy.orbitController = this;
    }

    public void RemoveShip(Enemy enemy)
    {
        // If enemy's right is itself, then it's the only ship in the orbitGroup
        // In this case the ship only needs to be removed from the orbit list
        if (enemy.rightEnemy != enemy)
        {
            enemy.rightEnemy.leftEnemy = enemy.leftEnemy;
            enemy.leftEnemy.rightEnemy = enemy.rightEnemy;
        }
        enemy.rightEnemy = enemy.leftEnemy = null;
        enemy.orbitController = null;
        orbitingEnemies.Remove(enemy);
    }

    void SpawnInitialEnemies()
    {
        // Get initial angle and change in angle
        float theta = 0f;
        float delta = 2 * Mathf.PI / initialEnemyCount;

        // Drawn lines around circle's points
        for (int i = 0; i < initialEnemyCount; i++)
        {
            Vector3 spawnpoint = new Vector3(Mathf.Sin(theta), Mathf.Cos(theta)) * orbitDistance + orbitPoint;

            Enemy newEnemy = Instantiate(enemyPrefab, spawnpoint, Quaternion.identity).GetComponent<Enemy>();
            AddShip(newEnemy);
            enemyRegister.enemies.Add(newEnemy);

            newEnemy.transform.up = spawnpoint - orbitPoint;
            newEnemy.playerTransform = playerTransform;

            theta += delta;
        }
    }
}

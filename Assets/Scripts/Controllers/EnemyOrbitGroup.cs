using UnityEngine;
using System.Collections.Generic;

public class EnemyOrbitGroup : MonoBehaviour
{
    public float orbitDistance;
    public float orbitmaxSpeed;
    public Vector3 orbitPoint;
    public List<Enemy> orbitingEnemies;
    [SerializeField] EnemyRegister enemyRegister;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int count = orbitingEnemies.Count;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}

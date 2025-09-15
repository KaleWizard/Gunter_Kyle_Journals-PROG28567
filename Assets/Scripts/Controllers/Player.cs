using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;

    [SerializeField] bool waitToSpawnBombs = false;

    public Vector2 bombOffset;

    // Update is called once per frame
    void Update()
    {
        TryToSpawnBomb(bombOffset);
        TryToWarp();
    }

    void TryToWarp() 
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Warp(0.5f);
        }
    }

    // Warps the player to the enemy's position linearly based on howFar
    // howFar = 0 --> player doesn't move
    // howFar = 0.5 --> player warps halfway to enemy
    // howFar = 1 --> player warps to enemy
    void Warp(float howFar)
    {
        Vector2 newPosition = Vector2.Lerp(transform.position, enemyTransform.position, howFar);
        transform.position = newPosition;
    }

    void TryToSpawnBomb(Vector3 bombOffset)
    {
        // Spawn a bomb above the player upon 'B' key pressed
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!waitToSpawnBombs)
            {
                SpawnBombAtOffset(bombOffset);
            }
            else
            {
                StartCoroutine(WaitToSpawnBombRoutine(bombOffset));
            }
        }
    }

    void SpawnBombAtOffset(Vector3 inOffset)
    {
        GameObject newBomb = Instantiate(bombPrefab);
        // Set new bomb's position to above player
        newBomb.transform.position = transform.position + inOffset;
    }

    IEnumerator WaitToSpawnBombRoutine(Vector3 inOffset)
    {
        yield return new WaitForSecondsRealtime(3f);
        SpawnBombAtOffset(inOffset);
    }
}

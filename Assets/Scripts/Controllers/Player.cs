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

    public float warpRatio = 0.5f;

    public int numberOfBombs = 3;
    public float bombSpacing = 0.33f;

    public float inDistance = 0.8f;
    Vector2[] cornerArray = { new Vector2(-1, 1).normalized,
                                    new Vector2(1, 1).normalized,
                                    new Vector2(-1, -1).normalized,
                                    new Vector2(1, -1).normalized };

    // Update is called once per frame
    void Update()
    {
        TryToSpawnBomb(bombOffset);
        TryToSpawnBombTrail(bombSpacing, numberOfBombs);
        TryToSpawnBombOnRandomCorner(inDistance);
        TryToWarp();
    }

    void TryToWarp() 
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Warp(enemyTransform, warpRatio);
        }
    }

    // Warps the player to the enemy's position linearly based on howFar
    // howFar = 0 --> player doesn't move
    // howFar = 0.5 --> player warps halfway to enemy
    // howFar = 1 --> player warps to enemy
    void Warp(Transform target, float ratio)
    {
        ratio = Mathf.Clamp(ratio, 0f, 1f);  
        Vector2 newPosition = Vector2.Lerp(transform.position, target.position, ratio);
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

    void TryToSpawnBombTrail(float bombSpacing, int numberOfBombs)
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnBombTrail(bombSpacing, numberOfBombs);
        }
    }

    void SpawnBombTrail(float bombSpacing, int numberOfBombs)
    {
        // Hold local position of next bomb spawned
        Vector2 localPos = -transform.up * bombSpacing;

        // Spawn correct number of bombs
        for (int i = 0; i < numberOfBombs; i++)
        {
            SpawnBombAtOffset(localPos);
            // Increase distance for each successive bomb placement
            localPos += -(Vector2) transform.up * bombSpacing;
        }
    }

    void TryToSpawnBombOnRandomCorner(float inDistance)
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnBombOnRandomCorner(inDistance);
        }
    }

    void SpawnBombOnRandomCorner(float inDistance)
    {
        int res = Random.Range(0, 4);
        SpawnBombAtOffset(inDistance * cornerArray[res]);
    }
}

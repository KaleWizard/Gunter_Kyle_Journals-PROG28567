using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;

    [SerializeField] bool waitToSpawnBombs = false;
    
    // Update is called once per frame
    void Update()
    {
        TryToSpawnBomb();
    }

    void TryToSpawnBomb()
    {
        Vector3 bombSpawnOffset = Vector3.up;
        // Spawn a bomb above the player upon 'B' key pressed
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!waitToSpawnBombs)
            {
                SpawnBombAtOffset(bombSpawnOffset);
            }
            else
            {
                StartCoroutine(WaitToSpawnBombRoutine(bombSpawnOffset));
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

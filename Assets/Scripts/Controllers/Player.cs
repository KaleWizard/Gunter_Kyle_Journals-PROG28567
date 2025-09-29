using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player : MonoBehaviour
{
    public List<Transform> asteroidTransforms;
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public Transform bombsTransform;

    [SerializeField] float maxSpeed = 5f;

    [SerializeField] float accelTime = 0.25f;
    [SerializeField] Vector3 velocity = Vector3.zero;

    [SerializeField] float decelTime = 1f;

    [SerializeField] float timer = 0f;
    [SerializeField] bool testingAcceleration = false;
    [SerializeField] bool testingDeceleration = false;

    bool decelJustPrinted = false;

    // Powerup stats
    [SerializeField] GameObject powerupPrefab;

    [SerializeField] float powerupRadius = 3f;
    [SerializeField] int numberOfPowerups = 5;

    void Update()
    {
        PlayerMovement();
        TryToSpawnPowerups(powerupRadius, numberOfPowerups);
    }

    void PlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = (new Vector3(horizontal, vertical)).normalized;

        if (inputDirection.sqrMagnitude > 0)
        {
            velocity += inputDirection * (maxSpeed / accelTime) * Time.deltaTime;
        } else
        {
            velocity -= (velocity.normalized * maxSpeed / decelTime) * Time.deltaTime;
        }

        TestPlayerAccel(inputDirection);
        TestPlayerDecel(inputDirection);

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.position += velocity * Time.deltaTime;

        
    }

    void TestPlayerAccel(Vector2 playerInput)
    {
        if (!testingAcceleration) return;

        if (playerInput.x != 0 || playerInput.y != 0)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            velocity = Vector3.zero;
        }

        if (velocity.magnitude > maxSpeed)
        {
            Debug.Log("Velocity Reached: " + velocity.magnitude.ToString() +
                      "\nTime Taken: " + timer.ToString());
            velocity = Vector3.zero;
            timer = 0;
        }
    }

    void TestPlayerDecel(Vector2 playerInput)
    {
        if (!testingDeceleration) return;

        const float minimumSpeed = 0.01f;

        if (playerInput.sqrMagnitude > 0f)
        {
            timer = 0f;
            decelJustPrinted = false;
        } else
        {
            timer += Time.deltaTime;
        }
        if (velocity.magnitude < minimumSpeed && !decelJustPrinted)
        {
            Debug.Log("Velocity Reached: " + velocity.magnitude.ToString() +
                      "\nTime Taken: " + timer.ToString());
            timer = 0;
            decelJustPrinted = true;
        }
    }

    void TryToSpawnPowerups(float radius, int numberOfPowerups)
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnPowerups(radius, numberOfPowerups);
        }
    }

    void SpawnPowerups(float radius, int numberOfPowerups)
    {
        // Get initial angle and change in angle
        float theta = 0f;
        float delta = 2 * Mathf.PI / numberOfPowerups;

        // Spawn powerups around player
        for (int i = 0; i < numberOfPowerups; i++)
        {
            Vector3 spawnpoint = new Vector2(Mathf.Sin(theta), Mathf.Cos(theta)) * radius;

            Instantiate(powerupPrefab, transform.position + spawnpoint, Quaternion.identity);

            theta += delta;
        }
    }
}
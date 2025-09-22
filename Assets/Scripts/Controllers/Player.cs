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

    [SerializeField] float accelTimer = 0f;
    [SerializeField] bool testingAcceleration = false;

    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 accelDirection = (new Vector3(horizontal, vertical)).normalized;

        velocity += accelDirection * (maxSpeed / accelTime) * Time.deltaTime;

        TestPlayerMovement(accelDirection);

        velocity = Vector3.ClampMagnitude(velocity,  maxSpeed);

        transform.position += velocity * Time.deltaTime;

        
    }

    void TestPlayerMovement(Vector2 playerInput)
    {
        if (!testingAcceleration) return;

        if (playerInput.x != 0 || playerInput.y != 0)
        {
            accelTimer += Time.deltaTime;
        }
        else
        {
            accelTimer = 0;
            velocity = Vector3.zero;
        }

        if (velocity.magnitude > maxSpeed)
        {
            Debug.Log("Velocity Reached: " + velocity.magnitude.ToString() +
                      "\nTime Taken: " + accelTimer.ToString());
            velocity = Vector3.zero;
            accelTimer = 0;
        }
    }
}
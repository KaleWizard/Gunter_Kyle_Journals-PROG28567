using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    // Idle state detection angle and distance
    [SerializeField] float detectionAngle;
    [SerializeField] float detectionDist;

    // Idle state point the enemy is wandering to
    // and distance from point to consider enemy "arrived"
    Vector2 wanderPoint = Vector2.zero;
    [SerializeField] float targetDistance = 0.25f;

    // Seeking state sight angle and distance
    [SerializeField] float sightAngle;
    [SerializeField] float sightDist;

    // Enemy stats
    [SerializeField] float speed = 4f;

    // Enemy's current state
    EnemyState state = EnemyState.Idle;

    enum EnemyState
    {
        Idle = 0,
        Seeking = 1
    }

    void Update()
    {
        EnemyMovement();
    }

    void EnemyMovement()
    {
        if (state == EnemyState.Idle)
        {
            IdleMovement();
        } else if (state == EnemyState.Seeking)
        {
            SeekingMovement();
        }
    }

    void IdleMovement()
    {
        // Pick a new point if close enough to the current point
        if (NearPoint(wanderPoint, targetDistance))
        {
            ChooseNewWanderPoint();
        }
        // Turn towards the current point
        TurnToPoint(wanderPoint);
        // Move towards the current point
        MoveForwards();
        // Check if player is within detection range
        // If so, switch to seeking state
        if (PlayerInRange(detectionAngle, detectionDist))
        {
            state = EnemyState.Seeking;
        }
    }

    void SeekingMovement()
    {
        // Turn towards the player
        TurnToPoint(playerTransform.position);
        // Move fowards
        MoveForwards();
        // Check if player is within sight range
        // If not, switch to idle state
        if (PlayerInRange(sightAngle, sightDist))
        {
            state = EnemyState.Idle;
        }
    }

    void TurnToPoint(Vector3 point)
    {

    }

    void MoveForwards()
    {
        // Move enemy in the direction they're facing
        transform.position += transform.up.normalized * speed * Time.deltaTime;
    }

    // Detects if player is within the given angle and distance
    // based on enemy's facing direction (transform.up)
    bool PlayerInRange(float angle, float distance)
    {
        // Detect if player is close enough to the enemy
        bool isPlayerClose = Vector3.Distance(transform.position, playerTransform.position) < distance;
        Vector3 playerDirection = playerTransform.position - transform.position;

        // Detect if the enemy is facing the player (within angle / 2 degrees
        bool isWithinAngle = Vector3.Angle(transform.up, playerDirection) < angle / 2;
        return isPlayerClose && isWithinAngle;
    }

    // Returns true if p1 and p2 are less than distance units apart
    bool PointNearPoint(Vector3 p1,  Vector3 p2, float distance)
    {
        return Vector3.Distance(p1, p2) < distance;
    }

    // Returns true if enemy is within distance units of point
    bool NearPoint(Vector2 point, float distance)
    {
        return Vector2.Distance(transform.position, point) < distance;
    }

    void ChooseNewWanderPoint()
    {
        // Get limits of the screen (in world coordinates)
        Vector2 screenTopLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        Vector2 screenBottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        // Select random points within the screen
        float randX = Random.Range(screenTopLeft.x, screenBottomRight.x);
        float randY = Random.Range(screenTopLeft.y, screenBottomRight.y);

        // Update new wander point
        wanderPoint = new Vector2(randX, randY);
    }
}

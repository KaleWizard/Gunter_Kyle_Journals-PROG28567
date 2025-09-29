using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    // Idle state detection angle and distance
    [SerializeField] float detectionAngle = 30f;
    [SerializeField] float detectionDist = 3f;

    // Idle state point the enemy is wandering to
    // and distance from point to consider enemy "arrived"
    Vector2 wanderPoint = Vector2.zero;
    [SerializeField] float targetDistance = 0.25f;

    // Seeking state sight angle and distance
    [SerializeField] float sightAngle = 35f;
    [SerializeField] float sightDist = 3.5f;

    // Enemy stats
    [SerializeField] float speed = 3f;
    [SerializeField] float angularSpeedSeeking = 20f;
    [SerializeField] float angularSpeedIdle = 15f;

    // Enemy's current state
    EnemyState state = EnemyState.Idle;

    // EnemyRadar stats
    [SerializeField] float circleRadius = 10;
    [SerializeField] int circlePoints = 10;

    enum EnemyState
    {
        Idle = 0,
        Seeking = 1
    }

    void Update()
    {
        EnemyMovement();
        EnemyRadar(circleRadius, circlePoints);
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
        DisplaySearchRange(detectionAngle, detectionDist, Color.white);

        // Pick a new point if close enough to the current point
        if (NearPoint(wanderPoint, targetDistance))
        {
            ChooseNewWanderPoint();
        }
        // Turn towards the current point
        TurnToPoint(wanderPoint, angularSpeedIdle);
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
        DisplaySearchRange(sightAngle, sightDist, Color.red);
        // Turn towards the player
        TurnToPoint(playerTransform.position, angularSpeedSeeking);
        // Move fowards
        MoveForwards();
        // Check if player is within sight range
        // If not, switch to idle state
        if (!PlayerInRange(sightAngle, sightDist))
        {
            state = EnemyState.Idle;
            ChooseNewWanderPoint();
        }
    }

    void TurnToPoint(Vector3 target, float angularSpeed)
    {
        // Get direction from enemy to target
        Vector3 targetDirection = (target - transform.position).normalized;

        // Detect direction to rotate
        // Get dot product of direction to target and transform.right
        // if result is < 0, rotate clockwise, else rotate counter-clockwise
        float det = Vector3.Dot(targetDirection, transform.right.normalized);
        float rotationDirection = det < 0? 1 : -1;

        Vector3 rotation = transform.eulerAngles;

        rotation.z += rotationDirection * angularSpeed * Time.deltaTime;

        transform.eulerAngles = rotation;
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
        // Detect if the enemy is facing the player (within angle/2 degrees)
        Vector3 playerDirection = playerTransform.position - transform.position;
        bool isWithinAngle = Vector3.Angle(transform.up, playerDirection) < angle / 2;

        // Detect if player is close enough to be detected
        return NearPoint(playerTransform.position, distance) && isWithinAngle;
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

    void DisplaySearchRange(float angle, float distance, Color c)
    {
        float sinTheta = Mathf.Sin(Mathf.Deg2Rad * angle / 2);
        float cosTheta = Mathf.Cos(Mathf.Deg2Rad * angle / 2);

        float x = transform.up.x;
        float y = transform.up.y;

        // Draw lines on sides of vision cone
        Vector3 direction = new Vector3 (x * cosTheta - y * sinTheta, x * sinTheta + y * cosTheta);
        direction = direction.normalized * distance;
        Debug.DrawLine(transform.position, transform.position + direction, c);

        direction = new Vector3(x * cosTheta + y * sinTheta, y * cosTheta - x * sinTheta);
        direction = direction.normalized * distance;
        Debug.DrawLine(transform.position, transform.position + direction, c);


        // Draw lines for arc of vision cone
        float runningAngle = Mathf.Atan2(direction.y, direction.x);
        Vector3 lastPoint = transform.position + new Vector3(Mathf.Cos(runningAngle), Mathf.Sin(runningAngle)) * distance;
        Vector3 currentPoint;
        int arcSegments = 8;

        for (int i = 1; i < arcSegments; i++)
        {
            runningAngle += Mathf.Deg2Rad * angle / (arcSegments - 1);
            currentPoint = transform.position + new Vector3(Mathf.Cos(runningAngle), Mathf.Sin(runningAngle)) * distance;
            Debug.DrawLine(lastPoint, currentPoint, c);
            lastPoint = currentPoint;
        }

    }

    void EnemyRadar(float radius, int circlePoints)
    {
        // Get initial angle and change in angle
        float theta = 0f;
        float delta = 2 * Mathf.PI / circlePoints;

        // Get radar colour
        Color color = NearPoint(playerTransform.position, radius) ? Color.red : Color.green;

        // Drawn lines around circle's points
        for (int i = 0; i < circlePoints; i++)
        {
            Vector3 start = new Vector2(Mathf.Sin(theta), Mathf.Cos(theta)) * radius;
            Vector3 end = new Vector2(Mathf.Sin(theta + delta), Mathf.Cos(theta + delta)) * radius;

            Debug.DrawLine(transform.position + start, transform.position + end, color);

            theta += delta;
        }
    }
}

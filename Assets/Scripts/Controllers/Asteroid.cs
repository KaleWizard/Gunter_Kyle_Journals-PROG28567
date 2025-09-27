using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float moveSpeed;
    public float arrivalDistance;
    public float maxFloatDistance;

    Vector3 direction;

    Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        ChooseNewTargetPoint();
    }

    // Update is called once per frame
    void Update()
    {
        AsteroidMovement();
    }

    void AsteroidMovement()
    {
        if (NearPoint(target, arrivalDistance))
        {
            ChooseNewTargetPoint();
        }

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void ChooseNewTargetPoint()
    {
        direction = Random.insideUnitCircle.normalized;

        target = transform.position + direction * maxFloatDistance;
    }

    // Returns true if asteroid is within distance units of point
    bool NearPoint(Vector2 point, float distance)
    {
        return Vector2.Distance(transform.position, point) < distance;
    }
}

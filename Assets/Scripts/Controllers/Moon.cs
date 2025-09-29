using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    public Transform planetTransform;

    [SerializeField] float radius = 3f;
    [SerializeField] float speed = 5f;

    float theta = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OrbitalMotion(radius, speed, planetTransform);
    }

    void OrbitalMotion(float radius, float speed, Transform target)
    {
        theta += Time.deltaTime * speed / radius;

        Vector3 relativePosition = new Vector2(Mathf.Sin(theta), Mathf.Cos(theta)) * radius;

        transform.position = target.position + relativePosition;
    }
}

using System;
using UnityEngine;

public class VectorPrinting : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform enemyTransform;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            PrintTestVectors();
        }
    }

    void PrintTestVectors()
    {
        Debug.Log(Normalize(new Vector2(3f, 4f)));
        Debug.Log(Normalize(new Vector2(-3f, 2f)));
        Debug.Log(Normalize(new Vector2(1.5f, -3.5f)));
        // Print similarity of player and enemy look directions as a percentage
        Debug.Log(((Vector2.Dot(playerTransform.up.normalized, enemyTransform.up.normalized) + 1) * 50f).ToString() + "%");
    }

    Vector2 Normalize(Vector2 v)
    {
        float mag = Mathf.Sqrt(v.x * v.x + v.y * v.y);
        if (mag == 0f)
        {
            return Vector2.zero;
        }
        return v / mag;
    }
}

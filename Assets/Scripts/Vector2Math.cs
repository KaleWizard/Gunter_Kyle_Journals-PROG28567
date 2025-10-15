using UnityEngine;

public class Vector2Math
{
    // Accepts angle in radians
    public static Vector2 Rotate(Vector2 v, float angle)
    {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }
}

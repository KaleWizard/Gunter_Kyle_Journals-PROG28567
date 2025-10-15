using UnityEngine;

public class CircleZone : MonoBehaviour
{
    public float radius;
    public Vector2 center;

    public CircleZone(float newRadius, Vector2 newCenter)
    {
        radius = newRadius;
        center = newCenter;
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DisruptionField : MonoBehaviour
{
    [SerializeField] float maxDisruptionTurning;
    [SerializeField] float minDisruptionTime;
    [SerializeField] float maxDisruptionTime;

    public List<Transform> shipsNotInField = new();

    public List<Vector3> ranges;

    public bool showField = true;
    public int circlePoints = 8;

    [SerializeField] GameObject fieldCirclePrefab;

    void Start()
    {
        CreateFieldObjects();
    }

    // Update is called once per frame
    void Update()
    {
        List<Transform> detectedShips = new();
        foreach (Transform t in shipsNotInField)
        {
            if (IsInField(t))
            {
                StartCoroutine(Disruption(t));
                detectedShips.Add(t);
            }
        }
        foreach (Transform t in detectedShips)
        {
            shipsNotInField.Remove(t);
        }
        DisplayField();
    }

    IEnumerator Disruption(Transform ship)
    {
        while (true)
        {
            float disruptionTurning = Random.Range(-maxDisruptionTurning, maxDisruptionTurning);
            float disruptionTime = Random.Range (minDisruptionTime, maxDisruptionTime);

            float timer = 0;
            while (timer <= disruptionTime)
            {
                Vector3 rotation = ship.eulerAngles;
                rotation.z += disruptionTurning * Time.deltaTime;
                ship.eulerAngles = rotation;
                yield return null;

                timer += Time.deltaTime;

                if (!IsInField(ship))
                {
                    shipsNotInField.Add(ship);
                    yield break;
                }
            }
        }
    }

    bool IsInField(Transform ship)
    {
        foreach (Vector3 zone in ranges)
        {
            Vector2 zoneCenter = transform.position + GetLocalCenter(zone);

            if (Vector2.Distance(zoneCenter, ship.position) < zone.z)
            {
                return true;
            }
        }
        return false;
    }

    void CreateFieldObjects()
    {
        foreach (Vector3 zone in ranges)
        {
            Vector3 zoneCenter = GetLocalCenter(zone);
            GameObject newCircle = Instantiate(fieldCirclePrefab, transform);
            newCircle.transform.localPosition = zoneCenter;
            newCircle.transform.localScale = Vector3.one * zone.z;
        }
    }

    void DisplayField()
    {
        if (!showField) return;

        foreach (Vector3 zone in ranges)
        {
            // Get initial angle and change in angle
            float theta = 0f;
            float delta = 2 * Mathf.PI / circlePoints;

            Vector3 zoneCenter = transform.position + GetLocalCenter(zone);

            // Drawn lines around circle's points
            for (int i = 0; i < circlePoints; i++)
            {
                Vector3 start = new Vector2(Mathf.Sin(theta), Mathf.Cos(theta)) * zone.z;
                Vector3 end = new Vector2(Mathf.Sin(theta + delta), Mathf.Cos(theta + delta)) * zone.z;

                Debug.DrawLine(zoneCenter + start, zoneCenter + end, Color.white);

                theta += delta;
            }
        }
    }

    Vector3 GetLocalCenter(Vector3 zone)
    {
        return Vector2Math.Rotate(zone, transform.eulerAngles.z * Mathf.Deg2Rad);
    }
}

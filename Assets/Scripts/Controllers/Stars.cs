using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public List<Transform> starTransforms;
    public float drawingTime;

    float progression = 0f;

    int current = 0;

    // Update is called once per frame
    void Update()
    {
        DrawConstellations();
    }

    void DrawConstellations()
    {
        progression += Time.deltaTime;
        if (progression > drawingTime)
        {
            progression -= drawingTime;
            current++;

            if (current + 1 > starTransforms.Count - 1)
            {
                current = 0;
            }
        }
        float ratio = progression / drawingTime;
        Vector2 lineStart = starTransforms[current].position;
        Vector2 lineEnd = Vector2.Lerp(lineStart, starTransforms[current + 1].position, ratio);

        Debug.DrawLine(lineStart, lineEnd);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipeline : MonoBehaviour
{
    [SerializeField] bool doPipelinesPersist = true;

    List<List<Vector2>> pipelineList = new List<List<Vector2>>();

    // Update is called once per frame
    void Update()
    {
        // Start new pipeline if user presses mouse down
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(RecordPipeline());
        }

        // Draw all pipelines in pipelineList
        foreach (List<Vector2> pipeline in pipelineList)
        {
            int posCount = pipeline.Count;
            for (int i = 1; i < posCount; i++)
            {
                Debug.DrawLine(pipeline[i - 1], pipeline[i]);
            }
        }
    }

    IEnumerator RecordPipeline()
    {
        // Create new pipeline and add it to the pipeline list
        List<Vector2> newPipeline = new List<Vector2>();
        pipelineList.Add(newPipeline);

        // Track magnitude sum and 
        float magnitudeSum = 0f;
        bool startedTracking = false;

        // Until user releases mouse button...
        while (Input.GetMouseButton(0))
        {
            // Add this frame's mouse position to the new pipeline
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPipeline.Add(mousePos);

            // Add length of new line to magnitude sum
            // Skip first iteration
            if (startedTracking)
            {
                magnitudeSum += Length(newPipeline[newPipeline.Count - 2] - mousePos);
            }
            startedTracking = true;

            // Wait for 0.1 seconds
            yield return new WaitForSecondsRealtime(0.1f);
        }

        // If pipelines are not set to persist, clear the list
        if (!doPipelinesPersist)
        {
            pipelineList.Clear();
        }

        // Print total length of pipeline to console
        Debug.Log("Pipeline Length: " + magnitudeSum.ToString());
    }

    // Returns the length of vector v
    float Length(Vector2 v)
    {
        return Mathf.Sqrt(v.x * v.x + v.y * v.y);
    }
}

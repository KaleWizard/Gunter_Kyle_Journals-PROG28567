using UnityEngine;
using System.Collections.Generic;

public class SquareSpawner : MonoBehaviour
{
    [SerializeField] float sizeChangeScale = 0.1f;

    float squareSize = 1f;

    List<Vector2> positionList = new List<Vector2>();
    List<float> sizeList = new List<float>();

    // Update is called once per frame
    void Update()
    {
        // Change square size based on mouse scrolling
        if (Input.mouseScrollDelta.y != 0f)
        {
            squareSize += Input.mouseScrollDelta.y * sizeChangeScale;
            squareSize = Mathf.Max(squareSize, 0);
        }

        // Draw semi-transparent square at the mouse's position
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        DrawSquare(mousePos, squareSize, squareSize, new Color(255, 255, 255, 0.25f));

        // If the mouse is clicked, add the position of the new square to position list
        if (Input.GetMouseButtonDown(0))
        {
            positionList.Add(mousePos);
            sizeList.Add(squareSize);
        }
        // Draw all squares in the position list
        int squareCount = sizeList.Count;
        for (int i = 0; i < squareCount; i++)
        {
            DrawSquare(positionList[i], sizeList[i], sizeList[i], Color.white);
        }
    }

    void DrawSquare(Vector2 position, float width, float height, Color c)
    {
        // Calculate distance from center of square to each side of square
        float halfWidth = width / 2;
        float halfHeight = height / 2;

        // Calculate square vertex position values
        float left = position.x - halfWidth;
        float right = position.x + halfWidth;
        float top = position.y + halfHeight;
        float bottom = position.y - halfHeight;

        // Draw top, left, right, and bottom lines
        Debug.DrawLine(new Vector2(left, top), new Vector2(right, top), c);        // top
        Debug.DrawLine(new Vector2(left, top), new Vector2(left, bottom), c);      // left
        Debug.DrawLine(new Vector2(right, top), new Vector2(right, bottom), c);    // right
        Debug.DrawLine(new Vector2(left, bottom), new Vector2(right, bottom), c);  // bottom
    }
}

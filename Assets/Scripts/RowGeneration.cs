using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class RowGeneration : MonoBehaviour
{
    float squareSize = 1f;
    float squarePadding = 0.5f;

    List<Vector2> positionList = new List<Vector2>();

    [SerializeField] TMP_InputField inputTextField;

    // Update is called once per frame
    void Update()
    {
        // Draw all squares in the position list
        foreach (Vector2 pos in positionList)
        {
            DrawSquare(pos, squareSize, squareSize, Color.white);
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

    public void GenerateNewRow()
    {
        // Get inputField value
        string inputText = inputTextField.text;

        // Check input for validity
        int inputCount;
        bool result = int.TryParse(inputText, out inputCount);

        if (!result || inputCount < 0)
        {
            Debug.Log("Generation failed. Invalid input!");
            return;
        }

        // Get leftmost square x-value
        float leftmostX = -1 * ((inputCount - 1) / 2f) * (squareSize + squarePadding);

        // Reset position list, add all new square positions
        positionList.Clear();
        for (int i = 0; i < inputCount; i++)
        {
            positionList.Add(new Vector2(leftmostX + (squareSize + squarePadding) * i, 0));
        }
    }
}

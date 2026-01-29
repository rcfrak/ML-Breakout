using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{   
    // 13 wide, 8 deep
    public Vector2Int size = new Vector2Int(13, 8);
    public GameObject brickPrefab;

    [Header("Spacing between bricks and top padding")]
    public Vector2 cellSize = new Vector2(1.3f, 0.42f);  // spacing between bricks
    public float topPadding = 1.0f;                      // space reserved for UI at top

    private Color[] rowColors = new Color[]
    {
        Color.hotPink,
        Color.red,
        new Color(1f, 0.5f, 0f),   // orange
        Color.yellow,
        Color.green,
        Color.blue,
        new Color(0.5f, 0f, 0.5f), // purple
        new Color(0f, 0.9f, 0.85f) // teal       
    };

    void Awake()
    {
        
        GenerateLevel();
    }

    void GenerateLevel()
    {
        Camera cam = Camera.main;
        float camTop = cam.transform.position.y + cam.orthographicSize;
        float topY = camTop - topPadding;
        float gridWidth = (size.x - 1) * cellSize.x;

        for (int i = 0; i < size.x; i++) // Columns/X-axis
        {
            for (int j = 0; j < size.y; j++) // Rows/Y-axis
            {
                GameObject newBrick = Instantiate(brickPrefab, transform);

                float x = 0 - (gridWidth * 0.5f) + i * cellSize.x;
                float y = topY - j * cellSize.y; // goes downward from the top
                newBrick.transform.position = new Vector3(x, y, 0f);

                // apply the color by row
                var renderer = newBrick.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.color = rowColors[j % rowColors.Length];
                }
            }
        }
    }
}

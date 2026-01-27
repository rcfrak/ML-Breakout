using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // 15 wide, 6 deep (can change these if you click on level generator in hierarchy, I find that 13 and 5 might work better honestly. 
    // also if you change it to less then 6, you take out the top rows first, which eventually will be getting 2 hit code so this makes it easy to create easy levels for the 
    // first few stages if we decide to do that.
    public Vector2Int size = new Vector2Int(15, 6);

    // 1.0 width 0.5 height, width offset is 1.15 and height offset is 0.65
    public Vector2 offset = new Vector3(1.15f, 0.65f);
    public GameObject brickPrefab;

    // color list to be called in level generator
    private Color[] rowColors = new Color[]
    {
        new Color(0.5f, 0f, 0.5f), // Purple (bottom)
        Color.blue,               // Dark Blue
        Color.green,              // Green
        Color.yellow,             // Yellow
        new Color(1f, 0.5f, 0f),  // Orange
        Color.red                 // Red (top)
    };

    private void Awake()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        for (int i = 0; i < size.x; i++) // Columns/X-axis
        {
            for (int j = 0; j < size.y; j++) // Rows/Y-axis
            {
                // Instantiate the brick as a child of this object
                GameObject newBrick = Instantiate(brickPrefab, transform);

                // Center the grid horizontally based on the size and offset specified prior
                float xPos = ((size.x - 1) * 0.5f - i) * offset.x;
                float yPos = j * offset.y;

                newBrick.transform.localPosition = new Vector3(-xPos, yPos, 0);

                // apply the color
                SpriteRenderer renderer = newBrick.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    // assign color by row, starting from purple going to red
                    renderer.color = rowColors[j % rowColors.Length];
                }
            }
        }
    }
}
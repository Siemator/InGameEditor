using UnityEngine;

public class Grid
{
    private int width, height;
    private int[,] gridArry;
    private Transform pointParent;
    private float cellSize = 7f; // Cell size (same multiplier as before)

    public Grid(int width, int height, Transform pointParent)
    {
        this.width = width;
        this.height = height;
        this.pointParent = pointParent;

        gridArry = new int[width, height];

        // Now draw grid lines with a LineRenderer
        DrawGridLines();

        foreach (int arry in gridArry)
        {
            Debug.Log(arry);
        }
    }

    private void DrawGridLines()
    {
        // Calculate total grid size and offset to center the grid
        float totalWidth = width * cellSize;
        float totalHeight = height * cellSize;
        Vector3 offset = new Vector3(totalWidth / 2f, totalHeight / 2f, 0);

        // Draw vertical lines (width + 1 lines)
        for (int i = 0; i <= width; i++)
        {
            Vector3 start = new Vector3(i * cellSize, 0, 0) - offset;
            Vector3 end = new Vector3(i * cellSize, totalHeight, 0) - offset;
            CreateLineRenderer(start, end, $"VerticalLine_{i}");
        }

        // Draw horizontal lines (height + 1 lines)
        for (int j = 0; j <= height; j++)
        {
            Vector3 start = new Vector3(0, j * cellSize, 0) - offset;
            Vector3 end = new Vector3(totalWidth, j * cellSize, 0) - offset;
            CreateLineRenderer(start, end, $"HorizontalLine_{j}");
        }
    }


    private void CreateLineRenderer(Vector3 start, Vector3 end, string lineName)
    {
        // Create a new GameObject for the line and parent it under the pointParent
        GameObject lineObj = new GameObject(lineName);
        lineObj.transform.parent = pointParent; // or assign to a separate parent if desired

        // Add and configure the LineRenderer
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.useWorldSpace = true;

        // Adjust the line width as needed
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;

        // Assign a simple material; using Sprites/Default works well for a basic line
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.sortingOrder = -1;
    }
}

using UnityEngine;

public class Grid
{
    private int width, height;
    private int[,] gridArry;
    private Transform pointParent;
    private float cellSize = 7f;
    private Color gridColor;

    /// <summary>
    /// Initializes a new instance of the Grid class with the specified dimensions, parent transform, and grid line color.
    /// </summary>
    /// <param name="width">The number of cells along the horizontal axis.</param>
    /// <param name="height">The number of cells along the vertical axis.</param>
    /// <param name="pointParent">The parent Transform to which the grid's line renderers are attached.</param>
    /// <param name="gridColor">The color used for drawing the grid lines.</param>
    /// <remarks>
    /// This constructor creates a two-dimensional grid array, draws the visual grid lines, and logs each cell's initial value.
    /// </remarks>
    public Grid(int width, int height, Transform pointParent, Color gridColor)
    {
        this.width = width;
        this.height = height;
        this.pointParent = pointParent;
        this.gridColor = gridColor;

        gridArry = new int[width, height];

        DrawGridLines();

        foreach (int arry in gridArry)
        {
            Debug.Log(arry);
        }
    }

    /// <summary>
    /// Draws the grid's vertical and horizontal lines to visualize the grid structure in the Unity scene.
    /// </summary>
    /// <remarks>
    /// Computes the total grid size using the cell size and grid dimensions, determines an offset to center the grid,
    /// and then draws vertical lines (width + 1) and horizontal lines (height + 1) by calculating their start and end positions.
    /// Each line is rendered by calling the CreateLineRenderer method.
    /// </remarks>
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


    /// <summary>
    /// Creates and configures a LineRenderer to draw a line between two world positions.
    /// </summary>
    /// <param name="start">The starting position of the line in world space.</param>
    /// <param name="end">The ending position of the line in world space.</param>
    /// <param name="lineName">The name assigned to the new GameObject holding the line.</param>
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
        lr.startColor = gridColor;
        lr.endColor = gridColor;
        lr.sortingOrder = -1;
    }
}

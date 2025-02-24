using UnityEngine;

public class EditorManager : MonoBehaviour
{
    public GameObject statsGraph;
    public Transform grindParent;  // This will be the parent for grid points and lines
    public bool drawGrid = false;

    private bool grapStatsu = false;

    void Start()
    {
        // Create a grid of 2 x 2 cells (adjust as needed)
        Grid grid = new Grid(12, 12, grindParent);
    }

    void Update()
    {
        // Your other update code…
    }

    public void statsSwitch()
    {
        grapStatsu = !grapStatsu;
        statsGraph.SetActive(grapStatsu);
    }

    public void vSync()
    {
        if (QualitySettings.vSyncCount == 0)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;
    }
}

using UnityEngine;

public class EditorManager : MonoBehaviour
{
    public GameObject statsGraph;

    private bool grapStatsu = false;

    void Start()
    {

    }

    void Update()
    {

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

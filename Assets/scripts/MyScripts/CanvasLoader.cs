using UnityEngine;

public class CanvasLoader : MonoBehaviour
{
    public void enableCanvas(GameObject InfoCanvas)
    {
        InfoCanvas.SetActive(true);
    }
    public void disableCanvas(GameObject InfoCanvas)
    {
        InfoCanvas.SetActive(false);
    }
}

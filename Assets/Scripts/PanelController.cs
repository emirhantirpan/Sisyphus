using UnityEngine;

public class PanelController : MonoBehaviour
{
    public static PanelController instance;

    private void Awake()
    {
        instance = this;
    }
    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
using UnityEngine;

public class OxygenManager : MonoBehaviour
{
    private void Update()
    {
        if (OxygenSlider.instance._stamina <= 0)
            GameStateManager.instance.EndGame();
    }
}

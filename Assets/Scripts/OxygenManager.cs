using UnityEngine;

public class OxygenManager : MonoBehaviour
{
    private bool gameEnded = false;

    private void Update()
    {
        if (gameEnded) return;

        if (OxygenSlider.instance == null || GameStateManager.instance == null)
            return;

        if (OxygenSlider.instance.GetStamina() <= 0f)
        {
            gameEnded = true;
            Debug.Log("Oxygen depleted → Game Over");
            GameStateManager.instance.EndGame();
        }
    }
}

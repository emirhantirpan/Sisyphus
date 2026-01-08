using UnityEngine;

public class OxygenManager : MonoBehaviour
{
    private void Update()
    {
        CheckOxygenLevel();
    }

    private void CheckOxygenLevel()
    {
        if (OxygenSlider.instance == null || GameStateManager.instance == null)
            return;

        if (OxygenSlider.instance.GetStamina() <= 0)
        {
            Debug.Log("Oxygen level is empty");
            GameStateManager.instance.EndGame();
        }
    }
}
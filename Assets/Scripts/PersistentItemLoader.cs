using UnityEngine;

public class PersistentItemLoader : MonoBehaviour
{
    private void Start()
    {
        LoadItems();
    }

    private void LoadItems()
    {
        int oxygenCount = MarketSaveManager.GetItemCount("oxygen_tube");
        if (oxygenCount > 0)
        {
            Debug.Log("🫁 Oxygen Loaded x" + oxygenCount);
        }

        if (MarketSaveManager.IsPurchased("double_clicker"))
        {
            Debug.Log("🖱 Double Clicker Active");
        }
    }
}
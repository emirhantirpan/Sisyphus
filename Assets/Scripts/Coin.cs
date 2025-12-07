using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CoinManager.instance != null)
            {
                CoinManager.instance.AddCoin(coinValue);
            }
            Destroy(gameObject);
        }
    }
}
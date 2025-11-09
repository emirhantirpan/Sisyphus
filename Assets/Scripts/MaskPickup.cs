using UnityEngine;

public class MaskPickup : MonoBehaviour
{
    public float maskDuration = 10f;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OxygenSlider.instance.ActivateMask(maskDuration);
            Destroy(gameObject);
        }
    }
}

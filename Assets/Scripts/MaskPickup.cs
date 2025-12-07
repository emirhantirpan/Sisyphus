using UnityEngine;

public class MaskPickup : MonoBehaviour
{
    [SerializeField] private float maskDuration = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateMaskEffect();
            Destroy(gameObject);
        }
    }

    private void ActivateMaskEffect()
    {
        if (OxygenSlider.instance != null)
        {
            OxygenSlider.instance.ActivateMask(maskDuration);
        }
    }
}
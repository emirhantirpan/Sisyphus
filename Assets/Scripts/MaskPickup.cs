using UnityEngine;

public class MaskPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private float oxygenAddAmount = 30f;
    [SerializeField] private string playerTag = "Player";

    [Header("Optional FX")]
    [SerializeField] private GameObject pickupVfxPrefab;
    [SerializeField] private AudioSource pickupSfx;

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;
        if (!other.CompareTag(playerTag)) return;

        if (OxygenSlider.instance != null)
        {
            OxygenSlider.instance.AddStamina(oxygenAddAmount);
        }

        collected = true;

        // FX
        if (pickupVfxPrefab != null)
            Instantiate(pickupVfxPrefab, transform.position, Quaternion.identity);

        if (pickupSfx != null)
            pickupSfx.Play();

        // Eğer SFX çalacaksa hemen yok etme (AudioSource pickup objesindeyse)
        if (pickupSfx != null && pickupSfx.clip != null)
        {
            // objeyi görünmez yap, collider kapat
            var col = GetComponent<Collider>();
            if (col != null) col.enabled = false;

            var rend = GetComponentInChildren<Renderer>();
            if (rend != null) rend.enabled = false;

            Destroy(gameObject, pickupSfx.clip.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
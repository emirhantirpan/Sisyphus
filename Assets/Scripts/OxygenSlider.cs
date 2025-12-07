using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OxygenSlider : MonoBehaviour
{
    public static OxygenSlider instance;

    [Header("UI References")]
    [SerializeField] private Slider slider;

    [Header("Oxygen Settings")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float baseDecreaseRate = 10f;
    [SerializeField] private float maskDecreaseRate = 3f;
    [SerializeField] private float movementThreshold = 0.1f;

    private float stamina;
    private float currentDecreaseRate;
    private bool isMaskActive = false;

    private const string OXYGEN_RATE_KEY = "SavedOxygenRate";

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        InitializeOxygen();
        LoadOxygenUpgrade();
    }

    private void FixedUpdate()
    {
        UpdateOxygen();
    }

    private void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeOxygen()
    {
        stamina = maxStamina;
        currentDecreaseRate = baseDecreaseRate;

        if (slider != null)
        {
            slider.maxValue = maxStamina;
            slider.value = stamina;
        }
    }

    private void LoadOxygenUpgrade()
    {
        float savedMultiplier = PlayerPrefs.GetFloat(OXYGEN_RATE_KEY, 1.5f);
        currentDecreaseRate = baseDecreaseRate * savedMultiplier;
    }

    private void UpdateOxygen()
    {
        if (rb == null) return;

        float movement = rb.linearVelocity.magnitude;

        if (movement > movementThreshold)
        {
            stamina -= currentDecreaseRate * Time.fixedDeltaTime;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (slider != null)
        {
            slider.value = stamina;
        }
    }

    public void ActivateMask(float duration)
    {
        if (isMaskActive)
        {
            StopAllCoroutines();
        }
        StartCoroutine(MaskEffectCoroutine(duration));
    }

    private IEnumerator MaskEffectCoroutine(float duration)
    {
        isMaskActive = true;
        float normalRate = currentDecreaseRate;
        currentDecreaseRate = maskDecreaseRate;

        yield return new WaitForSeconds(duration);

        currentDecreaseRate = normalRate;
        isMaskActive = false;
    }

    // Getters
    public float GetStamina() => stamina;
    public bool IsMaskActive() => isMaskActive;
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OxygenSlider : MonoBehaviour
{

    public static OxygenSlider instance;

    [SerializeField] private Slider _slider;
    [SerializeField] private Rigidbody _rb;

    [SerializeField] private float _maxStamina = 100f;
    [SerializeField] private float _normalDecreaseRate = 10f;
    [SerializeField] private float _maskDecreaseRate = 3f;
    

    public float _stamina;
    private float _movement;

    private float _currentDecreaseRate;
    public bool _isMaskActive = false;

    private void Awake()
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
    private void Start()
    {
        _stamina = _maxStamina;
        _slider.maxValue = _maxStamina;
        _slider.value = _stamina;
        _currentDecreaseRate = _normalDecreaseRate;

        float savedRate = PlayerPrefs.GetFloat("SavedOxygenRate", 1.5f);
        _normalDecreaseRate *= savedRate;
    }
    private void FixedUpdate()
    {
        OxygenRate();
    }
    public void OxygenRate()
    {
        _movement = _rb.linearVelocity.magnitude;

        if (_movement > 0.1f)
        {
            _stamina -= _currentDecreaseRate * Time.deltaTime;
        }
        _stamina = Mathf.Clamp( _stamina,0, _maxStamina);
        _slider.value = _stamina;
    }

    public void ActivateMask(float duration)
    {
        if (_isMaskActive)
        {
            StopAllCoroutines();
        }
        StartCoroutine(MaskTimerCoroutine(duration));
    }

    private IEnumerator MaskTimerCoroutine(float duration)
    {
        _isMaskActive = true;
        _currentDecreaseRate = _maskDecreaseRate;

        yield return new WaitForSeconds(duration);

        _currentDecreaseRate = _normalDecreaseRate;
        _isMaskActive = false;

    }
    
}

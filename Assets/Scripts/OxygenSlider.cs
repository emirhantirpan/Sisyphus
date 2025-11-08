using UnityEngine;
using UnityEngine.UI;

public class OxygenSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Rigidbody _rb;
    
    private float _stamina;
    private float _maxStamina = 100f;
    private float _decraeseStamina = 10f;
    private float _movement;
    

    private void Start()
    {
        _stamina = _maxStamina;
        _slider.maxValue = _maxStamina;
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
            _stamina -= _decraeseStamina * Time.deltaTime;
        }
        _stamina = Mathf.Clamp( _stamina,0, _maxStamina);
        _slider.value = _stamina;
    }
}

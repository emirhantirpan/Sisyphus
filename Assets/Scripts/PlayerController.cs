using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Lane Settings")]
    [SerializeField] private float[] lanePositionsX;
    [SerializeField] private float laneChangeForce = 3500f;

    [Header("Movement Settings")]
    public float minSwipeDistance = 50.0f;
    [SerializeField] private float baseForwardForce = 1000f;
    [SerializeField] private float horizontalDamping = 0.8f;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public OxygenSlider oxygenSlider;

    private int currentLane = 1;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private CameraController mainCamera;
    private float currentForwardForce;
    private float speedBoostMultiplier = 1f;

    private void Awake()
    {
        InitializeSingleton();
        InitializeComponents();
    }

    private void Start()
    {
        currentForwardForce = baseForwardForce;
        CacheCamera();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TriggerCameraJolt();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TriggerCameraRecover();
        }
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

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody>();
        oxygenSlider = GetComponent<OxygenSlider>();
    }

    private void CacheCamera()
    {
        if (Camera.main != null)
        {
            mainCamera = Camera.main.GetComponent<CameraController>();
        }
    }

    private void HandleInput()
    {
        if (GameStateManager.instance != null && GameStateManager.instance.isGameOver)
            return;

#if UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#elif UNITY_EDITOR
        HandleMouseInput();
#endif
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                ProcessSwipe();
            }
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            ProcessSwipe();
        }
    }

    private void ProcessSwipe()
    {
        float swipeDistanceX = endTouchPosition.x - startTouchPosition.x;

        if (Mathf.Abs(swipeDistanceX) > minSwipeDistance)
        {
            int direction = swipeDistanceX > 0 ? 1 : -1;
            ChangeLane(direction);
        }
    }

    private void ChangeLane(int direction)
    {
        int newLane = currentLane + direction;
        currentLane = Mathf.Clamp(newLane, 0, lanePositionsX.Length - 1);
    }

    private void ApplyMovement()
    {
        if (rb == null) return;

        ApplyForwardForce();
        ApplyLaneChangeForce();
    }

    private void ApplyForwardForce()
    {
        float effectiveForce = currentForwardForce * speedBoostMultiplier;
        rb.AddForce(Vector3.forward * effectiveForce * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    private void ApplyLaneChangeForce()
    {
        float targetX = lanePositionsX[currentLane];
        float xDifference = targetX - rb.position.x;
        float horizontalForce = xDifference * laneChangeForce;

        // Yatay hýzý azalt (damping)
        Vector3 currentVelocity = rb.linearVelocity;
        currentVelocity.x *= horizontalDamping;
        rb.linearVelocity = currentVelocity;

        // Hedefe doðru kuvvet uygula
        rb.AddForce(Vector3.right * horizontalForce * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    private void TriggerCameraJolt()
    {
        if (mainCamera != null)
        {
            mainCamera.TriggerJolt();
        }
    }

    private void TriggerCameraRecover()
    {
        if (mainCamera != null)
        {
            mainCamera.TriggerRecover();
        }
    }

    // Speed Boost Methods
    public void SetSpeedBoost(float multiplier)
    {
        speedBoostMultiplier = multiplier;
    }

    public void ResetSpeedBoost()
    {
        speedBoostMultiplier = 1f;
    }

    // Getters
    public int GetCurrentLane() => currentLane;
    public float GetCurrentSpeed() => rb != null ? rb.linearVelocity.magnitude : 0f;
    public bool IsSpeedBoosted() => speedBoostMultiplier > 1f;
}
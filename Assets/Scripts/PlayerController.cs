using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] private float[] lanePositionsX;
    [SerializeField] private float laneChangeForce = 500f;
    [SerializeField] private float minSwipeDistance = 50.0f;
    [SerializeField] private float forwardForce = 1000f;
     private CameraController mainCamera;

 
    private int currentLane = 1;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public OxygenSlider oxygenSlider;
    

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        rb = GetComponent<Rigidbody>();     
        oxygenSlider = GetComponent<OxygenSlider>();
    }
    void Start()
    {
        mainCamera = Camera.main.GetComponent<CameraController>();
    }

    void Update()
    {
       
        
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) { startTouchPosition = Input.mousePosition; }
        else if (Input.GetMouseButtonUp(0)) { endTouchPosition = Input.mousePosition; HandleSwipe(); }
#endif

#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) { startTouchPosition = touch.position; }
            else if (touch.phase == TouchPhase.Ended) { endTouchPosition = Input.mousePosition; HandleSwipe(); }
        }
#endif
    }

    
    void FixedUpdate()
    {
        if (rb == null) return;

        
        rb.AddForce(Vector3.forward * forwardForce * Time.fixedDeltaTime, ForceMode.Acceleration);


        float targetX = lanePositionsX[currentLane];

        
        float xDifference = targetX - rb.position.x;

       
        float horizontalForce = xDifference * laneChangeForce;

       
        Vector3 currentVelocity = rb.linearVelocity;
        currentVelocity.x *= 0.8f; 
        rb.linearVelocity = currentVelocity;

       
        rb.AddForce(Vector3.right * horizontalForce * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

  

    private void HandleSwipe()
    {
        float swipeDistanceX = endTouchPosition.x - startTouchPosition.x;

        if (swipeDistanceX > minSwipeDistance)
        {
            ChangeLane(1);
        }
        else if (swipeDistanceX < -minSwipeDistance)
        {
            ChangeLane(-1);
        }
    }

    private void ChangeLane(int direction)
    {
        int newLane = currentLane + direction;
        currentLane = Mathf.Clamp(newLane, 0, lanePositionsX.Length - 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (mainCamera != null)
            {
                mainCamera.TriggerJolt();
            }
            
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            if (mainCamera != null)
            {
                mainCamera.TriggerRecover();
            }
           
        }
    }
}
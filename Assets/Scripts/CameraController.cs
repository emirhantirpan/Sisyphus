using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private enum CameraState { Takip, Firlama, Kurtarma }


    public Transform player;

    
    public float joltForwardSpeed = 10f;

    
    public float rampAngle = 30.0f;

    public float recoveryJoltSpeed = 8f;

    
    private CameraState currentState;
    private Vector3 offset;
    private float initialX;

    
    private float rampSlope;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (player == null) { return; }
        offset = transform.position - player.position;
        initialX = transform.position.x;
        currentState = CameraState.Takip;

       
        rampSlope = Mathf.Tan(Mathf.Abs(rampAngle) * Mathf.Deg2Rad);
    }

    void LateUpdate()
    {
        
        if (player == null || GameStateManager.instance.isGameOver) { return; }

       
        Vector3 targetPosition = player.position + offset;
        targetPosition.x = initialX;

        
        switch (currentState)
        {
            
            case CameraState.Takip:
                transform.position = targetPosition; 
                break;

            
            case CameraState.Firlama:
                
                float deltaZ = joltForwardSpeed * Time.deltaTime;
                float deltaY = deltaZ * rampSlope; 

                
                transform.Translate(new Vector3(0, deltaY, deltaZ), Space.World);
                break;

            
            case CameraState.Kurtarma:
                
                float recDeltaZ = recoveryJoltSpeed * Time.deltaTime;
                float recDeltaY = recDeltaZ * rampSlope;
                transform.Translate(new Vector3(0, recDeltaY, recDeltaZ), Space.World);

               
                if (targetPosition.z > transform.position.z)
                {
                    currentState = CameraState.Takip; 
                    
                }
                break;
        }
    }

    public void TriggerJolt()
    {
        if (currentState == CameraState.Takip)
            currentState = CameraState.Firlama;
    }
    public void TriggerRecover()
    {
        if (currentState == CameraState.Firlama)
            currentState = CameraState.Kurtarma;
    }
    public bool IsPlayerInTakipMode()
    {
        return currentState == CameraState.Takip;
    }
}
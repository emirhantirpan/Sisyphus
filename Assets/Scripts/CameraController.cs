using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
 
    public Transform player;

    public float joltForwardSpeed = 15f;
    public float rampAngle = -30.0f;

    
    private Vector3 offset;
    private float initialX;
    private bool isJolting = false;

    
    private float rampYPerZ;

    void Start()
    {
        if (player == null) { return; }
        offset = transform.position - player.position;
        initialX = transform.position.x;
        isJolting = false;

        float angleRad = - rampAngle * Mathf.Deg2Rad;
        rampYPerZ = Mathf.Tan(angleRad);
    }

    void LateUpdate()
    {       
        if (player == null || GameManager.isGameOver)
        {
            return;
        }
       
        if (isJolting)
        {                     
            float deltaZ = joltForwardSpeed * Time.deltaTime;           
            float deltaY = deltaZ * rampYPerZ;
          
            transform.Translate(new Vector3(0, deltaY, deltaZ), Space.World);
        }
        
        else
        {            
            Vector3 targetPosition = player.position + offset;
            targetPosition.x = initialX; 
            transform.position = targetPosition;
        }
    }

    
    public void TriggerJolt()
    {       
        if (isJolting) return; 
        isJolting = true;
    }
    public void TriggerRecover()
    {
        isJolting = false;

    }
}
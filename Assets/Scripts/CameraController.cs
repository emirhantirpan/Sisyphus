using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    private Vector3 offset;
    
    void Start()
    {
        offset = transform.position - player.position;
    }

    
    void LateUpdate()
    {
        if(player == null)
        {
            return;
        }

        Vector3 newPosition = player.position + offset;

        newPosition.x = transform.position.x;

        transform.position = newPosition;
    }
}

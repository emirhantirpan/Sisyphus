using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool isGameOver = false;

    public Transform player;
    public Transform cameraTransform;

    public float gracePeriod = 2.0f;
    public float raycastDistance = 50f;

    private float timeSpentUnseen = 0.0f;

    
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
    private void Start()
    {
        isGameOver = false;
        Time.timeScale = 1;
        timeSpentUnseen = 0.0f;
    }

    private void Update()
    {
        if (isGameOver || player == null || cameraTransform == null)
        {
            return;
        }
        
        CheckIfPlayerIsVisible();
    }

   private void CheckIfPlayerIsVisible()
    {
        Vector3 targetPos = player.position + (Vector3.up * 0.5f);
        Vector3 direction = (targetPos- cameraTransform.position).normalized;

        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, direction, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                timeSpentUnseen = 0.0f;
            }
            else
            {
                timeSpentUnseen += Time.deltaTime;
            }
        }
        else
        {
            timeSpentUnseen += Time.deltaTime;
        }
        if(timeSpentUnseen > gracePeriod)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        if (isGameOver) return;
        isGameOver = true;
        Debug.Log("biti");
        Time.timeScale = 0;

    }
}

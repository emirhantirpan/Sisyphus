using UnityEngine;

public class VisibilityChecker : MonoBehaviour
{
    public float gracePeriod = 0.1f;
    public float raycastDistance = 25f;

    private float timeSpentUnseen = 0f;
    private Transform player;
    private Transform cam;

    private void Start()
    {
        if (PlayerController.instance != null)
            player = PlayerController.instance.transform;

        cam = Camera.main.transform;
    }

    private void Update()
    {
        if (GameStateManager.instance.isGameOver || player == null || cam == null) return;

        CheckVisibility();
    }

    private void CheckVisibility()
    {
        Vector3 targetPos = player.position + Vector3.up * 0.5f;
        Vector3 direction = (targetPos - cam.position).normalized;

        if (Physics.Raycast(cam.position, direction, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Player"))
                timeSpentUnseen = 0f;
            else
                timeSpentUnseen += Time.deltaTime;
        }
        else
        {
            timeSpentUnseen += Time.deltaTime;
        }

        if (timeSpentUnseen > gracePeriod)
        {
            GameStateManager.instance.EndGame();
        }
    }
}

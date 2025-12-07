using UnityEngine;

public class VisibilityChecker : MonoBehaviour
{
    [Header("Visibility Settings")]
    [SerializeField] private float gracePeriod = 0.1f;
    [SerializeField] private float raycastDistance = 25f;

    private float timeSpentUnseen = 0f;
    private Transform player;
    private Transform cam;

    private void Start()
    {
        InitializeReferences();
    }

    private void Update()
    {
        if (!CanCheckVisibility()) return;
        CheckVisibility();
    }

    private void InitializeReferences()
    {
        if (PlayerController.instance != null)
        {
            player = PlayerController.instance.transform;
        }

        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
    }

    private bool CanCheckVisibility()
    {
        if (GameStateManager.instance == null || player == null || cam == null)
            return false;

        return !GameStateManager.instance.isGameOver;
    }

    private void CheckVisibility()
    {
        Vector3 targetPos = player.position + Vector3.up * 0.5f;
        Vector3 direction = (targetPos - cam.position).normalized;

        if (IsPlayerVisible(direction))
        {
            timeSpentUnseen = 0f;
        }
        else
        {
            timeSpentUnseen += Time.deltaTime;

            if (timeSpentUnseen > gracePeriod)
            {
                GameStateManager.instance.EndGame();
            }
        }
    }

    private bool IsPlayerVisible(Vector3 direction)
    {
        if (Physics.Raycast(cam.position, direction, out RaycastHit hit, raycastDistance))
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }
}
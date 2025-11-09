using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool isGameOver = false;

    public Transform player;
    public Transform cameraTransform;

    [SerializeField] private float _deathDistanceBehind = 10f;
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
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }
        if (player == null || cameraTransform == null)
        {
            return;

        }
        CheckIfCaught();
    }

    private void CheckIfCaught()
    {
        float distance = player.position.z - cameraTransform.position.z;

        if(distance < -_deathDistanceBehind)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        if (isGameOver) return;
        isGameOver = true;

        Time.timeScale = 0;

    }
}

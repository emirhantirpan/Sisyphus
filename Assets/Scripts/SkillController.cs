using System.Collections;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public static SkillController instance;

    [Header("Skill Settings")]
    [SerializeField] private float doubleTapThreshold = 0.3f;
    [SerializeField] private float boostMultiplier = 2.0f;
    [SerializeField] private float boostDuration = 5.0f;
    [SerializeField] private float cooldownDuration = 10.0f;

    private bool canUseSkill = true;
    private bool isSkillActive = false;
    private float lastTapTime = 0f;
    private Vector2 firstTouchPosition;
    private Coroutine skillCoroutine;

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Update()
    {
        if (!CanProcessInput()) return;
        HandleDoubleTapInput();
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

    private bool CanProcessInput()
    {
        return canUseSkill &&
               !isSkillActive &&
               PlayerController.instance != null &&
               GameStateManager.instance != null &&
               !GameStateManager.instance.isGameOver;
    }

    private void HandleDoubleTapInput()
    {
#if UNITY_ANDROID || UNITY_IOS
        HandleTouchDoubleTap();
#elif UNITY_EDITOR
        HandleMouseDoubleTap();
#endif
    }

    private void HandleTouchDoubleTap()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                firstTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                ProcessTapEnd(touch.position);
            }
        }
    }

    private void HandleMouseDoubleTap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ProcessTapEnd(Input.mousePosition);
        }
    }

    private void ProcessTapEnd(Vector2 endPosition)
    {
        float distance = Vector2.Distance(firstTouchPosition, endPosition);

        if (IsTapNotSwipe(distance))
        {
            CheckForDoubleTap();
        }
    }

    private bool IsTapNotSwipe(float distance)
    {
        return distance < PlayerController.instance.minSwipeDistance;
    }

    private void CheckForDoubleTap()
    {
        float timeSinceLastTap = Time.time - lastTapTime;

        if (timeSinceLastTap <= doubleTapThreshold)
        {
            ActivateSkill();
            lastTapTime = 0f;
        }
        else
        {
            lastTapTime = Time.time;
        }
    }

    private void ActivateSkill()
    {
        if (skillCoroutine != null)
        {
            StopCoroutine(skillCoroutine);
        }

        skillCoroutine = StartCoroutine(SkillSequence());
        Debug.Log("Speed Boost Activated!");
    }

    private IEnumerator SkillSequence()
    {
        // Skill'i aktif et
        isSkillActive = true;
        ApplySpeedBoost();

        // Boost süresini bekle
        yield return new WaitForSeconds(boostDuration);

        // Boost'u kaldýr
        RemoveSpeedBoost();
        Debug.Log("Speed Boost Ended");

        // Cooldown baþlat
        canUseSkill = false;
        isSkillActive = false;

        yield return new WaitForSeconds(cooldownDuration);

        // Skill tekrar kullanýlabilir
        canUseSkill = true;
        Debug.Log("Speed Boost Ready!");
    }

    private void ApplySpeedBoost()
    {
        if (PlayerController.instance != null)
        {
            PlayerController.instance.SetSpeedBoost(boostMultiplier);
        }
    }

    private void RemoveSpeedBoost()
    {
        if (PlayerController.instance != null)
        {
            PlayerController.instance.ResetSpeedBoost();
        }
    }

    // Getters
    public bool IsSkillActive() => isSkillActive;
    public bool CanUseSkill() => canUseSkill;
    public float GetCooldownProgress()
    {
        if (canUseSkill) return 1f;
        return 0f; // Bu deðer cooldown UI için kullanýlabilir
    }
}
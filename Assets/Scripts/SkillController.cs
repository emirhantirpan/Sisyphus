using System.Collections;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private bool canUseSkill = true;
    private float lastTapTime = 0;
    [SerializeField] private float doubleTapThreshold = 0.3f;
    private Vector2 firstTouchPos;
   
    void Update()
    {
        if (canUseSkill == false)
        {
            return;
        }
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                firstTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                float distance = Vector2.Distance(firstTouchPos, touch.position);

                if (distance < PlayerController.instance.minSwipeDistance)
                {
                    if (Time.time - lastTapTime <= doubleTapThreshold)
                    {
                        lastTapTime = 0;
                        canUseSkill = false;
                        Debug.Log("Double Tap Algilandi");
                        PlayerController.instance.forwardForce = 2000f;
                        StartCoroutine(SetDoubleTap());
                    }
                    else
                    {
                        lastTapTime = Time.time;
                    }
                }
            }
            
        }
    }
    private IEnumerator SetDoubleTap()
    {
        Debug.Log("Sayac basladi");
        yield return new WaitForSeconds(5f);
        PlayerController.instance.forwardForce = 1000f;
        Debug.Log("Hizlanma bitti");
        yield return new WaitForSeconds(10f);
        canUseSkill = true;
        Debug.Log("Sayac bitti");
    }
}

// PlayerController.cs
// *** ADDFORCE (KUVVET EKLEME) YÖNTEMİNE GEÇİLDİ ***
// Bu, eğimli zeminlerde fizik motoruyla kavga etmemizi engeller.

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // --- Public Değişkenler ---

    // YENİ AÇIKLAMA: Bu değişkenler artık "hız" değil, "kuvvet çarpanı"
    // DEĞERLERİNİ ÇOK YÜKSELTMEN GEREKECEK (örn: 500-1000 gibi)
    [SerializeField] private float[] lanePositionsX;
    [SerializeField] private float laneChangeForce = 500f; // Şerit değiştirme "kuvveti"
    [SerializeField] private float minSwipeDistance = 50.0f;
    [SerializeField] private float forwardForce = 1000f; // İleri tırmanma "kuvveti"

    // --- Private Değişkenler ---
    private int currentLane = 1;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("PlayerController bir Rigidbody bileşeni gerektiriyor!");
        }
    }

    void Update()
    {
        // Girdi (Input) alma kısmı Update'te kalmalı (değişiklik yok)
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

    // FixedUpdate() - FİZİK GÜNCELLEMESİ BURADA
    void FixedUpdate()
    {
        if (rb == null) return;

        // --- 1. SÜREKLİ İLERİ KUVVET UYGULA ---
        // Objeyi sürekli olarak DÜNYANIN Z ekseninde (ileri) it.
        // Fizik motoru, bu kuvveti eğimli zeminle birleştirip tırmanma hareketine
        // dönüştürecek.
        // ForceMode.Acceleration: Objenin kütlesini (mass) görmezden gelir,
        //                         doğrudan ivme uygular. Bu daha tutarlı bir
        //                         hareket sağlar.
        rb.AddForce(Vector3.forward * forwardForce * Time.fixedDeltaTime, ForceMode.Acceleration);


        // --- 2. YATAY (ŞERİT) HAREKETİ İÇİN KUVVET UYGULA ---

        // Hedef X pozisyonunu al
        float targetX = lanePositionsX[currentLane];

        // Hedefe olan mesafeyi bul
        float xDifference = targetX - rb.position.x;

        // Yatay kuvveti hesapla. Bu, hedefe olan uzaklıkla orantılıdır.
        // (Hedefe yaklaştıkça yavaşlar, "Lerp" benzeri bir his verir).
        float horizontalForce = xDifference * laneChangeForce;

        // Hızı "ayarlamak" yerine, hızı "düzeltmek" için bir kuvvet uygula.
        // Ancak, yatay hızı çok fazla aşmasını da istemeyiz.
        // Önce mevcut yatay hızı "söndürelim" (yavaşlatalım).
        Vector3 currentVelocity = rb.linearVelocity;
        currentVelocity.x *= 0.8f; // Biraz sürtünme ekleyerek şeritler arasında "sallanmasını" engelle
        rb.linearVelocity = currentVelocity;

        // Şimdi şeride doğru çeken DÜZELTİCİ kuvveti ekle
        rb.AddForce(Vector3.right * horizontalForce * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    // --- Diğer Fonksiyonlar (Değişiklik Yok) ---

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
}
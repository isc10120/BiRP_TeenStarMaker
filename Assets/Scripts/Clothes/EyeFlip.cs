using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class EyeFlip : MonoBehaviour
{
    private Image face;

    [Header("Idle Sprites")]
    [SerializeField] private Sprite idle_S;
    [SerializeField] private Sprite idle_M;
    [SerializeField] private Sprite idle_L;

    [Header("Blink Frames (6 frames per size)")]
    [SerializeField] private Sprite[] blink_S; // size = 6
    [SerializeField] private Sprite[] blink_M; // size = 6
    [SerializeField] private Sprite[] blink_L; // size = 6

    [Header("Timing")]
    [SerializeField] private float minInterval = 1.5f;
    [SerializeField] private float maxInterval = 4.5f;
    [SerializeField] private float frameDuration = 0.1f;

    private Sprite nowIdle;
    private Sprite[] nowBlink;

    // Update 기반 상태
    private float nextBlinkAt;      // 다음 깜박임 시작 시간(Time.time 기준)
    private bool isBlinking;
    private int frameIndex;
    private float frameTimer;

    void Awake()
    {
        face = GetComponent<Image>();
    }

    void Start()
    {
        GameManager.Instance.onBodySizeChanged += ApplyBodySprites;

        ApplyBodySprites();
        face.sprite = nowIdle;

        ScheduleNextBlink();
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.onBodySizeChanged -= ApplyBodySprites;
    }

    void Update()
    {
        // 깜박이는 중이면 프레임 재생
        if (isBlinking)
        {
            // 안전장치: nowBlink가 비정상일 경우 즉시 종료
            if (nowBlink == null || nowBlink.Length == 0)
            {
                EndBlink();
                return;
            }

            frameTimer += Time.deltaTime;
            if (frameTimer >= frameDuration)
            {
                frameTimer -= frameDuration;

                // 다음 프레임
                if (frameIndex < nowBlink.Length)
                {
                    Sprite sp = nowBlink[frameIndex];
                    if (sp == null)
                    {
                        Debug.LogError($"[EyeFlip] Blink frame is NULL. index={frameIndex}");
                        EndBlink();
                        return;
                    }

                    face.sprite = sp;
                    // Debug.Log(sp.name); // 필요하면 켜
                    frameIndex++;
                }
                else
                {
                    EndBlink();
                }
            }

            return;
        }

        // 깜박일 시간 도달하면 시작
        if (Time.time >= nextBlinkAt)
        {
            StartBlink();
        }
    }

    private void StartBlink()
    {
        // 프레임 배열 체크
        if (nowBlink == null || nowBlink.Length == 0)
        {
            Debug.LogWarning("[EyeFlip] nowBlink is empty. Cannot blink.");
            ScheduleNextBlink();
            return;
        }

        isBlinking = true;
        frameIndex = 0;
        frameTimer = 0f;
    }

    private void EndBlink()
    {
        isBlinking = false;
        frameIndex = 0;
        frameTimer = 0f;

        // Idle 복귀
        face.sprite = nowIdle;

        ScheduleNextBlink();
    }

    private void ScheduleNextBlink()
    {
        nextBlinkAt = Time.time + Random.Range(minInterval, maxInterval);
    }

    private void ApplyBodySprites()
    {
        switch (GameManager.Instance.GetBodySize())
        {
            case BodySize.Small:
                nowIdle = idle_S;
                nowBlink = blink_S;
                break;
            case BodySize.Medium:
                nowIdle = idle_M;
                nowBlink = blink_M;
                break;
            case BodySize.Large:
                nowIdle = idle_L;
                nowBlink = blink_L;
                break;
        }

        // 사이즈 변경 시 처리 정책:
        // - 깜박이는 중이면: 다음 프레임부터 새 배열로 재생(단, 길이/널 방어는 Update에서)
        // - 깜박이는 중이 아니면: 즉시 Idle 반영
        if (!isBlinking)
            face.sprite = nowIdle;
    }
}

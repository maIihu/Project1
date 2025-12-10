using System;
using UnityEngine;
using System.Collections; // Cần dùng cho Coroutine

public class DamageFlashController : MonoBehaviour
{
    [SerializeField] private float flashDuration = 0.25f; 
    
    [SerializeField] private float flashSpeed = 10f; 
    
    private SpriteRenderer spriteRenderer;
    private Material flashMaterial; 
    private Coroutine flashRoutine;

    private readonly int FlashAmountID = Shader.PropertyToID("_FlashAmount");

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashMaterial = spriteRenderer.material; 
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) StartFlash();
    }
    
    public void StartFlash()
    {
        // Ngăn Coroutine trước nếu đang chạy
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        // Bắt đầu Coroutine mới
        flashRoutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        float startTime = Time.time;

        flashMaterial.SetFloat(FlashAmountID, 1f); 

        yield return new WaitForSeconds(flashDuration); 
        

        flashMaterial.SetFloat(FlashAmountID, 0f); 

        // Tùy chọn: Tắt dần (Dampen)
        
        float timer = 0f;
        while (timer < flashDuration)
        {
            timer += Time.deltaTime * flashSpeed;
            // Dùng Lerp để giảm dần FlashAmount từ 1 xuống 0
            float flashValue = Mathf.Lerp(1f, 0f, timer);
            flashMaterial.SetFloat(FlashAmountID, flashValue);
            yield return null;
        }
        flashMaterial.SetFloat(FlashAmountID, 0f);
        

        flashRoutine = null;
    }

    private void OnDisable()
    {
        if (flashMaterial != null)
        {
            flashMaterial.SetFloat(FlashAmountID, 0f);
        }
    }
}
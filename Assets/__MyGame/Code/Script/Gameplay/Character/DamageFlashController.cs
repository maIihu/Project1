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
        flashMaterial.SetColor("_FlashColor", new Color(1f, 0.3f, 0.3f)); 

    }
    
    public void StartFlash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        float startTime = Time.time;

        flashMaterial.SetFloat(FlashAmountID, 1f); 

        yield return new WaitForSeconds(flashDuration); 
        

        flashMaterial.SetFloat(FlashAmountID, 0f); 

        
        float timer = 0f;
        while (timer < flashDuration)
        {
            timer += Time.deltaTime * flashSpeed;
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
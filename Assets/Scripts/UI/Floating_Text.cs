using UnityEngine;
using NaughtyAttributes;
using TMPro;

public class Floating_Text : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float fadeInTime = 0.2f;
    public float fadeOutTime = 1f;
    public float lifeTime = 2f;

    private TextMeshPro textMesh;
    private Color originalColor;

    private float timer = 0f;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        originalColor = textMesh.color;
        SetAlpha(0f);
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Movimiento hacia arriba
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // Fade In
        if (timer < fadeInTime)
        {
            float alpha = Mathf.Lerp(0, 1, timer / fadeInTime);
            SetAlpha(alpha);
        }
        // Fade Out
        else if (timer > lifeTime - fadeOutTime)
        {
            float alpha = Mathf.Lerp(1, 0, (timer - (lifeTime - fadeOutTime)) / fadeOutTime);
            SetAlpha(alpha);
        }

        // Destruir al final
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void SetConfiguration(float floatSpeed, float fadeInTime, float fadeOutTime, float lifeTime) 
    {
        this.floatSpeed = floatSpeed;
        this.fadeInTime = fadeInTime;
        this.fadeOutTime = fadeOutTime;
        this.lifeTime = lifeTime;
}

    public void SetText(string message, Color color)
    {
        textMesh.text = message;
        originalColor = color;
        SetAlpha(0f);
    }

    private void SetAlpha(float alpha)
    {
        Color c = originalColor;
        c.a = alpha;
        textMesh.color = c;
    }
}

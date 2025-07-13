using UnityEngine;

public class Note : MonoBehaviour
{
    public string direction;
    public float speed = 5f;
    public bool canBePressed = false;

    public float lifeTime = 5f;
    private float lifeTimer = 0f;

    private void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            if (canBePressed && GameManager.instance != null)
            {
                GameManager.instance.MissNote();
            }

            if (GameManager.instance != null)
                GameManager.instance.UnregisterNote(this);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitZone"))
        {
            canBePressed = true;
            GameManager.instance?.RegisterNote(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("HitZone"))
        {
            if (canBePressed)
                GameManager.instance?.MissNote();

            canBePressed = false;
            GameManager.instance?.UnregisterNote(this);
            Destroy(gameObject);
        }
    }
}

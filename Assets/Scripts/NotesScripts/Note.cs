using UnityEngine;

public class Note : MonoBehaviour
{
    public string direction;           // The direction this note represents 
    public float speed = 5f;
    public bool canBePressed = false;
    public float lifeTime = 5f;
    private float lifeTimer = 0f;
    public bool resolved = false;

    void Start()
    {
        speed = Note_Data.speed; // Set the speed from Note_Data
    }
    private void Update()
    {
        // Move the note downwards
        transform.position += Vector3.down * speed * Time.deltaTime;

        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime && !resolved)
        {
            // If the note could be pressed, count as a miss
            if (canBePressed && GameManager.instance != null)
            {
                GameManager.instance.MissNote();
            }

            // Unregister the note from the GameManager
            if (GameManager.instance != null)
                GameManager.instance.UnregisterNote(this);

            // Destroy the note object
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitZone"))
        {
            canBePressed = true; // The note can now be pressed
            GameManager.instance?.RegisterNote(this); // Register the note as active
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If the note leaves the "HitZone" and hasn't been resolved
        if (collision.CompareTag("HitZone") && !resolved)
        {
            // If it could be pressed, count as a miss
            if (canBePressed)
                GameManager.instance?.MissNote();

            canBePressed = false; // The note can no longer be pressed
            GameManager.instance?.UnregisterNote(this); // Unregister the note
            Destroy(gameObject);
        }
    }
}

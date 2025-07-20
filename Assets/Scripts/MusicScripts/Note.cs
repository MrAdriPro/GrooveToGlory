using UnityEngine;

public class Note : MonoBehaviour
{
    public static float speed = 5f;
    public bool canBePressed = false;
    private float lifeTimer = 0f;
    public bool resolved = false;
    public Note_SO note;

    void Start()
    {
        speed = Note_Data.speed; // Set the speed from Note_Data
    }
    private void Update()
    {
        // Move the note downwards
        transform.position += Vector3.down * speed * Time.deltaTime;

        lifeTimer += Time.deltaTime;
        if (lifeTimer >= note.lifeTime && !resolved)
        {
            resolved = true;
            // If the note could be pressed, count as a miss
            if (canBePressed && FightManager.instance != null)
            {
                FightManager.instance.MissNote(this);
            }

            // Unregister the note from the GameManager
            if (FightManager.instance != null)
                FightManager.instance.UnregisterNote(this);

            // Destroy the note object
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitZone"))
        {
            
                canBePressed = true; // The note can now be pressed
                FightManager.instance?.RegisterNote(this); // Register the note as active
            

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If the note leaves the "HitZone" and hasn't been resolved
        if (collision.CompareTag("HitZone") && !resolved)
        {
            resolved = true;

            if (!note.isDangerous)
            {
                if (canBePressed)
                {
                    FightManager.instance?.MissNote(this);
                }

                FightManager.instance?.UnregisterNote(this);
            }

            Destroy(gameObject);
        }
    }
}

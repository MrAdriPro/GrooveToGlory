using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject leftNotePrefab;
    public GameObject downNotePrefab;
    public GameObject upNotePrefab;
    public GameObject rightNotePrefab;

    public Transform leftSpawn;
    public Transform downSpawn;
    public Transform upSpawn;
    public Transform rightSpawn;

    public float interval = 2f;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            SpawnRandomNote();
            timer = 0f;
        }
    }

    void SpawnRandomNote()
    {
        int dir = Random.Range(0, 4);
        switch (dir)
        {
            case 0:
                Instantiate(leftNotePrefab, leftSpawn.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(downNotePrefab, downSpawn.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(upNotePrefab, upSpawn.position, Quaternion.identity);
                break;
            case 3:
                Instantiate(rightNotePrefab, rightSpawn.position, Quaternion.identity);
                break;
        }
    }
}

using UnityEngine;
using NaughtyAttributes;

public class NoteSpawner : MonoBehaviour
{
    [Header("Note Prefabs")]
    public GameObject notePrefabLeft;
    public GameObject notePrefabDown;
    public GameObject notePrefabUp;
    public GameObject notePrefabRight;

    public float interval = 2f;
    private float timer;

    [BoxGroup("Spawners")]
    public Transform leftSpawn;
    [BoxGroup("Spawners")]
    public Transform rightSpawn;
    [BoxGroup("Spawners")]
    public Transform upSpawn;
    [BoxGroup("Spawners")]
    public Transform downSpawn;



    private void Update()
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
                Instantiate(notePrefabLeft, leftSpawn.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(notePrefabDown, downSpawn.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(notePrefabUp, upSpawn.position, Quaternion.identity);
                break;
            case 3:
                Instantiate(notePrefabRight, rightSpawn.position, Quaternion.identity);
                break;
        }
    }
    void SpawnNote(GameObject prefab, Transform spawnPoint, string direction)
    {
        GameObject note = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        Note noteScript = note.GetComponent<Note>();
        noteScript.direction = direction;
    }

}


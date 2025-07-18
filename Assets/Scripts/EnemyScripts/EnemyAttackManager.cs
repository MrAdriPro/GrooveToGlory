using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttackManager : MonoBehaviour
{

    private NoteSpawner spawner;
    

    public void Initialize(NoteSpawner noteSpawner)
    {
        spawner = noteSpawner;
    }
    public void TriggerEnemyEffect()
    {
        switch (spawner.enemyData.enemyType)
        {
            case EnemyData.EnemyType.Skeleton:
                LaunchBoneAttack();
                break;
            case EnemyData.EnemyType.Slime:
                StartCoroutine(LaunchSlimeAttack());
                break;
            case EnemyData.EnemyType.Zombie:
                LaunchZombieAttack();
                break;

        }
    }

    #region SpecialAttacks
    void LaunchBoneAttack()
    {
        int randomLane = Random.Range(0, spawner.spawnPoints.Length);

        if (spawner.boneNotePrefab != null && spawner.spawnPoints[randomLane] != null)
        {
            spawner.skipNextNote = true;
            GameObject boneNote = Instantiate(spawner.boneNotePrefab, spawner.spawnPoints[randomLane].position, Quaternion.identity);

            Note noteComponent = boneNote.GetComponent<Note>();
            if (noteComponent != null)
            {
                noteComponent.note.isDangerous = true;

                switch (randomLane)
                {
                    case 0:
                        noteComponent.note.direction = NoteDirection.Down;
                        break;
                    case 1:
                        noteComponent.note.direction = NoteDirection.Up;
                        break;
                    case 2:
                        noteComponent.note.direction = NoteDirection.Left;
                        break;
                    case 3:
                        noteComponent.note.direction = NoteDirection.Right;
                        break;
                }
            }
            FightManager.instance.RegisterNote(noteComponent);
        }


    }

    IEnumerator LaunchSlimeAttack()
    {
        spawner.slimeOverlay.SetActive(true);
        spawner.slimeCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(2f);

        float fadeDuration = 1.5f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            spawner.slimeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        spawner.slimeCanvasGroup.alpha = 0f;
        spawner.slimeOverlay.SetActive(false);
    }

    void LaunchZombieAttack()
    {
        //poner la velocidad de la nota en vez de bpm
        int[] speedOptions = {  10,  8, 13 };
        int newspeed = speedOptions[Random.Range(0, speedOptions.Length)];
        print($"cambio de speed a {newspeed}");
        Note.speed = newspeed;
    }
    #endregion
}

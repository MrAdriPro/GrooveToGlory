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
        spawner.nextNoteIsBone = true;
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
        int[] options = spawner.enemyData.speedOptions;

        int newspeed = options[Random.Range(0, options.Length)];
        print($"cambio de speed a {newspeed}");
        Note.speed = newspeed;
    }
    #endregion
}

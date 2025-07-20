using UnityEngine;
using NaughtyAttributes;
using System.Collections;

public class Text_Spawner : MonoBehaviour
{
    public static Text_Spawner instance;

    [BoxGroup("Floating text configuration")]
    public Transform position;
    [BoxGroup("Floating text configuration")]
    public Vector3 offset;
    [BoxGroup("Floating text configuration")]
    public float floatSpeed = 1f;
    [BoxGroup("Floating text configuration")]
    public float fadeInTime = 0.2f;
    [BoxGroup("Floating text configuration")]
    public float fadeOutTime = 1f;
    [BoxGroup("Floating text configuration")]
    public float lifeTime = 2f;
    [BoxGroup("Floating text configuration")]
    public string message = string.Empty;
    [BoxGroup("Floating text configuration")]
    public Color textColor = Color.white;
    [BoxGroup("Floating text configuration")]
    private Transform playerBody;
    [BoxGroup("Floating text configuration")]
    public GameObject floatingTextPrefab;


    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
    }
    private void Start()
    {
        playerBody = GameObject.FindGameObjectWithTag(Tags.PLAYER_BODY_TAG).transform;
    }

    public IEnumerator SpawnFloatingText(Vector3 position, string message, Color color, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject instance = Instantiate(floatingTextPrefab, position, Quaternion.identity);
        Floating_Text ft = instance.GetComponent<Floating_Text>();
        ft.SetConfiguration(floatSpeed, fadeInTime, fadeOutTime, lifeTime);
        ft.SetText(message, color);
    }

    
}

using Unity.VisualScripting;
using UnityEngine;

public class EnemyCombatTrigger : MonoBehaviour
{
    [Header("Combat Trigger")]

    public float startCombatDistance = 5f;

    private Transform player;
    private EnemyHealth enemyHealth;
    private Player_Movement playerMovement;
    private EnemyRandomMovement enemyRandomMovement;
    [SerializeField] private float zoomInFOV = 30f;
    [SerializeField] private float zoomOutFOV = 78f;
    [SerializeField] private float zoomDuration = 1.5f;

    void Start()
    {
        enemyRandomMovement = GetComponent<EnemyRandomMovement>();
        playerMovement = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<Player_Movement>();
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER_BODY_TAG).GetComponent<Transform>();
        enemyHealth = GetComponent<EnemyHealth>();
    }


    // Update is called once per frame
    void Update()
    {
        if (player == null || enemyHealth == null)
            return;
        //El distance squared hace lo mismo que el distance es decir hace sus raices cuadradas y tal, pero es mas eficiente porque evita más operaciones
        float distance = Extensions.DistanceSquared(transform.position, player.transform.position);
        // al evitarte otros calculos cuando pongas la distancia la tienes que multiplicar por si misma
        if (distance <= startCombatDistance * startCombatDistance)
        {
            playerMovement.enabled = false; // Disable player movement during combat
            enemyRandomMovement.agent.isStopped = true; // Stop enemy random movement

            // Set the current enemy in the FightManager
            FightManager.instance.currentEnemy = enemyHealth;

            // Start the combat
            FightManager.instance.StartCoroutine(FightManager.instance.ZoomEffectBeforeCombat(zoomOutFOV, zoomInFOV, zoomDuration, enemyHealth.data));
        }
    }
}

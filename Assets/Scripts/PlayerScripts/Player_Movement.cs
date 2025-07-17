using UnityEngine;
using NaughtyAttributes;

public class Player_Movement : MonoBehaviour
{
    //Variables

    [Header("Stats")]
    [BoxGroup("Stats")]
    [SerializeField] private float speed = 5f;

#pragma warning disable
    [Space(10)]
    [BoxGroup("Tags")]
    [SerializeField] bool ChanceTags = false;
#pragma warning restore

    [BoxGroup("Tags")]
    [ShowIf("ChanceTags")]
    [SerializeField] private string camTag = Tags.CAMERA_TAG;

    [BoxGroup("Tags")]
    [ShowIf("ChanceTags")]
    [SerializeField] private string playerBodyTag = Tags.PLAYER_BODY_TAG;

    [Header("Transforms")]
    [SerializeField] private Transform playerCam = null;
    [SerializeField] private Transform playerBody = null;


    [Header("Other components")]
    private Rigidbody rb;
    private Vector3 movement;
    //Functions


    private void Start()
    {
        SetStartConfiguration();
    }

    /// <summary>
    /// Generacion de la configuracion al iniciar
    /// </summary>
    private void SetStartConfiguration() 
    {
        playerCam = GameObject.FindGameObjectWithTag(camTag).GetComponent<Transform>();
        playerBody = GameObject.FindGameObjectWithTag(playerBodyTag).GetComponent<Transform>();
        rb = playerBody.GetComponent<Rigidbody>();

    }

    private void Update()
    {
        Movement();
    }

    void FixedUpdate()
    {
        if (!rb) return;

        Vector3 newPosition = rb.position + movement * (speed + Player_Stats.speed) * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    /// <summary>
    /// Movimiento del jugador en base a la camara
    /// </summary>
    private void Movement() 
    {
        if (!playerCam || !playerBody) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Direcci�n basada en c�mara
        Vector3 camForward = playerCam.forward;
        Vector3 camRight = playerCam.right;

        // Ignora la inclinaci�n vertical de la c�mara
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Movimiento relativo a la c�mara
        Vector3 direction = (camForward * v + camRight * h).normalized;

        // Cambiar la rotaci�n del cuerpo en base al movimiento relativo a la c�mara
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerBody.transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        }

        // Guardar la direcci�n final para mover al personaje
        movement = direction;

        Debug.Log($"H: {h}, V: {v}");
    }

}

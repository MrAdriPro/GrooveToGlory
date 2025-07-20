using UnityEngine;
using NaughtyAttributes;

public class ItemDrop : MonoBehaviour, I_ItemDrop
{

    #region Variables
    [BoxGroup("Item Drop Settings")]
    public GameObject VFX;
    public Collider itemCollider;
    [HideInInspector] public Transform startPos;

    [Header("Private Variables")]
    private Items item;
    private Vector3 endPos;
    private float timer = 0f;
    private bool animating = false;
    private Rigidbody rb;
    private Collider playerBody = null;

    #endregion

    #region Functions

    private void Start() => SetStartingAttributes();

    private void Update() => AnimationController();


    /// <summary>
    /// Metodo que inicia las variables necesarias
    /// </summary>
    public void SetStartingAttributes() 
    {
        playerBody = GameObject.FindGameObjectWithTag(Tags.PLAYER_BODY_TAG).GetComponent<Collider>();
        Physics.IgnoreCollision(itemCollider, playerBody, true);
    }

    /// <summary>
    /// Metodo que inicia todo los datos necesarios para la animacion
    /// </summary>
    public void SetStartingDropSettings(Items item) => SetRandomPosition(item);

    /// <summary>
    /// Establece una posicion aleatoria al dropear un item
    /// </summary>
    /// <param name="item"></param>
    public void SetRandomPosition(Items item) 
    {
        //Establece los atributos iniciales
        startPos.position = startPos.position;
        this.item = item;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        #region Random Position
        float x = 0;
        float z = 0;

        for (int i = 0; i < 100; i++)
        {
            if (x >= -ItemDropConfig.instance.minOffsetX.x && x <= ItemDropConfig.instance.minOffsetX.y)
                x = Random.Range(-ItemDropConfig.instance.xOffsetRange.x, ItemDropConfig.instance.xOffsetRange.y);
            else break;

        }

        for (int i = 0; i < 100; i++)
        {
            if (z >= -ItemDropConfig.instance.minOffsetZ.x && z <= ItemDropConfig.instance.minOffsetZ.y)
                z = Random.Range(-ItemDropConfig.instance.xOffsetRange.x, ItemDropConfig.instance.xOffsetRange.y);
            else break;
        }
        #endregion

        // Crea la posicion final y reiniciar varios atributos
        ItemDropConfig.instance.offset = new Vector3(x, 0, z);
        endPos = startPos.position + ItemDropConfig.instance.offset;
        timer = 0f;
        animating = true;
    }

    /// <summary>
    /// Metodo que controla la animacion de la caida del objeto
    /// </summary>
    public void AnimationController() 
    {
        if (!animating) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / ItemDropConfig.instance.duration);

        float height = ItemDropConfig.instance.heightCurve.Evaluate(t) * ItemDropConfig.instance.maxHeight;
        transform.position = Vector3.Lerp(startPos.position, endPos, t) + Vector3.up * height;

        if (t >= 1f)
        {
            rb.isKinematic = false; // activa la física al terminar
            animating = false;
            VFX = RarityController.instance.CreateVFX(this.transform,item.rarity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!animating) return;

        // Si colisiona antes de terminar la animación
        animating = false;
        rb.isKinematic = false;
        VFX = RarityController.instance.CreateVFX(this.transform, item.rarity);
    }

    #endregion
}


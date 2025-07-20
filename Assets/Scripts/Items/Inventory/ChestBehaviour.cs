using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class ChestBehaviour : MonoBehaviour, I_GenericMethods, I_ChestBehaviour
{
    #region Variables

    [BoxGroup("Chest variables")]
    [SerializeField] private Chest chestInfo;
    [BoxGroup("Chest variables")]
    [SerializeField] private float itemSpawnDelay = 0.2f;
    [BoxGroup("Chest variables")]
    [HideInInspector] public bool canBeInteracted = false;
    [BoxGroup("Chest variables")]
    [HideInInspector] public bool opened = false;
    [BoxGroup("Chest variables")]
    [SerializeField] private SpriteRenderer chestVisual = null;
    [Header("Private Variables")]
    private Collider playerBody = null;

    #endregion

    #region Functions

    private void Start() => SetStartingAttributes();

    private void Update() => InputController();

    /// <summary>
    /// Establece las variables de inicio
    /// </summary>
    public void SetStartingAttributes() 
    {
        chestVisual = transform.GetChild(0).GetComponent<SpriteRenderer>();
        playerBody = GameObject.FindGameObjectWithTag(Tags.PLAYER_BODY_TAG).GetComponent<Collider>();
        chestVisual.sprite = chestInfo.chestSprite;
    }

    /// <summary>
    /// Controlador de los inputs
    /// </summary>
    public void InputController()
    {
        if (!this) return;

        //Si se puede abrir y no se ha abierto en cofre
        if (canBeInteracted && !opened && !Extensions.Item_Indicator)
            Extensions.Chest_Indicator = true;

        if (Input.GetKeyDown(Player_Inputs.LeftClick))
            if (canBeInteracted && !opened) OpenChest();
    }

    /// <summary>
    /// Metodo que abre el cofre
    /// </summary>
    public void OpenChest() 
    {
        Extensions.Chest_Indicator = false;
        chestVisual.GetComponent<Animator>().SetTrigger("Open");
        StartCoroutine(SpawnItems());
    }

    /// <summary>
    /// Metodo que instancia de manera aleatoria los items genericos al abrir el cofre
    /// </summary>
    /// <returns></returns>
    public IEnumerator SpawnItems() 
    {
        #region Variables
        opened = true;
        int randItemPos = 0;
        int randomPer = 0;
        List<int?> CheckPositions = new List<int?>();
        #endregion
        yield return new WaitForSeconds(chestInfo.chestOpenTime);
        #region Normal Items Spawn
        for (int i = 0; i < chestInfo.chestCapacity; i++)
        {
            randItemPos = Random.Range(0,chestInfo.chestValues.items.Length);
            randomPer = Random.Range(0, 100);

            if (CheckPositions.Find(pos => pos.Equals(randItemPos)) == null)
            {
                if (randomPer <= chestInfo.chestValues.percentage[randItemPos]) 
                {
                    CheckPositions.Add(randItemPos);
                    CreateItem(chestInfo.chestValues.items[randItemPos], 0);
                    yield return new WaitForSeconds(itemSpawnDelay);
                }
            }
        }
        #endregion

        #region Currencies Spawn

        //Reinicia las variables
        CheckPositions.Clear();
        randItemPos = 0;
        randomPer = 0;
        int randAmount = 0;
        //Recorre las posibles instancias para spawnear
        for (int i = 0; i < chestInfo.chestValues.currencies.Length; i++)
        {
            randItemPos = Random.Range(0, chestInfo.chestValues.currencies.Length);
            randomPer = Random.Range(0, 100);
            randAmount = Random.Range(1, chestInfo.chestValues.maxAmount[randItemPos]);

            if (CheckPositions.Find(pos => pos.Equals(randItemPos)) == null)
            {
                if (randomPer <= chestInfo.chestValues.currencyPercentage[randItemPos])
                {
                    CheckPositions.Add(randItemPos);
                    CreateItem(chestInfo.chestValues.currencies[randItemPos], randAmount);
                    yield return new WaitForSeconds(itemSpawnDelay);
                }
            }

        }
        #endregion
    }


    /// <summary>
    /// Metodo que crea un item en el mundo con su configuracion necesaria
    /// </summary>
    /// <param name="item"></param>
    public void CreateItem(Items item, int amount)
    {
        //Crea el objeto y recoge su informacion
        GameObject instantiatedObject = instantiatedObject = Instantiate(item.itemPrefab, this.transform);
        SetItemDropSettings(instantiatedObject.GetComponent<ItemDrop>(), item);
        GenericItemHolder gih = instantiatedObject.GetComponent<GenericItemHolder>();

        //Si existe por algun casual GenericItemHolder lo elimina, ya que establecemos uno nosotros
        if (gih != null) Destroy(gih);

        //Establecemos la configuracion del GenericItemHolder
        gih = instantiatedObject.AddComponent<GenericItemHolder>();
        gih.item = item;

        if (gih.item is Currency) gih.currencyAmount = amount;

    }


    /// <summary>
    /// Establece las propiedades del scrip ItemDrop
    /// </summary>
    /// <param name="itemDropSettings"></param>
    /// <param name="item"></param>
    public void SetItemDropSettings(ItemDrop itemDropSettings, Items item) 
    {
        Physics.IgnoreCollision(itemDropSettings.itemCollider, playerBody, true);
        itemDropSettings.startPos = this.transform;
        itemDropSettings.SetStartingDropSettings(item);
    }

    #endregion

}



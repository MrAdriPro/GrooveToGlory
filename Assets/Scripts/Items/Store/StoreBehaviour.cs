using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine.UIElements;


public class StoreBehaviour : MonoBehaviour, I_StoreController
{
    #region Variables
    [BoxGroup("Store configuration")]
    public Store storeConfig;
    [BoxGroup("Store configuration")]
    public List<Transform> storeItemPositions = new List<Transform>();
    #endregion

    #region Functions
    private void Start() => SetStartingAttributes();

    /// <summary>
    /// Establece las variables de inicio
    /// </summary>
    public void SetStartingAttributes() 
    {
        SpawnStoreItems();
    }

    /// <summary>
    /// Crea los items de la tienda
    /// </summary>
    public void SpawnStoreItems() 
    {
        int randomPos = 0;
        foreach (var item in storeConfig.buyableItems)
        {

            GameObject instantiatedObject = Instantiate(item.item.itemPrefab, 
                                            storeItemPositions[randomPos].position, Quaternion.identity, storeItemPositions[randomPos]);

            Physics.IgnoreCollision(instantiatedObject.GetComponent<ItemDrop>().itemCollider, 
                                    GameObject.FindGameObjectWithTag(Tags.PLAYER_BODY_TAG).GetComponent<Collider>(), true);

            GenericItemHolder gih = instantiatedObject.GetComponent<GenericItemHolder>();
            if (gih != null) Destroy(gih);

            gih = instantiatedObject.AddComponent<GenericItemHolder>();
            gih.item = item.item;
            gih.currencyToUse = item.currencyUsed;
            gih.buyAmount = item.itemPrice;
            instantiatedObject.GetComponent<ItemDrop>().VFX =
                RarityController.instance.CreateVFX(storeItemPositions[randomPos], item.item.rarity);
        
            randomPos++;
        }
    }

    #endregion
}



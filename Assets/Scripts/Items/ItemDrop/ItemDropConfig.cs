using UnityEngine;
using NaughtyAttributes;

public class ItemDropConfig : MonoBehaviour
{

    #region Variables

    [Header("Instancia")]
    public static ItemDropConfig instance;
    [BoxGroup("Drop settings")]
    public AnimationCurve heightCurve;
    [BoxGroup("Drop settings")]
    public float duration = 1f;
    [BoxGroup("Drop settings")]
    public float maxHeight = 2f;
    [BoxGroup("Drop settings")]
    public Vector3 offset = Vector3.zero;
    [BoxGroup("Drop settings")]
    [Tooltip("Valor minimo: MinOffset")]
    public Vector2 xOffsetRange;
    [BoxGroup("Drop settings")]
    [Tooltip("Valor minimo: MinOffset")]
    public Vector2 ZOffsetRange;
    [BoxGroup("Drop settings")]
    public Vector2 minOffsetX;
    [BoxGroup("Drop settings")]
    public Vector2 minOffsetZ;

    #endregion

    #region Functions

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
    }

    private void OnValidate()
    {
        // Forzar que ZOffsetRange.x nunca sea menor que minOffset.x
        ZOffsetRange.x = Mathf.Clamp(ZOffsetRange.x, minOffsetZ.x + 0.5f, 10f);
        ZOffsetRange.y = Mathf.Clamp(ZOffsetRange.y, minOffsetZ.y + 0.5f, 10f);
        // Forzar que ZOffsetRange.x nunca sea menor que minOffset.x
        xOffsetRange.x = Mathf.Clamp(xOffsetRange.x, minOffsetX.x + 0.5f, 10f);
        xOffsetRange.y = Mathf.Clamp(xOffsetRange.y, minOffsetX.y + 0.5f, 10f);
    }

    #endregion

}

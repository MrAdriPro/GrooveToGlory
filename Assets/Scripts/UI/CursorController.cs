using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

public class CursorController : MonoBehaviour, I_CursorController
{
    #region Variables

    [SerializeField] private Animator cursorAnim = null;
    #endregion

    #region Functions
    private void Start() => SetStartingAttributes();

    private void Update() => MouseCursor();


    /// <summary>
    /// Establece los atributos de inicio
    /// </summary>
    public void SetStartingAttributes() 
    {
        Cursor.visible = false;
        cursorAnim = Hud_Controller.Instance.mouseCursor.GetComponent<Animator>();
    }

    /// <summary>
    /// Mueve el cursor donde estaria el nuestro
    /// </summary>
    public void MouseCursor()
    {
        if (Input.GetKeyDown(Player_Inputs.LeftClick)) SetCursor(CursorType.Use);
        else if (Extensions.Item_Indicator || Extensions.Chest_Indicator) SetCursor(CursorType.Hover);
        else SetCursor(CursorType.Normal);

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Hud_Controller.Instance.mouseCursor.GetComponentInParent<Canvas>().transform as RectTransform,
            Input.mousePosition,
        Hud_Controller.Instance.mouseCursor.GetComponentInParent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay ? null : Hud_Controller.Instance.mouseCursor.GetComponentInParent<Canvas>().worldCamera,
        out pos
        );
        Vector2 offset = new Vector2(5, -10f);
        Vector2 UseOffset = new Vector2(200, -100f);

        Hud_Controller.Instance.mouseCursor.GetComponent<RectTransform>().anchoredPosition = pos + offset;
    }

    /// <summary>
    /// Pone el boton de interaccion a activo
    /// </summary>
    /// <param name="active"></param>
    public void SetCursor(CursorType cursor)
    {
        switch (cursor)
        {
            case CursorType.Normal:
                cursorAnim.SetBool("NormalCursor", true);
                cursorAnim.SetBool("HoverCursor", false);
                cursorAnim.SetBool("UseCursor", false);
                break;
            case CursorType.Use:
                cursorAnim.SetBool("UseCursor", true);
                cursorAnim.SetBool("NormalCursor", false);
                cursorAnim.SetBool("HoverCursor", false);
                break;
            case CursorType.Hover:
                cursorAnim.SetBool("HoverCursor", true);
                cursorAnim.SetBool("NormalCursor", false);
                cursorAnim.SetBool("UseCursor", false);

                break;
            default:
                break;
        }
    }

    #endregion
}

using UnityEngine;
using NaughtyAttributes;

public static class Player_Inputs
{
    //Variables

    [Header("Camera Variables")]
    [Foldout("Camera Variables")]
    public static float Mouse_Sens = 0f;

    [Foldout("Camera Variables")]
    public static float smoothRot = 2f;

    [Foldout("Camera Variables")]
    public static Vector3 camOffset = new Vector3(5f, 7f, -7f);

    [Space(10)]
    [Header("Inputs")]
    [Foldout("Inputs")]
    public static KeyCode Move_Forward = KeyCode.W;
    public static KeyCode Move_Right = KeyCode.D;
    public static KeyCode Move_Backwards = KeyCode.S;
    public static KeyCode Move_Left = KeyCode.A;
    public static KeyCode Interact = KeyCode.E;
    public static KeyCode LeftClick = KeyCode.Mouse0;
    public static KeyCode RightClick = KeyCode.Mouse1;

    public static KeyCode Consumable1 = KeyCode.Alpha1;
    public static KeyCode Consumable2 = KeyCode.Alpha2;
    public static KeyCode OpenInventory = KeyCode.Tab;
    public static KeyCode PauseMenu = KeyCode.Escape;

    //Functions




}

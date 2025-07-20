using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class SpriteStack : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite[] slices;

    [Header("Stack Settings")]
    [Range(0.01f, 10f)]
    public float heightOffset = 1f;

    [Range(0f, 360f)]
    public float cameraAngle = 45f;

    public float zOffset = 0f;

    [Header("Rotation")]
    [Range(0f, 360f)]
    public float rotation = 0f;

    private List<Transform> sliceTransforms = new List<Transform>();

    private float lastHeightOffset;
    private float lastCameraAngle;
    private float lastZOffset;
    private float lastRotation;

    private void Start()
    {
        DrawStack();
        StoreCurrentValues();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetStack();
        }

        if (ValuesChanged())
        {
            ResetStack();
            StoreCurrentValues();
        }

        UpdateRotation();
    }

    void ResetStack()
    {
        ClearStack();
        DrawStack();
    }

    void ClearStack()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        sliceTransforms.Clear();
    }

    void DrawStack()
    {
        float angleRad = cameraAngle * Mathf.Deg2Rad;
        float dcos = Mathf.Cos(angleRad);
        float dsin = -Mathf.Sin(angleRad);

        for (int i = 0; i < slices.Length; i++)
        {
            float dist = i + zOffset;
            float offsetX = dist * dcos * heightOffset;
            float offsetY = dist * dsin * heightOffset;

            GameObject sliceObj = new GameObject($"Slice_{i}");
            sliceObj.transform.SetParent(transform);
            sliceObj.transform.localPosition = new Vector3(-offsetX, -offsetY, 0);

            SpriteRenderer renderer = sliceObj.AddComponent<SpriteRenderer>();
            renderer.sprite = slices[i];
            renderer.sortingOrder = i;

            sliceTransforms.Add(sliceObj.transform);
        }

        UpdateRotation();
    }

    void UpdateRotation()
    {
        Quaternion rot = Quaternion.Euler(0f, 0f, rotation);
        foreach (Transform t in sliceTransforms)
        {
            t.localRotation = rot;
        }
    }

    bool ValuesChanged()
    {
        return heightOffset != lastHeightOffset ||
               cameraAngle != lastCameraAngle ||
               zOffset != lastZOffset;
    }

    void StoreCurrentValues()
    {
        lastHeightOffset = heightOffset;
        lastCameraAngle = cameraAngle;
        lastZOffset = zOffset;
        lastRotation = rotation;
    }

    [Button("Guardar valores desde Play Mode")]
    private void GuardarValores()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;

        // Guardar propiedades del script
        SerializedObject so = new SerializedObject(this);
        so.FindProperty("heightOffset").floatValue = heightOffset;
        so.FindProperty("cameraAngle").floatValue = cameraAngle;
        so.FindProperty("zOffset").floatValue = zOffset;
        so.FindProperty("rotation").floatValue = rotation;
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(this);

        // Guardar la rotación del objeto padre (transform)
        Undo.RecordObject(transform, "Guardar rotación desde Play Mode");
        transform.rotation = transform.rotation; // reafirmar valor actual
        EditorUtility.SetDirty(transform);

        Debug.Log("🎉 ¡Valores y rotación del objeto guardados desde Play Mode!");
#endif
    }
}

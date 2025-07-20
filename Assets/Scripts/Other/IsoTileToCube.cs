using UnityEngine;
using NaughtyAttributes;

public class IsoTileToCube : MonoBehaviour
{
    public Sprite topSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    void Start()
    {
        CreateFace("Top", topSprite, new Vector3(0, 0.5f, 0), Quaternion.Euler(90, 0, 0));
        CreateFace("Left", leftSprite, new Vector3(-0.5f, 0, 0), Quaternion.Euler(0, 90, 0));
        CreateFace("Right", rightSprite, new Vector3(0.5f, 0, 0), Quaternion.Euler(0, -90, 0));
    }

    void CreateFace(string name, Sprite sprite, Vector3 localPos, Quaternion localRot)
    {
        GameObject face = new GameObject(name);
        face.transform.SetParent(transform);
        face.transform.localPosition = localPos;
        face.transform.localRotation = localRot;

        var mf = face.AddComponent<MeshFilter>();
        var mr = face.AddComponent<MeshRenderer>();
        mf.mesh = QuadMesh();


        UnityEngine.Material mat = new UnityEngine.Material(Shader.Find("Unlit/Transparent"));
        mat.mainTexture = sprite.texture;
        mr.material = mat;
    }

    Mesh QuadMesh()
    {
        Mesh m = new Mesh();
        m.vertices = new Vector3[] {
            new Vector3(-0.5f, -0.5f, 0),
            new Vector3( 0.5f, -0.5f, 0),
            new Vector3(-0.5f,  0.5f, 0),
            new Vector3( 0.5f,  0.5f, 0)
        };
        m.uv = new Vector2[] {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        m.triangles = new int[] { 0, 2, 1, 2, 3, 1 };
        m.RecalculateNormals();
        return m;
    }
}

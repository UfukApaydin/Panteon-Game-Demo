using System.Collections.Generic;
using UnityEngine;

public class InstancedSpriteRenderer : MonoBehaviour
{
    public Material instancedMaterial;
    public Texture2D spriteTexture;

    public List<Matrix4x4> transforms = new List<Matrix4x4>();
    public List<Vector4> colors = new List<Vector4>();
    private MaterialPropertyBlock propertyBlock;

    private int soldierCount;

    void Start()
    {
        instancedMaterial.SetTexture("_MainTex", spriteTexture);
        propertyBlock = new MaterialPropertyBlock();

        InitializeSoldiers(5);
    }

    public void InitializeSoldiers(int count)
    {
        soldierCount = count;
        transforms = new List<Matrix4x4>(count);
        colors = new List<Vector4>(count);

        for (int i = 0; i < count; i++)
        {
            transforms.Add(Matrix4x4.identity);
            colors.Add(new Vector4(1, 1, 1, 1)); // Default white color
        }
    }

    public void UpdateSoldierTransform(int index, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if (index < 0 || index >= soldierCount) return;

        transforms[index] = Matrix4x4.TRS(position, rotation, scale);
    }

    public void UpdateSoldierColor(int index, Color color)
    {
        if (index < 0 || index >= soldierCount) return;

        colors[index] = color;
    }

    void Update()
    {
        // Update GPU instance data
        propertyBlock.SetMatrixArray("_InstanceTransform", transforms.ToArray());
        propertyBlock.SetVectorArray("_InstanceColor", colors.ToArray());

        // Render instances
        Graphics.DrawMeshInstanced(
            MeshGenerator.Quad(),
            0,
            instancedMaterial,
            transforms.ToArray(),
            transforms.Count,
            propertyBlock
        );
    }

}

public static class MeshGenerator
{
    public static Mesh Quad()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f, 0),
            new Vector3(0.5f, -0.5f, 0),
            new Vector3(0.5f, 0.5f, 0),
            new Vector3(-0.5f, 0.5f, 0)
        };

        mesh.uv = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };

        mesh.triangles = new int[]
        {
            0, 1, 2,
            2, 3, 0
        };

        mesh.RecalculateNormals();
        return mesh;
    }
}


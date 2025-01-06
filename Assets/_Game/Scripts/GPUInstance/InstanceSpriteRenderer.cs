using System.Linq;
using UnityEngine;

public class InstancedSpriteRenderer : MonoBehaviour
{
    public Material instancedMaterial;
    public Texture2D spriteTexture;

    public int instanceCount = 1000;
    public Vector3[] positions;
    public Vector4[] colors;

    private Matrix4x4[] transforms;
    private MaterialPropertyBlock propertyBlock;

    void Start()
    {
        // Assign the texture to the material
        instancedMaterial.SetTexture("_MainTex", spriteTexture);

        // Initialize positions and colors
        positions = new Vector3[instanceCount];
        colors = new Vector4[instanceCount];
        transforms = new Matrix4x4[instanceCount];
        propertyBlock = new MaterialPropertyBlock();

        for (int i = 0; i < instanceCount; i++)
        {
            positions[i] = Random.insideUnitSphere * 10f;
            colors[i] = new Color(Random.value, Random.value, Random.value, 1.0f);

            // Create instance transform
            Matrix4x4 transform = Matrix4x4.TRS(
                positions[i],
                Quaternion.identity,
                Vector3.one * 0.5f
            );
            transforms[i] = transform;
        }
    }

    void Update()
    {
        // Update GPU instance data
        for (int i = 0; i < instanceCount; i++)
        {
            propertyBlock.SetVectorArray("_InstanceColor", colors);
            propertyBlock.SetMatrixArray("_InstanceTransform", transforms);
        }

        // Render instances
        Graphics.DrawMeshInstanced(
            MeshGenerator.Quad(), // A simple quad mesh
            0,
            instancedMaterial,
            transforms,
            instanceCount,
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


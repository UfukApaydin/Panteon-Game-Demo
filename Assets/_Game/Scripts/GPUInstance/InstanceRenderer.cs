using UnityEngine;

public class InstanceRenderer : MonoBehaviour
{
    public InstancedSpriteRenderer instanceRenderer;
    public int soldierIndex;

    private Vector3 position;
    private Quaternion rotation;
    private Vector3 scale;

    void Start()
    {
        position = transform.position;
        rotation = Quaternion.identity;
        scale = Vector3.one;

        // Initialize position in the instanceRenderer
        instanceRenderer.UpdateSoldierTransform(soldierIndex, transform.position, transform.localRotation, transform.localScale);
    }

    void Update()
    {
        // Update movement logic
       

        // Update instanceRenderer
        instanceRenderer.UpdateSoldierTransform(soldierIndex, transform.position, transform.localRotation, transform.localScale);
    }
}

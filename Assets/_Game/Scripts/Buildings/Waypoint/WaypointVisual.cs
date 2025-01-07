using ObjectPoolSystem;
using UnityEngine;

public class WaypointVisual : MonoBehaviour, IPoolable
{

    public GameObject GameObject => gameObject;

    PoolSystem _poolSystem;
    LineRenderer _lineRenderer;

    public void Init(PoolSystem poolSystem)
    {
        _poolSystem = poolSystem;
        _lineRenderer = GetComponent<LineRenderer>();

    }
    public void Activate()
    {
        gameObject.SetActive(true);
        _lineRenderer.enabled = true;
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
        _lineRenderer.enabled = false;
    }

    public void UpdateArgs(params object[] args)
    {
        _lineRenderer.SetPositions(new Vector3[] {transform.position, (Vector3)args[0] });
    }
}

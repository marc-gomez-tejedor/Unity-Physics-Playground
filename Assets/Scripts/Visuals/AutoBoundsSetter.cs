using UnityEngine;

/// <summary>
/// Automatically updates _ObjectMin and _ObjectMax shader properties
/// with the MeshRenderer's local-space bounds.
/// Attach this to any GameObject with a MeshRenderer + MeshFilter.
/// </summary>
[ExecuteAlways]
[DisallowMultipleComponent]
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class AutoBoundsSetter : MonoBehaviour
{
    private MeshRenderer _renderer;
    private MeshFilter _filter;
    private static readonly int ObjectMinID = Shader.PropertyToID("_ObjectMin");
    private static readonly int ObjectMaxID = Shader.PropertyToID("_ObjectMax");

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _filter = GetComponent<MeshFilter>();
        UpdateBounds();
    }

#if UNITY_EDITOR
    // Update in editor mode so it refreshes automatically
    private void OnValidate() => UpdateBounds();
    private void Update() => UpdateBounds();
#endif

    private void UpdateBounds()
    {
        if (!_renderer || !_filter || !_filter.sharedMesh) return;

        var mesh = _filter.sharedMesh;
        var bounds = mesh.bounds; // Local-space AABB of the mesh

        // Use a MaterialPropertyBlock so it doesn’t create new material instances
        var block = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(block);

        block.SetVector(ObjectMinID, bounds.min);
        block.SetVector(ObjectMaxID, bounds.max);

        _renderer.SetPropertyBlock(block);
    }
}

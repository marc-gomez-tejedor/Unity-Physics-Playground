using System;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum GravityType
{
    GravitySource = 1 << 0,
    GravityPlane = 1 << 1,
    GravityBox = 1 << 2,
    GravitySphere = 1 << 3,
    GravityIsle = 1 << 4,
    GravityCastedByPlayer = 1 << 5,
    GravityCentrifugalCylinder = 1 << 6,
    GravityRayByPlayer = 1 << 7,
}

/// <summary>
/// Optional struct for reusable gravity query presets.
/// Example:
///   static readonly GravityQuerySettings PlayerQuery = new(GravityType.GravityRayByPlayer, GravityType.GravityCastedByPlayer);
/// </summary>
[Serializable]
public readonly struct GravityQuerySettings
{
    public readonly GravityType ExcludeMask;
    public readonly GravityType IncludeMask;

    public GravityQuerySettings(GravityType excludeMask = 0, GravityType includeMask = 0)
    {
        ExcludeMask = excludeMask;
        IncludeMask = includeMask;
    }
}

public static class CustomGravity
{
    private static readonly List<GravitySource> sources = new List<GravitySource>();

    // Core unified logic
    private static Vector3 ComputeGravity(Vector3 position, GravityType exclude, GravityType include)
    {
        Vector3 g = Vector3.zero;

        for (int i = 0; i < sources.Count; i++)
        {
            var source = sources[i];
            var type = source.GravityType;

            // 1. Exclude mask
            if ((exclude & type) != 0)
                continue;

            // 2. Explicit include logic
            if (source.RequireExplicitInclude)
            {
                if ((include & type) == 0)
                    continue;
            }

            g += source.GetGravity(position);
        }

        return g;
    }

    //      --- Public API ---

    /// <summary>
    /// Unified gravity query. Includes all normal sources by default.
    /// - exclude: skips these gravity types.
    /// - include: adds explicitly-required gravity types.
    /// </summary>
    public static Vector3 GetGravity(Vector3 position, GravityType exclude = 0, GravityType include = 0)
        => ComputeGravity(position, exclude, include);

    public static Vector3 GetGravity(Vector3 position, out Vector3 upAxis, GravityType exclude = 0, GravityType include = 0)
    {
        Vector3 g = ComputeGravity(position, exclude, include);
        upAxis = g.sqrMagnitude > 0f ? -g.normalized : Vector3.up;
        return g;
    }

    /// <summary>
    /// Overload using a pre-defined GravityQuerySettings struct.
    /// </summary>
    public static Vector3 GetGravity(Vector3 position, in GravityQuerySettings settings)
        => ComputeGravity(position, settings.ExcludeMask, settings.IncludeMask);

    public static Vector3 GetGravity(Vector3 position, out Vector3 upAxis, in GravityQuerySettings settings)
    {
        Vector3 g = ComputeGravity(position, settings.ExcludeMask, settings.IncludeMask);
        upAxis = g.sqrMagnitude > 0f ? -g.normalized : Vector3.up;
        return g;
    }

    public static Vector3 GetUpAxis(Vector3 position, GravityType exclude = 0, GravityType include = 0)
    {
        Vector3 g = ComputeGravity(position, exclude, include);
        return g.sqrMagnitude > 0f ? -g.normalized : Vector3.up;
    }
    public static Vector3 GetUpAxis(Vector3 position, in GravityQuerySettings settings)
    {
        Vector3 g = ComputeGravity(position, settings.ExcludeMask, settings.IncludeMask);
        return g.sqrMagnitude > 0f ? -g.normalized : Vector3.up;
    }

    
    //      --- Source registry --- 
    public static void Register(GravitySource source)
    {
        Debug.Assert(!sources.Contains(source), $"Duplicate registration of gravity source: {source.name}", source);
        sources.Add(source);
    }

    public static void Unregister(GravitySource source)
    {
        Debug.Assert(sources.Contains(source), $"Unregistration of unknown gravity source: {source.name}", source);
        sources.Remove(source);
    }
}

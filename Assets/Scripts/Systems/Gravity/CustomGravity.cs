using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;


[System.Flags]
public enum GravityType
{
    GravitySource = 1 << 0,
    GravityPlane  = 1 << 1,
    GravityBox    = 1 << 2,
    GravitySphere = 1 << 3,
    GravityIsle   = 1 << 4,
    GravityCastedByPlayer = 1 << 5,
    GravityCentrifugalCylinder = 1 << 6,
}

public static class CustomGravity
{
    static List<GravitySource> sources = new List<GravitySource>();
    public static Vector3 GetGravity(Vector3 position)
    {
        Vector3 g = Vector3.zero;
        for (int i = 0; i < sources.Count; i++)
        {
            g += sources[i].GetGravity(position);
        }
        return g;
    }
    public static Vector3 GetGravity(Vector3 position, out Vector3 upAxis)
    {
        Vector3 g = Vector3.zero;
        for (int i = 0; i < sources.Count; i++)
        {
            g += sources[i].GetGravity(position);
        }
        upAxis = -g.normalized;
        return g;
    }
    public static Vector3 GetGravity(Vector3 position, GravityType ignore = 0)
    {
        Vector3 g = Vector3.zero;
        for (int i = 0; i < sources.Count; i++)
        {
            var source = sources[i];
            if ((ignore & source.GravityType) != 0)
            {
                continue;
            }
            g += sources[i].GetGravity(position);
        }
        return g;
    }
    public static Vector3 GetGravity(Vector3 position, out Vector3 upAxis, GravityType ignore = 0)
    {
        Vector3 g = Vector3.zero;
        for (int i = 0; i < sources.Count; i++)
        {
            var source = sources[i];

            if ((ignore & source.GravityType) != 0)
            {
                continue;
            }
            g += sources[i].GetGravity(position);
        }
        upAxis = -g.normalized;
        return g;
    }
    public static Vector3 GetGravityInclude(Vector3 position, GravityType include)
    {
        Vector3 g = Vector3.zero;
        for (int i = 0; i < sources.Count; i++)
        {
            var source = sources[i];
            if ((include & source.GravityType) == 0)
            {
                continue;
            }
            g += sources[i].GetGravity(position);
        }
        return g;
    }
    public static Vector3 GetGravityInclude(Vector3 position, out Vector3 upAxis, GravityType include)
    {
        Vector3 g = Vector3.zero;
        for (int i = 0; i < sources.Count; i++)
        {
            var source = sources[i];
            if ((include & source.GravityType) == 0)
            {
                continue;
            }
            g += sources[i].GetGravity(position);
        }
        upAxis = -g.normalized;
        return g;
    }
    public static Vector3 GetUpAxis(Vector3 position)
    {
        Vector3 g = Vector3.zero;
        for (int i = 0; i < sources.Count; i++)
        {
            g += sources[i].GetGravity(position);
        }
        return -g.normalized;
    }
    public static Vector3 GetUpAxis(Vector3 position, GravityType ignore = 0)
    {
        Vector3 g = Vector3.zero;
        for (int i = 0; i < sources.Count; i++)
        {
            var source = sources[i];
            if ((ignore & source.GravityType) != 0)
            {
                continue;
            }
            g += sources[i].GetGravity(position);
        }
        return -g.normalized;
    }
    public static Vector3 GetUpAxisInclude(Vector3 position, GravityType include)
    {
        Vector3 g = Vector3.zero;
        for (int i = 0; i < sources.Count; i++)
        {
            var source = sources[i];
            if ((include & source.GravityType) == 0)
            {
                continue;
            }
            g += sources[i].GetGravity(position);
        }
        return -g.normalized;
    }
    public static void Register(GravitySource source)
    {
        Debug.Assert(!sources.Contains(source), "Duplicate registration of gravity source!", source);
        sources.Add(source);
    }
    public static void Unregister(GravitySource source)
    {
        Debug.Assert(sources.Contains(source), "Unregistration of unknown gravity source!", source);
        sources.Remove(source);
    }
}

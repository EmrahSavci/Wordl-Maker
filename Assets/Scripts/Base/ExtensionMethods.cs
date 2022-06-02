using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static bool CheckLayer(GameObject go, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << go.layer));
    }

    public static Color SetAlpha(this Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
#if UNITY_EDITOR
    public static void DrawDisc(Vector3 center, float radius, Color color)
    {
        Handles.color = color;
        Handles.DrawWireDisc(center, Vector3.down, radius);
    }
#endif
}
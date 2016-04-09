using System.Reflection;
using UnityEditorInternal;
using UnityEngine;

public class LevelDesigner : MonoBehaviour
{
    public GameObject prefab;
    public int layerIndex;

    public Color color = Color.white;

    public Vector2 gizmoPosition;
    public Vector3 gizmoSize = new Vector3(1,1,1);
    public Color gizmoColor = Color.grey;

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(new Vector3(gizmoPosition.x, gizmoPosition.y, 0), new Vector3(gizmoSize.x, gizmoSize.y, 1));
    }

    public string[] GetSortingLayerNames()
    {
        System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }
}

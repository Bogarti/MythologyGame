using UnityEditor;
using UnityEngine;

public class LevelDesignerMenu : Editor
{
    [MenuItem("GameObject/Create Other/Level Designer")]
    public static void ShowLevelDesigner()
    {
        if(GameObject.Find("Level Designer") == null)
        {
            GameObject designer = new GameObject();
            designer.name = "Level Designer";
            designer.AddComponent<LevelDesigner>();

            foreach (string layerName in designer.GetComponent<LevelDesigner>().GetSortingLayerNames())
            {
                if(GameObject.Find(layerName) == null)
                {
                    GameObject empty = new GameObject();
                    empty.name = layerName;
                }
            }

            GameObject[] selected = new GameObject[1];
            selected[0] = designer;
            Selection.objects = selected;
        }
        else
        {
            Debug.Log("Level Designer already exists");
        }
    }
}

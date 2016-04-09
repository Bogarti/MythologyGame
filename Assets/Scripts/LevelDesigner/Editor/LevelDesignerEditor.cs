using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(LevelDesigner))]
public class LevelDesignerEditor : Editor
{
    LevelDesigner script;

    Vector2 oldTilePos = new Vector2();

    bool leftControl = false;
    BatchMode batchmode = BatchMode.None;
    enum BatchMode
    {
        Create,
        Delete,
        None
    }

    bool flip = false;
    Vector2 offset = Vector2.zero;
    bool lockOnRaster = true;

    void OnEnable()
    {
        script = (LevelDesigner)target;

        if (!Application.isPlaying)
        {
            if (SceneView.lastActiveSceneView != null)
            {
                Tools.current = Tool.View;
                SceneView.lastActiveSceneView.in2DMode = true;
                
                //script.prefab = (GameObject)(AssetDatabase.LoadAllAssetsAtPath("Assets/Prefabs/Environment")[0]);
                leftControl = false;
                batchmode = BatchMode.None;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Sprite");
        script.prefab = (GameObject)EditorGUILayout.ObjectField(script.prefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Layer");
        script.layerIndex = EditorGUILayout.Popup(script.layerIndex, script.GetSortingLayerNames());
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Color");
        script.color = EditorGUILayout.ColorField(script.color);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Flip");
        flip = EditorGUILayout.Toggle(flip);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        offset = EditorGUILayout.Vector2Field("Offset", offset);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Use Raster");
        lockOnRaster = EditorGUILayout.Toggle(lockOnRaster);
        EditorGUILayout.EndHorizontal();

        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }

    void OnSceneGUI()
    {
        if(script.prefab != null)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            //script.gizmoSize = script.prefab.transform.localScale;
            SpriteRenderer renderer = script.prefab.GetComponent<SpriteRenderer>();
            Sprite sprite = renderer.sprite;
            script.gizmoSize.z = 1;
            script.gizmoSize.x = sprite.texture.width / sprite.pixelsPerUnit * script.prefab.transform.localScale.x;
            script.gizmoSize.y = sprite.texture.height / sprite.pixelsPerUnit * script.prefab.transform.localScale.y;

            Vector2 tilePos = new Vector2();
            if (lockOnRaster)
            {
                if (script.gizmoSize.x % 2 == 0)
                    tilePos.x = Mathf.RoundToInt(ray.origin.x + offset.x) - offset.x;
                else
                    tilePos.x = Mathf.RoundToInt(ray.origin.x + 0.5f + offset.x) - 0.5f - offset.x;

                if (script.gizmoSize.y % 2 == 0)
                    tilePos.y = Mathf.RoundToInt(ray.origin.y + offset.y) - offset.y;
                else
                    tilePos.y = Mathf.RoundToInt(ray.origin.y + 0.5f + offset.y) - 0.5f - offset.y;
            }
            else
            {
                tilePos.x = ray.origin.x;
                tilePos.y = ray.origin.y;
            }

            if (tilePos != oldTilePos)
            {
                script.gizmoPosition = tilePos;
                oldTilePos = tilePos;
                SceneView.RepaintAll();
            }

            Event current = Event.current;
            if (current.keyCode == KeyCode.LeftControl)
            {
                if (current.type == EventType.keyDown)
                {
                    leftControl = true;
                }
                else if (current.type == EventType.keyUp)
                {
                    leftControl = false;
                    batchmode = BatchMode.None;
                }
            }
            if (leftControl && current.type == EventType.mouseDown)
            {
                if (current.button == 0)
                    batchmode = BatchMode.Create;
                else if (current.button == 1)
                    batchmode = BatchMode.Delete;
            }
            if (current.type == EventType.mouseDown || batchmode != BatchMode.None)
            {
                string name = string.Format("{0}_{1}_{2}", script.GetSortingLayerNames()[script.layerIndex], tilePos.y, tilePos.x);

                if (current.button == 0 || batchmode == BatchMode.Create)
                    CreateTile(tilePos, name);

                if (current.button == 1 || batchmode == BatchMode.Delete)
                    DeleteTile(name);
            }

            SetGizmoColor();
        }

        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }

    void CreateTile(Vector2 tilePos, string name)
    {
        if (!GameObject.Find(name))
        {
            Vector3 pos = new Vector3(tilePos.x, tilePos.y, 0);
            GameObject tile = (GameObject)Instantiate(script.prefab, pos, Quaternion.identity);
            tile.name = name;
            tile.transform.parent = GameObject.Find(script.GetSortingLayerNames()[script.layerIndex]).transform;
            if (flip)
                tile.transform.localScale = new Vector3(-1, tile.transform.localScale.y);
            else
                tile.transform.localScale = new Vector3(1, tile.transform.localScale.y);

            SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
            renderer.sortingLayerName = script.GetSortingLayerNames()[script.layerIndex];
            renderer.color = script.color;

            try
            {
                if (renderer.sortingLayerName != "Default")
                    tile.GetComponent<Collider2D>().enabled = false;
            }
            catch (MissingComponentException){}
        }
    }

    void DeleteTile(string name)
    {
        GameObject tile = GameObject.Find(name);
        if (tile != null)
            DestroyImmediate(tile);
    }

    void SetGizmoColor()
    {
        switch (batchmode)
        {
            case BatchMode.None:
                script.gizmoColor = Color.grey;
                break;

            case BatchMode.Create:
                script.gizmoColor = Color.white;
                break;

            case BatchMode.Delete:
                script.gizmoColor = Color.red;
                break;
        }
    }
}

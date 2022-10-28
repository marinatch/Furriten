#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;

public class PhysicsTerrain : EditorWindow
{
    [MenuItem("Marina/Physics Terrain")]
    static public void Init()
    {
        PhysicsTerrain window = (PhysicsTerrain)GetWindow(typeof(PhysicsTerrain), true, "Physics Terrain", true);
        window.Show();
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }

    private void OnGUI()
    {
        GUILayout.Space(20);
        if (GUILayout.Button("Generate Physics"))
        {
            GameObject tempParent = GameObject.FindGameObjectWithTag("Terrain");
            for (int i = 0; i < tempParent.transform.childCount; i++)
            {
                if (tempParent.transform.GetChild(i).GetComponent<BoxCollider2D>() == null)
                {
                    tempParent.transform.GetChild(i).gameObject.AddComponent<BoxCollider2D>();
                }
                Vector2 size = tempParent.transform.GetChild(i).GetComponent<SpriteRenderer>().size;
                tempParent.transform.GetChild(i).GetComponent<BoxCollider2D>().size = size;
            }
        }
    }
}

#endif

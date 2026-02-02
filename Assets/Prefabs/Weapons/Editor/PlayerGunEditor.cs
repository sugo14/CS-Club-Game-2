#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Gun))]
public class PlayerGunEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Gun script = (Gun)target;
        
        if (GUILayout.Button("Fire"))
        {
            script.Fire();
        }
        
        if (GUILayout.Button("Reload"))
        {
            script.Reload();
        }
    }
}
#endif
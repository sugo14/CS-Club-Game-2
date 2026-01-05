#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerGun))]
public class PlayerGunEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        PlayerGun script = (PlayerGun)target;
        
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
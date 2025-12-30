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
            script.Fire(); // Or whatever method you want to call
        }
        
        if (GUILayout.Button("Reload"))
        {
            script.Reload();
        }
    }
}
#endif
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Cards/CardData")]
public class CardData : ScriptableObject
{
    public string cardTitle;       // Unique field name
    public string cardDescription; // Unique field name

    // Event for notifying subscribers about changes
    public event Action onChanged;

    [HideInInspector] public int version = 0; // Version counter for runtime detection

    // Call this when changing values via script
    public void NotifyChange()
    {
        version++;
        onChanged?.Invoke();
    }

#if UNITY_EDITOR
    // Automatically notify in Editor when values change
    private void OnValidate()
    {
        version++;
        onChanged?.Invoke();
    }
#endif
}
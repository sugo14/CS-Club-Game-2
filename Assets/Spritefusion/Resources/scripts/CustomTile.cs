using UnityEngine;
    using UnityEngine.Tilemaps;
    using System.Collections.Generic;
  
    [CreateAssetMenu(fileName = "CustomTile", menuName = "Spritefusion/Custom Tile")]
    public class CustomTile : Tile
    {
        [System.Serializable]
        public class Attribute
        {
            public string key;
            public string value;
        }
  
        [SerializeField]
        public List<Attribute> attributes = new List<Attribute>();
  
        // Runtime access methods
        public string GetAttribute(string key, string defaultValue = "")
        {
            foreach (var attr in attributes)
            {
                if (attr.key == key)
                    return attr.value;
            }
            return defaultValue;
        }
  
        public T GetAttribute<T>(string key, T defaultValue = default(T))
        {
            string value = GetAttribute(key);
            if (string.IsNullOrEmpty(value))
                return defaultValue;
  
            try
            {
                return (T)System.Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
  
        public bool HasAttribute(string key)
        {
            foreach (var attr in attributes)
            {
                if (attr.key == key)
                    return true;
            }
            return false;
        }
    }
    
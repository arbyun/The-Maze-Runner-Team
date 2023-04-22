using System;
using System.IO;
using UnityEngine;

namespace Utilities
{
    
    [Serializable]
    internal class Configs
    {
        
        [Serializable]
        public struct RigidBodyConfigs
        {
            public float gravity;
            public float angularDrag;
            public float linearDrag;
            public float mass;
        }
        
        public bool invincible;
        public RigidBodyConfigs bulletPhysics;
        public RigidBodyConfigs playerPhysics;

        /// <summary> Reads the GlobalConfig.json.</summary>
        /// <returns> A configs object.</returns>
        public static Configs Load()
        {
            string path = Application.persistentDataPath + "/GlobalConfig.json";

            if (File.Exists(path))
            {
                string text = File.ReadAllText(path);
                Configs cfg = JsonUtility.FromJson<Configs>(text);
                return cfg;
            }
            else
            {
                Debug.LogError("Configuration file not found!");
                return null;
            }
        }
    }
}
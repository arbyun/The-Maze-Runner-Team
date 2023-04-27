/*using System;
using System.IO;
using UnityEngine;

namespace Utilities
{
    [Serializable]
    internal class Configs
    {
        public bool invincible;
        public RigidBodyConfigs bulletPhysics;
        public RigidBodyConfigs playerPhysics;

        /// <summary> Reads the GlobalConfig.json.</summary>
        /// <returns> A configs object.</returns>
        public static Configs Load()
        {
            var path = Application.persistentDataPath + "/GlobalConfig.json";

            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                var cfg = JsonUtility.FromJson<Configs>(text);
                return cfg;
            }

            Debug.LogError("Configuration file not found!");
            return null;
        }

        [Serializable]
        public struct RigidBodyConfigs
        {
            public float gravity;
            public float angularDrag;
            public float linearDrag;
            public float mass;
        }
    }
}*/
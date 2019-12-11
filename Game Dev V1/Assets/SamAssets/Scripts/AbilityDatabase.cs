using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

namespace AbilitySystem
{
    [CreateAssetMenu(fileName = "Ability Database", menuName = "Create/AbilitySystem/Ability Database")]
    public class AbilityDatabase : ScriptableObject
    {
        [SerializeField]
        string path;
        [ReadOnly, SerializeField]
        int lastId = 0;
        public List<Ability> database;

        bool delete;

        public Ability GetAbilityById(int Id)
        {
            foreach (Ability abil in database)
            {
                if (abil.ID == Id) return abil;
            }
            return null;
        }
#if UNITY_EDITOR
        [Button]
        public void UpdateDatabase()
        {

            Ability[] abilities = FindAssetsByType<Ability>().ToArray();
            foreach (Ability ability in abilities)
            {
                if (!database.Contains(ability))
                {
                    if (lastId == -1)
                    {
                        lastId = database.Count;
                    }
                    ability.SetID(lastId);
                    database.Add(ability);
                    EditorUtility.SetDirty(ability);
                    lastId++;

                }
            }
        }

        public void AddAbility(Ability abilityToAdd)
        {
            if (database.Contains(abilityToAdd)) return;
            database.Add(abilityToAdd);
            abilityToAdd.SetID(lastId);
            lastId++;
            EditorUtility.SetDirty(abilityToAdd);
            EditorUtility.SetDirty(this);
        }
        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }
#endif
        [Button]
        public void Clear()
        {
            database.Clear();
            lastId = 0;
        }



    }



}
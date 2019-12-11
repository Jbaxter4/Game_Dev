using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System.Text.RegularExpressions;

[CreateAssetMenu(fileName ="New Ability", menuName ="Create/New Ability")]
public class Ability : ScriptableObject
{
    int id;
    public int ID { get { return id; } }
    public string abilityName;
    public float damage;
    public float cooldown;
    public float range;
    public AbilityPrefab attachedPrefab;
    public TelegraphTypes telegraphType;
    public Sprite abilityIcon;
    public void SetID(int _id)
    {
        id = _id;
    }
#if UNITY_EDITOR
    [Button]
    public void CreatePrefabClass()
    {
        string text = new StreamReader("Assets/AbilitySystem/AbilityPrefabTemplate.txt").ReadToEnd();
        text = Regex.Replace(text, "%PREFABNAME", name + "Prefab");
        StreamWriter writer = new StreamWriter("Assets/AbilitySystem/AbilityPrefabs/Scripts/" + name + "Prefab.cs");
        writer.Write(text);
        writer.Close();
        AssetDatabase.Refresh();


    }
    [Button]
    public void CreatePrefabObject()
    {

        string filePath = "Assets/AbilitySystem/AbilityPrefabs/Scripts";
        System.Type type = AssetDatabase.LoadAssetAtPath<MonoScript>(filePath + "/" + name + "Prefab.cs").GetClass();
        Debug.Log(type);
        GameObject go = new GameObject();
        go.AddComponent(type);
        go.name = name + "Prefab";
        AbilityPrefab ap = go.GetComponent<AbilityPrefab>();
        ap.abilityInfo = this;
        attachedPrefab = PrefabUtility.SaveAsPrefabAsset(go, "Assets/AbilitySystem/AbilityPrefabs/GameObjects/" + name + "Prefab.prefab").GetComponent<AbilityPrefab>();
        PrefabUtility.SaveAsPrefabAsset(go, "Assets/AbilitySystem/AbilityPrefabs/GameObjects/" + name + "Prefab.prefab");
        DestroyImmediate(go);
    }
#endif

    public enum TelegraphTypes
    {
        None,
        Target,
        Line
    }
}

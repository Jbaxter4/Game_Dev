using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Experimental.Rendering.HDPipeline;
using TMPro;
public class CharacterAbilityManager : MonoBehaviour
{
    [SerializeField]
    Transform charModel;
    [SerializeField]
    DecalProjectorComponent rangeDecal, targetDecal, lineTelegraph;
    [SerializeField]
    LayerMask targetDecalLayerMask;
    [SerializeField]
    int slotAmount;
    [SerializeField]
    Transform[] abilitySlots;
    [SerializeField]
    AbilityPrefab[] slottedAbilities;
    public AbilityPrefab[] SlottedAbilities { get { return slottedAbilities; } }
    [SerializeField]
    Transform actionBar;

    [SerializeField]
    KeyCode[] keyBindings;
    float[] cooldownTimers;
    bool queuingSpell;
    int queuedIndex;


    public static CharacterAbilityManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    private void Start()
    {
        cooldownTimers = new float[slotAmount];
    }
    private void Update()
    {
        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            if(cooldownTimers[i] > 0)
            {
                cooldownTimers[i] -= Time.deltaTime;
                if (cooldownTimers[i] > 0)
                {
                    actionBar.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = Mathf.CeilToInt(cooldownTimers[i]).ToString();
                    actionBar.GetChild(i).GetChild(0).GetComponent<Image>().fillAmount = cooldownTimers[i] / slottedAbilities[i].abilityInfo.cooldown;
                }
                else
                {
                    actionBar.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                    actionBar.GetChild(i).GetChild(0).GetComponent<Image>().fillAmount = 0;
                }

            }
        }
        if (queuingSpell)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if(Physics.Raycast(ray, out hit,100, targetDecalLayerMask))
            {
                if (slottedAbilities[queuedIndex].abilityInfo.telegraphType == Ability.TelegraphTypes.Target)
                {
                    float distanceFromPlayer = Vector3.Distance(transform.position, new Vector3(hit.point.x, transform.position.y, hit.point.z));
                    if (distanceFromPlayer <= slottedAbilities[queuedIndex].abilityInfo.range)
                    {
                        targetDecal.transform.position = hit.point;
                    }
                    else
                    {
                        Vector3 dir = hit.point - new Vector3(transform.position.x, hit.point.y, transform.position.z);

                        targetDecal.transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z) + dir.normalized * slottedAbilities[queuedIndex].abilityInfo.range;
                    }
                }
                
            }
            
        }
        for (int i = 0; i < slotAmount; i++)
        {
            if (Input.GetKeyUp(keyBindings[i]))
            {
                if(slottedAbilities[i] == null)
                {
                    Debug.Log("No ABILITY SLOTTED");
                }
                else
                {
                    if (queuingSpell == false) return;
                    AbilityInfo info = new AbilityInfo();
                    info.hitPoint = targetDecal.transform.position;
                    info.playerForward = charModel.forward;
                    info.castBy = this;
                    info.team = CharacterCombat.instance.TeamManager;
                    slottedAbilities[i].OnUseAbilityPlayer(info);
                    rangeDecal.gameObject.SetActive(false);
                    queuingSpell = false;
                    targetDecal.gameObject.SetActive(false);
                    lineTelegraph.gameObject.SetActive(false);
                    cooldownTimers[i] = slottedAbilities[i].abilityInfo.cooldown;
                }
            }

            if (Input.GetKeyDown(keyBindings[i]))
            {
                if (slottedAbilities[i] == null)
                {
                    Debug.Log("No ABILITY SLOTTED");
                }
                else
                {
                    Debug.Log(cooldownTimers[i]);
                    if (cooldownTimers[i] > 0) return;
                    rangeDecal.gameObject.SetActive(true);
                    rangeDecal.size = new Vector3(slottedAbilities[i].abilityInfo.range*2, slottedAbilities[i].abilityInfo.range*2, 2);
                    queuingSpell = true;
                    queuedIndex = i;
                    if (slottedAbilities[queuedIndex].abilityInfo.range == 0 && slottedAbilities[queuedIndex].abilityInfo.telegraphType == Ability.TelegraphTypes.None) return;
                    if (slottedAbilities[queuedIndex].abilityInfo.telegraphType == Ability.TelegraphTypes.Target)targetDecal.gameObject.SetActive(true);
                    else if(slottedAbilities[queuedIndex].abilityInfo.telegraphType == Ability.TelegraphTypes.Line)
                    {
                        lineTelegraph.gameObject.SetActive(true);
                        lineTelegraph.size = new Vector3(1,2, slottedAbilities[queuedIndex].abilityInfo.range);
                        Vector3 pos = lineTelegraph.transform.localPosition;
                        pos.z = slottedAbilities[queuedIndex].abilityInfo.range * 0.5f;
                        lineTelegraph.transform.localPosition = pos;

                    }
                }
            }
        }
    }
    public void SlotAbilityInFreeSlot(Ability ability)
    {
        for (int i = 0; i < abilitySlots.Length; i++)
        {
            if (slottedAbilities[i] == null)
                SlotAbilityAtSlot(i, ability);
        }
    }
    [Button]
    void SlotAbilityAtSlot(int slotIndex, Ability ability)
    {
        ability = ScriptableObject.Instantiate(ability);
        if(abilitySlots[slotIndex].childCount > 0)
        {
            Destroy(abilitySlots[slotIndex].transform.GetChild(0).gameObject);
        }

        Image image = actionBar.GetChild(slotIndex).GetComponent<Image>();
        image.sprite = ability.abilityIcon;
        slottedAbilities[slotIndex] = Instantiate(ability.attachedPrefab, abilitySlots[slotIndex]);
    }

    [Button]
    public void CreateSlots()
    {
        slottedAbilities = new AbilityPrefab[slotAmount];
        abilitySlots = new Transform[slotAmount];
        keyBindings = new KeyCode[slotAmount];
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        for (int i = 0; i < slotAmount; i++)
        {
            GameObject go = new GameObject();
            go.transform.parent = transform;
            go.name = "Slot " + (i + 1);
            abilitySlots[i] = go.transform;

        }
    }
}
public class AbilityInfo
{
    public Vector3 hitPoint;
    public Vector3 playerForward;
    public CharacterAbilityManager castBy;
    public TeamManager team;
}

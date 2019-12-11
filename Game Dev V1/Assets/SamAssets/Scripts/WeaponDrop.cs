using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using Sirenix.OdinInspector;
public class WeaponDrop : MonoBehaviour
{
    [SerializeField]
    Rarity rarity;
    [SerializeField]
    Weapon weapon;
    public Weapon Weapon { get { return weapon; } }
    [SerializeField]
    VisualEffect ve;
    [SerializeField]
    Transform gunParent;
    [SerializeField]
    Vector2 heightMinMax;
    float timer = 0;
    float scaleTimer = 0;
    [SerializeField]
    float bobSpeed, rotationSpeed, scaleSpeed;
    [SerializeField]
    AnimationCurve curve;


    private void Start()
    {
        InitializeDrop();
    }
    [Button]
    public void InitializeDrop()
    {
        weapon = ScriptableObject.Instantiate(weapon);
        weapon.rarity = rarity;
        ve.SetVector4("MainColour", GameManager.instance.rarityColors[(int)rarity]);
        
        weapon.InitializeWeapon();
        GameObject model = Instantiate(weapon.model, gunParent);
        model.GetComponent<ColourRandomizer>().SetColours(weapon.colours);



    }
    private void Update()
    {
        
        if (scaleTimer <= 1 )
        {
            gunParent.localScale= Vector3.Lerp(Vector3.zero, Vector3.one * 0.5f, scaleTimer);
            
        }
        gunParent.transform.localPosition = new Vector3(0, Mathf.Lerp(heightMinMax.x, heightMinMax.y, curve.Evaluate(timer%1)), 0);
        gunParent.Rotate(Vector3.up, Time.deltaTime* rotationSpeed);
        timer += Time.deltaTime  * bobSpeed;
        scaleTimer += Time.deltaTime * scaleSpeed;
    }
    public void PickUpDrop(bool equip)
    {
       bool pickedUp = CharacterInventory.instance.PickupWeapon(weapon, equip);
        if (pickedUp) Destroy(gameObject);
    }
    public void PickUpDropNoInventory(bool equip)
    {
        bool pickedUp = CharacterInventory.instance.PickupWeaponNoInventory(weapon, equip);
        if(pickedUp)Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 12)
        {
            CharacterInventory.instance.SetHoveredWeapon(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            CharacterInventory.instance.ClearHoveredWeapon(this);
        }
    }





}

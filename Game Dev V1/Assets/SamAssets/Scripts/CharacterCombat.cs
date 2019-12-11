using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class CharacterCombat : MonoBehaviour, IDamagable
{

    public static CharacterCombat instance;
    [SerializeField]
    int currentHealth;
    [SerializeField]
    TeamManager teamManager;
    public TeamManager TeamManager { get { return teamManager; } }
    [SerializeField]
    Transform characterModel;
    [SerializeField, ReadOnly]
    int[]  currentAmmo;
   

    [SerializeField, ReadOnly]
    float attackTimer, reloadTimer;
    [SerializeField, ReadOnly]
    bool reloading;
    [SerializeField]
    Weapon[] equipedWeaponSlots = new Weapon[3];
    public Weapon[] EquipedWeaponSlots { get { return equipedWeaponSlots; } }
    [SerializeField]
    bool[] unlockedWeaponSlots = new bool[3];
    [SerializeField, ReadOnly]
    Weapon equipedWeapon;
    int equipedWeaponSlot;
    public Weapon EquipedWeapon { get { return equipedWeapon; } }
    int burstCounter;
    float burstIntervalTimer;
 

    [SerializeField]
    Transform weaponSlot;
    public Transform WeaponSlot { get { return weaponSlot; } }
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    private void Start()
    {
        equipedWeapon = null;
        currentAmmo = new int[equipedWeaponSlots.Length];
        currentHealth = GetComponent<CharacterClassManager>().GetCharacterClass().startingHealth;
    }
    public void SetWeaponInSlotAndEquip(Weapon weapon, int slot)
    {
        SetWeaponInSlot(slot, weapon);
        EquipWeapon(slot);
    }
    public void SetWeaponInCurrentSlotAndEquip(Weapon weapon)
    {
        SetWeaponInSlot(equipedWeaponSlot, weapon);
        EquipWeapon(equipedWeaponSlot);
    }
    public void EquipWeapon( int slot, bool switching = false)
    {
        if (equipedWeaponSlots[slot] == null) return;
        if (weaponSlot.childCount > 0) Destroy(weaponSlot.GetChild(0).gameObject);
        equipedWeaponSlot = slot;
        equipedWeapon = equipedWeaponSlots[slot];
        
        
        equipedWeapon.OnEquipWeapon(TeamManager);
        if(!switching)currentAmmo[equipedWeaponSlot] = equipedWeapon.maxAmmo;
        AmmoCounter.instance.SetAmmoText(currentAmmo[equipedWeaponSlot] + "/" + equipedWeapon.maxAmmo);
    }
    public void SetWeaponInSlot(int slot, Weapon weapon)
    {
        equipedWeaponSlots[slot] = weapon;
        currentAmmo[slot] = weapon.maxAmmo;
    }
    public bool IsSlotEmptyAndUnlocked(int slotIndex)
    {
        return unlockedWeaponSlots[slotIndex] && equipedWeaponSlots[slotIndex] == null;

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EquipWeapon(0, true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            EquipWeapon(1, true);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            EquipWeapon(2, true);
        }
        if (attackTimer>= 0)attackTimer -= Time.deltaTime;
        if (reloadTimer >= 0) reloadTimer -= Time.deltaTime;
        if (burstIntervalTimer >= 0) burstIntervalTimer -= Time.deltaTime;
        if(burstCounter > 0 && burstIntervalTimer <= 0)
        {
            FireWeapon(true);
        }
        if(equipedWeapon &&  reloadTimer <=0 && reloading)
        {
            currentAmmo[equipedWeaponSlot] = equipedWeapon.maxAmmo;
            AmmoCounter.instance.SetAmmoText(currentAmmo[equipedWeaponSlot] + "/" + equipedWeapon.maxAmmo);
            reloading = false;
        }
        if(Input.GetButtonDown("Fire1"))
        {
            if(equipedWeapon &&  attackTimer <= 0 && reloadTimer <=0)
            {
                FireWeapon(false);
            }
        }
        if (Input.GetButton("Fire1"))
        {
            if ( equipedWeapon && attackTimer <= 0 && reloadTimer <= 0 && equipedWeapon.automatic)
            {
                FireWeapon(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    void FireWeapon(bool burst)
    {
        if (!equipedWeapon) return;
        if (currentAmmo[equipedWeaponSlot] >= 1)
        {

            equipedWeapon.OnFire();
            if(equipedWeapon.weaponType == WeaponTypes.Gun )
            {
                Gun gun = equipedWeapon as Gun;
                if(gun.bulletsPerBurst > 1)
                {
                    
                    if(!burst)burstCounter = gun.bulletsPerBurst - 1;
                    burstIntervalTimer = gun.burstSpeed;
                }
            }
            
            if (burst) burstCounter -= 1;
            currentAmmo[equipedWeaponSlot] -= 1;
            AmmoCounter.instance.SetAmmoText(currentAmmo[equipedWeaponSlot] + "/" + equipedWeapon.maxAmmo);
            attackTimer = equipedWeapon.attackSpeed;
        }
        else
        {
            Reload();
        }
    }


    void Reload()
    {
        if (!reloading && equipedWeapon && equipedWeapon.canReload)
        {
            AudioSource.PlayClipAtPoint(equipedWeapon.reload, transform.position);
            burstCounter = 0;
            reloadTimer = equipedWeapon.reloadSpeed;
            reloading = true;
        }
    }

    public void OnDamage(int damage)
    {
        currentHealth -= damage;
    }

    public void OnHeal(int healAmount)
    {
        currentHealth += healAmount;
    }

    public bool isDead()
    {
        return currentHealth <= 0;
    }
}

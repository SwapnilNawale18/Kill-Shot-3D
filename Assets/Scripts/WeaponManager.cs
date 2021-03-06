/*using System.Collections;
using System.Collections.Generic;*/
using UnityEngine;
using Mirror;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private PlayerWeapon primaryWeapon;

    //[SerializeField]
    private PlayerWeapon currentWeapon;

    [SerializeField]
    private Transform weaponHolder;
    
    [SerializeField]
    private string weaponLayerName = "Weapon";
    // Start is called before the first frame update
    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;

        GameObject weaponIns =Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        weaponIns.transform.SetParent(weaponHolder);

        if(isLocalPlayer)
        {
            SetLayerRecursively(weaponIns, LayerMask.NameToLayer(weaponLayerName));
        }
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}

/*using System.Collections;
using System.Collections.Generic;*/
using UnityEngine;
using Mirror;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;
    //[SerializeField]
    //private GameObject weaponGFX;
    // Start is called before the first frame update
    void Start()
    {
        if(cam == null)
        {
            Debug.LogError("no camera informed on the firing system.");
            this.enabled = false;
        }

        //weaponGFX.layer = LayerMask.NameToLayer(weaponLayerName);

        weaponManager = GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();
        //Semi automatic fire
        if(currentWeapon.fireRate <= 0f)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if(Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
            }
            else if(Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
        
    }

    [Client]
    private void Shoot()
    {
        Debug.Log("Shot made");
        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask))
        {
            //Debug.Log("Object touches:" + hit.collider.name);
            if(hit.collider.tag == "Player")
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage);
            }
        }

    }

    [Command]
    private void CmdPlayerShot(string playerId, float damage)
    {
        Debug.Log(playerId + " was hit.");

        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(damage);
    }
}

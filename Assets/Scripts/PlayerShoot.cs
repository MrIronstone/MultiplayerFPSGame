using UnityEngine;
using Mirror;


[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    
    private const string PLAYER_TAG = "Player";


    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private void Start()
    {
        if ( cam == null)
        {
            Debug.LogError("PlayerShoot: No Camera referanced!");
            this.enabled = false ;
        }

        weaponManager = GetComponent<WeaponManager>();

    }

    void SetLayerRecursivelyWeapon(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursivelyWeapon(child.gameObject, newLayer);
        }
    }

    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if(currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }


        
    }

    // Is called on the server when the player shoots
    [Command]
    void CmdOnShoot ()
    {
        RpcDoShootEffect();
    }

    // Is called on all clients when we need to do a shoot effect
    [ClientRpc]
    void RpcDoShootEffect ()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    // Is called on the server when we hit something
    // Takes in the hit point and normal of the surface.
    [Command]
    void CmdOnHit ( Vector3 _pos, Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal);
    }

    // Is called on all clients, so we can spawn effects
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
    {
        GameObject _hitEffect = (GameObject) Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, 2f);
    }

    [Client]
    void Shoot()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        // We are shooting, call the OnShoot method on the server
        CmdOnShoot();

        RaycastHit _hit;
        if( Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
        {
            if( _hit.collider.CompareTag(PLAYER_TAG))
            {
                CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
            }

            // We hit
            CmdOnHit(_hit.point, _hit.normal);
        }

    }

    [Command]
    void CmdPlayerShot (string _playerID, int _damage)
    {
        Debug.Log( _playerID + " has been shot.");

        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }

}

using UnityEngine;
using Mirror;

public class PlayerShoot : NetworkBehaviour
{
    
    private const string PLAYER_TAG = "Player";


    [SerializeField]
    private PlayerWeapon weapon;
    [SerializeField]
    private GameObject weaponGFX;
    [SerializeField]
    private string weaponLayerName = "Weapon1";


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

        // hata alıyorum sebebini anlamadım.
        // SetLayerRecursivelyWeapon(weaponGFX, LayerMask.NameToLayer(weaponLayerName));

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
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }


    [Client]
    void Shoot()
    {
        RaycastHit _hit;
        if( Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            if( _hit.collider.CompareTag(PLAYER_TAG))
            {
                CmdPlayerShot(_hit.collider.name, weapon.damage);
            }
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

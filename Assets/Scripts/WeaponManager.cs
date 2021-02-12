using UnityEngine;
using Mirror;

public class WeaponManager : NetworkBehaviour
{

    [SerializeField]
    private string weaponLayerName = "Weapon1";

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private PlayerWeapon primaryWeapon;



    private PlayerWeapon currentWeapon;


    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public PlayerWeapon GetCurrentWeapon ()
    {
        return currentWeapon; 
    }

    void EquipWeapon ( PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;

        GameObject _weaponIns = (GameObject) Instantiate(_weapon.graphics,weaponHolder.position, weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);

        if (isLocalPlayer)
            SetLayerRecursivelyWeapon(_weaponIns, LayerMask.NameToLayer(weaponLayerName));
    }

    void SetLayerRecursivelyWeapon(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursivelyWeapon(child.gameObject, newLayer);
        }
    }
}

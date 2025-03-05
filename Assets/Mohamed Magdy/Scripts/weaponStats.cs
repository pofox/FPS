using Unity.VisualScripting;
using UnityEngine;

public class weaponStats : MonoBehaviour
{
    public void updateStats()
    {
        Variables myStats = GetComponent<Variables>();
        Variables activeWeapon = GetComponentsInChildren<Variables>()[1];
        myStats.declarations.Set("damege",activeWeapon.declarations.Get("damege"));
        myStats.declarations.Set("ShootingSpeed", activeWeapon.declarations.Get("ShootingSpeed"));
        myStats.declarations.Set("capacity", activeWeapon.declarations.Get("capacity"));
        myStats.declarations.Set("ShootingRange", activeWeapon.declarations.Get("ShootingRange"));
    }
}

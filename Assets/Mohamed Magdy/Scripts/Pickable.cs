using Unity.VisualScripting;
using UnityEngine;

public enum pickableType
{
    weapon,
    health
}
public class Pickable : MonoBehaviour
{
    public void pickMe(Transform weapons)
    {
        pickableType type = (pickableType)GetComponentsInChildren<Variables>()[0].declarations.Get("type");
        switch (type)
        {
            case pickableType.weapon:
                Transform weapon = transform.GetChild(0);
                weapon.gameObject.layer = LayerMask.NameToLayer("weapon");
                weapon.parent = weapons;
                weapon.localPosition = Vector3.zero;
                weapon.localRotation = Quaternion.identity;
                weapon.gameObject.SetActive(false);
                break;
            case pickableType.health:
                break;
            default:
                break;
        }
        Destroy(gameObject);
    }
}

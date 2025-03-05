using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField] Transform weapons;
    int currantWeapon = 0;
    void Start()
    {
        int index = Convert.ToInt16(!GameManager.Instance.firstGame);
        currantWeapon = GameManager.Instance.Save.data[index].currentWeapon;
        for (int i = 0; i < weapons.childCount; i++)
        {
            weapons.GetChild(i).gameObject.SetActive(i == currantWeapon);
        }
        weapons.GetComponent<weaponStats>().updateStats();
    }
    void OnChangeWeapon(InputValue value)
    {
        if (!GameManager.Instance.paused)
        {
            weapons.GetChild(currantWeapon).gameObject.SetActive(false);
            currantWeapon = (currantWeapon + 1) % weapons.childCount;
            weapons.GetChild(currantWeapon).gameObject.SetActive(true);
            weapons.GetComponent<weaponStats>().updateStats();
        }
    }
    private void OnDestroy()
    {
        GameManager.Instance.Save.data[1].currentWeapon = currantWeapon;
    }
}

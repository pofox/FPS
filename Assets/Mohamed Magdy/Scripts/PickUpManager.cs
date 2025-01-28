using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    [SerializeField] List<Pickable> list;
    [SerializeField] Transform weapons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int index = Convert.ToInt16(!GameManager.Instance.firstGame);
        List<int> picked = GameManager.Instance.Save.data[index].PickedUp;
        foreach (int item in picked)
        {
            list[item].pickMe(weapons);
        }
    }
    private void OnDestroy()
    {
        List<int> PickedUp = new();
        for (int i = 0; i < list.Count; i++) {
            if (list[i] == null)
            {
                PickedUp.Add(i);
            }
        }
        GameManager.Instance.Save.data[1].PickedUp = PickedUp;
    }
}

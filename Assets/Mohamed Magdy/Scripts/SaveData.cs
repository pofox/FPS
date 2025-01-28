using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class SaveDataList
{
    public List<SaveData> data;
}
[System.Serializable]
public class SaveData
{
    //Enemy
    public float e_totalHealth;
    public float e_damage;
    public float e_level;
    //Player
    public float p_health;
    public Vector3 p_position;
    public Quaternion p_rotation;
    public float p_score;
    public Vector3 p_lastCheckPoint;
    public int currentWeapon;
    public List<int> PickedUp;
    //UI
    public float music;
    public float sfx;
}

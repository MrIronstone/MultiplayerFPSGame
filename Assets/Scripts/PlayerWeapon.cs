﻿using UnityEngine;


[System.Serializable]
public class PlayerWeapon
{
    public string name = "AK-47";

    public int damage = 10;

    public float range = 100f;


    public float fireRate = 0f;

    [SerializeField]
    public GameObject graphics;


}

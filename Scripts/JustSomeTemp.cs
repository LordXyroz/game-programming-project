﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unimplemented script, used to test quest rewards
public class JustSomeTemp : MonoBehaviour {
    public List<GameObject> rewards = new List<GameObject>();
    public int gold = 0;

	public void GiveItem(GameObject item, int gold)
    {
        rewards.Add(item);
        this.gold += gold;
    }
}

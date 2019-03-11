using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unused script
public class QuestDBScript : MonoBehaviour {
    public static QuestDBScript questsDB = null;

    public List<LocationQuest> locationQuests = new List<LocationQuest>();

    public void Start()
    {
        if (questsDB == null)
            questsDB = this;
        else if (questsDB != this)
            Destroy(this);
    }
}

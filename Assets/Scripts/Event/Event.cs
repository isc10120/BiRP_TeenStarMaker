using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Event")]

public class Event : ScriptableObject
{
    public string eventName;
    public int energyCost;

}

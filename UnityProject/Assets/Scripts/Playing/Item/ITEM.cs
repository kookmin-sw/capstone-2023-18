using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ITEMS
{
    NONE = 0,
    BOOST = 1,
    HOMING = 2,
}
public class ITEM : MonoBehaviour
{
    [Header("ICON IMAGES")]
    public Sprite[] ICONS;
}

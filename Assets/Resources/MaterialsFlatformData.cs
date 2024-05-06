using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Materials Flatform", menuName = "Game Data")]
public class MaterialsFlatformData : ScriptableObject
{
    public Flatform[] flatforms;
}

[System.Serializable]
public struct Flatform
{
    public string id;
    public Material material;
    public bool canMove;
}
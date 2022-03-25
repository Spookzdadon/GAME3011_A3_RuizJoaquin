using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase
{
    public static ItemScript[] Items { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]private static void Initialize() => Items = Resources.LoadAll<ItemScript>("Items/");
}

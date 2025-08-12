using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoSingleton<NPCManager>
{
    [Header("Connected Components")]
    [SerializeField] private ShopNPCInventoryManager shopNPCInventoryManager;

    public ShopNPCInventoryManager ShopNPCInventoryManager => shopNPCInventoryManager;
}

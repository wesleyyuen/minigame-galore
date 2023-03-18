using System;
using System.Collections.Generic;
using UnityEngine;

public class TrainerModel : MonoBehaviour
{
    [SerializeField] protected string _name;
    [SerializeField] private List<InventoryItem> _inventory = new List<InventoryItem>();
    public Trainer TrainerInfo {get; protected set;}

    protected virtual void Awake()
    {
        TrainerInfo = new Trainer(_name, this);
    }

    protected virtual void Start()
    {
        foreach (var item in _inventory)
        {
            if (item.item == null || item.amount == 0) return;
            TrainerInfo.AddItem(item.item, item.amount);
        }
    }

    [Serializable]
    private class InventoryItem
    {
        public Item item;
        public int amount;
    }
}

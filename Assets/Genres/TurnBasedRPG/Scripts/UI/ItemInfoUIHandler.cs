using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemInfoUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _amount;

    public void SetUI(Item item, int amount)
    {
        _name.text = item.Name;
        _amount.text = amount.ToString();
    }
}

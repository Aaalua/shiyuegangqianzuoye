using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLua;
using UnityEngine.UI;

[CustomLuaClass]
public class ItemGrid : MonoBehaviour
{
    private Item item;
    public int count;
    Text showCount;

    public Item Item
    {
        get
        {
            return item;
        }

        set
        {
            item = value;
            count = value.num;
            setShowText(value.num.ToString());

        }
    }

    private void Start()
    {
        showCount = GetComponentInChildren<Text>();
    }

    public void setShowText(string text)
    {
        showCount.text = text;
    }

}
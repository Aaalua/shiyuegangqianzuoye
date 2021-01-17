using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SLua;
using UnityEngine.Events;

[CustomLuaClass]
public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public string itemName;
    public int id;
    public int num = 0;
    public string info = "";

    private Sprite currentImage;

    private GameObject oldParent;

    Vector2 offset = Vector2.zero;

    public Sprite CurrentImage
    {
        get
        {
            if (currentImage == null)
                currentImage = GetComponent<Image>().sprite;
            return currentImage;
        }

        set
        {
            currentImage = value;
            GetComponent<Image>().sprite = value;
        }
    }
    public GameObject OldParent
    {
        get
        {
            if (oldParent == null)
            {
                if (transform.parent != null)
                    oldParent = transform.parent.gameObject;
            }

            return oldParent;
        }

        set
        {
            if (value != null)
            {
                oldParent = value;
                transform.SetParent(value.transform);
                this.transform.SetAsFirstSibling();
                transform.position = Vector3.zero;
                this.GetComponent<RectTransform>().offsetMin = Vector2.zero;
                this.GetComponent<RectTransform>().offsetMax = Vector2.zero;
                this.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            }

        }
    }



    private void Start()
    {
        OldParent = OldParent;
    }

    public void setPosition(Vector3 pos)
    {
        transform.position = pos;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject canvas = GameObject.Find("item_movePanel");
        this.transform.SetParent(canvas.transform);
        this.transform.SetAsFirstSibling();
        offset = new Vector2(transform.position.x, transform.position.y) - eventData.position;
        transform.position = eventData.position + offset;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + offset;
        transform.GetComponent<Image>().raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject go = eventData.pointerCurrentRaycast.gameObject;
        if (go == null)
        {
            OldParent = OldParent;
            transform.GetComponent<Image>().raycastTarget = true;
            return;
        }

        ItemGrid container = go.GetComponent<ItemGrid>();
        Item item = go.GetComponent<Item>();
        if (container != null)
        {
            OldParent.GetComponent<ItemGrid>().setShowText("");
            OldParent = go;
            container.Item = this;
        }
        else if (item != null)
        {
            swapItem(item);
        }
        else
        {
            OldParent = OldParent;
        }
        transform.GetComponent<Image>().raycastTarget = true;
    }

    public void swapItem(Item item)
    {
        GameObject swapParent = item.OldParent;
        item.OldParent.GetComponent<ItemGrid>().Item = this;
        item.OldParent = OldParent;
        OldParent.GetComponent<ItemGrid>().Item = item;
        OldParent = swapParent;

    }
}

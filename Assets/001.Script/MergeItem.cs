using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MergeItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public static MergeItem instance;
    Image sr;
    public Item item;
    bool isSelect = false;
    public GameObject contactItem;
    
    Vector2 minPos;
    Vector2 maxPos;
    

    private Vector2 lastMousePosition;

    private void Awake() { instance = this;}
    public void InitItem(Item i)
    {
    
        item.itemType = i.itemType;
        item.itemImg =i.itemImg;

        item.swordID = Merge.instance.ID++; //���� ���̵� �ο�

        sr = GetComponent<Image>();
        sr.transform.SetParent(Merge.instance.parentObj.transform);
        sr.sprite = item.itemImg;
    }

    private void Update() {
        
    }


    private void OnMouseDown()
    {
        isSelect = true;
        
    }

    private void OnEnable() {
        
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(isSelect && item.itemType == collision.GetComponent<MergeItem>().item.itemType)
        {
            
            if(contactItem != null)
            {
                contactItem = null;
            }

            contactItem = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(isSelect && item.itemType == collision.GetComponent<MergeItem>().item.itemType && contactItem != null)
        {
            contactItem = null;
        }
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnPointerUp(PointerEventData eventData)
    {
        isSelect = false;
        if (contactItem != null)
        {
            //합치는거
            Debug.Log("merge");
            Destroy(contactItem);
            Destroy(gameObject);
            MergeItem contact = contactItem.GetComponent<MergeItem>();
            MergeItem gObj = gameObject.GetComponent<MergeItem>();
            Merge.instance.swordList.Remove(contact);
            Merge.instance.swordList.Remove(gObj);
            Merge.instance.mergingItem(item.itemType + 1);
            Merge.instance.CheckNewSword(item.itemType);
            //검사

        }
    }  

    public void OnDrag(PointerEventData eventData)
    {
        isSelect = true;
        Vector2 mousePos = Input.mousePosition;
        Vector2 diff = mousePos - lastMousePosition;
        RectTransform rect = GetComponent<RectTransform>();

        Vector3 newPosition = rect.position +  new Vector3(diff.x, diff.y, transform.position.z);
        Vector3 oldPos = rect.position;
        rect.position = newPosition;
        if(!IsRectTransformInsideSreen(rect))
        {
            rect.position = oldPos;
            mousePos = oldPos;
        }
        lastMousePosition = mousePos;
        
    }

    private bool IsRectTransformInsideSreen(RectTransform rectTransform)
    {
        bool isInside = false;
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        int visibleCorners = 0;
        //경계영역
        Rect rect = new Rect(0,150,Screen.width, 1000);
        foreach(Vector3 corner in corners)
        {
            if(rect.Contains(corner))
            {
                visibleCorners++;
            }
        }
        if(visibleCorners == 4)
        {
            isInside = true;
        }
        return isInside;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastMousePosition = eventData.position;
    }
}

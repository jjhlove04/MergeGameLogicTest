using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MergeItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static MergeItem instance;
    Image sr;
    public Item item;
    bool isSelect = false;
    public GameObject contactItem;
    
    


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


    private void OnMouseDown()
    {
        isSelect = true;
        
    }

    private void OnEnable() {
        
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hi");
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
        Vector3 mousePos = Input.mousePosition;
        transform.position = mousePos;
    }

   
}

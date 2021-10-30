using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
[System.Serializable]
public class Item
{
    public int itemType;
    public Sprite itemImg;
    public int swordID;
}

public class Merge : MonoBehaviour
{
    [Tooltip("정렬을 위한 트랜스폼의 위치입니다")]
    public GameObject[] sortPos;
    [Tooltip("현재 정렬이 가능한지 체크")]
    public bool autoSortCheck = false;
    [Tooltip("현재 합성이 가능한지 체크")]
    public bool readyAuto = false;

    public List<MergeItem> swordList = new List<MergeItem>();
    public int ID = 0;
    public GameObject newSwordPanel;

    public static Merge instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if(instance != null)
        {
            return;
        }
    }


    public List<Item> itemdata = new List<Item>();
    public GameObject itemPrefabs;
    public GameObject spawnPos;
    GameObject go;
    [SerializeField]
    private BoxCollider2D createArea;
    public GameObject parentObj;
        Vector3 magnetPos;
        int nextNum = 0;

    ///<summary>
    ///랜덤한 위치를 받아오는 함수
    ///</summary>
    public Vector3 GetRandomPosition()
    {
        Vector3 basePosition = transform.position;
        Vector3 size = createArea.size;

        float posX = basePosition.x + Random.Range(-size.x / 2.5f, size.x / 2.5f);
        float posY = basePosition.y + Random.Range(-size.y / 2.5f, size.y / 2.5f);

        Vector3 randomPos = new Vector3(posX, posY, 0);

        return randomPos;
    }
    ///<summary>
    ///칼 생성
    ///</summary>
    public void ItemCreate(int num)
    {
        Vector3 randomPos = GetRandomPosition();
        GameObject go = createSword(spawnPos.transform.position , num);
        
        
        go.transform.DOMove(randomPos, 1f);
    }

    public GameObject createSword(Vector3 pos, int num)
    {
        go = Instantiate(itemPrefabs, pos, Quaternion.identity);
        MergeItem item = go.GetComponent<MergeItem>();
        item.InitItem(itemdata[num]);
        
        swordList.Add(item);
        CheckNewSword(item.item.itemType);
        return go;
    }

    public void mergingItem(int num)
    {
        Vector3 Pos = Input.mousePosition;
        GameObject go = createSword(Pos, num);
    }

    

    public void AutoMerge(int num)
    {
        Vector3 randomPos = GetRandomPosition();
        magnetPos = randomPos;
        MergeItem sword = swordList[Random.Range(0, swordList.Count)];
        MergeItem swordFinding = null;
       
        foreach (var i in swordList)
        {
            if(i.item.itemType == sword.item.itemType && 
               i.item.swordID != sword.item.swordID )
            {
                swordFinding = i;
            }
        }
        if (swordFinding != null)
        {
            sword.transform.DOMove(randomPos, 1f);
            swordFinding.transform.DOMove(randomPos, 1f);

            nextNum = swordFinding.item.itemType + 1;

            Destroy(swordFinding.gameObject,1.2f);
            Destroy(sword.gameObject,1.2f);

            swordList.Remove(sword);
            swordList.Remove(swordFinding);

            Invoke("AutoMergeDelay", 1.3f);
        }

    }

    void AutoMergeDelay()
    {
        createSword(magnetPos,nextNum);
    }

    ///<summary>
    ///이 함수는 자동정렬을 위한 함수
    ///</summary>

    public void SortSword()
    {
        
            swordList = swordList.OrderByDescending( x => x.item.itemType).ToList();
            for(int i = 0; i < swordList.Count; i++)
            {
                //단 이 로직으로 할경우 합치는 도중에 정렬을 해버리면 합쳐지는 녀석은 DOTween이 무시됨.
                // 합쳐지고 았을 때는 정렬이 안되도록 막는 로직이 필요함. 
                swordList[i].transform.DOMove(sortPos[i].transform.position,1);
            }
        
    }
    [HideInInspector]
    public int NewSwordIndex = -1;
    ///<summary>
    ///새로운 칼 검사 로직
    ///</summary>
    public void CheckNewSword(int itemType)
    {
        if(itemType > NewSwordIndex)
        {
            NewSwordIndex = itemType;
            newSwordPanel.SetActive(true);

        }
    }

    public void PopSwordPanel()
    {
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            AutoMerge(MergeItem.instance.item.itemType +1);
        }
         if(Input.GetKeyDown(KeyCode.B))
         {
             SortSword();
         }
    }
}

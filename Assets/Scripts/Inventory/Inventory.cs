using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.IO;
using System.Collections;
using Firebase.Analytics;

public enum TypeInventory { Outfit, EnemyBag, Shop, CLOSE, Habar }

public class Inventory : MonoBehaviour
{
    public int[] IndexOnGunsCell = new int[3]{ 0, 0, 0 };
    public event System.Action OnSetWeapon;
    public event System.Action Loaded;

    [SerializeField] private Player player;
    [SerializeField] private Object objectItem;
    [Header("Objects for close/open")]
    [SerializeField] private GameObject inventoryObj;
    [SerializeField] private GameObject inventoryEnemyObj;
    [SerializeField] private GameObject inventoryShopObj;
    [SerializeField] private GameObject inventoryHabarObj;
    [SerializeField] private GameObject inventoryOutFitObj;
    [Header("Empty objects")]
    [SerializeField] private GameObject mainObj;
    [SerializeField] private GameObject enemyObj;
    [SerializeField] private GameObject shopObj;
    [SerializeField] private GameObject habarObj;
    [SerializeField] private GameObject outfitObj;
    [SerializeField] private GameObject infoObj;
    [Header("Other objects")]
    [SerializeField] private TextMeshProUGUI info;
    [SerializeField] private GameObject useButton;
    [Header("Count cells")]
    [Range(0, 64)][SerializeField] private int coutCellMain = 24;
    [Range(0, 64)][SerializeField] private int coutCellEnemy = 24;
    [Range(0, 64)][SerializeField] private int coutCellShop = 24;
    [Range(0, 8)][SerializeField] private int countCellOutfill = 4;

    [SerializeField] private Camera cam;
    [SerializeField] private EventSystem es;

    [Header("Y/n")]
    [SerializeField] private GameObject objYN;
    [SerializeField] private TextMeshProUGUI textYN;

    public List<ItemInventory> items = new List<ItemInventory>();
    private List<ItemInventory> itemsBag = new List<ItemInventory>(5);
    private DataBase _data;
    private int _currentID = -1;
    private ItemInventory _currentItem;
    private PlayerInterface _pi;
    private int[] indexAmmo;

    //--------------------DELITE-------------------------
    private void RandomItems()
    {
        for (int i = 0; i < coutCellMain; i++)
        {
            AddItem(items, i, _data.items[0], 0, 100);
        }

        for (int i = 0; i < Random.Range(8, 15); i++)
        {
            AddItem(items, Random.Range(0, 20), _data.items[Random.Range(0, _data.items.Length)], Random.Range(3, 8), 100);
        }
    }
    private void Ammos()
    {
        AddItem(items, 0, _data.items[1], 100, 100);
        AddItem(items, 1, _data.items[2], 100, 100);
        AddItem(items, 2, _data.items[3], 100, 100);
        AddItem(items, 3, _data.items[24], 100, 100);
        AddItem(items, 4, _data.items[25], 100, 100);
    }
    //--------------------DELITE-------------------------

    private void OnEnable()
    {
        player.OnSetActivInventory += SetActivInventory;
    }

    private void OnDisable()
    {
        player.OnSetActivInventory -= SetActivInventory;
    }

    public void Start()
    {  
        _data = GetComponent<DataBase>();
        _pi = GetComponent<PlayerInterface>();
        if (items.Count == 0) AddGraphics();

        for (int i = 0; i < 5; i++)
        {
            itemsBag.Add(new ItemInventory());
            AddItem(itemsBag, i, _data.items[0], 0, 100);
        }

        NewList();
        if (StaticVal.nameSave != null)
        {
            string json = File.ReadAllText(Application.persistentDataPath + StaticVal.nameSave);
            ObjectSave save = JsonUtility.FromJson<ObjectSave>(json);

            for (int i = 0; i < 29; i++)
            {
                foreach (Item item in _data.items)
                {
                    if (item.id == save.idItem[i])
                        items[i].item = item;
                }
            }
            for (int i = 0; i < 29; i++)
            {
                items[i].count = save.coutItem[i];
            }
            for (int i = 0; i < 29; i++)
            {
                items[i].cond = save.condItem[i];
            }
            for (int i = 0; i < 29; i++)
            {
                items[i].INDIFICATOR = save.indificatorItem[i];
            }
            Loaded.Invoke();
        }
        else
        {
            AddItem(items, 0, _data.items[3], 15, 70);
            AddItem(items, 1, _data.items[30], 1, 50);
            /*
            int num = 0;
            foreach (Item ii in _data.items)
            {
                if (ii.type == TypeItem.Ammo)
                {
                    AddItem(items, num, ii, 150, 50);
                    num += 1;
                }
                if (ii.type == TypeItem.Weapon)
                {
                    AddItem(items, num, ii, 1, 50);
                    num += 1;
                }
                if (ii.type == TypeItem.Cothl)
                {
                    AddItem(items, num, ii, 1, 50);
                    num += 1;
                }
            }
            */
        }
        UpdateInventory();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RandomItems();
            UpdateInventory();
        }

        if (Input.GetKeyDown(KeyCode.X))
            AddItem(items, 0, _data.items[2], 100, 100);

        if (Input.GetKeyDown(KeyCode.M))
        {
            Ammos();
            UpdateInventory();
        }
    }



    public void SetActivInventory(TypeInventory _ti)
    {
        inventoryObj.SetActive(!inventoryObj.activeSelf);
        if (_ti == TypeInventory.Outfit)
        {     
            inventoryOutFitObj.SetActive(true);
            inventoryEnemyObj.SetActive(false);
            inventoryShopObj.SetActive(false);
            inventoryHabarObj.SetActive(false);
        }
        else if (_ti == TypeInventory.Shop)
        {
            inventoryOutFitObj.SetActive(false);
            inventoryEnemyObj.SetActive(false);
            inventoryShopObj.SetActive(true);
            inventoryHabarObj.SetActive(false);
        }
        else if (_ti == TypeInventory.Habar)
        {
            inventoryOutFitObj.SetActive(false);
            inventoryEnemyObj.SetActive(false);
            inventoryShopObj.SetActive(false);
            inventoryHabarObj.SetActive(true);
        }
        else if (_ti == TypeInventory.EnemyBag)
        {
            inventoryOutFitObj.SetActive(false);
            inventoryEnemyObj.SetActive(true);
            inventoryShopObj.SetActive(false);
            inventoryHabarObj.SetActive(false); 
        }

        if (_ti == TypeInventory.CLOSE)
        {
            for (int i = 0; i < 5; i++)
                AddItem(itemsBag, i, items[coutCellMain + countCellOutfill + i].item, items[coutCellMain + countCellOutfill + i].count,
                    items[coutCellMain + countCellOutfill + i].cond);

            _pi.SetBag(itemsBag);
        }

        UpdateInventory();
    }

    public void AddItem(List<ItemInventory> _list, int id, Item item, int count, int cond)
    {
        _list[id].item = item; //даём ади
        _list[id].cond = cond;
        _list[id].count = count;    //даём количество

        if (_list[id].count > _list[id].item.maxCount)
            _list[id].count = _list[id].item.maxCount;
    }

    public void AddInventoryItem(int id, ItemInventory invItem)
    {
        items[id].item = invItem.item;
        items[id].cond = invItem.cond;
        items[id].count = invItem.count;
    }

    public void AddGraphics()
    {
        for(int i = 0; i < coutCellMain; i++)
        {
            GameObject newItem = Instantiate(objectItem, mainObj.transform) as GameObject;

            newItem.name = i.ToString();

            ItemInventory ii = new ItemInventory();
            ii.item = _data.items[0];
            ii.INDIFICATOR = i;
            ii.itemGameObj = newItem;

            ii.textObj = newItem.GetComponentInChildren<TextMeshProUGUI>();
            ii.imgBgObj = newItem.GetComponentsInChildren<Image>()[0];
            ii.imgObj = newItem.GetComponentsInChildren<Image>()[1];
            RectTransform rt = newItem.GetComponent<RectTransform>();
            Button tempButton = newItem.GetComponent<Button>();

            rt.localPosition = new Vector3(0, 0, 0); 
            rt.localScale = new Vector3(1, 1, 1);
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);

            tempButton.onClick.AddListener(delegate { StartCoroutine(SelectObj(newItem)); });

            items.Add(ii);
        }
        for (int i = coutCellMain; i < coutCellMain + countCellOutfill; i++)
        {
            GameObject newItem = Instantiate(objectItem, outfitObj.transform) as GameObject;

            newItem.name = i.ToString();

            ItemInventory ii = new ItemInventory();
            ii.item = _data.items[0];
            ii.INDIFICATOR = i;
            ii.itemGameObj = newItem;

            if (i == coutCellMain)
            {
                ii.type = TypeItem.Weapon;
                IndexOnGunsCell[0] = coutCellMain;
            }
            if (i == coutCellMain + 1)
            {
                ii.type = TypeItem.Weapon;
                IndexOnGunsCell[1] = coutCellMain + 1;
            }
            if (i == coutCellMain + 2)
            {
                ii.type = TypeItem.Pnv;
            }
            if (i == coutCellMain + 3)
            {
                ii.type = TypeItem.Cothl;
                IndexOnGunsCell[2] = coutCellMain + 3;
            }

            ii.textObj = newItem.GetComponentInChildren<TextMeshProUGUI>();
            ii.imgBgObj = newItem.GetComponentsInChildren<Image>()[0];
            ii.imgObj = newItem.GetComponentsInChildren<Image>()[1];
            RectTransform rt = newItem.GetComponent<RectTransform>();
            Button tempButton = newItem.GetComponent<Button>();

            rt.localPosition = new Vector3(0, 0, 0); 
            rt.localScale = new Vector3(1, 1, 1); 
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);

            tempButton.onClick.AddListener(delegate { StartCoroutine(SelectObj(newItem)); });

            items.Add(ii);
        }
        for (int i = coutCellMain + countCellOutfill; i < coutCellMain + countCellOutfill + coutCellEnemy; i++)
        {
            GameObject newItem = Instantiate(objectItem, enemyObj.transform) as GameObject;

            newItem.name = i.ToString();

            ItemInventory ii = new ItemInventory();
            ii.item = _data.items[0];
            ii.INDIFICATOR = i;
            ii.itemGameObj = newItem;

            ii.textObj = newItem.GetComponentInChildren<TextMeshProUGUI>();
            ii.imgBgObj = newItem.GetComponentsInChildren<Image>()[0];
            ii.imgObj = newItem.GetComponentsInChildren<Image>()[1];
            RectTransform rt = newItem.GetComponent<RectTransform>();
            Button tempButton = newItem.GetComponent<Button>();

            rt.localPosition = new Vector3(0, 0, 0);
            rt.localScale = new Vector3(1, 1, 1);
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);

            tempButton.onClick.AddListener(delegate { StartCoroutine(SelectObj(newItem)); });

            items.Add(ii);
        }
        for (int i = coutCellMain + countCellOutfill + coutCellEnemy; i < coutCellMain + countCellOutfill + coutCellEnemy + coutCellShop; i++)
        {
            GameObject newItem = Instantiate(objectItem, shopObj.transform) as GameObject;

            newItem.name = i.ToString();

            ItemInventory ii = new ItemInventory();
            ii.item = _data.items[0];
            ii.INDIFICATOR = i;
            ii.itemGameObj = newItem;
            ii.type = TypeItem.SHOP;

            ii.textObj = newItem.GetComponentInChildren<TextMeshProUGUI>();
            ii.imgBgObj = newItem.GetComponentsInChildren<Image>()[0];
            ii.imgObj = newItem.GetComponentsInChildren<Image>()[1];
            RectTransform rt = newItem.GetComponent<RectTransform>();
            Button tempButton = newItem.GetComponent<Button>();

            rt.localPosition = new Vector3(0, 0, 0);
            rt.localScale = new Vector3(1, 1, 1);
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);

            tempButton.onClick.AddListener(delegate { StartCoroutine(SelectObj(newItem)); });

            items.Add(ii);
        }
        for (int i = coutCellMain + countCellOutfill + coutCellEnemy + coutCellShop; i < coutCellMain + countCellOutfill + coutCellEnemy + coutCellShop + 1; i++)
        {
            GameObject newItem = Instantiate(objectItem, habarObj.transform) as GameObject;

            newItem.name = i.ToString();

            ItemInventory ii = new ItemInventory();
            ii.item = _data.items[0];
            ii.INDIFICATOR = i;
            ii.itemGameObj = newItem;
            ii.type = TypeItem.HABAR;

            ii.textObj = newItem.GetComponentInChildren<TextMeshProUGUI>();
            ii.imgBgObj = newItem.GetComponentsInChildren<Image>()[0];
            ii.imgObj = newItem.GetComponentsInChildren<Image>()[1];
            RectTransform rt = newItem.GetComponent<RectTransform>();
            Button tempButton = newItem.GetComponent<Button>();

            rt.localPosition = new Vector3(0, 0, 0);
            rt.localScale = new Vector3(1, 1, 1);
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);

            tempButton.onClick.AddListener(delegate { StartCoroutine(SelectObj(newItem)); });

            items.Add(ii);
        }
    }



    public void SetShop()
    {
        StaticVal.onYN = false;
    }

    public void Use()
    {
        if (_currentItem.item.type == TypeItem.Medical)
        {
            player.Medic(_currentItem.item.typeMediac);
            if (_currentItem.count - 1 <= 0)
            {
                AddItem(items, _currentID, _data.items[0], 0, 100);
            }
            else
            {
                items[_currentID].count--;
            }

            NullSelectedObject();
            UpdateInventory();
        }
        else if(_currentItem.item.type == TypeItem.Detector)
        {
            player._playerInterface.OpenDetector();
            SetActivInventory(TypeInventory.Outfit);

            NullSelectedObject();
            UpdateInventory();
        }
        else if (_currentItem.item.type == TypeItem.Cothl)
        {
            AddInventoryItem(_currentID, items[IndexOnGunsCell[2]]);
            AddInventoryItem(IndexOnGunsCell[2], _currentItem);
            OnSetWeapon.Invoke();

            NullSelectedObject();
            UpdateInventory();
        }
        else if (_currentItem.item.type == TypeItem.Weapon)
        {
            AddInventoryItem(_currentID, items[IndexOnGunsCell[0]]);
            AddInventoryItem(IndexOnGunsCell[0], _currentItem);
            OnSetWeapon.Invoke();

            NullSelectedObject();
            UpdateInventory();
        }
    }

    public void TakeEvereyething()
    {
        for (int i = coutCellMain + countCellOutfill; i < coutCellMain + countCellOutfill + coutCellEnemy; i++)
        {
            AddItemsInventoryToNull(items[i]);
            AddItem(items, i, _data.items[0], 0, 100);
        }
        NullSelectedObject();
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        for(int i = 0; i < coutCellMain + countCellOutfill + coutCellEnemy + coutCellShop + 1; i++)
        {
            if (items[i].item.id != 0 && items[i].count > 1) 
            {
                items[i].textObj.text = items[i].count.ToString(); 
            }
            else
            {
                items[i].textObj.text = "";
            }

            items[i].imgObj.sprite = items[i].item.img;
            if (!items[i].selected)
            {
                items[i].imgBgObj.sprite = items[i].item.imgBG;
            }
            else
            {
                items[i].imgBgObj.sprite = items[i].item.imgSelected;
            }
        }
    }

    public IEnumerator SelectObj(GameObject obj)
    {
        if (_currentID == -1)
        {
            _currentID = int.Parse(es.currentSelectedGameObject.name); 
            _currentItem = CopyInventoryItem(items[_currentID]);

            items[_currentID].selected = true;

            if (_currentItem.item.id == 0 && _currentItem.count < 1)
            {
                NullSelectedObject();
                yield break;
            }

            infoObj.SetActive(true);
            if (_currentItem.type != TypeItem.SHOP) 
                useButton.SetActive(true);
            info.text = items[_currentID].item.info;

            UpdateInventory();
        }
        else if (int.Parse(es.currentSelectedGameObject.name) != _currentID)
        {
            ItemInventory ii = items[int.Parse(es.currentSelectedGameObject.name)];

            if (_currentItem.type == TypeItem.SHOP && ii.type == TypeItem.NULL)
            {
                if ((ii.count + _currentItem.count <= ii.item.maxCount && _currentItem.item == ii.item) || ii.item.id == 0)
                {
                    objYN.SetActive(true);
                    textYN.text = "Вы действительно хотите купить за " + _currentItem.item.money + " руб. ?";
                    StaticVal.onYN = true;
                    ItemInventory it = _currentItem;

                    while (StaticVal.onYN)
                        yield return new WaitForEndOfFrame(); ;

                    if (_currentID != -1)
                    {
                        if (StaticVal.money - it.item.money >= 0)
                        {
                            if (ii.item.id == 0)
                                AddInventoryItem(ii.INDIFICATOR, _currentItem);
                            else
                                ii.count = CalculatedCountItems(ii.count, _currentItem.count, _currentItem.item.maxCount, ii,
                                    _currentID, false);

                            StaticVal.money -= _currentItem.item.money;
                            if (StaticVal.firebaseApp != null)
                                FirebaseAnalytics.LogEvent("by_store", new Parameter("ID_item", it.item.id));
                        }
                        NullSelectedObject();
                        UpdateInventory();
                    }

                    yield break;
                }
                else
                {
                    NullSelectedObject();
                    UpdateInventory();

                    yield break;
                }
            }
            else if (ii.type == TypeItem.HABAR && _currentItem.type == TypeItem.NULL && _currentItem.item.type == TypeItem.Habar)
            {
                objYN.SetActive(true);
                textYN.text = "Вы действительно хотите продать за " + (_currentItem.item.money * _currentItem.count) + " руб. ?";
                StaticVal.onYN = true;

                while (StaticVal.onYN)
                    yield return new WaitForEndOfFrame();

                StaticVal.money += _currentItem.item.money * _currentItem.count;

                if (StaticVal.firebaseApp != null)
                    FirebaseAnalytics.LogEvent("take_habar+" + _currentItem.item.id.ToString());

                AddItem(items, _currentID, _data.items[0], 0, 100);
                NullSelectedObject();
                UpdateInventory();

                yield break;
            }

            if (ii.type == TypeItem.NULL && ii.item.id == 0 && ii.type != TypeItem.SHOP)
            {
                AddInventoryItem(int.Parse(es.currentSelectedGameObject.name), _currentItem);
                AddItem(items, _currentID, _data.items[0], 0, 100);
                if (_currentItem.item.id >= 70)
                {
                    OnSetWeapon.Invoke();
                }
            }
            else if (ii.type != TypeItem.NULL && _currentItem.item.type == ii.type && ii.item.id == 0 && ii.type != TypeItem.SHOP)
            {
                AddInventoryItem(int.Parse(es.currentSelectedGameObject.name), _currentItem);
                AddItem(items, _currentID, _data.items[0], 0, 100);
                OnSetWeapon.Invoke();
            }
            else if (_currentItem.item == ii.item && ii.type != TypeItem.SHOP)
            {
                ii.count = CalculatedCountItems(ii.count, _currentItem.count, _currentItem.item.maxCount, ii, _currentID);
            }

            NullSelectedObject();
            UpdateInventory();
        }
    }

    private int CalculatedCountItems(int cout, int coutAdding, int maxCout, ItemInventory itemInventory, int index, bool nullCurentItem = true)
    {
        if (cout + coutAdding <= maxCout)
        {
            if (nullCurentItem)
                AddItem(items, index, _data.items[0], 0, 100);
            return cout + coutAdding;
        }
        else
        {
            if (nullCurentItem)
                AddItem(items, index, itemInventory.item, cout + coutAdding - maxCout, itemInventory.cond);
            return maxCout;
        }
    }

    public void NullSelectedObject()
    {
        StaticVal.onYN = false;
        infoObj.SetActive(false);
        useButton.SetActive(false);
        if (_currentID != -1) 
            items[_currentID].selected = false;
        _currentItem = null;
        _currentID = -1;
    }
    
    private ItemInventory CopyInventoryItem(ItemInventory old)
    {
        ItemInventory New = new ItemInventory();

        New.item = old.item;
        New.itemGameObj = old.itemGameObj;
        New.count = old.count;
        New.cond = old.cond;
        New.type = old.type;
        New.INDIFICATOR = old.INDIFICATOR;

        return New;
    }



    public bool AddItemsInventoryToNull(ItemInventory item)
    {
        for (int i = 0; i < coutCellMain; i++)
        {
            if (items[i].item.id == 0)
            {
                AddInventoryItem(i, item);
                UpdateInventory();
                return true;
            }
        }

        return false;
    }

    public bool AddItemsToNull(Item item, int _count, int _cond)
    {
        for (int i = 0; i < coutCellMain; i++)
        {
            if (items[i].item.id == 0)
            {
                AddItem(items, i, item, _count, _cond);
                UpdateInventory();
                return true;
            }
        }

        return false;
    }

    public void GetAllBag(List<ItemInventory> _bag)
    {
        for (int i = coutCellMain + countCellOutfill; i < coutCellMain + countCellOutfill + coutCellEnemy; i++)
        {
            AddItem(items, i, _data.items[0], 0, 100);
        }

        for (int i = 0; i < _bag.Count; i++)
        {
            AddInventoryItem(coutCellMain + countCellOutfill + i, _bag[i]);
        }          
    }

    public Armor FindArmor()
    {
        Armor _armor = null;
        int id_2 = items[IndexOnGunsCell[2]].item.id;

        foreach (Armor arm in _data.armors)
        {
            if (arm.id == id_2)
            {
                _armor = arm;
            }
        }

        return _armor;
    }

    public Gun[] FindGun()
    {
        Gun[] _guns = new Gun[2];
        
        int id_0 = items[IndexOnGunsCell[0]].item.id;
        int id_1 = items[IndexOnGunsCell[1]].item.id;

        foreach (Gun gunInData in _data.guns)
        {
            if (gunInData.id == id_0)
            {
                _guns[0] = gunInData.Clone();
            }
            if (gunInData.id == id_1)
            {
                _guns[1] = gunInData.Clone();
            }
        }

        return _guns;
    }

    public int FindAmmos(Gun _gun)
    {
        NewList();
        int ammos = 0;

        foreach (ItemInventory item in items)
        {
            if (item.item.typeAmmo == _gun.typeAmmo && item.item.type == TypeItem.Ammo)
            {
                ammos += item.count;

                for (int i = 0; i < indexAmmo.Length; i++)
                {
                    if (indexAmmo[i] == -1)
                    {
                        indexAmmo[i] = item.INDIFICATOR;
                        break;
                    }
                }
            }
        }

        return ammos;
    }

    public void SetAmmos(int _ammoForReload)
    {
        int _ammos = _ammoForReload;
        for (int i = 0; i < indexAmmo.Length; i++)
        {
            if (indexAmmo[i] != -1)
            {
                if (items[indexAmmo[i]].count == _ammos)
                {
                    AddItem(items, indexAmmo[i], _data.items[0], 0, 100);
                    break;
                }
                else if (items[indexAmmo[i]].count < _ammos)
                {
                    _ammos -= items[indexAmmo[i]].count;
                    AddItem(items, indexAmmo[i], _data.items[0], 0, 100);
                }
                else if (items[indexAmmo[i]].count > _ammos)
                {
                    AddItem(items, indexAmmo[i], items[indexAmmo[i]].item,
                        items[indexAmmo[i]].count - _ammos, 100);
                    break;
                }
            }
        }
    }

    private void NewList()
    {
        indexAmmo = new int[30];

        for (int i = 0; i < indexAmmo.Length; i++)
        {
            indexAmmo[i] = -1;
        }
    }

    public int FindItem(Item itemFind, int itemCount, bool del = false)
    {
        foreach (ItemInventory item in items)
        {           
            if (item.item.id == itemFind.id && item.count >= itemCount)
            {
                if (del)
                {
                    if (item.count - itemCount >= 0)
                    {
                        AddItem(items, item.INDIFICATOR, _data.items[0], 0, 100);
                        UpdateInventory();
                    }
                    else
                    {
                        items[item.INDIFICATOR].count -= itemCount;
                        UpdateInventory();
                    }
                }
                
                return item.INDIFICATOR;
            }
        }
        return -1;
    }
}

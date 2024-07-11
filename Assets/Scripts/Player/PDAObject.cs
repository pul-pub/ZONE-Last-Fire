using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PDAObject : MonoBehaviour
{
    [SerializeField] private Transform parentQuest;
    [SerializeField] private RectTransform rectParent;
    [SerializeField] private Object objQuest;
    [SerializeField] private Object scrolsQuest;

    private PlayerInterface _playerInterface;
    private List<DelitesObject> questsList = new List<DelitesObject>();

    private void Awake()
    {
        _playerInterface = GetComponent<PlayerInterface>();
    }

    public void UpdateQuestsList()
    {
        if (objQuest != null)
        {
            for (int i = 0; i < questsList.Count; i++)
            {
                questsList[i].Delete();
            }
            questsList.Clear();
            rectParent.sizeDelta = new Vector2(0, 15);
            foreach (Quest q in _playerInterface.Quests)
            {
                GameObject gObj = Instantiate(objQuest, Vector3.zero, new Quaternion(0, 0, 0, 0), parentQuest) as GameObject;

                questsList.Add(gObj.GetComponent<DelitesObject>());
                rectParent.sizeDelta += new Vector2(0, 105);
                gObj.GetComponentInChildren<Button>().onClick.AddListener(delegate { SetActivQuest(gObj.GetComponentInChildren<TextMeshProUGUI>().text); });
                gObj.GetComponentInChildren<TextMeshProUGUI>().text = q.header;
                if (q.id == _playerInterface.ActivQuest.id)
                {
                    gObj.GetComponentInChildren<Image>().color = Color.red;
                }
            }
        }
    }

    private void SetActivQuest(string text)
    {
        if (text != _playerInterface.ActivQuest.header)
        {
            foreach (Quest qq in  _playerInterface.Quests)
                if (qq.header == text)
                    _playerInterface.ActivQuest = qq;
            UpdateQuestsList();
        }
    }
}

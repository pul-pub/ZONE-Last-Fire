using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    [SerializeField] private GameObject treeObj;
    [SerializeField] private TextMeshProUGUI texXP;
    [SerializeField] private GameObject[] notActivTree;
    [SerializeField] private Image[] progresBar;
    [SerializeField] private Sprite[] spritsProgressBar;

    private DataBase data;

    private void Awake()
    {
        data = GetComponent<DataBase>();
    }

    private void Update()
    {
        texXP.text =  "Свободный опыт: " + StaticVal.notSelectedXP;
    }

    public void OpenTreeSkills(bool isActiv)
    {
        treeObj.SetActive(isActiv);
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < StaticVal.idSkills.Length; i++)
        {
            if (StaticVal.idSkills[i] != -1)
                progresBar[StaticVal.idSkills[i]].sprite = spritsProgressBar[15];
        }

        foreach (Skill s in data.skils)
        {
            for (int i = 0; i < StaticVal.idSkills.Length; i++)
            {
                if (s.Id == StaticVal.idSkills[i])
                {
                    if (s.postSkill_1 != null)
                        notActivTree[s.postSkill_1.Id].SetActive(true);
                    if (s.postSkill_2 != null)
                        notActivTree[s.postSkill_2.Id].SetActive(true);
                }
            }
        } 
    }

    public void StudySkill(int id)
    {
        foreach (Skill s in data.skils)
        {
            if (s.Id == id)
            {
                for (int i = 0; i < StaticVal.idSkills.Length; i++)
                    if (s.Id == StaticVal.idSkills[i])
                        continue;

                if (StaticVal.notSelectedXP >= s.CountXP)
                {
                    StaticVal.notSelectedXP -= s.CountXP;
                    for (int j = 0; j < StaticVal.idSkills.Length; j++)
                    {
                        if (StaticVal.idSkills[j] == -1)
                        {
                            StaticVal.idSkills[j] = s.Id;
                            break;
                        }
                    }
                            
                }
            }
        }

        UpdateUI();
    }
}

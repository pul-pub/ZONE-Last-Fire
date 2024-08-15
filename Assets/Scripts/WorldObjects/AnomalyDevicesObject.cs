using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalyDevicesObject : MonoBehaviour
{
    [SerializeField] public Quest q;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public PlayerInterface playerInterface;
    [SerializeField] public Inventory inventory;
    public Sprite sp;

    private float timer = 1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.7f, 3), 0, whatIsGround);
            if (cols.Length > 0)
            {
                foreach (Collider2D col in cols)
                {
                    if (col.CompareTag("Player") && timer <= 0)
                    {
                        foreach (Quest qs in playerInterface.Quests)
                        {
                            if (qs.FindItem(inventory, false) != -1 && qs.id == 80)
                            {
                                qs.FindItem(inventory);
                                playerInterface.EndingQuests.Add(qs);
                                playerInterface.Quests.Remove(qs);
                                playerInterface.Quests.Add(q);
                                playerInterface.ActivQuest = q;
                                GetComponent<SpriteRenderer>().sprite = sp;
                                timer = 3600;
                                break;
                            }
                        }
                        if (timer != 3600)
                        {
                            timer = 1;
                        }
                    }
                }
            }
        }


        if (timer >= 0)
            timer -= Time.deltaTime;
    }

    public void OpenTree()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.7f, 3), 0, whatIsGround);
        if (cols.Length > 0)
        {
            foreach (Collider2D col in cols)
            {
                if (col.CompareTag("Player") && timer <= 0)
                {
                    foreach (Quest qs in playerInterface.Quests)
                    {
                        if (qs.FindItem(inventory, false) != -1 && qs.id == 80)
                        {
                            qs.FindItem(inventory);
                            playerInterface.EndingQuests.Add(qs);
                            playerInterface.Quests.Remove(qs);
                            playerInterface.Quests.Add(q);
                            playerInterface.ActivQuest = q;
                            timer = 3600;
                            GetComponent<SpriteRenderer>().sprite = sp;
                            break;
                        }
                    }
                    if (timer != 3600)
                    {
                        timer = 1;
                    }
                }
            }
        }
    }
}

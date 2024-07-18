using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Prompts : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private GameObject prompt;
    [SerializeField] private TextMeshProUGUI textPrompt;

    void Update()
    {
        if (StaticVal.promptMode)
        {
            Collider2D[] cols = Physics2D.OverlapBoxAll(player.transform.position, new Vector2(0.1f, 3), 0, whatIsGround);

            if (cols.Length > 2)
            {
                foreach (Collider2D col in cols)
                {
                    if (col.gameObject.GetComponents<DoorObject>().Length > 0)
                    {
                        prompt.SetActive(true);
                        textPrompt.text = "Перейти на локацию";
                    }
                    else if (col.GetComponentsInParent<EnemyMutant>().Length > 0)
                    {
                        EnemyMutant en = col.GetComponentInParent<EnemyMutant>();

                        if (en.IsDided)
                        {
                            prompt.SetActive(true);
                            textPrompt.text = "Осмотреть труп";
                        }
                    }
                    else if (col.GetComponentsInParent<Enemy>().Length > 0)
                    {
                        Enemy en = col.GetComponentInParent<Enemy>();

                        if (en.IsDided)
                        {
                            prompt.SetActive(true);
                            textPrompt.text = "Осмотреть труп";
                        }
                        else if (!en.IsEnemyForPlayer && en.Dialog.Length > 0)
                        {
                            prompt.SetActive(true);
                            textPrompt.text = "Говорить";
                        }
                    }
                    else if (col.GetComponents<Artifact>().Length > 0)
                    {
                        if (col.GetComponent<SpriteRenderer>().enabled)
                        {
                            prompt.SetActive(true);
                            textPrompt.text = "Подобрать";
                        }
                    }
                }
            }
            else
            {
                prompt.SetActive(false);
            }
        }
    }
}

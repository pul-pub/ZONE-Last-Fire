using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsTreeObject : MonoBehaviour
{
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public SkillController skillController;

    private float timer;

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
                        skillController.OpenTreeSkills(true);
                        timer = 1;
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
                    skillController.OpenTreeSkills(true);
                    timer = 1;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Detector : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Transform tr;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Vector2 sizeCheckBox;
    [SerializeField] public float UpdateTime;

    public bool isScan = false;
    private float timer = 1f;

    private void Update()
    {
        if (timer >= 0)
            timer -= Time.deltaTime;

        if (timer <= 0 && isScan)
            Check();
    }

    private void Check()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(tr.position, sizeCheckBox, 0, whatIsGround);

        if (cols.Length > 0)
        {
            foreach (Collider2D col in cols)
            {
                if (col.GetComponents<Artifact>().Length > 0)
                {
                    float x = (tr.position.x - col.transform.position.x) * -1;

                    player._playerInterface.SetArtPos(x);

                    if (x > -2 && x < 2)
                    {
                        col.GetComponent<Artifact>().enabled = true;
                        col.GetComponent<SpriteRenderer>().enabled = true;
                        col.GetComponentInChildren<Light2D>().enabled = true;
                    }

                    timer = UpdateTime;
                    return;
                }
            }

            player._playerInterface.SetArtPos(-100);
        }

        timer = UpdateTime;
    }
}

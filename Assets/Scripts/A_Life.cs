using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class A_Life : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private bool IsSpawnEnemy = true;
    [SerializeField] private Vector2[] positions;
    [SerializeField] private Object[] objects;
    [Space]
    [SerializeField] private Transform PARENT;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Vector2 sizeCheckBox;
    [SerializeField] public float UpdateTimeEnemy;

    [Header("Artifacts")]
    [SerializeField] private bool IsSpawnArt = true;
    [SerializeField] private Vector2[] positionsArt;
    [SerializeField] private Object[] Artifacts;
    [Space]
    [SerializeField] public float UpdateTimeArt;

    [Header(" вестовые люди")]
    [SerializeField] private bool IsResetPeopl = false;
    [SerializeField] private GameObject[] objs;

    [Header("For Enemy settings")]
    [SerializeField] private Transform player;

    private float timer_1 = 0;
    private float timer_2 = 0;

    private void Update()
    {
        if (timer_1 >= 0)
            timer_1 -= Time.deltaTime;

        if (timer_2 >= 0)
            timer_2 -= Time.deltaTime;

        if (timer_1 <= 0 && IsSpawnEnemy)
            AddRandomEnemys();

        if (timer_2 <= 0 && IsSpawnArt)
            AddRandomArtifacts();

        if (IsResetPeopl)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                Debug.Log(SceneManager.GetActiveScene().buildIndex);
                if (objs[i].activeSelf != StaticVal.peopls[SceneManager.GetActiveScene().buildIndex, i])
                {
                    objs[i].SetActive(StaticVal.peopls[SceneManager.GetActiveScene().buildIndex, i]);
                    Debug.Log("Reset" + objs[i].name);
                }
            }
        }
    }

    private void AddRandomArtifacts()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            Collider2D[] cols = Physics2D.OverlapBoxAll(positionsArt[i], sizeCheckBox, 0, whatIsGround);
            int num = Random.Range(0, 100);

            foreach (Collider2D col in cols)
            {
                if (col.GetComponentsInParent<Player>().Length > 0 || col.GetComponents<Artifact>().Length > 0)
                {
                    timer_2 = UpdateTimeArt;
                    return;
                }
            }

            if (num >= 50 && num < 70)
                Instantiate(Artifacts[0], positionsArt[i], Quaternion.EulerRotation(0, 0, 0), PARENT);

            if (num >= 90 && num < 100)
                Instantiate(Artifacts[1], positionsArt[i], Quaternion.EulerRotation(0, 0, 0), PARENT);
        }

        timer_2 = UpdateTimeArt;
    }

    private void AddRandomEnemys()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            Collider2D[] cols = Physics2D.OverlapBoxAll(positions[i], sizeCheckBox, 0, whatIsGround);

            if (cols.Length <= 0)
            {
                GameObject obj = Instantiate(objects[Random.Range(0, objects.Length)], positions[i],
                            Quaternion.EulerRotation(0, 0, 0), PARENT) as GameObject;

                if (obj.GetComponentsInParent<Enemy>().Length > 0)
                {
                    Enemy en = obj.GetComponentsInParent<Enemy>()[0];

                    en.target = player;
                    en.parent = PARENT;
                }
                else if (obj.GetComponentsInParent<EnemyMutant>().Length > 0)
                {
                    EnemyMutant en = obj.GetComponentsInParent<EnemyMutant>()[0];

                    en.target = player;
                }
            }
        }

        timer_1 = UpdateTimeEnemy;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelitesObject : MonoBehaviour
{
    public float timeDestroy = 5f;

    private float timer = 0f;

    private void Awake()
    {
        timer = timeDestroy;
    }

    void Update()
    {
        if (timeDestroy > -1)
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
                Delete();
        }
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}

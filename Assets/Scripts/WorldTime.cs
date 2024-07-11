using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldTime : MonoBehaviour
{
    [SerializeField] private float speedTime = 1.5f;
    [SerializeField] private bool IsSetGragics = true;
    [SerializeField] private Transform sky;
    [SerializeField] private GameObject rain;
    [SerializeField] private Light2D light;
    private float timer;

    void Awake()
    {
        timer = speedTime;

        if (StaticVal.nameSave != null)
        {
            string json = File.ReadAllText(Application.persistentDataPath + StaticVal.nameSave);
            ObjectSave save = JsonUtility.FromJson<ObjectSave>(json);

            StaticVal.time = save.time;
            StaticVal.idRain = save.isRain;
            if (IsSetGragics)
            {
                sky.localPosition = save.posSky;
                light.intensity = save.lights;
            }
                
        }

        if (IsSetGragics)
        {
            SetPosSky();
            SetLight();
        }
    }
    
    void Update()
    {
        timer -= Time.deltaTime;

        if ( timer <= 0)
        {
            timer = speedTime;
            AddMinute();
            if (IsSetGragics)
                SetLight();
            SetPosSky();
            UpdateLight();
        }
    }

    private void AddMinute()
    {
        StaticVal.time[1] += 1;

        if (StaticVal.time[1] >= 60)
        {
            StaticVal.time[1] = 0;
            StaticVal.time[0] += 1;

            rain.SetActive(false);

            int randRain = Random.Range(0, 10);
            if (randRain >= 8 && IsSetGragics)
            {
                rain.SetActive(true);
            }
        }

        if (StaticVal.time[0] >= 24)
        {
            StaticVal.time[0] = 0;
        }
    }

    private void SetPosSky()
    {
        if (((StaticVal.time[0] * 60) + StaticVal.time[1]) % 24 == 0)
        {
            if (StaticVal.time[0] >= 3 && StaticVal.time[0] < 12)
            {
                sky.localPosition -= new Vector3(0, 1.3f, 0);
            }
            else if (StaticVal.time[0] >= 12 && StaticVal.time[0] < 15)
            {
                sky.localPosition = new Vector3(0, 0.5f, 0);
            }
            else if (StaticVal.time[0] >= 15 && StaticVal.time[0] < 24)
            {
                sky.localPosition += new Vector3(0, 1.3f, 0);
            }
            else
            {
                sky.localPosition = new Vector3(0, 29, 0);
            }
        }
    }

    private void SetLight()
    {
        if (StaticVal.time[0] >= 3 && StaticVal.time[0] < 12)
        {
            light.intensity += 0.0013f;
        }
        else if (StaticVal.time[0] >= 12 && StaticVal.time[0] < 15)
        {
            light.intensity = 0.902f;
        }
        else if (StaticVal.time[0] >= 15 && StaticVal.time[0] < 24)
        {
            light.intensity -= 0.0013f;
        }
        else
        {
            light.intensity = 0.20f;
        }
    }

    private void UpdateLight()
    {
        if (StaticVal.time[0] >= 3 && StaticVal.time[0] < 12)
        {
            StaticVal.light += 0.0013f;
        }
        else if (StaticVal.time[0] >= 12 && StaticVal.time[0] < 15)
        {
            StaticVal.light = 1.002f;
        }
        else if (StaticVal.time[0] >= 15 && StaticVal.time[0] < 24)
        {
            StaticVal.light -= 0.0013f;
        }
        else
        {
            StaticVal.light = 0.30f;
        }
    }
}

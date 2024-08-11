using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ExperiencePopup : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float fadeDuration = 1f;

    private Label experienceLabel;
    private float startTime;
    public string str;

    void Start()
    {
        // Получаем доступ к UIDocument и Label
        UIDocument uiDocument = GetComponent<UIDocument>();
        experienceLabel = uiDocument.rootVisualElement.Q<Label>("ExperienceLabel");

        // Устанавливаем начальное значение
        foreach (int i in StaticVal.idSkills)
        {
            if (i != -1)
            {
                foreach (Skill s in StaticVal.dataBase.skils)
                {
                    if (s.Modifier == TypeModifier.Lerning)
                    {
                        if (s.LevelModifier == 1)
                        {
                            str = "+2 Опыта";
                        }
                        else if (s.LevelModifier == 2)
                        {
                            str = "+4 Опыта";
                        }
                        else if (s.LevelModifier == 3)
                        {
                            str = "+6 Опыта";
                        }
                    }
                }
            }
        }

        if (str == null || str == "")
        {
            str = "+1 Опыта";
        }
        
        experienceLabel.text = str;
        startTime = Time.time;
    }

    void Update()
    {
        // Двигаем метку вверх
        experienceLabel.transform.position -= Vector3.up * moveSpeed * Time.deltaTime;

        // Плавно уменьшаем прозрачность
        float t = (Time.time - startTime) / fadeDuration;
        experienceLabel.style.opacity = Mathf.Lerp(1f, 0f, t);

        // Уничтожаем объект после завершения анимации
        if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }
}

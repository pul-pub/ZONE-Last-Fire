using System.Collections;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [Space]
    [SerializeField] private Transform bg1;
    [SerializeField] private Transform bg2;
    [SerializeField] private Transform bg3;

    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.K))
        {
            transform.position += new Vector3(5f, 0, 0) * Time.deltaTime;
        }
        else
        {
            transform.position = target.position;
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);

            bg1.position = new Vector3(target.position.x / 4, bg1.position.y, 0);
            bg2.position = new Vector3(target.position.x / 8, bg1.position.y, 0);
            bg3.position = new Vector3(target.position.x / 16, bg1.position.y, 0);
        }
    }

    public void Dowen()
    {
        StartCoroutine(Sited());
    }

    public void Up()
    {
        StartCoroutine(NotSited());
    }

    IEnumerator Sited()
    {
        while (_cam.fieldOfView > 40)
        {
            _cam.fieldOfView -= 0.4f;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator NotSited()
    {
        while (_cam.fieldOfView < 45)
        {
            _cam.fieldOfView += 0.4f;
            yield return new WaitForEndOfFrame();
        }
    }
}

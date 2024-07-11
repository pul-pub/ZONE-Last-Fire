using UnityEngine;

public class DoorObject : MonoBehaviour
{
    public string SceneName = "name";
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public PlayerInterface playerInterface;
    [SerializeField] public Vector2 pos;

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
                        playerInterface.OpenWindowToMoveScene(this);
                        timer = 10;
                    }
                }
            }
        }


        if (timer >= 0)
            timer -= Time.deltaTime;
    }

    public void GoToScene()
    {
        if (gameObject.activeSelf) 
        {
            Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.7f, 3), 0, whatIsGround);
            if (cols.Length > 0)
            {
                foreach (Collider2D col in cols)
                {
                    if (col.CompareTag("Player") && timer <= 0)
                    {
                        playerInterface.OpenWindowToMoveScene(this);
                        timer = 10;
                    }
                }
            }
        }
        
    }
}

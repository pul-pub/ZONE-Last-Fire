using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private ObjectWeapon weapon;
    [Space]
    [SerializeField] private GameObject storeGObj;
    [SerializeField] private Object store;
    [SerializeField] private Transform parent;
    [Space]
    [SerializeField] private Object bolt;

    public void OnMoveDownStore()
    {
        GameObject gb = Instantiate(store, storeGObj.transform.position, storeGObj.transform.rotation, parent) as GameObject;

        Sprite sp = weapon.GetSpriteStor();
        if (sp != null)
        {
            gb.GetComponent<SpriteRenderer>().sprite = sp;
        }

        storeGObj.SetActive(false);
    }

    public void AddBolt()
    {
        GameObject gb = Instantiate(bolt, storeGObj.transform.position, storeGObj.transform.rotation, parent) as GameObject;

        if (player.transform.localScale.x > 0)
        {
            gb.GetComponent<Rigidbody2D>().velocity = player.transform.right.normalized * 5;
        }
        else if (player.transform.localScale.x < 0)
        {
            gb.GetComponent<Rigidbody2D>().velocity = -player.transform.right.normalized * 5;
            gb.transform.localScale = new Vector2(gb.transform.localScale.x * -1, gb.transform.localScale.y);
        }
    }

    public void OnEndAnimation()
    {
        storeGObj.SetActive(true);
        weapon.Reload();
    }
}

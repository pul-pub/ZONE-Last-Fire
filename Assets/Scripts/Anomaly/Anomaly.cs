using UnityEngine;

public class Anomaly : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator _anim;
    [Header("Attack")]
    [SerializeField] private Player player;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float sizeCheckShoot;
    [SerializeField] private float dm = 10;
    [SerializeField] private float startTimeBtwShot = 3;
    [SerializeField] private bool animation;

    private float _timeBtwShot = 1f;

    private void Update()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position + offset, sizeCheckShoot, whatIsGround);

        foreach (Collider2D col in cols)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                if (_timeBtwShot <= 0)
                {
                    if (animation)
                        _anim.Play("attackVoronka");
                    _timeBtwShot = startTimeBtwShot;
                    player._Object.TakeDamage(dm, TypeDamage.Gap);
                }
            }
            else if (col.gameObject.CompareTag("Bolt"))
            {
                if (_timeBtwShot <= 0)
                {
                    if (animation)
                        _anim.Play("attackVoronka");
                    _timeBtwShot = startTimeBtwShot;
                    Destroy(col.gameObject);
                }
            }
        }
        

        if (_timeBtwShot > 0)
        {
            _timeBtwShot -= Time.deltaTime;
        }
        else if (animation)
        {
            _anim.gameObject.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + offset, sizeCheckShoot);
    }
}

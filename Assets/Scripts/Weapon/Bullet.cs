using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float maxDistanc = 11f;
    public Vector2 sizeRay;
    public TypeBullet typeBullet;
    public float damage;
    public LayerMask whatIsGround;
    private Vector2 startPos;

    private void Awake()
    {
        startPos = transform.position;
    }

    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.BoxCast(transform.position, sizeRay, 0f, Vector2.zero, 0f, whatIsGround);
        if(hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy") && typeBullet == TypeBullet.Enemy)
            {
                Enemy _enemy = hitInfo.collider.GetComponentInParent<Enemy>();
               

                if (_enemy != null && _enemy.IsEnemyForPlayer && !_enemy.IsDided)
                {
                    _enemy._Object.TakeDamage(damage);
                    Destroy(gameObject);
                }
                    
                EnemyMutant _enemyMutant = hitInfo.collider.GetComponentInParent<EnemyMutant>();

                if (_enemyMutant != null && !_enemyMutant.IsDided) 
                {
                    _enemyMutant._Object.TakeClearDamage(damage);
                    Destroy(gameObject);
                }
            }
            else if (hitInfo.collider.CompareTag("Player") && typeBullet == TypeBullet.Player)
            {
                hitInfo.collider.GetComponentInParent<Player>()._Object.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        if (transform.position.x - startPos.x < maxDistanc * -1 || transform.position.x - startPos.x > maxDistanc)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, sizeRay);
    }
}

using UnityEngine;

public enum TypeDamage { Bullet, Gap }

[CreateAssetMenu(menuName = "Object_m", fileName = "None")]
public class MoveedObject : ScriptableObject
{
    public float Health = 100f;
    public float Arm = 100f;
    public float Speed = 5f;
    public float ForceJamp = 5f;
    public LayerMask WhatIsGround;

    public Armor armor;
    public Rigidbody2D rb;
    public Transform tr;

    public BoxCollider2D colliderLeg;

    public bool toRight = true;
    public bool isSit = false;

    public void Move(Vector3 vector, bool isGo)
    {
        if (!isSit && isGo)
        {
            rb.velocity = new Vector3(vector.x * Speed, rb.velocity.y);
        }

        if (!toRight && vector.x > 0)
        {
            Flip();
        }
        else if (toRight && vector.x < 0)
        {
            Flip();
        }
    }

    public void Jamp()
    {
        bool isGrounded = Physics2D.BoxCast(tr.position, new Vector3(1, 3, 0), 0f, new Vector3(), 0f, WhatIsGround);
        if (isGrounded)
        {
            rb.velocity = Vector2.up * ForceJamp;
        }
    }

    public void SitDowen(Vector2 offset)
    {
        isSit = true;
        colliderLeg.offset = offset;
        if (toRight) 
            rb.velocity += new Vector2(2, 0);
        else
            rb.velocity += new Vector2(-2, 0);
    }

    public void SendUp(Vector2 offset)
    {
        isSit = false;
        colliderLeg.offset = offset;
    }

    private void Flip()
    {
        toRight = !toRight;
        Vector3 scaler = tr.localScale;
        scaler.x = scaler.x  * -1;
        tr.localScale = scaler;
    }
    
    public void TakeDamage(float _damage, TypeDamage td = TypeDamage.Bullet)
    {
        float _dm;
        if (armor != null)
        {
            if (td == TypeDamage.Bullet)
                _dm = _damage - (armor.bulletResistance / 4);
            else
                _dm = _damage - (armor.tearProtection / 4);
        }
        else
            _dm = _damage;

        if (_dm > 0)
            Health -= _dm;
    }

    public void TakeClearDamage(float _damage)
    {
        Health -= _damage;
    }
}

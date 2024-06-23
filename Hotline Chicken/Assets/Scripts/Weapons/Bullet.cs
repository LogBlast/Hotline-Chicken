using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Bullet est différent de la classe Projectile !
// Bullet est la balle que le joueur va tirer avec ses armes

public class Bullet : MonoBehaviour
{
    [SerializeField] public float damage = 10f;
    [SerializeField] public Vector3 bulletSize = new Vector3(1f, 1f, 1f);

    private Weapon weapon;


    void Start()
    {
        transform.localScale = bulletSize;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage, weapon);
            Destroy(gameObject);
        }

        Destroy(gameObject);
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    public void SetWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
    }

}

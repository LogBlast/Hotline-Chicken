using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Projectile est différent de la classe Bullet !
// Projectile est la balle que l'ennemi va tirer

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    public float damage = 10f;

    [SerializeField] public Vector3 bulletSize = new Vector3(1f, 1f, 1f);

    void Start()
    {
        transform.localScale = bulletSize;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player != null) 
        {
            Debug.Log("Joueur touché !");
            player.TakeDamage(damage);
        }
        
        Destroy(gameObject);
        
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
}

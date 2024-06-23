using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    [SerializeField] private float fieldOfView = 90f; // Angle du champ de vision
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private Transform viewPoint;

    [SerializeField] private GameObject shotPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float nextFireTime = 0f;
    [SerializeField] private float projectileDamage = 10f;

    [SerializeField] private float speedMov = 2f;


    [SerializeField] public Color32 hitColor = new Color32(255, 0, 0, 255);
    [SerializeField] public float colorChangeDuration = 0.2f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gunSound;
    [SerializeField] private AudioClip damageSound;

    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private Color32 colNoDamage;

    private bool hasLineOfSight = false;

    private GameManager gameManager;


    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            colNoDamage = spriteRenderer.color;
        }


    }

    private void Update()
    {
        DetectPlayer();
    }



    private void DetectPlayer()
    {

        Vector2 directionToPlayer = (Vector2)(player.transform.position - viewPoint.position);

        
        RaycastHit2D hit = Physics2D.Raycast(viewPoint.position, directionToPlayer, viewDistance);

       
      //  Debug.DrawRay(viewPoint.position, directionToPlayer.normalized * viewDistance, Color.red);

        // Vérifier si le joueur est dans le champ de vision et le raycast l'a détecté
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
           

            RotateToPlayer(directionToPlayer);
            MoveToPlayer();

            if (Time.time >= nextFireTime)
            {
                Shoot(directionToPlayer);
                nextFireTime = Time.time + 1f / fireRate;
            }
        }

    }

    private void Shoot(Vector2 directionToPlayer)
    {
        if (shotPrefab != null && firePoint != null)
        {

            if (audioSource != null && gunSound != null)
            {
                audioSource.PlayOneShot(gunSound);
            }

            GameObject project = Instantiate(shotPrefab, firePoint.position, firePoint.rotation);
            Projectile projectScript = project.GetComponent<Projectile>();
            if (projectScript != null)
            {
                project.transform.right = directionToPlayer.normalized;
                projectScript.SetDamage(projectileDamage);
            }
        }
    }

    private void RotateToPlayer(Vector2 directionToPlayer)
    {
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void MoveToPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speedMov * Time.deltaTime);
    }

    public void TakeDamage(float damage, Weapon weapon)
    {
        health -= damage;

        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        if (health <= 0f)
        {
            Die(weapon);
        }
        else
        {
            StartCoroutine(ChangeColorOnHit());
        }
    }

    void Die(Weapon weapon)
    {

        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null && weapon != null)
        {
            playerScript.AddScore(weapon.scoreKill);
        }

        gameManager.DecreaseEnemyCount();
        Destroy(gameObject);
    }

    private IEnumerator ChangeColorOnHit()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(colorChangeDuration);
        spriteRenderer.color = colNoDamage;

    }


    // Dessiner le champ de vision dans l'diteur Unity
    private void OnDrawGizmosSelected()
    {
        if (viewPoint != null)
        {
            Gizmos.color = Color.red;

            // Dessiner la direction centrale du champ de vision
            Vector3 direction = viewPoint.right * viewDistance;
            Gizmos.DrawRay(viewPoint.position, direction);

            // Dessiner les bords du champ de vision
            float halfFOV = fieldOfView / 2f;
            direction = Quaternion.Euler(0, 0, -halfFOV) * viewPoint.right * viewDistance;
            Gizmos.DrawRay(viewPoint.position, direction);

            direction = Quaternion.Euler(0, 0, halfFOV) * viewPoint.right * viewDistance;
            Gizmos.DrawRay(viewPoint.position, direction);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rd;
    [SerializeField] private float speed;

    [SerializeField] private float maxHealth = 100f;
    private float health;

    [SerializeField] private Color32 hitColor = new Color32(255,0 , 0, 255);
    [SerializeField] private float colorChangeDuration = 0.2f;

    [SerializeField] private Weapon currentWeapon = null;
    [SerializeField] private GameObject weapon1Prefab;
    [SerializeField] private GameObject weapon2Prefab;
    [SerializeField] private GameObject weapon3Prefab;
    [SerializeField] private Transform rightHandAnchor;
    [SerializeField] private GameObject panelDeath;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip damageSound;

    private SpriteRenderer spriteRenderer;
    private Color32 colNoDamage;

    private UIManager uiManager;

    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        health = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer != null)
        {
            colNoDamage = spriteRenderer.color;
        }

        uiManager = FindObjectOfType<UIManager>();
        if(uiManager != null)
        {
            uiManager.SetPlayerHealth(health);
        }

    }

    void Update()
    {

        if (!PauseMenu.isPaused)
        {

        lookMouse();
        Move();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(weapon1Prefab, rightHandAnchor);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(weapon2Prefab, rightHandAnchor);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon(weapon3Prefab, rightHandAnchor);
        }

        }
    }


    private void lookMouse(){
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = (Vector3)(mousePos - new Vector2(transform.position.x, transform.position.y));
    }

    private void Move(){
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rd.velocity = input.normalized * speed;
    }


    public void ChangeWeapon(Weapon newWeapon)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }
        currentWeapon = newWeapon;
    }

    private void EquipWeapon(GameObject weaponPrefab, Transform anchor)
    {
        if (anchor == null)
        {
            Debug.LogError("Aucun point d'ancrage spécifié pour l'arme.");
            return;
        }

        // Instancier l'arme au point d'ancrage spécifié
        GameObject newWeaponObject = Instantiate(weaponPrefab, anchor.position, anchor.rotation, anchor);

        // Réinitialiser l'échelle locale pour éviter les héritages d'échelle
        newWeaponObject.transform.localScale = Vector3.one;

        // Récupérer le composant Weapon du nouvel objet créé
        Weapon newWeapon = newWeaponObject.GetComponent<Weapon>();

        // Ajuster l'échelle locale de l'arme
        newWeaponObject.transform.localScale = newWeapon.weaponSize;

        // Ajuster l'orientation de l'arme
        newWeaponObject.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

        // Assigner l'arme à currentWeapon
        ChangeWeapon(newWeapon);
    }

    public void TakeDamage(float damage)
    {
        health -=damage;


        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        if (uiManager != null)
        {
            uiManager.SetPlayerHealth(health);
        }


        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(ChangeColorOnHit());
        }
    }

    void Die()
    {
        spriteRenderer.color = Color.red;

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        panelDeath.SetActive(true);
        Time.timeScale = 0f;

        Debug.Log("Player died !");
    }

    private IEnumerator ChangeColorOnHit()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(colorChangeDuration);
        spriteRenderer.color = colNoDamage;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            Projectile projectile = collision.GetComponent<Projectile>();
            if(projectile != null)
            {
                TakeDamage(projectile.damage);
                Destroy(collision.gameObject);
            }
        }
    }

    public void AddScore(int point)
    {
        if(uiManager != null)
        {
            uiManager.AddScore(point);
        }
    }


    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0f, maxHealth); // Assure que la santé ne dépasse pas maxHealth

        if (uiManager != null)
        {
            uiManager.SetPlayerHealth(health);
        }
    }

    public IEnumerator SpeedBoost(float duration)
    {
        float originalSpeed = speed;
        speed *= 1.5f; // Exemple : augmente la vitesse de 50%

        yield return new WaitForSeconds(duration);   // permet de suspendre l'exécution de la coroutine pendant un certain temps spécifié (je ne connaissais pas du tout yield avant)

        speed = originalSpeed; // Retour à la vitesse originale après la durée spécifiée

        Debug.Log("Speed boost ended.");
    }
}

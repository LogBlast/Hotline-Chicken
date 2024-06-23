using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]  public GameObject bulletPrefab;
    [SerializeField]  public Transform firePoint;
    [SerializeField]  public Vector3 weaponSize = new Vector3(0.07f, 0.07f, 0.07f);
    [SerializeField]  public float bulletForce = 20f;

    [SerializeField]  public float damage = 10f;
    [SerializeField]  public float fireRate = 0.5f;
    private float nextTimeToFire = 0f; // Temps jusqu'au prochain tir
    [SerializeField]  public float timeToDestroy = 2f;

    [SerializeField] public int scoreKill = 10;

    [SerializeField]  public bool isAutomatic = false; // Tir automatique ou non

    [SerializeField]  public AudioClip weaponSound;
    private AudioSource audioSource;

    [SerializeField] private Animator flashAnimator;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }



    void Update()
    {
        if (isAutomatic)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }
        }
        
    }

    void Shoot()
    {

        audioSource.PlayOneShot(weaponSound);
        flashAnimator.SetTrigger("Shoot");


        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log("Mouse Position: " + mousePos);


        Vector2 direction = (mousePos - (Vector2)firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);


        bullet.transform.up = direction;
        bullet.transform.localScale = new Vector3(2f, 2f, 2f);
        bullet.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);


        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
    


        
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.SetDamage(damage);
            bulletComponent.SetWeapon(this);
        }


        Debug.Log("Destroy bullet after " + timeToDestroy + " seconds");
        Destroy(bullet, timeToDestroy);



    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] float destroyDelay;
 


    // Start is called before the first frame update
    void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other){
        
        
    }

    private void OnTriggerEnter2D (Collider2D other){

    if(other.tag == "Boost"){

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderTrigger : MonoBehaviour
{
    [SerializeField] public GameObject boulder;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player")|| collision.collider.CompareTag("Interagable"))
        {
            boulder.SetActive(true);
            Destroy(gameObject);  
        }
        
    }
}

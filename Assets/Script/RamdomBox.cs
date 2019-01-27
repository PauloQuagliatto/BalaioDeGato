using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamdomBox : MonoBehaviour
{
   [SerializeField] public GameObject pepino;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Interagable"))
            Destroy(gameObject);
            pepino.SetActive(true);

    }
}

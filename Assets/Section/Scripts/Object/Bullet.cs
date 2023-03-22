using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section4 {
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        private float speed = 5f;

        // Start is called before the first frame update
        void Start()
        {
            rb.velocity = transform.right * speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
/*            if (collision.CompareTag("Player"))*/
                Destroy(gameObject);
        }
    } 
}

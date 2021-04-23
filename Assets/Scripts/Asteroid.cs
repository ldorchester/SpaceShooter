using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 150f;
    private float _speed = 3;
    [SerializeField] private GameObject _explosionPrefab;

    void Start()
    {

    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        if (transform.position.y < -6.75f)
        {
            Destroy(this.gameObject);
        }
        if (transform.position.x > 11.5f)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, .25f);
        }

        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }    

        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }
}




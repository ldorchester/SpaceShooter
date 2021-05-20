using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 150f;
    private float _speed = 3;
    private AudioSource _audioSource;
    private Animator _anim;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            _audioSource.Play();
            _anim.SetTrigger("OnShootUpDeath");
            Destroy(this.gameObject, 1.5f);
        }

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            player.Damage();
            _anim.SetTrigger("OnShootUpDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 1.5f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _anim.SetTrigger("OnShootUpDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 1.5f);
        }
    }
}




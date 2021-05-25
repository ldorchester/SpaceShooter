using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 150f;
    private float _speed = 3;
    private AudioSource _audioSource;
    private Animator _anim;
    SpawnManager _spawnManager;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        /*var _randomTurn = Random.Range(1, 2);
        if (_randomTurn == 1)
        {
            transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        }
        if (_randomTurn == 2)
        {
            transform.Rotate(Vector3.back * _rotateSpeed * Time.deltaTime);
        }
       */
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
            _spawnManager.NumberEnemyDestroyed();
            Destroy(this.gameObject, 1.5f);
        }

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            player.Damage();
            _anim.SetTrigger("OnShootUpDeath");
            _speed = 0;
            _audioSource.Play();
            _spawnManager.NumberEnemyDestroyed();
            Destroy(this.gameObject, 1.5f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _anim.SetTrigger("OnShootUpDeath");
            _speed = 0;
            _audioSource.Play();
            _spawnManager.NumberEnemyDestroyed();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 1.5f);
        }
    }
}




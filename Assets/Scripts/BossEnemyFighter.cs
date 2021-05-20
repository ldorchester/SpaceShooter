using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyFighter : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    private AudioSource _audioSource;
    SpawnManager _spawnManager;
    private Player _player;
    private Animator _anim;
    [SerializeField] private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    void Start()
    {
        if (_player != null)
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        _audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        FireLasers();
    }

    //-----------------------------Custom Methods Below ------------------------------------------------
    void CalculateMovement()
    {
        transform.Translate(new Vector3(0, -1, 0) * _speed * Time.deltaTime);

        if (transform.position.y <= -6.5f)
        {
            float xRandomRespawn = Random.Range(-9, 9);
            transform.position = new Vector3(xRandomRespawn, 4.5f, 0);
        }
    }

     void FireLasers()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(1f, 3f);
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
        }
    }
 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            _audioSource.Play();
            _anim.SetTrigger("OnBomberDeath");
            _speed = 1;
            _player = GameObject.Find("Player").GetComponent<Player>();
            _player.AddToScore(30);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2f);
        }

        if (other.tag == "Player")
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            _player.AddToScore(30);
            _player.Damage();
            _anim.SetTrigger("OnBomberDeath");
            _speed = 1;
            _audioSource.Play();
            Destroy(this.gameObject, 2f);
        }
       
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _audioSource.Play();
            _anim.SetTrigger("OnBomberDeath");
            _speed = 1;
            _player = GameObject.Find("Player").GetComponent<Player>();
            _player.AddToScore(30);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2f);
        }
    }
}

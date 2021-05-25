using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    private AudioSource _audioSource;
    private Animator _anim;
    SpawnManager _spawnManager;
    private Player _player;
    [SerializeField] private GameObject _bombPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    [SerializeField] private float _speed = 1f;
    private bool _movingLeft;

    void Start()
    {
        _movingLeft = true;
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
        DropBombs();

    }

    //-----------------------------Custom Methods Below ------------------------------------------------
    void CalculateMovement()
    {
        if (_movingLeft == true)
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        if (transform.position.x <= -10f)
        {
            _movingLeft = false;
        }
        if (_movingLeft == false)
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        if (transform.position.x >= 9f)
        {
            _movingLeft = true;
        }
    }

    void DropBombs()
    {
       if (Time.time > _canFire)
       {
            if (this.gameObject != null)
            {
                _fireRate = Random.Range(1f, 3f);
                _canFire = Time.time + _fireRate;
                Instantiate(_bombPrefab, transform.position + new Vector3(0, -1f, 0), Quaternion.identity);
            }
       }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            _audioSource.Play();
            _anim.SetTrigger("OnShootUpDeath");
            _speed = 1;
            _player = GameObject.Find("Player").GetComponent<Player>();
            _player.AddToScore(20);
            _spawnManager.NumberEnemyDestroyed();
            Destroy(this.gameObject, 2f);
        }

        //If Enemy Collides with Laser
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _audioSource.Play();
            _anim.SetTrigger("OnShootUpDeath");
            _speed = 1;
            _player = GameObject.Find("Player").GetComponent<Player>();
            _player.AddToScore(20);
            _spawnManager.NumberEnemyDestroyed();
            Destroy(this.gameObject, 2f);
        }
    }

}


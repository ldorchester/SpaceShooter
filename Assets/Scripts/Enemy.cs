using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    private int _randomMove;
    private AudioSource _audioSource;
    [SerializeField] SpawnManager _spawnManager;
    private Player _player;
    private Animator _anim;
    [SerializeField] private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
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

        _randomMove = _spawnManager.CalculateRandomEnemyMovement();

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

        switch (_randomMove)
        {
            case 0:
                transform.Translate(new Vector3(0, -1, 0) * _speed * Time.deltaTime);
                break;
            case 1:
                transform.Translate(new Vector3(1, -1, 0) * _speed * Time.deltaTime);
                break;
            case 2:
                transform.Translate(new Vector3(-1, -1, 0) * _speed * Time.deltaTime);
                break;
            default:
                transform.position = new Vector3(0, 0, 0);
                break;
        }
        if (transform.position.y <= -6.5f)
        {
            float xRandomRespawn = Random.Range(-9, 9);
            transform.position = new Vector3(xRandomRespawn, 4.5f, 0);
        }

        // if (transform.position.y <= -6.5f)
        // {
        //     float xRandomRespawn = Random.Range(-9, 9);
        //     transform.position = new Vector3(xRandomRespawn, 4.5f, 0);
        // }
    }

    void FireLasers()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + new Vector3(0, -0.2f, 0), Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            _audioSource.Play();
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2f);
        }

        //If Enemy Collides with Player
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2f);
        }

        //If Enemy Collides with Laser
        if (other.tag == "Laser") 
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddToScore(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2f);
        }
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    private int _randomMove;
    private AudioSource _audioSource;
    SpawnManager _spawnManager;
    private Player _player;
    private Animator _anim;
    [SerializeField] private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    private bool _isShieldActive;
    private int _shieldPower;
    [SerializeField] private GameObject _shieldSprite;
    private int _randomShieldNumber;
    private Rigidbody2D rb;
    private Vector2 _movement;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        _isShieldActive = false;
        _shieldSprite.SetActive(false);
        CheckForShield();

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
    private void FixedUpdate()
   {
        MoveEnemy(_movement);
    }
 
    void MoveEnemy(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * 2f * Time.deltaTime));
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
                Vector3 direction3 = _player.transform.position - transform.position;
                direction3.Normalize();
                _movement = direction3;
                //transform.Translate(new Vector3(1, -1, 0) * _speed * Time.deltaTime);
                break;
            case 2:
                //transform.Translate(new Vector3(-1, -1, 0) * _speed * Time.deltaTime);
                Vector3 direction2 = _player.transform.position - transform.position;
                direction2.Normalize();
                _movement = direction2;
                break;
            case 3:
                Vector3 direction = _player.transform.position - transform.position;
                direction.Normalize();
                _movement = direction;
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

    private void CheckForShield()
    {
        _randomShieldNumber = Random.Range(1, 2);
        if (_randomShieldNumber == 1)
        {
            ShieldActive();
        }
    }

    private void ShieldActive()
    {
        _shieldPower = 1;
        _isShieldActive = true;
        _shieldSprite.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Missile" && _isShieldActive == true)
        {
            Destroy(other.gameObject);
            _shieldPower--;
            _shieldSprite.SetActive(false);
            StartCoroutine(ChangeShieldActiveDelay());
        }
        if (other.tag == "Missile" && _isShieldActive == false)
        {
            Destroy(other.gameObject);
            _audioSource.Play();
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2f);
        }

        if (other.tag == "Player" && _isShieldActive == true)
        {
            Player player = other.transform.GetComponent<Player>();

            
            player.Damage();
            //  }
          //  _anim.SetTrigger("OnEnemyDeath");
          //  _speed = 0;
          //  _audioSource.Play();
          //  Destroy(this.gameObject, 2f);
          //  Destroy(other.gameObject);
            _shieldPower--;
            _shieldSprite.SetActive(false);
            StartCoroutine(ChangeShieldActiveDelay());
        }
        if (other.tag == "Player" && _isShieldActive == false)
        {
            Player player = other.transform.GetComponent<Player>();

           // if (player != null)
           // {
                player.Damage();
          //  }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2f);
        }

        if (other.tag == "Laser" && _isShieldActive == true)
        {
            Destroy(other.gameObject);
            _shieldPower--;
            _shieldSprite.SetActive(false);
            StartCoroutine(ChangeShieldActiveDelay());
        }
        if (other.tag == "Laser" && _isShieldActive == false)
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

    IEnumerator ChangeShieldActiveDelay()
    {
        yield return new WaitForSeconds(.5f);
        _isShieldActive = false;
    }

}


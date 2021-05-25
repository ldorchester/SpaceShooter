using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    private float _bottomSpeed = 3f;
    private int _randomMove;
    private int _agroRange = 15;
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
    private bool _isMoving;
    private bool _movingRight;
    private bool _movingLeft;
    

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        _isShieldActive = false;
        _shieldSprite.SetActive(false);
        _isMoving = true;
        _movingRight = false;
        _movingLeft = false;
        CheckForShield();

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

        _randomMove = _spawnManager.CalculateRandomEnemyMovement();

        _audioSource = GetComponent<AudioSource>();

    }
   

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        CanSeePowerup(_agroRange);
        FireLasers();
    }
    private void FixedUpdate()
    {
        MoveEnemy(_movement);
    }
   
    //-----------------------------Custom Methods Below ------------------------------------------------
    bool CanSeePowerup(float distance)
    {
        bool val = false;
        float castDist = distance;
        Vector2 endPos = transform.position + Vector3.down * distance;

        RaycastHit2D hit = Physics2D.Linecast(transform.position, endPos, 1 << LayerMask.NameToLayer("Action"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("PowerUp"))
            {
                val = true;
                FireLasersAtPowerup();
            }
            else
            {
                val = false;
            }
        }
        return val;
    }

    void MoveEnemy(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * 2f * Time.deltaTime));
    }

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
            case 3:
                CalculateMovementDownStopEnemy();
                /*if (_player != null)
                {
                    Vector3 direction = _player.transform.position - transform.position;
                    direction.Normalize();
                    _movement = direction;
                    MoveEnemy(_movement);
                }
                */
                break;
            default:
                break;
        }

        if (transform.position.y <= -6.5f)
        {
            float xRandomRespawn = Random.Range(-9, 9);
            transform.position = new Vector3(xRandomRespawn, 4.5f, 0);
        }
    }

    void CalculateMovementDownStopEnemy()
    {
        if (_isMoving == true)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if (transform.position.y <= 2f)
        {
            _isMoving = false;
        }
        if (_isMoving == false)
        {
            StartCoroutine(WaitAndThenMove());
        }
    }

    IEnumerator WaitAndThenMove()
    {
        yield return new WaitForSeconds(2f);
        if (transform.position.x >= 6.28)
        {
            _movingLeft = true;
        }
        if (transform.position.x <= -9f)
        {
            { _movingRight = true; }
        }
        CalculateBottomMovement();
    }

    void CalculateBottomMovement()
    {
        if (_movingRight == true)
        {
            transform.Translate(Vector3.right * _bottomSpeed * Time.deltaTime);
        }
        if (transform.position.x >= 6.28f)
        {
            _movingRight = false;
        }
        if (_movingRight == false)
        {
            transform.Translate(Vector3.left * _bottomSpeed * Time.deltaTime);
        }
        if (transform.position.x <= -9f)
        {
            _movingRight = true;
        }
    }
    void FireLasersAtPowerup()
    {
        if (Time.time > _canFire)
        {
            _fireRate = 1f;
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + new Vector3(0, -0.2f, 0), Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
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
            _player = GameObject.Find("Player").GetComponent<Player>();
            _player.AddToScore(10);
            _audioSource.Play();
            _anim.SetTrigger("OnEnemyDeath");
            _spawnManager.NumberEnemyDestroyed();
            Destroy(this.gameObject, 2f);
        }

       if (other.tag == "Player" && _isShieldActive == true)
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            _player.Damage();
            _shieldPower--;
            _shieldSprite.SetActive(false);
            StartCoroutine(ChangeShieldActiveDelay());
        }
        if (other.tag == "Player" && _isShieldActive == false)
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            _player.AddToScore(10);
            _player.Damage();
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _spawnManager.NumberEnemyDestroyed();
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
            _player = GameObject.Find("Player").GetComponent<Player>();
            _player.AddToScore(10);
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _spawnManager.NumberEnemyDestroyed();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2f);
        }
    }

    IEnumerator ChangeShieldActiveDelay()
    {
        yield return new WaitForSeconds(.3f);
        _isShieldActive = false;
    }

}


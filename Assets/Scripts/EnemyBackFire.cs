using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBackFire : MonoBehaviour
{
    private int _speed = 2;
    private int _bottomSpeed = 2;
    [SerializeField] float _agroRange = 6f;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    [SerializeField] private bool _isMoving = true;
    [SerializeField] private bool _movingRight = false;
    [SerializeField] private bool _movingLeft = false;
    [SerializeField] Transform _castPoint;
    [SerializeField] private GameObject _laserPrefab;
    private AudioSource _audioSource;
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovementDown();
        CanSeePlayer(_agroRange);
    }

    void CalculateMovementDown()
    {
        if (_isMoving == true)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if (transform.position.y <= -5f)
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

    bool CanSeePlayer(float distance)
    {
        bool val = false;
        float castDist = distance;
        Vector2 endPos = _castPoint.position + Vector3.up * distance;

        RaycastHit2D hit = Physics2D.Linecast(_castPoint.position, endPos, 1 << LayerMask.NameToLayer("Action"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                val = true;
                FireLasers();
            }
            else
            {
                val = false;
            }
        }
        return val;
    }
   
    void FireLasers()
    {
        if (Time.time > _canFire)
        {
          _fireRate = 1f;
          _canFire = Time.time + _fireRate;
          Instantiate(_laserPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
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

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            // if (player != null)
            // {
            player.Damage();
            //  }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 1f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 1f);
        }
    }
}
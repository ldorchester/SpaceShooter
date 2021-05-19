using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapons : MonoBehaviour
{
    private int _hits = 4;
    private AudioSource _audioSource;
    private Animator _anim;
    [SerializeField] private GameObject _laserPrefab;
    private Player _player;
    private EnemyBoss _enemyBoss;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is Null");
        }

        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= 1.5f)
        {
            FireLasers();
        }
    }

    void FireLasers()
    {
        if (Time.time > _canFire)
        {
            _fireRate = 2f;
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            _hits--;
            if (_hits == 0)
            {
                _player.AddToScore(100);
                _enemyBoss = transform.parent.gameObject.GetComponent<EnemyBoss>();
                _anim.SetTrigger("OnBomberDeath");
                _audioSource.Play();
                _enemyBoss.WingsDestroyed();
                Destroy(this.gameObject, 2f);
            }
        }
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _hits--;
            if (_hits == 0)
            {
                _player.AddToScore(100);
                _enemyBoss = transform.parent.gameObject.GetComponent<EnemyBoss>();
                _anim.SetTrigger("OnBomberDeath");
                _audioSource.Play();
                _enemyBoss.WingsDestroyed();
                Destroy(this.gameObject, 2f);
            }
            
        }
    }
}


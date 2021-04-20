﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _fireRate = 0.25f;
    private float _canFire = -1f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shieldSprite;
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private GameObject _rightEngine;
    private SpawnManager _spawnManager;
    [SerializeField] public int ammo = 20;
    [SerializeField] private int _score;
    private UI_Manager _uiManager;
    [SerializeField] private AudioClip _laserClip;
    [SerializeField] private AudioClip _explosionClip;
    [SerializeField] private AudioSource _audioSource;

    private bool _isTripleShotActive = false;
    private bool _isSpeedActive = false;
    private bool _isShieldActive = false;


    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }
    }

    void Update()
    {
        //User Input & Movement
        CalculateMovement();

        //User Input Shift Key for SpeedBoost
        SpeedBoost();

        //Stop player from leaving screen
        TopBottomBounds();

        //Wrap player when they enter side of screen
        PlayerWrap();

        //Check to see if player is firing and spawn a laser & cool down so player can only fire 0.25.
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && ammo > 0)
        {
            FireLaser();
        }
    }


    //-------------------------Custom Methods Section--------------------------------
    public void AddToScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
    
    public void AmmoPowerUp()
    {
        ammo = 20;
        _uiManager.UpdateAmmo(ammo);
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldSprite.SetActive(true);
    }

    public void SpeedActive()
    {
        _isSpeedActive = true;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isSpeedActive = false;
        _speed = 5f;
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void HealthPowerUp()
    {
        if (_lives == 1)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
            _leftEngine.SetActive(false);
        }
        else if (_lives == 2)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
            _rightEngine.SetActive(false);
        }
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldSprite.SetActive(false);
            return;
        }

        _lives --;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
 
        if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
    
    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        ammo--;
        _uiManager.UpdateAmmo(ammo);

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.clip = _laserClip;
        _audioSource.Play();
        
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (_isSpeedActive == true)
        {
            _speed = 8;
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
    }

    void SpeedBoost()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            _speed = 6f;
        }
        else
        {
            _speed = 4f;
        }
    }
    void PlayerWrap()
    {
        float sideBoundsx = 10.75f;
        if (transform.position.x >= sideBoundsx)
        {
            transform.position = new Vector3(-sideBoundsx, transform.position.y, 0);
        }
        else if (transform.position.x <= -sideBoundsx)
        {
            transform.position = new Vector3(sideBoundsx, transform.position.y, 0);
        }
    }

    void TopBottomBounds()
    {
        float topBounds = 0f;
        float bottomBounds = -3.8f;
        if (transform.position.y >= topBounds)
        {
            transform.position = new Vector3(transform.position.x, topBounds, 0);
        }
        else if (transform.position.y < bottomBounds)
        {
            transform.position = new Vector3(transform.position.x, bottomBounds, 0);
        }
    }

  
}

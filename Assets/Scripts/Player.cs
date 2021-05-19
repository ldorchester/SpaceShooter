using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxThruster = 100;
    public float currentThruster;
    private float _powerupSpeed;
    public ThrusterBar thrusterBar;
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _fireRate = 0.25f;
    [SerializeField] private int _shieldPower;
    [SerializeField] private int _hits;
    private int _numberOfMissiles;
    private float _canFire = -1f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _missile1;
    [SerializeField] private GameObject _missile2;
    [SerializeField] private GameObject _missile3;
    [SerializeField] private GameObject _missile4;
    [SerializeField] private GameObject _missile5;
    [SerializeField] private GameObject _Missiles;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shieldSprite;
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private GameObject _rightEngine;
    private SpawnManager _spawnManager;
    [SerializeField] public int ammo = 15;
    [SerializeField] public int _missiles = 0;
    [SerializeField] private int _score;
    private UI_Manager _uiManager;
    [SerializeField] private AudioClip _laserClip;
    [SerializeField] private AudioClip _missileClip;
    [SerializeField] private AudioClip _explosionClip;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] public CameraShake cameraShake;

    private bool _isTripleShotActive = false;
    private bool _isSpeedActive = false;
    private bool _isShieldActive = false;
    private bool _isPenaltyActive = false;
    private bool _normalMovement;


    void Start()
    {
        _numberOfMissiles = 5;
        _powerupSpeed = 4;
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

        currentThruster = maxThruster;
        thrusterBar.SetMaxThruster(maxThruster);
        _normalMovement = true;
    }

    void Update()
    {

        //Shield method to take 3 hits
        CheckShieldDamage();

        //User Input & Movement
        CalculateMovement();

        //User Input Shift Key for SpeedBoost
       // SpeedBoost();

        //Stop player from leaving screen
        TopBottomBounds();

        //Wrap player when they enter side of screen
        PlayerWrap();

        //Check to see how many lives the player has and where the engine damage is at
        CheckPlayerHealth();

        //Check how many missiles player has
        NumberOfMissiles();

        //Attract Powerups to player
        PowerUpCollector();

        //Check to see if player is firing and spawn a laser & cool down so player can only fire 0.25.
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && ammo > 0)
        {
            FireLaser();
        }

        //Check to see if player is firing secondary missile
        if (Input.GetKeyDown(KeyCode.E) && Time.time > _canFire && _numberOfMissiles > 0)
        {
            FireMissile();
        }

        //Check to see if player is hitting shift for speed boost
        if (Input.GetKey(KeyCode.LeftShift) && (currentThruster != 0) && (_isSpeedActive != true))
        {
            SpeedBoost(.25f);
        }
        else 
        {
           _speed = 4f;
            if (!Input.GetKey(KeyCode.LeftShift) && (currentThruster < 100f))
            {
                currentThruster = currentThruster + .5f;
                thrusterBar.SetThruster(currentThruster);
            }
        }
    }



    //-------------------------Custom Methods Section--------------------------------

    void PowerUpCollector()
    {
        if (Input.GetKey(KeyCode.C))
        {
            GameObject[] _powerups = GameObject.FindGameObjectsWithTag("PowerUp");
            Transform[] powerupTransform = new Transform[_powerups.Length];

            for (int i = 0; i < +_powerups.Length; i++)
            {
                powerupTransform[i] = _powerups[i].transform;
                powerupTransform[i].position = Vector3.MoveTowards(powerupTransform[i].position, transform.position, _powerupSpeed * Time.deltaTime);
            }
        }
    }
    void SpeedBoost(float thrusters)
    {
        _speed = 6f;
        currentThruster -= thrusters;
        thrusterBar.SetThruster(currentThruster);
        if (currentThruster <= 0)
        {
            currentThruster = 0;
        }
    }

    public void CheckPlayerHealth()
    {
        if (_lives == 1)
        {
            _leftEngine.SetActive(true);
            _rightEngine.SetActive(true);
        }

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
            _rightEngine.SetActive(false);
        }

        if (_lives == 3)
        {
            _leftEngine.SetActive(false);
            _rightEngine.SetActive(false);
        }
    }
    
    public void AddToScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
    
    public void AmmoPowerUp()
    {
        ammo = 15;
        _uiManager.UpdateAmmo(ammo);
    }

    public void ShieldActive()
    {
        _shieldPower = 3;
        _isShieldActive = true;
        _shieldSprite.SetActive(true);
    }

    public void CheckShieldDamage()
    {
        if (_isShieldActive == true)
        { 
            if (_shieldPower == 3)
            {
                _shieldSprite.GetComponent<Renderer>().material.color = Color.blue;
            }
            if (_shieldPower == 2)
            {
                _shieldSprite.GetComponent<Renderer>().material.color = Color.green;
            }
            if (_shieldPower == 1)
            {
                 _shieldSprite.GetComponent<Renderer>().material.color = Color.red;
            }
            if (_shieldPower == 0)
            {
                _shieldSprite.SetActive(false);
                _isShieldActive = false;
            }
        }
        else
        {
            return;
        }
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

    public void Missiles()
    {
        _numberOfMissiles = 5;
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void HealthPowerUp()
    {
        if (_lives == 3)
        {
            return;
        }
        _lives++;
        _uiManager.UpdateLives(_lives);
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            if (_shieldPower == 0)
            {
                return;
            }
            if (_shieldPower == -1)
            {
                return;
            }
            _shieldPower--;
        }

        if (_isShieldActive == false)
        {
            StartCoroutine(cameraShake.Shake(.15f, .4f));
            _lives--;
            _uiManager.UpdateLives(_lives);
        }
       if (_lives <= 0)
       {
            _spawnManager.OnPlayerDeath();
            var powerup = GameObject.FindGameObjectsWithTag("PowerUp");
            foreach (var PowerUp in powerup)
            {
                Destroy(PowerUp);
            }
            var enemyshootup = GameObject.FindGameObjectsWithTag("EnemyShootUp");
            foreach (var EnemyShootUp in enemyshootup)
            {
                Destroy(EnemyShootUp);
            }
            var bomber = GameObject.FindGameObjectsWithTag("Bomber");
            foreach (var Bomber in bomber)
            {
                Destroy(Bomber);
            }
            var enemy = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var Enemy in enemy)
            {
                Destroy(Enemy);
            }
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
       }
    }
    public Transform GetClosestEnemy(GameObject[] enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }

        return bestTarget;
    }
    void FireMissile()
    {
        GameObject[] _enemiesActive = GameObject.FindGameObjectsWithTag("Enemy");
        _canFire = Time.time + _fireRate;
        _missiles--;
        _numberOfMissiles--;
        //_uiManager.UpdateMissileAmmo(_missiles);
        Transform target = GetClosestEnemy(_enemiesActive);
        GameObject missile = Instantiate(_Missiles, transform.position, Quaternion.identity);
        missile.GetComponent<Missile>().MissileTarget(target);

        _audioSource.clip = _missileClip;
        _audioSource.Play();
    }

    void NumberOfMissiles()
    {
        if (_numberOfMissiles == 5)
        {
            _missile1.SetActive(true);
            _missile2.SetActive(true);
            _missile3.SetActive(true);
            _missile4.SetActive(true);
            _missile5.SetActive(true);
        }
        if (_numberOfMissiles == 4)
        {
            _missile1.SetActive(true);
            _missile2.SetActive(true);
            _missile3.SetActive(true);
            _missile4.SetActive(true);
            _missile5.SetActive(false);
        }
        if (_numberOfMissiles == 3)
        {
            _missile1.SetActive(true);
            _missile2.SetActive(true);
            _missile3.SetActive(true);
            _missile4.SetActive(false);
            _missile5.SetActive(false);
        }
        if (_numberOfMissiles == 2)
        {
            _missile1.SetActive(true);
            _missile2.SetActive(true);
            _missile3.SetActive(false);
            _missile4.SetActive(false);
            _missile5.SetActive(false);
        }
        if (_numberOfMissiles == 1)
        {
            _missile1.SetActive(true);
            _missile2.SetActive(false);
            _missile3.SetActive(false);
            _missile4.SetActive(false);
            _missile5.SetActive(false);
        }
        if (_numberOfMissiles == 0)
        {
            _missile1.SetActive(false);
            _missile2.SetActive(false);
            _missile3.SetActive(false);
            _missile4.SetActive(false);
            _missile5.SetActive(false);
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
        if (_normalMovement == true)
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
        if (_normalMovement == false)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

            if (_isSpeedActive == true)
            {
                _speed = 3;
                transform.Translate(direction * _speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(direction * 2 * Time.deltaTime);
            }
        }
    }

    public void PenaltyMovement()
    {
        _isPenaltyActive = true;
        _normalMovement = false;
        StartCoroutine(PenaltyPowerDownRoutine());
    }

    IEnumerator PenaltyPowerDownRoutine()
    {
        yield return new WaitForSeconds(7f);
        _normalMovement = true;
        _isPenaltyActive = false;
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

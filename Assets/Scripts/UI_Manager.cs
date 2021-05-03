using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _laserAmmo;
    [SerializeField] private Text _missileAmmo;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private Image _livesImg;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Text _waveText;
    private int _wave;
    private Player _player;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _laserAmmo.text = "Lasers: " + 15;
        _missileAmmo.text = "Missiles";
        _wave = 1;
        StartCoroutine(ShowWave());

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
    }

    

    // Update is called once per frame
    void Update()
    {
       
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateAmmo(int updateAmmo)
    {
        _laserAmmo.text = "Laser: " + updateAmmo;
        if (updateAmmo <= 5)
        {
            _laserAmmo.GetComponent<Text>().color = Color.red;
        }
        else
        {
            _laserAmmo.GetComponent<Text>().color = Color.white;
        }
    }

    public void UpdateWave(int currentwave)
    {
        _wave = currentwave;
        StartCoroutine(ShowWave());
    }

    public void UpdateLives(int currentlives)
    {
        _livesImg.sprite = _liveSprites[currentlives];

        if (currentlives == 0)
        {
            GameOverSequence();
        }
    }

    public void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    private IEnumerator GameOverFlickerRoutine()
    {
        while (_gameOverText == true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(.5f);
        }
    }

    public IEnumerator ShowWave()
    {
        _waveText.text = "Wave: " + _wave;
        _waveText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        _waveText.gameObject.SetActive(false);
    }
    

   /* public void UpdateWave(int currentwave)
    {
        if (currentwave == 1)
        {
            ShowWave();
        }
        if (currentwave == 2)
        {
            ShowWave();
        }    
        if (currentwave == 3)
        {
            ShowWave();
        }
        if (currentwave == 4)
        {
            GameOverSequence();
        }
    }
   */

   
}

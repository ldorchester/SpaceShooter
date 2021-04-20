﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _laserAmmo;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private Image _livesImg;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    private Player player;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _laserAmmo.text = "Lasers: " + 20;

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
    }

    public void UpdateLives(int currentlives)
    {
        _livesImg.sprite = _liveSprites[currentlives];

        if (currentlives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    private IEnumerator GameOverFlickerRoutine()
    {
        while(_gameOverText == true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(.5f);
        }
    }
}
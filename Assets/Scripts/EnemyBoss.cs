﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    private float _downSpeed = 2f;
    private float _turnSpeed = 30f;
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    private UI_Manager _uiManager;
    [SerializeField] public CameraShake cameraShake;
    [SerializeField] private int _wingsDestroyed;
    [SerializeField] GameObject _explosion1;


    // Start is called before the first frame update
    void Start()
    {
        if (_player != null)
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        _wingsDestroyed = 4;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, -1, 0) * _downSpeed * Time.deltaTime);
        if (transform.position.y <= 2f)
        {
            StartCoroutine(StopAndBeginTurn());
        }
    }

    public void WingsDestroyed()
    {
        _wingsDestroyed--;
        if (_wingsDestroyed == 0)
        {
            StartCoroutine(DestroyBossRoutine());
        }
    }
    IEnumerator StopAndBeginTurn()
    {
        _downSpeed = 0f;
        yield return new WaitForSeconds(2f);
        transform.Rotate(new Vector3(0, 0, 1) * _turnSpeed * Time.deltaTime);
    }

    IEnumerator DestroyBossRoutine()
    {
        _turnSpeed = 0;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.AddToScore(1000);
        // GameObject.Find("BossExplosion1").GetComponent<Animator>();
        //  GameObject.Find("BossExplosion1").GetComponent<AudioSource>();
        // _anim.SetTrigger("OnBomberDeath");
        // _audioSource.Play();
        _uiManager.YouWinSequence();
        yield return new WaitForSeconds(.75f);
        Destroy(this.gameObject);


        var powerup = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (var PowerUp in powerup)
        {
            Destroy(PowerUp);
        }
        var bossenemyfighters = GameObject.FindGameObjectsWithTag("BossEnemyFighter");
        foreach (var BossEnemyFighters in bossenemyfighters)
        {
            Destroy(BossEnemyFighters);
        }
        // StartCoroutine(cameraShake.Shake(.2f, 4f));
       // yield return new WaitForSeconds(8f);
    }
/*
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
*/

}

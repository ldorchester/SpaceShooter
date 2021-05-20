using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnemy : MonoBehaviour
{
    [SerializeField] GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private Animator _anim;
    private AudioSource _audioSource;
    private float _turnSpeed = 30f;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

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
        transform.Rotate(Vector3.forward * _turnSpeed* Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            StartCoroutine(StopStartSpin());
            _anim.SetTrigger("OnShootUpDeath");
            Destroy(this.gameObject, 2.5f);
        }
    }

    IEnumerator StopStartSpin()
    {
        yield return new WaitForSeconds(.5f);
        _audioSource.Play();
        yield return new WaitForSeconds(1.9f);
        _spawnManager.StartSpawning();
    }
}

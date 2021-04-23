using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Missile : MonoBehaviour
{
    public Transform _target;

    private Player _player;
    private Rigidbody2D _rb;
    private Animator _anim;
    private AudioSource _audioSource;

    public float _speed = 5;
    public float _rotateSpeed = 200f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MissileCountdownRoutine());
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player in Missile Script is NULL");
        }

        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.LogError("Rigidbody in Missile Script is NULL");
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        if (_target)
        {
            Vector2 direction = (Vector2)_target.position - _rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            _rb.angularVelocity = -rotateAmount * _rotateSpeed;
        }

        _rb.velocity = transform.up * _speed;
    }
    public void MissileTarget (Transform target)
    {
        _target = target;
    }

    IEnumerator MissileCountdownRoutine()
    {
        yield return new WaitForSeconds(4f);
        Destroy(this.gameObject);
    }
}

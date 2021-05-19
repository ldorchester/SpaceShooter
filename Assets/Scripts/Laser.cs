using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 10.0f;
    private int _rightWingHit = 4;
    private int _leftWingHit = 4;
    private int _rightBackWingHit = 4;
    private int _leftBackWingHit = 4;
    private bool _isEnemyLaser = false;
    [SerializeField] GameObject _rightWing;
    [SerializeField] GameObject _leftWing;
    [SerializeField] GameObject _rightBackWing;
    [SerializeField] GameObject _leftBackWing;


    private void Start()
    {

    }
    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }

        if (_rightWingHit == 0)
        {
            Destroy(_rightWing);
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

   private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            
            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        } 

        if (other.tag == "Enemy" && _isEnemyLaser == true)
        {
            Destroy(this.gameObject);
        }

        if (other.tag == "PowerUp" && _isEnemyLaser == true)
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }

        if (other.tag == "MissilePowerUp" && _isEnemyLaser == true)
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }

        if (other.tag == "TripleShot" && _isEnemyLaser == true)
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLasers : MonoBehaviour
{
    


    void Update()
    {
        transform.Translate(Vector3.down * 10 * Time.deltaTime);

        if (transform.position.y < -6.4f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       /* if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }
       */

        if (other.tag == "PowerUp")
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }

        if (other.tag == "MissilePowerUp")
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }

        if (other.tag == "TripleShot")
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }
    }

}

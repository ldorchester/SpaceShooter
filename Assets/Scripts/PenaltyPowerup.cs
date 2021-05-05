using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenaltyPowerup : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(PenaltyPowerUpLength());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_audioClip, transform.position);

            if (player != null)
            {
                player.PenaltyMovement();
            }
        }
        Destroy(this.gameObject);
    }

    IEnumerator PenaltyPowerUpLength()
    {
        yield return new WaitForSeconds(4f);
        Destroy(this.gameObject);
    }
}

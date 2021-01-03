using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float powerUpSpeed = 2f;

    [SerializeField] private int powerUpID;

    [SerializeField] private AudioClip _powerUpSound;
 

    // Update is called once per frame
    void Update()
    {
        PowerUpMove();
    }

    private void PowerUpMove()
    {
        transform.Translate(Vector3.down * Time.deltaTime * powerUpSpeed);
        if (transform.position.y <= -7f)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_powerUpSound, transform.position);

            if (player != null)
            {
                switch (powerUpID)
                {
                    case 0:
                        player.EnableTripleShot();
                        break;
                    case 1:
                        player.EnableSpeedBoost();
                        break;
                    case 2:
                        player.EnableShield();
                        break;
                    
                }
              
                Destroy(this.gameObject);

        }

    }
       
    }
}

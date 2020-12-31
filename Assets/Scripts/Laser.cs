using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Laser : MonoBehaviour
{
 
    [SerializeField] private float _laserSpeed = 12f;
    private bool _isEnemyLaser = false;


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
        
       // DestroyLaser();
    }
      
    private void MoveUp()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _laserSpeed);

        if (transform.position.y >= 8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _laserSpeed);
        
        if (transform.position.y <= -8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _isEnemyLaser ==  true)
        {
            Player player = collision.GetComponent<Player>();
            if (!player)
            {
                Debug.LogError("Player is null");
            }
            else
            {
                player.ReceiveDamage();
            }
                
        }
    }
}




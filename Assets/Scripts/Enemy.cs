using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _enemySpeed = 4f;
    [SerializeField] private int _enemyScoreValue = 10;
    private Animator _enemyAnimator;
    private AudioSource _explosionSound;
    [SerializeField] private GameObject _enemyLaserPrefab;
    private float _enemyFireRate = 3.0f;
    private float _canFire = -1.0f;

    private void Start()
    {
        _explosionSound = GetComponent<AudioSource>();
        _enemyAnimator = GetComponent<Animator>();

    }
    void Update()
    {
        EnemyMovement();
        if (Time.time > _canFire)
        {
            _enemyFireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _enemyFireRate;

            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Hit: Player");
            Player player = other.GetComponent<Player>();
            if (!player)
            {
                return;
            }
            else
            {
                player.ReceiveDamage();
                _enemyAnimator.SetTrigger("OnEnemyDeath");
                _enemySpeed = 0;
                _explosionSound.Play();
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.3f);
            }
        }

        if (other.gameObject.CompareTag("Projectile"))
        {
            Player player = FindObjectOfType<Player>();
            Destroy(other.gameObject);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject.GetComponentInChildren<Laser>());
            player.AddScore(_enemyScoreValue);
            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            _explosionSound.Play();
            Destroy(this.gameObject, 2f);

        }
    }

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _enemySpeed);
        if (transform.position.y <= -7f)
        {
            Destroy(this.gameObject);
        }
        else
        {
            return;
        }
    }
}

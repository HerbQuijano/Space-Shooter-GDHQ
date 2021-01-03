using System.Collections;
using System.Linq.Expressions;
//using System.Security.Policy;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] public float _shipSpeed;
    private float _baseCubeSpeed;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject tripleShotPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    private float _nextFire = 0.0f;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private GameObject _shieldFXPrefab;
   
    [SerializeField] private GameObject _rightEngineDamage;
    [SerializeField] private GameObject _leftEngineDamage;

    [SerializeField] private int _playerHealth = 3;
    private SpawnManager _spawnManager;
    [SerializeField] private bool _isTripleShotActive = false;
    [SerializeField] private bool _isShieldActive = false;
    [SerializeField] private bool _isSpeedBoostActive = false;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private int _score = 0;
    private UIManager _ui_manager;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _ui_manager = FindObjectOfType<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _baseCubeSpeed = _shipSpeed;

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager NULL");
        }
        else
        {
            return;
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is null");
        }
        else
        {
            _audioSource.clip = _audioClip;
        }
    }

    void Update()
    {
        CalculateMovement();
        Fire();

    }

    private void Fire()
    {

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= _nextFire)
        {
            _nextFire = Time.time + _fireRate;
            if (_isTripleShotActive == true)
            {
                Instantiate(tripleShotPrefab, new Vector3(transform.position.x, transform.position.y + 1.05f, 0), Quaternion.identity);
            }
            if (_isTripleShotActive == false)
            {
                Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y + 1.05f, 0), Quaternion.identity);
            }
            _audioSource.Play();


        }

    }


    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


        if (_isSpeedBoostActive == false)
        {
            _shipSpeed = _baseCubeSpeed;
        }
        if (_isSpeedBoostActive == true)
        {
            _shipSpeed = 10f;
        }

        transform.Translate(direction * _shipSpeed * Time.deltaTime);



        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -11.3f, 11.3f), transform.position.y, 0);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5f, 0), 0);

        if (transform.position.x >= 11.3f)
        {

            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }


    }

    public void ReceiveDamage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldFXPrefab.SetActive(false);
            return;
        }
        _playerHealth = _playerHealth - 1;

        if (_playerHealth == 2)
        {
            _leftEngineDamage.SetActive(true);
        }
        else if (_playerHealth == 1)
        {
            _rightEngineDamage.SetActive(true);
        }
        
        
        _ui_manager.UpdateLives(_playerHealth);
       

        if (_playerHealth <= 0)
        {
            Instantiate(_explosionPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);

        }


    }

    public void EnableTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(PowerDown());
    }

    public void EnableSpeedBoost()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(PowerDown());
    }

    IEnumerator PowerDown()
    {
        yield return new WaitForSeconds(6f);
        _isSpeedBoostActive = false;
        _isTripleShotActive = false;
        _isShieldActive = false;
        _shieldFXPrefab.SetActive(false);

    }

    public void EnableShield()
    {
        _isShieldActive = true;
        _shieldFXPrefab.SetActive(true);
        StartCoroutine(PowerDown());


    }

    public void AddScore(int score)
    {

        this._score = _score + score;
        _ui_manager.UpdateScore(_score);

    }

    public string GetLives()
    {
        return _playerHealth.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyLaser"))
        {
            Destroy(other.gameObject);
            //ReceiveDamage();
        }


    }





}







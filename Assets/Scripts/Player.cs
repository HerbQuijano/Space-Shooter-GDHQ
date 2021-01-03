using System.Collections;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
//using System.Security.Policy;
using UnityEngine;
using UnityEngine.TestTools;
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
    private int _shieldStrength = 3;
    private SpawnManager _spawnManager;
    [SerializeField] private bool _isTripleShotActive = false;
    [SerializeField] private bool _isShieldActive = false;
    [SerializeField] private bool _isSpeedBoostActive = false;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private int _score = 0;
    private UIManager _ui_manager;
    public int ammoCount = 15;

    private bool _isShiftPressed = false;
//    private Color m_shieldColor;
//   private float m_Red, m_Blue, m_Green;
    private SpriteRenderer m_spriteRenderer;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _ui_manager = FindObjectOfType<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _baseCubeSpeed = _shipSpeed;
        m_spriteRenderer = _shieldFXPrefab.GetComponent<SpriteRenderer>();

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
                if (ammoCount > 0)
                {
                    Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y + 1.05f, 0), Quaternion.identity);
                    _audioSource.Play();
                    ammoCount -= 1;
                    _ui_manager.UpdateAmmo(ammoCount);
                }
                else if (ammoCount <= 0)
                {
                    _ui_manager.UpdateAmmo(ammoCount);
                    _audioSource.Stop();
                }
                
            }
            


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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isShiftPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isShiftPressed = false;
        }

        if (_isShiftPressed == true)
        {
            transform.Translate(direction * (_shipSpeed * 1.5f) * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _shipSpeed * Time.deltaTime);
        }






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
            ShieldDamage();
            return;
        }
        _playerHealth -= 1;

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

    private void ShieldDamage()
    {
        _shieldStrength -= 1;
        ShieldColorChange();


       if (_shieldStrength == 0)
        {
            _isShieldActive = false;
            _shieldFXPrefab.SetActive(false);
        }

    }

    private void ShieldColorChange()
    {
        

        switch (_shieldStrength)
        {
            case 3:
                m_spriteRenderer.color = Color.white;
                break;
            case 2:
                m_spriteRenderer.color = Color.yellow;
                break;
            case 1:
                m_spriteRenderer.color = Color.red;
                break;
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
        _shieldStrength = 3;
        m_spriteRenderer.color = Color.white;
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

    public string GetAmmo()
    {
        return ammoCount.ToString();
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







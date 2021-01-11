//using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _livesText;
    [SerializeField] Player player;
    [SerializeField] Sprite[] _livesSprites;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartLevelText;
    [SerializeField] private Text _ammoCount;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _scoreText.text = "Score: " + 0;
        _livesText.text = "Lives: " + player.GetLives();
        _ammoCount.text = "Ammo: " + player.GetAmmo();
        _gameOverText.gameObject.SetActive(false);

        if (!_gameManager)
        {
            Debug.LogError("GameManager does not exist");
            return;
        }


    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int playerHealth)
    {

        _livesImage.sprite = _livesSprites[playerHealth];
        _livesText.text = "Lives: " + playerHealth.ToString();
        if (playerHealth == 0)
        {
            GameOverSequence();

        }
    }
    public void UpdateAmmo(int ammoCount)
    {

        _ammoCount.text = "Ammo: " + ammoCount.ToString();
        if (ammoCount <= 0)
        {
            _ammoCount.color = Color.red;

        }
        else if (ammoCount > 0)
        {
            _ammoCount.color = Color.white;
        }
    }

    private void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartLevelText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
        _gameManager.GameOver();

    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {

            _gameOverText.text = "GAME OVER!";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);

        }


    }



}




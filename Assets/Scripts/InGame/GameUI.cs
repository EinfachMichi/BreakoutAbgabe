using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace InGame
{
    public class GameUI : MonoBehaviour
    {
        public static GameUI instance;
        
        [Header("Info Panel")]
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Button continueButton;
        [SerializeField] private TextMeshProUGUI continueButtonText;
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [Space]
        
        [Header("In Game UI")] 
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI liveText;
        [SerializeField] private GameObject optionsPanel;
        [SerializeField] private GameObject pressSpaceToStart;

        [SerializeField] private Image[] stars;
        [SerializeField] private Sprite goldenStar;
        [SerializeField] private Sprite silverStar;

        private bool _pause;
        private Scene _currentScene;
        
        private void Awake()
        {
            instance = this;
        }

        private void OnEnable()
        {
            GameManager.OnLevelComplete += LevelCompletePanel;
            GameManager.OnGameOver += GameOverPanel;
        }

        private void Start()
        {
            _currentScene = SceneManager.GetActiveScene();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ContinueButton();
            }
        }

        public void ShowPressSpaceToStart()
        {
            pressSpaceToStart.SetActive(true);
        }

        public void HidePressSpaceToStart()
        {
            pressSpaceToStart.SetActive(false);
        }
        
        private void ToggleOptions(bool value)
        {
            HidePressSpaceToStart();
            if (value)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            optionsPanel.SetActive(value);
        }
        
        public void UpdateUI()
        {
            scoreText.text = GameManager.instance.score.ToString();
            liveText.text = GameManager.instance.lives.ToString();
        }
        
        private void GameOverPanel()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            title.text = "Game Over!";
            if (SaveSystem.instance.activeSave.levels.Length < _currentScene.buildIndex + 1)
            {
                continueButton.gameObject.SetActive(false);
            }
            else
            {
                continueButton.onClick.AddListener(RetryButton);
                continueButtonText.text = "Retry";
            }
            finalScoreText.text = GameManager.instance.score.ToString();
            SetStars();
            infoPanel.SetActive(true);
            SaveStats();
        }

        private void LevelCompletePanel()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            title.text = "Level Complete!";
            if (SaveSystem.instance.activeSave.levels.Length < _currentScene.buildIndex + 1)
            {
                continueButton.gameObject.SetActive(false);
            }
            else
            {
                continueButton.onClick.AddListener(NextLevelButton);
                continueButtonText.text = "Next Level";
            }
            finalScoreText.text = GameManager.instance.score.ToString();
            SetStars();
            infoPanel.SetActive(true);
            if(_currentScene.buildIndex + 1 <= SaveSystem.instance.activeSave.levels.Length)
            {
                SaveSystem.instance.activeSave.levels[_currentScene.buildIndex].isUnlocked = true;
            }
            SaveStats();
        }

        private void SaveStats()
        {
            if (!(GameManager.instance.CalculateStars() < SaveSystem.instance.activeSave.levels[_currentScene.buildIndex - 1].stars))
            {
                SaveSystem.instance.activeSave.levels[_currentScene.buildIndex - 1].stars = GameManager.instance.CalculateStars();
            }
            SaveSystem.instance.Save();
        }

        private void SetStars()
        {
            int temp = GameManager.instance.CalculateStars();
            for (int i = 0; i < 3; i++)
            {
                if (temp > i)
                {
                    stars[i].sprite = goldenStar;
                }
                else
                {
                    stars[i].sprite = silverStar;
                }
            }
        }

        public void ContinueButton()
        {
            _pause = !_pause;
            ToggleOptions(_pause);
            if (_pause)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
        
        public void RetryButton()
        {
            SceneManager.LoadScene(_currentScene.buildIndex);
        }

        public void NextLevelButton()
        {
            SceneManager.LoadScene(_currentScene.buildIndex + 1);
        }

        public void GoToMenuButton()
        {
            optionsPanel.SetActive(false);
            SceneManager.LoadScene("Menu Scene");
        }

        private void OnDisable()
        {
            GameManager.OnGameStart -= ShowPressSpaceToStart;
            GameManager.OnLevelComplete -= LevelCompletePanel;
            GameManager.OnGameOver -= GameOverPanel;
        }
    }
}
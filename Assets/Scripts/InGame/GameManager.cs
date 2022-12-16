using System;
using UnityEngine;

namespace InGame
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        
        public static event Action OnGameStart;
        public static event Action OnGameOver;
        public static event Action OnLevelComplete;

        public int score;
        public int scorePerBrick;
        public int lives;
        public bool inGame;

        private int _brickCount;
        [HideInInspector] public bool levelComplete;
        private AudioManager _audioManager;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            _audioManager = FindObjectOfType<AudioManager>();
            Time.timeScale = 1f;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !inGame)
            {
                StartGame();
            }
        }

        public void AddScore(int level)
        {
            score += level * scorePerBrick;
            GameUI.instance.UpdateUI();
        }
        
        public void LoseLive()
        {
            score /= 2;
            if (score < 0) score = 0;
            
            if (lives > 0)
            {
                lives--;
                inGame = false;
            }
            else
            {
                GameOver();
            }
            GameUI.instance.UpdateUI();
        }

        public int CalculateStars()
        {
            int stars = 0;
            int maxPossibleScore = BrickSpawner.instance.GetMaxScorePossible(scorePerBrick);
            print(maxPossibleScore);
            if (score == maxPossibleScore) stars = 3;
            else if (score >= (int)maxPossibleScore * 0.66f) stars = 2;
            else if (score >= (int)maxPossibleScore * 0.33f) stars = 1;
            else stars = 0;
            return stars;
        }
        
        public void UpdateBrickCount(int value)
        {
            _brickCount += value;
            if (_brickCount <= 0)
            {
                LevelComplete();
            }
        }
        
        private void StartGame()
        {
            inGame = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            OnGameStart?.Invoke();
        }

        private void LevelComplete()
        {
            inGame = false;
            levelComplete = true;
            Time.timeScale = 0f;
            _audioManager.Play("Level Complete", _audioManager.effectSounds);
            GameUI.instance.HidePressSpaceToStart();
            OnLevelComplete?.Invoke();
        }
        
        private void GameOver()
        {
            inGame = false;
            Time.timeScale = 0f;
            GameUI.instance.HidePressSpaceToStart();
            OnGameOver?.Invoke();   
        }
    }
}
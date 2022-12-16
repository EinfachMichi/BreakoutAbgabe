using System;
using UnityEngine;

namespace InGame
{
    public class BallMovement : MonoBehaviour
    {
        public float speed;
        public float runtimeSpeed;
        [SerializeField] private float bounciness;
        [SerializeField] private ParticleSystem bounceEffect;
        
        private Rigidbody2D _rb;
        private GameObject _player;
        private PlayerMovement _playerMovement;
        private AudioManager _audioManager;
        public float speedUpPowerUpTime;
        private bool _speedup;
        public float slowDownPowerUpTime;
        private bool _slowdown;
        private float _timer;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            GameManager.OnGameStart += LaunchBall;
        }
    
        private void Start()
        {
            _audioManager = FindObjectOfType<AudioManager>();
            _player = GameObject.Find("Player");
            _playerMovement = _player.GetComponent<PlayerMovement>();

            runtimeSpeed = speed;
        }
    
        private void Update()
        {
            if (!GameManager.instance.inGame && !GameManager.instance.levelComplete)
            {
                AttachBallToPlayer();
            }

            if (_speedup)
            {
                _timer += Time.deltaTime;
                if (_timer >= speedUpPowerUpTime)
                {
                    _speedup = false;
                    runtimeSpeed = speed;
                }
            }
            else if (_slowdown)
            {
                _timer += Time.deltaTime;
                if (_timer >= slowDownPowerUpTime)
                {
                    _slowdown = false;
                    runtimeSpeed = speed;
                }
            }
            else
            {
                _timer = 0;
            }
        }

        public void SpeedUp()
        {
            _speedup = true;
            _timer = 0;
            runtimeSpeed = speed * 1.2f;
        }

        public void SlowDown()
        {
            _slowdown = true;
            _timer = 0;
            runtimeSpeed = speed * 0.75f;
        }
        
        private void AttachBallToPlayer()
        {
            GameUI.instance.ShowPressSpaceToStart();
            transform.position = _player.transform.position + Vector3.up;
        }
    
        private void LaunchBall()
        {
            GameUI.instance.HidePressSpaceToStart();
            if (_playerMovement.direction == Direction.Right) _rb.velocity = (Vector2.up + Vector2.right) * runtimeSpeed / 2;
            else _rb.velocity = (Vector2.up + Vector2.left) * runtimeSpeed / 2;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Bottom Trigger")) return;

            GameManager.instance.LoseLive();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            _audioManager.Play("Click", _audioManager.effectSounds);
            Instantiate(bounceEffect, transform.position, Quaternion.identity);
            if (!col.collider.CompareTag("Player")) return;
            
            float newXDirection = (transform.position.x - col.collider.transform.position.x) / col.collider.bounds.size.x;
            _rb.velocity = new Vector2(newXDirection * bounciness, 1).normalized * runtimeSpeed;
        }
        
        private void OnDisable()
        {
            GameManager.OnGameStart -= LaunchBall;
        }
    }
}
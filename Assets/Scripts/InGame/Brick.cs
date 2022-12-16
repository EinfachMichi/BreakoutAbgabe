using System;
using Unity.VisualScripting;
using UnityEngine;

namespace InGame
{
    public class Brick : MonoBehaviour
    {
        public Color color;
        public int health = 1;
        public PowerUpType type;
        public float explosionRadius;
        public LayerMask brickLayer;
        public ParticleSystem explosion;
        [SerializeField] private Sprite[] crackedBricks;
        
        private SpriteRenderer _sr;
        private AudioManager _audioManager;

        private int _level;

        private void Awake()
        {
            _sr = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            _audioManager = FindObjectOfType<AudioManager>();
            GameManager.instance.UpdateBrickCount(1);
            SetProperties();
        }

        private void SetProperties()
        {
            _level = health;
            _sr.color = color;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            switch (type)
            {
                case PowerUpType.Bomb:
                    Explode();
                    Destroy(gameObject);
                    return;
                case PowerUpType.Speed:
                    SpeedUp(col);
                    GameManager.instance.AddScore(_level);
                    GameManager.instance.UpdateBrickCount(-1);
                    Destroy(gameObject);
                    return;
                case PowerUpType.Slow:
                    SlowDown(col);
                    GameManager.instance.AddScore(_level);
                    GameManager.instance.UpdateBrickCount(-1);
                    Destroy(gameObject);
                    return;
            }
            health--;
            switch (health)
            {
                case 1:
                    _sr.sprite = crackedBricks[3];
                    break;
                case 2:
                    _sr.sprite = crackedBricks[2];
                    break;
                case 3:
                    _sr.sprite = crackedBricks[1];
                    break;
                case 4:
                    _sr.sprite = crackedBricks[0];
                    break;
            }
            if (health <= 0)
            {
                GameManager.instance.AddScore(_level);
                GameManager.instance.UpdateBrickCount(-1);
                Destroy(gameObject);
            }
        }

        private void Explode()
        {
            _audioManager.Play("Explosion", _audioManager.effectSounds);
            Instantiate(explosion, transform.position, Quaternion.identity);
            Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, Vector2.one * explosionRadius, 0f, brickLayer);
            foreach (Collider2D col in cols)
            {
                GameManager.instance.AddScore(col.GetComponent<Brick>()._level);
                GameManager.instance.UpdateBrickCount(-1);
                Destroy(col.gameObject);
            }
        }

        private void SlowDown(Collision2D col)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            col.collider.GetComponent<BallMovement>().SlowDown();
        }
        
        private void SpeedUp(Collision2D col)
        {
            col.collider.GetComponent<BallMovement>().SpeedUp();
        }
    }
}

public enum PowerUpType
{
    Default, Bomb, Speed, Slow
}
using UnityEngine;

namespace InGame
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;

        [HideInInspector] public Direction direction;
    
        private Rigidbody2D _rb;
        private float _horizontalInput;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");

            direction = _horizontalInput > 0 ? Direction.Right : Direction.Left;
        }

        private void FixedUpdate()
        {
            _rb.velocity = new Vector2(_horizontalInput * speed, 0f);
        }
    }

    public enum Direction
    {
        Right, Left
    }
}
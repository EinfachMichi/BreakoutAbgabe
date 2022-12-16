using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace InGame
{
    public class BrickSpawner : MonoBehaviour
    {
        public static BrickSpawner instance;
        
        [SerializeField] private Vector2 startPos;
        [SerializeField] private float xGap, yGap;
        [Space]
        
        public BrickRowPattern[] brickRows;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            SpawnBricks();
        }

        private void SpawnBricks()
        {
            Vector2 pos = startPos;
            for (int y = 0; y < brickRows.Length; y++)
            {
                for (int x = 0; x < brickRows[y].bricks.Length; x++)
                {
                    Instantiate(brickRows[y].bricks[x].brick, pos, Quaternion.identity);
                    pos.x += 1.5f + xGap;
                }
                pos.x = startPos.x;
                pos.y -= yGap;
            }   
        }

        public int GetMaxScorePossible(int scorePerBrick)
        {
            int temp = 0;
            for (int y = 0; y < brickRows.Length; y++)
            {
                for (int x = 0; x < brickRows[y].bricks.Length; x++)
                {
                    temp += brickRows[y].bricks[x].level * scorePerBrick;
                }
            }   
            return temp;
        }
    }
}
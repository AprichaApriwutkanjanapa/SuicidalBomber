using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Student
{

    public class SnakeController : MonoBehaviour
    {
        public GameObject snakeSegmentPrefab;
        private LinkedList<GameObject> snakeBody = new LinkedList<GameObject>();
        private Vector3 direction = Vector3.right;
        public float moveSpeed = 0.5f;

        void Start()
        {
            // Add the initial head of the snake
            GameObject head = Instantiate(snakeSegmentPrefab, transform.position, Quaternion.identity);
            snakeBody.AddFirst(head);
            StartCoroutine(MoveSnake());
        }

        void Update()
        {
            // Update direction based on input
            if (Input.GetKeyDown(KeyCode.W))
                direction = Vector3.up;
            else if (Input.GetKeyDown(KeyCode.S))
                direction = Vector3.down;
            else if (Input.GetKeyDown(KeyCode.A))
                direction = Vector3.left;
            else if (Input.GetKeyDown(KeyCode.D))
                direction = Vector3.right;
            else if (Input.GetKeyDown(KeyCode.Space))
                GrowSnake();
        }

        IEnumerator MoveSnake()
        {
            while (true)
            {
                yield return new WaitForSeconds(moveSpeed);
                Move();
            }
        }

        void Move()
        {
            Vector3 newPosition = snakeBody.First.Value.transform.position + direction;

            // 1. Move the tail to the head's new position
        }

        public void GrowSnake()
        {
            // 2. Instantiate a new segment at the tail's position
        }
    }


}
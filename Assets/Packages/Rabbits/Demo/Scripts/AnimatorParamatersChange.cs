using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FiveRabbitsDemo
{
    public class RandomMovementWithAnimation : MonoBehaviour
    {
        public float moveSpeed = 2f; // Tốc độ di chuyển của enemy
        public Vector2 movementRange = new Vector2(10f, 10f); // Phạm vi di chuyển của enemy

        private Vector3 startPosition; // Vị trí ban đầu của enemy
        private Vector3 targetPosition;
        private Animator m_animator;

        void Start()
        {
            m_animator = GetComponent<Animator>();
            startPosition = transform.position; // Lưu vị trí ban đầu
            SetRandomTargetPosition();

            // Chạy animation "Run"
            m_animator.SetInteger("AnimIndex", 1);
            m_animator.SetTrigger("Next");
        }

        void Update()
        {
            // Di chuyển enemy về phía vị trí mục tiêu
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Hướng đầu của enemy về phía vị trí mục tiêu
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
            }

            // Nếu enemy đã đến gần vị trí mục tiêu, thiết lập một vị trí mục tiêu mới
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                SetRandomTargetPosition();
            }
        }

        void SetRandomTargetPosition()
        {
            float randomX = Random.Range(startPosition.x - movementRange.x, startPosition.x + movementRange.x);
            float randomZ = Random.Range(startPosition.z - movementRange.y, startPosition.z + movementRange.y);

            // Giới hạn vị trí mục tiêu trong phạm vi cho phép
            randomX = Mathf.Clamp(randomX, startPosition.x - movementRange.x, startPosition.x + movementRange.x);
            randomZ = Mathf.Clamp(randomZ, startPosition.z - movementRange.y, startPosition.z + movementRange.y);

            targetPosition = new Vector3(randomX, transform.position.y, randomZ);
        }
    }
}

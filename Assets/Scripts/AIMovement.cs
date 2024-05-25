using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // Tốc độ di chuyển của enemy
    public float changeTime = 2f; // Thời gian thay đổi hướng di chuyển
    public float areaSize = 10f; // Kích thước của khu vực di chuyển ngẫu nhiên

    private Vector3 targetPosition;
    private float timer;
    private Animator animator;

    void Start()
    {
        // Khởi tạo giá trị ban đầu
        SetNewRandomPosition();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Tính toán hướng di chuyển
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Di chuyển enemy về hướng targetPosition
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Xoay enemy theo hướng di chuyển
        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }

        // Đếm thời gian và thay đổi hướng di chuyển khi đến giới hạn thời gian
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SetNewRandomPosition();
            timer = changeTime;
        }

        // Nếu enemy đã tới gần vị trí đích thì chọn vị trí mới
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewRandomPosition();
        }

        animator.SetInteger("AnimIndex", 2);
        // animator.SetTrigger("Next");
    }

    void SetNewRandomPosition()
    {
        // Tạo vị trí ngẫu nhiên trong khu vực xác định
        float randomX = Random.Range(-areaSize, areaSize);
        float randomZ = Random.Range(-areaSize, areaSize);
        targetPosition = new Vector3(randomX, transform.position.y, randomZ);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Khi va chạm với bất kỳ vật thể nào có collider, chọn vị trí ngẫu nhiên mới
        SetNewRandomPosition();
        Debug.Log("collide");
    }
}

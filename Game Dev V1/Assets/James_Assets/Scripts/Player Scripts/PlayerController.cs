using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    PlayerStats PStats;
    public float moveSpeed;
    void Start()
    {
        moveSpeed = 6f;
        PStats = GetComponent<PlayerStats>();
    }
    void Update()
    {
        transform.Translate(moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime, 0f, moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
    }
}

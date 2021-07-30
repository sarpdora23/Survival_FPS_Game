using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController charactercontroller;
    public float speed = 5f;
    public float gravity = 20f;
    public float jump_force = 10f;
    private Vector3 movement_direction;
    private float vertical_velocity;

    // Start is called before the first frame update
    private void Awake()
    {
        charactercontroller = gameObject.GetComponent<CharacterController>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveThePlayer();
    }
    void MoveThePlayer()
    {
        movement_direction = new Vector3(Input.GetAxis(Axis.Horizontal), 0, Input.GetAxis(Axis.Vertical));
        movement_direction = gameObject.transform.TransformDirection(movement_direction);
        movement_direction *= speed * Time.deltaTime;
        ApplyGravity();
        charactercontroller.Move(movement_direction);
    }
    void ApplyGravity()
    {
        vertical_velocity -= gravity * Time.deltaTime;
        PlayerJump();
        movement_direction.y = vertical_velocity * Time.deltaTime;
    }
    void PlayerJump()
    {
        if (charactercontroller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            vertical_velocity = jump_force;
        }
    }
}

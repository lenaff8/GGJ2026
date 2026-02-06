using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Configuraci�n de Movimiento")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Detecci�n de Suelo")]
    [SerializeField] private float groundCheckDistance = 0.6f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private InputSystem_Actions controls;
    private Vector2 moveInput;
    private bool isTouchingGround;
    private bool facingRight = true;
    private Animator animator;
    private bool wasInIdle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        controls = new InputSystem_Actions();

        controls.Player.Jump.performed += context => Jump();
        wasInIdle = true;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();

        CheckGround();
        HandleFlip();
    }

    private void FixedUpdate()
    {
        Move();
        SetAnimatorValues();
        SoundCheck();
    }

    private void SetAnimatorValues()
    {
        animator.SetBool("HorizontalBool", rb.linearVelocity.x != 0);
        animator.SetFloat("Vertical", rb.linearVelocity.y);
        animator.SetBool("VerticalBool", rb.linearVelocity.y != 0);
    }
    
    private void SoundCheck()
    {
        if (rb.linearVelocity.x != 0 && wasInIdle && isTouchingGround)
        {
            wasInIdle = false;
            SoundManager.Instance.Play("Running");
        }
        else if (rb.linearVelocity.x == 0 && !wasInIdle)
        {
            wasInIdle = true;
            SoundManager.Instance.Stop("Running");
        }
    }
    
    private void Move()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        if (isTouchingGround)
        {
            SoundManager.Instance.Play("Jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void CheckGround()
    {
        bool wasTouchingGround = isTouchingGround;
        isTouchingGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        if(!wasTouchingGround && isTouchingGround)
            SoundManager.Instance.Play("Landing");

    }

    private void HandleFlip()
    {
        if ((moveInput.x > 0 && !facingRight) || (moveInput.x < 0 && facingRight))
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isTouchingGround ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
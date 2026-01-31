using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    public enum MovementAxis
    {
        Horizontal,
        Vertical
    }

    public enum MovementDirection
    {
        RightOrUp = 1,
        LeftOrDown = -1
    }

    [Header("Movement configuration")]
    [Tooltip("Movement speed")]
    [SerializeField] private float speed = 4f;

    [Tooltip("The axis the platform will move on")]
    [SerializeField] private MovementAxis axis = MovementAxis.Horizontal;

    [Tooltip("The initial direction of the movement")]
    [SerializeField] private MovementDirection direction = MovementDirection.RightOrUp;

    [Header("Patrol configuration")]
    [Tooltip("The total movement distance")]
    [SerializeField] private float distance = 5f;

    [Tooltip("Wait time when the movement ends")]
    [SerializeField] private float waitTime = 1f;

    [Header("Activación")]
    [Tooltip("If true, waits until the player touches the platform to move")]
    [SerializeField] private bool moveOnTouch = false;

    private Rigidbody2D platformRb;
    private Rigidbody2D playerRb;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool isMovingToDestination = true;
    private bool canMove = true;
    private bool hasStartedMoving = false;

    private void Awake()
    {
        platformRb = GetComponent<Rigidbody2D>();
        platformRb.bodyType = RigidbodyType2D.Kinematic;
        platformRb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Start()
    {
        startPosition = transform.position;

        float calcDist = distance * (int)direction;
        if (axis == MovementAxis.Horizontal)
            targetPosition = new Vector2(startPosition.x + calcDist, startPosition.y);
        else
            targetPosition = new Vector2(startPosition.x, startPosition.y + calcDist);

        if (!moveOnTouch) hasStartedMoving = true;
    }

    private void FixedUpdate()
    {
        if (hasStartedMoving && canMove)
        {
            MovePlatform();
        }
        else
        {
            platformRb.linearVelocity = Vector2.zero;
        }

        MovePlayer();
    }

    private void MovePlatform()
    {
        Vector2 actualDestination = isMovingToDestination ? targetPosition : startPosition;
        float remainingDistance = Vector2.Distance(transform.position, actualDestination);

        if (remainingDistance < 0.1f)
            StartCoroutine(WaitAndReturn());
        else
        {
            Vector2 dirVector = (actualDestination - (Vector2)transform.position).normalized;
            platformRb.linearVelocity = dirVector * speed;
        }
    }

    private void MovePlayer()
    {
        if (playerRb != null && platformRb.linearVelocity != Vector2.zero)
        {
            Vector3 platformMovement = platformRb.linearVelocity * Time.fixedDeltaTime;

            if (platformRb.linearVelocity.y > 0)
            {
                playerRb.transform.position += new Vector3(platformMovement.x, 0, 0);

            }
            else
                playerRb.transform.position += platformMovement;
        }
    }

    private IEnumerator WaitAndReturn()
    {
        canMove = false;
        platformRb.linearVelocity = Vector2.zero;
        transform.position = isMovingToDestination ? targetPosition : startPosition;

        yield return new WaitForSeconds(waitTime);

        isMovingToDestination = !isMovingToDestination;
        canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (moveOnTouch && !hasStartedMoving)
                hasStartedMoving = true;

            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (platformRb == null && !Application.isPlaying) startPosition = transform.position;

        Vector2 fin = Vector2.zero;
        float distCalc = distance * (int)direction;

        if (axis == MovementAxis.Horizontal)
            fin = new Vector2(startPosition.x + distCalc, startPosition.y);
        else
            fin = new Vector2(startPosition.x, startPosition.y + distCalc);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPosition, fin);
    }
}
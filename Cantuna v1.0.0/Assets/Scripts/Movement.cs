using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EndlessRunnerPlayer : MonoBehaviour
{
    [Header("Velocidades")]
    public float startSpeed = 2f;
    public float maxSpeed = 30f;
    public float minSpeed = 2f;
    public float timeToMaxSpeed = 90f;
    public float collisionSlowdownFactor = 0.8f;
    public float timeBonusThreshold = 15f;

    [Tooltip("Controla la rapidez para moverse a otro carril.")]
    public float lateralSpeed = 10f;

    [Header("Salto y Gravedad")]
    public float jumpForce = 10f;
    public float gravity = 20f;

    [Header("Carriles")]
    private float[] lanePositions = { 101.46f, 104.8f, 108.13f };
    private int currentLane = 1;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float currentSpeed;

    private float elapsedTime = 0f;
    private float timeSinceLastCollision = 0f;
    private bool recoveringSpeed = false;

    // Variables para gestionar la recuperación de velocidad tras un choque
    private float previousSpeed = 0f;
    private float recoveryTime = 2f; 
    private float recoveryTimer = 0f;

    private bool isJumping = false;
    private int coinsCollected = 0;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = startSpeed;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        timeSinceLastCollision += Time.deltaTime;

        // Lógica de recuperación de velocidad tras una colisión
        if (recoveringSpeed)
        {
            recoveryTimer += Time.deltaTime;
            currentSpeed = Mathf.Lerp(currentSpeed, previousSpeed, recoveryTimer / recoveryTime);

            if (recoveryTimer >= recoveryTime)
            {
                recoveringSpeed = false;
            }
        }
        else
        {
            float timeMultiplier = (timeSinceLastCollision > timeBonusThreshold) ? 1.2f : 1.0f;
            float speedProgress = elapsedTime / timeToMaxSpeed;
            currentSpeed = Mathf.Lerp(startSpeed, maxSpeed, speedProgress * timeMultiplier);
        }

        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
        moveDirection.x = currentSpeed;

        if (Input.GetKeyDown(KeyCode.D) && currentLane > 0)
        {
            currentLane--;
        }
        else if (Input.GetKeyDown(KeyCode.A) && currentLane < 2)
        {
            currentLane++;
        }

        float offsetZ = lanePositions[currentLane] - transform.position.z;
        float distanceThisFrame = lateralSpeed * Time.deltaTime;
        float moveZ = 0f;

        if (Mathf.Abs(offsetZ) <= distanceThisFrame)
        {
            Vector3 tempPos = transform.position;
            tempPos.z = lanePositions[currentLane];
            transform.position = tempPos;
            moveZ = 0f;
        }
        else
        {
            moveZ = Mathf.Sign(offsetZ) * lateralSpeed;
        }

        moveDirection.z = moveZ;

        if (controller.isGrounded)
        {
            moveDirection.y = -1f;
            if (Input.GetKeyDown(KeyCode.W))
            {
                moveDirection.y = jumpForce;
                isJumping = true;
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (isJumping && Input.GetKeyDown(KeyCode.S))
        {
            moveDirection.y = -jumpForce * 1.5f;
            isJumping = false;
        }

        controller.Move(moveDirection * Time.deltaTime);
    }

    /// <summary>
    /// Reduce la velocidad al chocar con un obstáculo y luego la recupera progresivamente.
    /// </summary>
    public void SlowDown()
    {
        previousSpeed = currentSpeed; // Guardamos la velocidad antes del choque
        currentSpeed *= collisionSlowdownFactor; // Reducimos la velocidad temporalmente
        elapsedTime = Mathf.Clamp(elapsedTime - 10f, 0f, timeToMaxSpeed);
        timeSinceLastCollision = 0f;

        recoveringSpeed = true;
        recoveryTimer = 0f;
    }

    /// <summary>
    /// Incrementa la cantidad de monedas.
    /// </summary>
    public void CollectCoin()
    {
        coinsCollected++;
    }
}

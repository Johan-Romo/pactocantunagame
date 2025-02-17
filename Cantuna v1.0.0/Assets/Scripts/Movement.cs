using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EndlessRunnerPlayer : MonoBehaviour
{
    [Header("Velocidades")]
    public float startSpeed = 2f;
    public float lateralSpeed = 5f;
    public float maxSpeed = 30f;
    public float minSpeed = 2f;
    public float timeToMaxSpeed = 90f;
    public float collisionSlowdownFactor = 0.8f;
    public float timeBonusThreshold = 15f;

    [Header("Salto y Gravedad")]
    public float jumpForce = 10f;
    public float gravity = 20f;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float elapsedTime = 0f;
    private float currentSpeed;
    private float timeSinceLastCollision = 0f;
    private bool recoveringSpeed = false;

    private int coinsCollected = 0; // 🔥 Se usa correctamente ahora

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = startSpeed;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        timeSinceLastCollision += Time.deltaTime;

        if (recoveringSpeed)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime / 5f);
            if (currentSpeed >= maxSpeed * 0.95f) recoveringSpeed = false;
        }
        else
        {
            float timeMultiplier = (timeSinceLastCollision > timeBonusThreshold) ? 1.2f : 1.0f;
            float speedProgress = elapsedTime / timeToMaxSpeed;
            currentSpeed = Mathf.Lerp(startSpeed, maxSpeed, speedProgress * timeMultiplier);
        }

        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);

        float xMove = currentSpeed;
        float zMove = 0f;

        if (Input.GetKey(KeyCode.A)) zMove = -lateralSpeed;
        else if (Input.GetKey(KeyCode.D)) zMove = lateralSpeed;

        if (controller.isGrounded)
        {
            moveDirection.y = -1f;
            if (Input.GetKeyDown(KeyCode.Space)) moveDirection.y = jumpForce;
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        moveDirection.x = xMove;
        moveDirection.z = zMove;
        controller.Move(moveDirection * Time.deltaTime);
    }

    /// <summary>
    /// Reduce la velocidad al chocar con un obstáculo.
    /// </summary>
    public void SlowDown()
    {
        currentSpeed *= collisionSlowdownFactor;
        elapsedTime = Mathf.Clamp(elapsedTime - 10f, 0f, timeToMaxSpeed);
        timeSinceLastCollision = 0f;
        recoveringSpeed = true;
    }

    /// <summary>
    /// Aumenta la cantidad de monedas recolectadas y acelera la velocidad.
    /// </summary>
    public void CollectCoin()
    {
        coinsCollected++; // 🔥 Ahora sí se usa la variable correctamente
    }
}

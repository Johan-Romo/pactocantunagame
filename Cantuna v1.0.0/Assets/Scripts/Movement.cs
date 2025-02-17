using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EndlessRunnerPlayer : MonoBehaviour
{
    [Header("Velocidades")]
    public float forwardSpeed = 10f;
    public float lateralSpeed = 5f;

    [Header("Salto")]
    public float jumpForce = 8f;
    public float gravity = 20f;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private InfiniteObstacle obstacleGenerator;

    // 🔥 Contadores para IA
    private int coinsCollected = 0;
    private int collisions = 0;
    private float survivalTime = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        obstacleGenerator = FindObjectOfType<InfiniteObstacle>(); // Referencia al generador
    }

    void Update()
    {
        survivalTime += Time.deltaTime; // Contar tiempo jugado

        float xMove = forwardSpeed;
        float zMove = 0f;

        if (Input.GetKey(KeyCode.A)) { zMove = -lateralSpeed; }
        else if (Input.GetKey(KeyCode.D)) { zMove = lateralSpeed; }

        if (controller.isGrounded)
        {
            moveDirection.y = -1f;
            if (Input.GetKeyDown(KeyCode.Space)) { moveDirection.y = jumpForce; }
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
    /// Detecta colisiones con obstáculos y monedas.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            collisions++; // Registra colisión
            obstacleGenerator.PlayerCollided(); // Ajusta dificultad
            Debug.Log("Jugador chocó con un obstáculo. Total: " + collisions);
        }
        else if (other.CompareTag("Coin"))
        {
            coinsCollected++; // Cuenta monedas
            obstacleGenerator.CoinCollected();
            Destroy(other.gameObject);
            Debug.Log("Moneda recogida. Total: " + coinsCollected);
        }
    }
}

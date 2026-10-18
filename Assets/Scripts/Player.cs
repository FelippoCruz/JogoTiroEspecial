using UnityEngine;
using UnityEngine.InputSystem; // Input System novo (substitui a classe Input antiga)

// Colocar no objeto "Player". Mouse mira, clique esquerdo atira.
public class Player : MonoBehaviour
{
    [Header("Mira")]
    [Tooltip("No Input System o mouse já vem em pixels por quadro, então o valor é bem menor que antes (comece com 0.1).")]
    public float mouseSensitivity = 0.1f;
    public float minPitch = -60f;
    public float maxPitch = 60f;

    [Header("Movimento (desligado: no jogo o jogador fica parado)")]
    public bool allowMovement = false;
    public float speed = 10f;

    [Header("Referências")]
    public Transform cam;
    public Transform gun;
    public GameObject bulletPrefab;
    public Transform gunTip;

    private float rotationX;

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        Mouse mouse = Mouse.current;
        Keyboard keyboard = Keyboard.current;
        if (mouse == null) return;

        // Se o cursor foi liberado (Esc), o primeiro clique só trava de novo, sem atirar.
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            if (mouse.leftButton.wasPressedThisFrame) LockCursor();
            return;
        }

        // ---- Mira ----
        // delta já é "quanto o mouse andou neste quadro": não multiplica por Time.deltaTime.
        Vector2 delta = mouse.delta.ReadValue() * mouseSensitivity;

        // Gira o corpo para esquerda/direita
        transform.Rotate(Vector3.up * delta.x);

        // Gira câmera e arma para cima/baixo
        rotationX -= delta.y;
        rotationX = Mathf.Clamp(rotationX, minPitch, maxPitch);
        if (cam != null) cam.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        if (gun != null) gun.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // ---- Movimento (opcional) ----
        if (allowMovement && keyboard != null)
        {
            float h = 0f;
            float v = 0f;

            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) h += 1f;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) h -= 1f;
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) v += 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) v -= 1f;

            transform.Translate(h * speed * Time.deltaTime, 0f, v * speed * Time.deltaTime);
        }

        // ---- Tiro ----
        if (mouse.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || gunTip == null)
        {
            Debug.LogWarning("Player: falta Bullet Prefab ou Gun Tip no Inspector.");
            return;
        }

        Instantiate(bulletPrefab, gunTip.position, gunTip.rotation);
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

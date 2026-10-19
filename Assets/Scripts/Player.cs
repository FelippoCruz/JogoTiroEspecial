
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Mira")]
    public float mouseSensitivity = 0.1f;
    public float minPitch = -60f;
    public float maxPitch = 60f;

    [Header("Movimento")]
    public bool allowMovement = false;
    public float speed = 10f;

    [Header("Referências")]
    public Transform cam;
    public Transform gun;
    public GameObject bulletPrefab;
    public Transform gunTip;

    [Header("Disparo")]
    public float aimDistance = 1000f;
    public LayerMask aimMask = ~0;

    [Header("Munição")]
    [SerializeField]
    private PlayerBullet.AmmoType selectedAmmo =
        PlayerBullet.AmmoType.Normal;

    private float rotationX;
    private Camera playerCamera;

    private void Start()
    {
        LockCursor();

        if (cam != null)
        {
            playerCamera = cam.GetComponent<Camera>();

            if (playerCamera == null)
                playerCamera = cam.GetComponentInChildren<Camera>();
        }

        if (playerCamera == null)
            playerCamera = Camera.main;

        if (playerCamera == null)
            Debug.LogWarning("Player: nenhuma câmera encontrada.");
    }

    private void Update()
    {
        Mouse mouse = Mouse.current;
        Keyboard keyboard = Keyboard.current;

        if (keyboard != null)
            HandleAmmoSelection(keyboard);

        if (mouse == null)
            return;

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            if (mouse.leftButton.wasPressedThisFrame)
                LockCursor();

            return;
        }

        // Mira horizontal.
        Vector2 delta = mouse.delta.ReadValue() * mouseSensitivity;
        transform.Rotate(Vector3.up * delta.x);

        // Mira vertical.
        rotationX -= delta.y;
        rotationX = Mathf.Clamp(rotationX, minPitch, maxPitch);

        if (cam != null)
            cam.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        if (gun != null)
            gun.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // Movimento opcional.
        if (allowMovement && keyboard != null)
        {
            float h = 0f;
            float v = 0f;

            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                h += 1f;

            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                h -= 1f;

            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
                v += 1f;

            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
                v -= 1f;

            transform.Translate(
                h * speed * Time.deltaTime,
                0f,
                v * speed * Time.deltaTime
            );
        }

        // Disparo.
        if (mouse.leftButton.wasPressedThisFrame)
            Shoot();
    }

    private void HandleAmmoSelection(Keyboard keyboard)
    {
        if (keyboard.digit1Key.wasPressedThisFrame ||
            keyboard.numpad1Key.wasPressedThisFrame)
        {
            SelectAmmo(PlayerBullet.AmmoType.Normal);
        }
        else if (keyboard.digit2Key.wasPressedThisFrame ||
                 keyboard.numpad2Key.wasPressedThisFrame)
        {
            SelectAmmo(PlayerBullet.AmmoType.Heavy);
        }
        else if (keyboard.digit3Key.wasPressedThisFrame ||
                 keyboard.numpad3Key.wasPressedThisFrame)
        {
            SelectAmmo(PlayerBullet.AmmoType.Fire);
        }
        else if (keyboard.digit4Key.wasPressedThisFrame ||
                 keyboard.numpad4Key.wasPressedThisFrame)
        {
            SelectAmmo(PlayerBullet.AmmoType.Ice);
        }
    }

    private void SelectAmmo(PlayerBullet.AmmoType ammo)
    {
        selectedAmmo = ammo;
        Debug.Log("Munição selecionada: " + ammo);
    }

    private void Shoot()
    {
        if (bulletPrefab == null || gunTip == null)
        {
            Debug.LogWarning(
                "Player: falta Bullet Prefab ou Gun Tip."
            );
            return;
        }

        if (playerCamera == null)
        {
            Debug.LogWarning("Player: câmera não encontrada.");
            return;
        }

        // Raio pelo centro da tela.
        Ray aimRay = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        Vector3 targetPoint;

        if (Physics.Raycast(
            aimRay,
            out RaycastHit hit,
            aimDistance,
            aimMask,
            QueryTriggerInteraction.Collide))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint =
                aimRay.origin + aimRay.direction * aimDistance;
        }

        // A bala sai do cano e segue para o ponto mirado.
        Vector3 direction =
            (targetPoint - gunTip.position).normalized;

        GameObject bulletObject = Instantiate(
            bulletPrefab,
            gunTip.position,
            Quaternion.LookRotation(direction)
        );

        PlayerBullet bullet =
            bulletObject.GetComponent<PlayerBullet>();

        if (bullet != null)
        {
            bullet.Initialize(direction, selectedAmmo);
        }
        else
        {
            Debug.LogError(
                "O prefab da bala precisa ter o componente PlayerBullet."
            );

            Destroy(bulletObject);
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

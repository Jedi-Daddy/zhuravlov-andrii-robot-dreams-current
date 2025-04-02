using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerControllerIS : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 7f;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform barrelEndTransform;
    [SerializeField]
    private Transform bulletParent;
    [SerializeField]
    private GameObject muzzleFlash;

    // Reference to the BlasterSimply pistol GameObject
    [SerializeField]
    private Transform blasterPistolTransform;

    public int curHp = 100;
    public int maxHp = 100;
    private bool isDead = false;
    private bool flashingDamage;

    public MeshRenderer mr;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction shootAction;

    // Recoil system settings
    [Header("Recoil Settings")]
    [SerializeField] private float positionalRecoilForce = 0.1f;      // How far back the gun moves
    [SerializeField] private float rotationalRecoilForceX = 5f;       // Rotation on X axis (up/down)
    [SerializeField] private float rotationalRecoilForceY = 2f;       // Rotation on Y axis (left/right)
    [SerializeField] private float rotationalRecoilForceZ = 3f;       // Rotation on Z axis (roll)
    [SerializeField] private float recoilRecoverySpeed = 10f;         // Position recovery speed
    [SerializeField] private float rotationalRecoverySpeed = 15f;     // Rotation recovery speed

    // Store original position and rotation
    private Vector3 originalPistolPosition;
    private Quaternion originalPistolRotation;

    // Target position and rotation during recoil
    private Vector3 targetPistolPosition;
    private Quaternion targetPistolRotation;

    private bool isRecoiling = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];

        cameraTransform = Camera.main.transform;

        // Try to find the BlasterSimply pistol if not assigned
        if (blasterPistolTransform == null)
        {
            // Try to find parent of barrelEndTransform with "Blaster" in the name
            Transform current = barrelEndTransform;
            while (current != null && current != transform)
            {
                if (current.name.Contains("Blaster") || current.name.Contains("Pistol") || current.name.Contains("Gun"))
                {
                    blasterPistolTransform = current;
                    Debug.Log($"Found weapon transform: {blasterPistolTransform.name}");
                    break;
                }
                current = current.parent;
            }

            // If still not found, use barrelEndTransform's parent as fallback
            if (blasterPistolTransform == null && barrelEndTransform != null)
            {
                blasterPistolTransform = barrelEndTransform.parent;
                Debug.Log($"Using fallback weapon transform: {blasterPistolTransform?.name ?? "null"}");
            }
        }

        // Store original position and rotation
        if (blasterPistolTransform != null)
        {
            originalPistolPosition = blasterPistolTransform.localPosition;
            originalPistolRotation = blasterPistolTransform.localRotation;
            targetPistolPosition = originalPistolPosition;
            targetPistolRotation = originalPistolRotation;

            Debug.Log($"Initialized weapon at position: {originalPistolPosition}, rotation: {originalPistolRotation.eulerAngles}");
        }
        else
        {
            Debug.LogError("BlasterSimply transform not found! Assign it in the inspector or make sure your weapon hierarchy has 'Blaster', 'Pistol', or 'Gun' in its name.");
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        shootAction.performed += _ => ShootGun();
    }

    private void OnDisable()
    {
        if (shootAction != null)
        {
            shootAction.performed -= _ => ShootGun();
        }
        else
        {
            Debug.LogError("Shoot action is null in OnDisable.");
        }
    }

    private void ShootGun()
    {
        if (muzzleFlash != null)
        {
            GameObject flash = Instantiate(muzzleFlash, barrelEndTransform.position, barrelEndTransform.rotation);
            flash.transform.SetParent(barrelEndTransform);
            flash.transform.rotation = Quaternion.LookRotation(barrelEndTransform.forward);
            Destroy(flash, 0.25f);
        }

        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelEndTransform.position, Quaternion.identity, bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            bulletController.target = hit.point;
            bulletController.hit = true;
        }

        // Apply recoil to the whole pistol
        ApplyRecoil();
    }

    private void ApplyRecoil()
    {
        if (blasterPistolTransform == null) return;

        isRecoiling = true;

        // Calculate recoil position - move back along local Z axis
        Vector3 recoilPositionOffset = blasterPistolTransform.localRotation * new Vector3(0, 0, -positionalRecoilForce);
        targetPistolPosition = originalPistolPosition + recoilPositionOffset;

        // Calculate rotation with randomized components for natural feel
        float recoilX = -rotationalRecoilForceX; // Upward recoil
        float recoilY = Random.Range(-rotationalRecoilForceY, rotationalRecoilForceY); // Random horizontal
        float recoilZ = Random.Range(-rotationalRecoilForceZ, rotationalRecoilForceZ); // Random roll

        Quaternion recoilRotation = Quaternion.Euler(recoilX, recoilY, recoilZ);
        targetPistolRotation = originalPistolRotation * recoilRotation;

        // Apply immediate partial recoil for responsive feel (30% of full recoil)
        blasterPistolTransform.localPosition = Vector3.Lerp(blasterPistolTransform.localPosition, targetPistolPosition, 0.3f);
        blasterPistolTransform.localRotation = Quaternion.Lerp(blasterPistolTransform.localRotation, targetPistolRotation, 0.3f);

        // Debug output to verify recoil is applied
        Debug.Log($"Applied recoil - Position: {blasterPistolTransform.localPosition}, Rotation: {blasterPistolTransform.localRotation.eulerAngles}");
    }

    void Update()
    {
        if (isDead) return; // If player is dead, disable controls

        // Handle weapon recoil recovery
        HandleWeaponRecoil();

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Makes the player jump
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void HandleWeaponRecoil()
    {
        if (blasterPistolTransform == null) return;

        // In recoil state: weapon is at the recoil point, moving back to rest
        if (isRecoiling)
        {
            // Smoothly interpolate between current and target recoil positions
            blasterPistolTransform.localPosition = Vector3.Lerp(
                blasterPistolTransform.localPosition,
                targetPistolPosition,
                Time.deltaTime * recoilRecoverySpeed);

            blasterPistolTransform.localRotation = Quaternion.Lerp(
                blasterPistolTransform.localRotation,
                targetPistolRotation,
                Time.deltaTime * rotationalRecoverySpeed);

            // Once we're close to the target recoil position, transition to recovery phase
            if (Vector3.Distance(blasterPistolTransform.localPosition, targetPistolPosition) < 0.001f)
            {
                isRecoiling = false;
                targetPistolPosition = originalPistolPosition;
                targetPistolRotation = originalPistolRotation;
            }
        }
        else
        {
            // Recovery phase: smoothly return to original position/rotation
            blasterPistolTransform.localPosition = Vector3.Lerp(
                blasterPistolTransform.localPosition,
                originalPistolPosition,
                Time.deltaTime * recoilRecoverySpeed);

            blasterPistolTransform.localRotation = Quaternion.Lerp(
                blasterPistolTransform.localRotation,
                originalPistolRotation,
                Time.deltaTime * rotationalRecoverySpeed);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        curHp -= damage;
        Debug.Log($"Игрок получил {damage} урона. Текущее ХП: {curHp}");

        if (curHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Игрок погиб!");
        // You could add death animation, respawn or game over screen
    }
}
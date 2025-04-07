using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerControllerIS : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 7f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform barrelEndTransform;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private GameObject muzzleFlash;

    [SerializeField] private Transform blasterPistolTransform;

    public int curHp = 100;
    public int maxHp = 100;
    private bool isDead = false;

    public GameObject gameOverPanel;
    public Button mainMenuButton;
    public Image hpBarFill; // Полоска HP UI

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction shootAction;

    [Header("Recoil Settings")]
    [SerializeField] private float positionalRecoilForce = 0.1f;
    [SerializeField] private float rotationalRecoilForceX = 5f; // теперь будет использоваться только X для отдачи вверх
    [SerializeField] private float recoilRecoverySpeed = 10f;
    [SerializeField] private float rotationalRecoverySpeed = 15f;

    private Vector3 originalPistolPosition;
    private Quaternion originalPistolRotation;
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

        if (blasterPistolTransform == null)
        {
            Transform current = barrelEndTransform;
            while (current != null && current != transform)
            {
                if (current.name.Contains("Blaster") || current.name.Contains("Pistol") || current.name.Contains("Gun"))
                {
                    blasterPistolTransform = current;
                    break;
                }
                current = current.parent;
            }
            if (blasterPistolTransform == null && barrelEndTransform != null)
            {
                blasterPistolTransform = barrelEndTransform.parent;
            }
        }

        if (blasterPistolTransform != null)
        {
            originalPistolPosition = blasterPistolTransform.localPosition;
            originalPistolRotation = blasterPistolTransform.localRotation;
            targetPistolPosition = originalPistolPosition;
            targetPistolRotation = originalPistolRotation;
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
        GameObject bullet = Instantiate(bulletPrefab, barrelEndTransform.position, Quaternion.identity, bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            bulletController.target = hit.point;
            bulletController.hit = true;
        }

        ApplyRecoil();
    }

    private void ApplyRecoil()
    {
        if (blasterPistolTransform == null) return;

        isRecoiling = true;
        Vector3 recoilPositionOffset = new Vector3(0, positionalRecoilForce, 0); // отдача вверх
        targetPistolPosition = originalPistolPosition + recoilPositionOffset;

        float recoilX = -rotationalRecoilForceX; // отдача вверх
        Quaternion recoilRotation = Quaternion.Euler(recoilX, 0f, 0f);
        targetPistolRotation = originalPistolRotation * recoilRotation;

        blasterPistolTransform.localPosition = Vector3.Lerp(blasterPistolTransform.localPosition, targetPistolPosition, 0.3f);
        blasterPistolTransform.localRotation = Quaternion.Lerp(blasterPistolTransform.localRotation, targetPistolRotation, 0.3f);
    }

    void Update()
    {
        if (isDead) return;

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

        if (isRecoiling)
        {
            blasterPistolTransform.localPosition = Vector3.Lerp(
                blasterPistolTransform.localPosition,
                targetPistolPosition,
                Time.deltaTime * recoilRecoverySpeed);

            blasterPistolTransform.localRotation = Quaternion.Lerp(
                blasterPistolTransform.localRotation,
                targetPistolRotation,
                Time.deltaTime * rotationalRecoverySpeed);

            if (Vector3.Distance(blasterPistolTransform.localPosition, targetPistolPosition) < 0.001f)
            {
                isRecoiling = false;
                targetPistolPosition = originalPistolPosition;
                targetPistolRotation = originalPistolRotation;
            }
        }
        else
        {
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

        if (hpBarFill != null)
        {
            hpBarFill.fillAmount = (float)curHp / maxHp;
        }

        if (curHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Игрок погиб!");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(() => {
                Time.timeScale = 1f;
                SceneManager.LoadScene("MenuLection16");
            });

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
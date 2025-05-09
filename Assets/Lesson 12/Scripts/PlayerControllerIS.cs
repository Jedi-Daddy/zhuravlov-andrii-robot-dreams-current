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

    [SerializeField] private GameObject healEffectPrefab;

    public int curHp = 100;
    public int maxHp = 100;
    private bool isDead = false;
    public int coins = 5;
    public Text coinsText;
    public Text hpText;
    public int Coins { get; set; }

    public GameObject gameOverPanel;
    public Button mainMenuButton;
    public Image hpBarFill; // ������� HP UI

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;
    private Animator animator;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction shootAction;

    [Header("Recoil Settings")]
    [SerializeField] private float positionalRecoilForce = 0.2f;
    [SerializeField] private float rotationalRecoilForceX = 10f; // ������ ����� �������������� ������ X ��� ������ �����
    [SerializeField] private float recoilRecoverySpeed = 5f;
    [SerializeField] private float rotationalRecoverySpeed = 15f;

    private Vector3 originalPistolPosition;
    private Quaternion originalPistolRotation;
    private Vector3 targetPistolPosition;
    private Quaternion targetPistolRotation;

    private bool isRecoiling = false;

    [Header("Detection")]
    public float detectionRange = 15f; // ����������, �� ������� ���� �������� �����������
    public float losePlayerDistance = 20f; // ����������, ����� �������� ���� �������� ������������ ������

    public static PlayerControllerIS Instance { get; private set; }

    private enum PlayerAnimState
    {
        Idle,
        Run
    }

    private PlayerAnimState currentAnimState = PlayerAnimState.Idle;

    private void ChangeAnimState(PlayerAnimState newState)
    {
        if (currentAnimState == newState) return;
        currentAnimState = newState;

        switch (newState)
        {
            case PlayerAnimState.Idle:
                animator.CrossFade("IdlePistol", 0.1f);
                break;
            case PlayerAnimState.Run:
                animator.CrossFade("Run", 0.1f);
                break;
        }
    }

    private void Awake()
    {
        Instance = this;
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        animator = GetComponent<Animator>();

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
        Vector3 recoilPositionOffset = new Vector3(0, positionalRecoilForce, 0); // ������ �����
        targetPistolPosition = originalPistolPosition + recoilPositionOffset;

        float recoilX = -rotationalRecoilForceX; // ������ �����
        Quaternion recoilRotation = Quaternion.Euler(recoilX, 0f, 0f);
        targetPistolRotation = originalPistolRotation * recoilRotation;

        blasterPistolTransform.localPosition = Vector3.Lerp(blasterPistolTransform.localPosition, targetPistolPosition, 0.3f);
        blasterPistolTransform.localRotation = Quaternion.Lerp(blasterPistolTransform.localRotation, targetPistolRotation, 0.3f);
    }

    void Update()
    {
        if (isDead) return;

        // ������� / ������� ��������� �� Tab
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            InventorySystem.Instance.ToggleInventory();
        }

        HandleWeaponRecoil();

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        // FSM-��������
        if (move.magnitude > 0.1f)
        {
            ChangeAnimState(PlayerAnimState.Run);
        }
        else
        {
            ChangeAnimState(PlayerAnimState.Idle);
        }
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
        Debug.Log($"����� ������� {damage} �����. ������� ��: {curHp}");

        if (hpBarFill != null)
        {
            hpBarFill.fillAmount = (float)curHp / maxHp;
        }

        if (curHp <= 0)
        {
            Die();
        }
    }

    private void Start()
    {
        UpdateCoinsUI();
        UpdateHPUI();
    }

    public void Heal(int amount)
    {
        curHp += amount;
        if (curHp > maxHp)
            curHp = maxHp;

        UpdateHPUI();

        // ��������� ������ �������
        if (healEffectPrefab != null)
        {
            // ��������� ������ ���� ���� ������ ��������� (��������, �� ������ �����)
            Vector3 spawnPosition = transform.position + new Vector3(0, -0.5f, 0);
            GameObject fx = Instantiate(healEffectPrefab, spawnPosition, Quaternion.identity);

            fx.transform.SetParent(transform); // ������� �� �������
            Destroy(fx, 2f); // ������� ����� 2 �������
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("����� �����!");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(() => {
                Time.timeScale = 1f;
                SceneManager.LoadScene("LastMenu");
            });

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        UpdateCoinsUI();
    }

    public void SpendCoins(int amount)
    {
        Coins -= amount;
        if (Coins < 0)
            Coins = 0;

        UpdateCoinsUI();
    }

    public void UpdateCoinsUI()
    {
        coinsText.text = "Coins: " + Coins;
    }

    public void UpdateHPUI()
    {
        if (hpBarFill != null)
            hpBarFill.fillAmount = (float)curHp / maxHp;
    }
}

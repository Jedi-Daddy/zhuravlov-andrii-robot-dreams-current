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

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];

        cameraTransform = Camera.main.transform;

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
    }

    void Update()
    {
        if (isDead) return; // Если игрок мертв, управление отключается

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
        // Можно добавить анимацию смерти, респавн или экран поражения
    }
}

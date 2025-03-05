using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int maxHp = 100;
    private int currentHp;
    private Renderer enemyRenderer;
    private Color originalColor;
    private bool isFlashing;

    [Header("UI Elements")]
    public Image hpFill; 

    private void Start()
    {
        currentHp = maxHp;
        enemyRenderer = GetComponent<Renderer>();
        originalColor = enemyRenderer.material.color;
        UpdateHPBar();
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log("Получен урон: " + damage + ", текущее HP: " + currentHp);
        if (currentHp < 0) currentHp = 0;

        UpdateHPBar(); 

        if (!isFlashing)
        {
            StartCoroutine(FlashRed());
        }

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void UpdateHPBar()
    {
        if (hpFill != null)
        {
            float healthPercent = (float)currentHp / maxHp;
            hpFill.rectTransform.localScale = new Vector3(healthPercent, 1, 1); 
        }
    }

    private IEnumerator FlashRed()
    {
        isFlashing = true;
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        enemyRenderer.material.color = originalColor;
        isFlashing = false;
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject); 
    }
}
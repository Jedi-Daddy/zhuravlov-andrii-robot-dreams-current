using UnityEngine;
using UnityEngine.UI;

public class Dragon : MonoBehaviour
{
    private int HP = 100;
    public Image HealthBarFill; // Image displaying HP
    public Animator animator;

    void UpdateHealthBar()
    {
        HealthBarFill.fillAmount = Mathf.Clamp01(HP / 100f);
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        HP = Mathf.Max(HP, 0); // So that it doesn't go into the minus
        UpdateHealthBar();

        if (HP <= 0)
        {
            animator.SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }
}
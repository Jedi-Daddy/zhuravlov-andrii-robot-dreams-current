using UnityEngine;
using UnityEngine.UI;

public class Dragon : MonoBehaviour
{
    private int HP = 100;
    public Image HealthBarFill; // Image displaying HP
    public Animator animator;

    void UpdateHealthBar()
    {
        // Convert HP to a normalized value between 0 and 1 for display on the HealthBar
        HealthBarFill.fillAmount = Mathf.Clamp01(HP / 100f);  // 100 is the dragon's maximum health
    }

    public void TakeDamage(int damageAmount)
    {
        // Reduce HP by transferred damage
        HP -= damageAmount;
        HP = Mathf.Max(HP, 0); // Limit health to a minimum value of 0
        UpdateHealthBar();

        if (HP <= 0)
        {
            animator.SetTrigger("Die");  // Death animation
            GetComponent<Collider>().enabled = false;  // Disable the collider
        }
        else
        {
            animator.SetTrigger("Damage");  // Animation of receiving damage
        }
    }
}

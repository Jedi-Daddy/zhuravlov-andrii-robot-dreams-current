using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dragon : MonoBehaviour
{
    private int HP = 100;
    public Slider HealthBar;
    public Animator animator;

    void Update()
    {
        HealthBar.value = HP;
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            animator.SetTrigger("Die");
            GetComponent<Collider> ().enabled = false;
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }
}

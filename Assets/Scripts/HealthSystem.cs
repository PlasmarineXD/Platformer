using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;

    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }
    public float Health { get => currentHealth; private set => currentHealth = value; }
    public bool bCanBeDamage;

    private Animator Anim;
    private bool dead = false;

    public UnityEvent OnDamaged;
    public UnityEvent OnDead;

    private void Awake()
    {
        Anim = GetComponent<Animator>();
    }

    public void TakeDamage(float Damage)
    {
        if(bCanBeDamage == true && dead == false)
        {
            currentHealth = Mathf.Clamp(currentHealth - Damage, 0, MaxHealth);
            if(currentHealth > 0)
            {
                Anim.SetTrigger("Hurt");
                OnDamaged?.Invoke();
            }
            else
            {
                Anim.SetTrigger("Dead");
                dead = true;
                OnDead?.Invoke();
            }
        }
    }

    public void TakeHeal(float Heal)
    {
        currentHealth = Mathf.Clamp(currentHealth + Heal, 0, maxHealth);
    }

    public void AddMaxHealth(float Heal)
    {
        maxHealth += Heal;
        TakeHeal(Heal);
    }
}

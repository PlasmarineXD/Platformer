using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Enemy : MonoBehaviour
{
    protected Animator Anim;
    protected HealthSystem Health;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Anim = GetComponent<Animator>();
        Health = GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void LateUpdate()
    {

    }

    public virtual void TakeDamage()
    {

    }

    public virtual void OnDead()
    {

    }
}

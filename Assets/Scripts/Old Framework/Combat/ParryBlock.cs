using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryBlock : MonoBehaviour
{
    public LifeClass cmp_life;
    [Header("Block Options")]
    [HideInInspector]
    public bool block = false;
    public int damageBlock;
    public bool blockAllDamage;

    [Header("Parry Options")]
    public float parryTime;
    public AttackPreset counterAttack;
    [HideInInspector]
    public bool parry = false;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryDamage(int damage,DamageDealer incommingHit)
    {
        if (parry)
        {
            incommingHit.CancelAttack();
        }
        else if (block)
        {
            if (blockAllDamage)
            {
                incommingHit.CancelAttack();
            }
            else
            {
                if(cmp_life) cmp_life.LooseHealth(Mathf.Max(0, damage - damageBlock));
            }
        }
        else
        {
            if (cmp_life) cmp_life.LooseHealth(damage);
        }
    }

    public void InitParry()
    {
        timer = parryTime;
        parry = true;
        block = true;
    }

    public void EndParry()
    {
        timer = 0;
        parry = false;
        block = false;
    }

    public void ParryTime()
    {
        if(timer > 0)
        {
            parry = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class DamageDealer : MonoBehaviour
{
    public event Action HitCheck = delegate { };
    public event Action HitCanceled = delegate { };
    public string[] damageTags;
    public BoxCollider cmp_collider;
    public int damage;
    public float attackTime;
    private void OnTriggerEnter(Collider other)
    {
        
        for(int i = 0; i < damageTags.Length;i++)
        {
            if (other.CompareTag(damageTags[i]))
            {
                Damage(other.gameObject);
            }
        }

    }
    void Damage(GameObject obj)
    {
        if(obj.GetComponent<Parry>()!=null)
        {
            Parry tmpParry = obj.GetComponent<Parry>();
            tmpParry.TryDamage(damage, this);
            if (tmpParry.parry) return;
        }       
        else if(obj.GetComponent<LifeClass>()!=null)
        {
            obj.GetComponent<LifeClass>().LooseHealth(damage);
        }
        HitCheck.Invoke();
    }

    public void CancelAttack()
    {
        if (gameObject.activeSelf == false) return;
        gameObject.SetActive(false);
        HitCanceled.Invoke();
    }
}

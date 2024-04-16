using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public string[] damageTags;
    public BoxCollider cmp_collider;
    public int damage;
    public float attackTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        if(obj.GetComponent<LifeClass>()!=null)
        {
            obj.GetComponent<LifeClass>().LooseHealth(damage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public DamageDealer cmp_hitBox;
    public Animator cmp_anim;
    public float extraDmg;
    public AttackPreset baseNrmAttack;
    private AttackPreset actualNrmAttack;
    public AttackPreset baseSpcAttack;
    private AttackPreset actualSpcAttack;

    private void OnEnable()
    {
        if(actualNrmAttack) actualNrmAttack = baseNrmAttack;
        if(actualSpcAttack) actualSpcAttack = baseSpcAttack;
    }

    public void NormalAttack()
    {
        if (actualNrmAttack==null) return;
        RepositionHitbox(actualNrmAttack.possitionOffset);
    }
    public void SpecialAttack()
    {
        if (actualSpcAttack == null) return;
    }
    public void AttackHitBox()
    {

    }

    private void ResizeHitbox(Vector3 size)
    {
        cmp_hitBox.cmp_collider.size = size;
    }

    private void RepositionHitbox(Vector3 pos)
    {
        cmp_hitBox.gameObject.transform.position = new Vector3(
            transform.position.x + (pos.x * Vector3.forward.x), 
            transform.position.y + (pos.y * Vector3.forward.y), 
            transform.position.z + (pos.z * Vector3.forward.z));
    }

}

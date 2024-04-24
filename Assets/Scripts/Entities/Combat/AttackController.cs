using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class AttackController : MonoBehaviour
{
    public DamageDealer cmp_hitBox;
    public Animator cmp_anim;

    public float extraDmg;
    public AttackPreset baseNrmAttack;
    private AttackPreset _actualNrmAttack;
    public AttackPreset baseSpcAttack;
    private AttackPreset _actualSpcAttack;


    public float attackResetTime;
    private bool _onAttackTime;
    private float _waitTime;
    private float _timer;

    private ActionControls inputSys;


    private void Start()
    {
        RessetAttacks();
    }
    private void OnEnable()
    {        
        RessetAttacks();
    }


    private void Update()
    {
        //Funciones que puedes retirar de este Update y llamarlas luego desde el PlayerController si lo ves necesario
        RessetCooldown();  //Cooldown para resetear los ataques al default
        AttackTimeQueue(); //Cooldown de espera antes de volver a poder castear otro ataque

    }
    public void NormalAttack()
    {
        AttackSett(_actualNrmAttack);
    }
    public void SpecialAttack()
    {
        AttackSett(_actualSpcAttack);
    }

    private void AttackSett(AttackPreset attack)
    {
        if (attack == null || _onAttackTime == true) return;
        cmp_hitBox.attackTime = attack.duration;
        cmp_hitBox.damage = attack.damage;
        RepositionHitbox(attack.possitionOffset);
        ResizeHitbox(attack.hitSize);
        cmp_anim.runtimeAnimatorController = attack.animation;
        _waitTime = attack.duration;


        _actualNrmAttack = attack.nextNrmCombo;
        _actualSpcAttack = attack.nextSpcCombo;
    }
    public void AttackHitBox()
    {

    }

    private void RessetAttacks()
    {
        if (baseNrmAttack) _actualNrmAttack = baseNrmAttack;
        if (baseSpcAttack) _actualSpcAttack = baseSpcAttack;
    }

    private void ResizeHitbox(Vector3 size)
    {
        cmp_hitBox.cmp_collider.size = size;
    }

    private void RepositionHitbox(Vector3 pos)
    {
        cmp_hitBox.gameObject.transform.localPosition = pos;
    }

    private void AttachPossition()
    {

    }
    #region Cooldowns
    //--------------Cooldowns---------------//

    private void RessetCooldown()
    {

    }
    public void AttackTimeQueue() 
    {
        if (_waitTime > 0)
        {
            _onAttackTime = true;
            _waitTime -= Time.deltaTime;
        }
        else
        {
            _onAttackTime = false;
        }
    }
    #endregion
    #region VisualGraphs
    //----------------------Draw&Visual-------------------//
#if UNITY_EDITOR
    public bool hideBoxesGraph;
    private AttackPreset drawNormalAtk;
    private AttackPreset drawSpcialAtk;

    private void OnDrawGizmos()
    {
        if (hideBoxesGraph == false)
        {
            if (_actualNrmAttack) DrawNewAttack(drawNormalAtk, _actualNrmAttack, Color.magenta);
            else DrawNewAttack(drawNormalAtk, baseNrmAttack, Color.magenta);

            if (_actualSpcAttack) DrawNewAttack(drawNormalAtk, _actualSpcAttack, Color.black);
            else DrawNewAttack(drawSpcialAtk, baseSpcAttack, Color.black);
        }
    }

    private void DrawNewAttack(AttackPreset actualAtk, AttackPreset refreshAtk, Color col)
    {
        if (actualAtk == refreshAtk || refreshAtk == null) return;

        Gizmos.matrix = gameObject.transform.localToWorldMatrix;

        Gizmos.color = col;
        Gizmos.DrawWireCube(refreshAtk.possitionOffset, refreshAtk.hitSize);
    }
#endif
    #endregion
}

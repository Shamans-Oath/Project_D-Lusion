using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private int _dummylife;
    public LifeClass lifeclass;
    public Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        _dummylife = lifeclass.currentHealth;
        anim = GetComponentInChildren<Animator>();
        
        //_basecolor = _mat.color;
    }

    // Update is called once per frame
    void Update()
    {
        AlertDamage();
    }

    public void AlertDamage()
    {

        if (_dummylife != lifeclass.currentHealth)
        {
            Debug.Log("corre animación");
            _dummylife = lifeclass.currentHealth;
            Debug.Log(lifeclass.currentHealth);
            Hurt();

        }
    }

    private void Hurt()
    {
        Debug.Log("triggered");
        anim.SetTrigger("isHurt");
    }

    private void GoBaseform() 
    {
        
    }

}

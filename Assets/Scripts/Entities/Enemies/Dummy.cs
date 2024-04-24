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
            anim = GetComponent<Animator>();
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
        }
    }
    
    private void Hurt() 
    {

    }

}

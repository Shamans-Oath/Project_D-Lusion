using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(PlayerController))]
public class PlayerView : MonoBehaviour
{
    private PlayerController _cmp_controller;
    public Animator cmp_anim;
    // Start is called before the first frame update
    void Start()
    {
        _cmp_controller = gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimatorUpdater()
    {
        cmp_anim.SetFloat("Blend",_cmp_controller.currentFurymeter);
    }


}

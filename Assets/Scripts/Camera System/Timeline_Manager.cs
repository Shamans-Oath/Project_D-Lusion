using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class Timeline_Manager : MonoBehaviour
{
    [SerializeField] private PlayableDirector[] _timeline;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
       _timeline[0].gameObject.SetActive(true);}
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator cmp_Anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(WaitTransition(levelName));
    }

    IEnumerator WaitTransition(string levelName)
    {
        cmp_Anim.SetTrigger("Start");

        yield return new WaitForSeconds(cmp_Anim.GetCurrentAnimatorClipInfo(0).Length);

        SceneManager.LoadScene(levelName);
    }
}

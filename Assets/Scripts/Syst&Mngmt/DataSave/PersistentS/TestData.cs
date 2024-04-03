using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "New ElemntData", menuName = "Data/TestData")]
public class TestData : PersistentScriptableObject
{
    [SerializeField]
    public float f;
    [SerializeField]
    public bool b;
    [SerializeField]
    public string s;

    public void TestCodeModify()
    {
        f = 3.7f;
        b = true;
        s = "This is a test?";
    }

    public override void Reset()
    {
        f = 0;
        b = false;
        s = "null";
        base.Reset();
    }
}

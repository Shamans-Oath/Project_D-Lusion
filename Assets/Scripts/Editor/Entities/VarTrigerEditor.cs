using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using UnityEditor;
[CustomEditor(typeof(VarTriger))]

public class VarTrigerEditor : Editor
{
    public string[] options = new string[] { "Cube", "Sphere", "Plane" };
    public int index = 0;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        VarTriger mainScrpt = (VarTriger)target;
        if (mainScrpt.selectedScript == null) return;
        
        MonoBehaviour trigerScript = mainScrpt.selectedScript;

        index = EditorGUILayout.Popup(index, options);
        
    }


    
    public string[] GetOptions(object obj)
    {
        FieldInfo[] info = GetParams(obj);
        string[] values = new string[info.Length];

        for(int i = 0;i<info.Length;i++)
        {
            //values
        }
        return null;
    }

    private void Awake()
    {
        //UseParams(dataArray);
    }

    public FieldInfo[] GetParams(object obj)
    {
        Type theType = obj.GetType();
        FieldInfo[] fields = theType.GetFields();
        return fields;
    }
    public void UseParams(MonoBehaviour scriptListed)
    {

       
            FieldInfo[] fieldArray = GetParams(scriptListed);
            for (int e = 0; e < fieldArray.Length; e++)
            {
                if (fieldArray[e].FieldType == typeof(float))
                {

                    if (fieldArray[e].GetCustomAttribute<RangeAttribute>() != null)
                    {
                        /*RangeAttribute range;
                        GameObject tmp = Instantiate(slidePref, contentParent);
                        Slider sld = tmp.GetComponent<Slider>();
                        sld.value = (float)fieldArray[e].GetValue(scriptListed);

                        tmp.GetComponent<Text>().text = fieldArray[e].Name;

                        range = fieldArray[e].GetCustomAttribute<RangeAttribute>();
                        sld.minValue = range.min;
                        sld.maxValue = range.max;

                        int tmpE = e;
                       
                        sld.onValueChanged.AddListener(delegate
                        {
                            scriptListed.SetVariable(fieldArray[tmpE].Name, sld.value);
                        });*/
                    }
                    else
                    {
                        /*GameObject tmp = Instantiate(inputNumPref, contentParent);
                        InputField inp = tmp.GetComponentInChildren<InputField>();
                        tmp.GetComponent<Text>().text = fieldArray[e].Name;
                        inp.text = fieldArray[e].GetValue(scriptListed).ToString();


                        int tmpE = e;
                    
                        inp.onValueChanged.AddListener(delegate
                        {
                            scriptListed.SetVariable(fieldArray[tmpE].Name, float.Parse(inp.text));
                        });*/
                    }

                }
                else if (fieldArray[e].FieldType == typeof(int))
                {
                    if (fieldArray[e].GetCustomAttribute<RangeAttribute>() != null)
                    {
                        /*RangeAttribute range;
                        GameObject tmp = Instantiate(slidePref, contentParent);
                        Slider sld = tmp.GetComponent<Slider>();
                        sld.value = (int)fieldArray[e].GetValue(scriptListed);

                        tmp.GetComponent<Text>().text = fieldArray[e].Name;

                        range = fieldArray[e].GetCustomAttribute<RangeAttribute>();
                        sld.minValue = range.min;
                        sld.maxValue = range.max;

                        int tmpE = e;
                     
                        sld.onValueChanged.AddListener(delegate
                        {
                            int va = (int)sld.value;
                            scriptListed.SetVariable(fieldArray[tmpE].Name, va);
                        });*/
                    }
                    else
                    {
                        /*GameObject tmp = Instantiate(inputNumPref, contentParent);
                        InputField inp = tmp.GetComponentInChildren<InputField>();
                        tmp.GetComponent<Text>().text = fieldArray[e].Name;
                        inp.text = fieldArray[e].GetValue(scriptListed).ToString();


                        int tmpE = e;
                 
                        inp.onValueChanged.AddListener(delegate
                        {
                            int va = int.Parse(inp.text);
                            scriptListed.SetVariable(fieldArray[tmpE].Name, va);
                        });*/
                    }
                }

            }
        

    }

}

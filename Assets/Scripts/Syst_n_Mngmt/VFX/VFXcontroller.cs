using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXcontroller : MonoBehaviour
{
    public static VFXcontroller instance;
    public VFXdata[] vfxdata;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public ParticleSystem InstanceVFX(string nameVFX,Vector3 position,Quaternion rotation)
    {
        Debug.Log("Instancing: " + nameVFX);

        for(int  i = 0; i < vfxdata.Length;i++)
        {
            if(vfxdata[i].name == nameVFX)
            {
                return Instantiate(vfxdata[i].vfx,position,rotation);
            }
        }

        return null;
    }
}

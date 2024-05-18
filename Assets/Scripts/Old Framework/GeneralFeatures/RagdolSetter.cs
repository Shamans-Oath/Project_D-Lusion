using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdolSetter : MonoBehaviour
{
    public Animator cmp_anim;
    public GameObject ragdolModel;
    public Collider[] cmp_normalCols;
    public Rigidbody[] cmp_normalRB;
    private Collider[] cmp_ragdollCols;
    private Rigidbody[] cmp_ragdollRB;

    private void Awake()
    {
        GetRagdoll();
        
    }

    private void GetRagdoll()
    {
        if (ragdolModel)
        {
            cmp_ragdollCols = ragdolModel.GetComponentsInChildren<Collider>();
            cmp_ragdollRB = ragdolModel.GetComponentsInChildren<Rigidbody>();
        }
        RagdollSetActive(false);
    }

    public void RagdollSetActive(bool active)
    {
        if (ragdolModel == null) return;

        if (cmp_anim) cmp_anim.enabled = !active;


        foreach (Collider col in cmp_ragdollCols)
        {
            col.enabled = active;
        }
        foreach (Rigidbody rig in cmp_ragdollRB)
        {
            rig.isKinematic = !active;
        }

        foreach (Collider col in cmp_normalCols)
        {
            col.enabled = !active;
        }
        foreach (Rigidbody rig in cmp_normalRB)
        {
            rig.isKinematic = active;
        }
    }
}

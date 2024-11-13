using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.VFX;

public class StylizedBomb : MonoBehaviour
{
    // Start is called before the first frame update

    //[SerializeField] private CameraController cameraController;
    [SerializeField] private VisualEffect sparkParticles;

    private void Awake()
    {
        sparkParticles.Stop();
    }

    // Update is called once per frame
     public void StartExplosion()
    {
        sparkParticles.Play();

    }
}

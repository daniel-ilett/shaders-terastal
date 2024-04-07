using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerastalGoose : MonoBehaviour
{
    [SerializeField] private Renderer[] terastalRenderers;
    [SerializeField] private Material terastalMaterial;
    [SerializeField] private GameObject terastalHat;
    [SerializeField] private Animator animator;

    public void Terastallize()
    {
        for(int i = 0; i < terastalRenderers.Length; ++i)
        {
            terastalRenderers[i].material = terastalMaterial;
        }
        
        terastalHat.SetActive(true);
    }

    public void Honk()
    {
        animator.SetTrigger("Honk");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class doorTrigger : MonoBehaviour
{
    
    public Animator animator;
    
    void Start()
    {

    }

    // Update is called once per frame
    
    void Update()
    {

    }

    
    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool("isOpen", true);
    }

   
    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("isOpen", false);
    }
}
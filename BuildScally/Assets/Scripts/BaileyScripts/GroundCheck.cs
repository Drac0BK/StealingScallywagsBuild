using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    //checks if the player is on the ground
    [SerializeField] private LayerMask platformLayerMask;
    CharacterController characterController;
    private bool isGrounded;
    
    private void OnTriggerStay(Collider collision)
    { 
        isGrounded = collision != null && (((1 << collision.gameObject.layer) & platformLayerMask) != 0);
    }

    private void OnTriggerExit(Collider collision)
    {
        isGrounded = false;
    }

    public bool GetGrounded() { return isGrounded; }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    // NOT FOR FINAL RELEASE ***************************************************
    [SerializeField]
    int weaponIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MyPlayer>() != null)
        {
            other.GetComponent<MyPlayer>().SetWeapon(weaponIndex);
            Destroy(gameObject);
        }
    }
}

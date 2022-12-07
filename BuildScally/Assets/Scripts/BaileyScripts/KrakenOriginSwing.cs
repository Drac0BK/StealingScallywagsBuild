using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenOriginSwing : MonoBehaviour
{
    bool isActive = false;
    [SerializeField]
    GameObject krakenSwingFrom;
    [SerializeField]
    GameObject dustEffect;
    [SerializeField]
    Animator animator;
    [SerializeField]
    float timer;
    
    [SerializeField]
    int playerCount;
    [SerializeField]
    bool isSwinging;

    public GameObject shadow;

    bool canSwing = false;
    float canSwingTimer = 6.0f;

    public void SetActive(bool a) { isActive = a; }

    private void Update()
    {
        // if the player is in range of swing, begin timer, if 0 begin swing
        if(isActive && !canSwing)
        canSwingTimer -= Time.deltaTime;
        if(canSwingTimer < 0)
            canSwing = true;

        if (isActive && canSwing)
        {        
            if (playerCount != 0 && !isSwinging)
                timer += Time.deltaTime;

            if (timer >= 1)
            {
                Swing(transform.position);
                timer = 0;
            }
                
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //begin timer
        if (other.GetComponent<MyPlayer>() != null && canSwing)
            playerCount += 1;
    }

    private void OnTriggerExit(Collider other)
    {
        //stop timer
        if (other.GetComponent<MyPlayer>() != null && canSwing)
            playerCount -= 1;
    }


    public void Swing(Vector3 attackPos)
    {
        //begin the swing, turn the tentacle and turn on ui on field for hit
        isSwinging = true;
        Quaternion targetRotation = Quaternion.LookRotation(attackPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);
        shadow.SetActive(true);
        StartCoroutine(SwingStart(attackPos));
    }
    public IEnumerator SwingStart(Vector3 attackPos)
    {
        //begin the swing and if the player is in range, apply hit and restart timer
        animator.SetTrigger("KrakenSlam");
        yield return new WaitForSeconds(5.1f);
        dustEffect.SetActive(true);
        shadow.SetActive(false);
        Collider[] colliders = Physics.OverlapSphere(attackPos, 4.0f);
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Rigidbody>() != null)
                collider.GetComponent<Rigidbody>().AddExplosionForce(20, attackPos, 7.5f, 0.1f, ForceMode.Impulse);
            if (collider.GetComponent<MyPlayer>() != null && collider.GetComponent<MyPlayer>().isHit == false)
                collider.GetComponent<MyPlayer>().StartCoroutine("InvulFrames", 3f);
        }


        yield return new WaitForSeconds(1);

        dustEffect.SetActive(false);
        isSwinging = false;
    }
}

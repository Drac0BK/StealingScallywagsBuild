using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Audio;

public class Weapon : MonoBehaviour
{
    public bool isCutlass = false;
    public bool isBlunder = false;
    public bool isBomb = false;
    [SerializeField] bool hasFired;
    [SerializeField] bool hasThrown;
    [SerializeField] bool hasSwung;
    bool firstSwing = false;
    public float timer;
    public float swingTimer;

    public Vector3 bombPosition;
    public GameObject explosion;
    public GameObject gunFire;
    public GameObject gunSmoke;
    public GameObject swordParticles;

    public GameObject bulletPrefab;

    public GameObject fuse;
    public GameObject fusePoint;

    public GameObject myPlayer;
    Animator animator;

    public int ownerNumber;

    private void Start()
    {
        if(myPlayer != null)
        animator = myPlayer.GetComponent<MyPlayer>().characterAnimator;
    }

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;

        string name = GetComponent<MeshFilter>().sharedMesh.name;
        // turns mesh according to which weapon is on
        if (name == "cutlasssword_LOW")
        {
            isCutlass = true;
            isBomb = false;
            isBlunder = false;
        }

        else if (name == "pCube3")
        {
            isCutlass = false;
            isBomb = false;
            isBlunder = true;
        }
        else if (name == "bomb_LOW")
        {
            isCutlass = false;
            isBomb = true;
            isBlunder = false;
        }
        // allows the weapon to be used again
        if (timer <= 0)
        {
            GetComponent<MeshRenderer>().material.color = Color.white;
            
            if (hasSwung == true)
                hasSwung = false;
            else if (hasFired == true && hasThrown == false)
            {
                gunSmoke.SetActive(false);
                hasFired = false;
            }
            else if (hasThrown == true)
            {
                GetComponent<MeshRenderer>().enabled = true;
                hasThrown = false;
            }
        }
    }

    // turns off gun smoke and allows to shoot again
    public void ReloadFinish()
    {
        gunSmoke.SetActive(false);
        hasFired = false;
        timer = 0;
    }
    // turns mesh on and can throw bomb again
    public void ThrowFinish()
    {
        GetComponent<MeshRenderer>().enabled = true;
        hasThrown = false;
        timer = 0;
    }


    public void Attack(GameObject player)
    {
        if (isCutlass && !hasSwung)
        {
                StartCoroutine(SwordSlash());
                animator.ResetTrigger("Sword_Use_2");
                firstSwing = false;
                animator.SetTrigger("Sword_Use_1");
            // plays the according sound effect based on the weapon used
            if (AudioManager.Instance != null)
            {
                if (ownerNumber == 0)
                {
                    float rand = Random.Range(0, 2);
                    if (rand == 0)
                        AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Player_1, "SwordSwing_2", false, 0.0f);
                    if (rand == 1)
                        AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Player_1, "SwordSwing_4", false, 0.0f);
                }
                else if (ownerNumber == 1)
                {
                    float rand = Random.Range(0, 2);
                    if (rand == 0)
                        AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Player_1, "SwordSwing_2", false, 0.0f);
                    if (rand == 1)
                        AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Player_1, "SwordSwing_4", false, 0.0f);
                }
                else if (ownerNumber == 2)
                {
                    float rand = Random.Range(0, 2);
                    if (rand == 0)
                        AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Player_1, "SwordSwing_2", false, 0.0f);
                    if (rand == 1)
                        AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Player_1, "SwordSwing_4", false, 0.0f);
                }
                else if (ownerNumber == 3)
                {
                    float rand = Random.Range(0, 2);
                    if (rand == 0)
                        AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Player_1, "SwordSwing_2", false, 0.0f);
                    if (rand == 1)
                        AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Player_1, "SwordSwing_4", false, 0.0f);
                }
            }
            else
            {
                Debug.LogError("There is an audio manager attached");
            }
            // hits any player collided with the sword
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3f);
            foreach (var hitCollider in hitColliders)
                if (hitCollider.GetComponent<MyPlayer>() != null && hitCollider.GetComponent<MyPlayer>().isHit == false && hitCollider.gameObject != player.gameObject)
                    if (!hitCollider.transform.GetComponent<MyPlayer>().GetInvul())
                    {
                        hitCollider.GetComponent<Rigidbody>().MovePosition(hitCollider.transform.position + player.transform.forward * 2);
                        hitCollider.GetComponent<MyPlayer>().StartCoroutine("InvulFrames", 1f);
                        hitCollider.GetComponent<MyPlayer>().hitPoints -= 1;
                        timer = 1.5f;
                    }
            hasSwung = true;

        }
        else if(isBlunder && !hasFired)
        {
            //plays the animation and fires the spawned bullet 
            animator.SetTrigger("Gun_Use");

            GameObject bullet = Instantiate(bulletPrefab, myPlayer.transform.position + myPlayer.transform.forward * 2, myPlayer.transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(myPlayer.transform.forward * 12, ForceMode.Force);
            
            // sets the timer and begin reloading. 
            hasFired = true;
            myPlayer.GetComponent<MyPlayer>().reloadStart = hasFired;
            timer = 3.5f;
            myPlayer.GetComponent<MyPlayer>().reloadStart = hasFired;
            StartCoroutine(GunShot());
        }
        else if(isBomb && !hasThrown)
        {
            if (AudioManager.Instance != null)
            {
                if (ownerNumber == 0)
                    AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Bomb_Player_1, "Bomb_FuseBurning", false, 0.0f);
                else if (ownerNumber == 1)
                    AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Bomb_Player_2, "Bomb_FuseBurning", false, 0.0f);
                else if (ownerNumber == 2)
                    AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Bomb_Player_3, "Bomb_FuseBurning", false, 0.0f);
                else if (ownerNumber == 3)
                    AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Bomb_Player_4, "Bomb_FuseBurning", false, 0.0f);
            }
            {
                Debug.LogError("There is an audio manager attached");
            }

            animator.SetTrigger("Bomb_Use");
            hasThrown = true;
            timer = 7f;
            GameObject thrownBomb = Instantiate(gameObject, player.transform.position + player.transform.up * 2, player.transform.rotation);
            thrownBomb.AddComponent<Rigidbody>();
            thrownBomb.GetComponent<Rigidbody>().AddForce(player.transform.up * 2 + player.transform.forward * 10, ForceMode.Impulse);
            thrownBomb.GetComponent<MeshCollider>().enabled = true;
            thrownBomb.GetComponent<Weapon>().SetThrown(false);
            thrownBomb.GetComponent<Weapon>().StartCoroutine(Explode(thrownBomb));
            myPlayer.GetComponent<MyPlayer>().bombStop = hasThrown;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }


    public IEnumerator Explode(GameObject bomb)
    {
        GetComponent<MeshRenderer>().material.color = Color.black;
        //Instantiate(bombRadius, transform, true);

        GameObject FuseSpark = Instantiate(fuse, bomb.GetComponent<Weapon>().fusePoint.transform.position, 
            bomb.GetComponent<Weapon>().fusePoint.transform.rotation, bomb.GetComponent<Weapon>().fusePoint.transform);
        yield return new WaitForSeconds(2f);
        //bomb sound effect for each player
        if (AudioManager.Instance != null)
        {
            if (ownerNumber == 0)
            {
                AudioManager.Instance.StopAudio(UnityCore.Audio.AudioType.Bomb_Player_1, "Bomb_FuseBurning", false, 0.0f);
                AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Bomb_Player_1, "Bomb_Explosion", false, 0.0f);
            }
            else if (ownerNumber == 1)
            {
                AudioManager.Instance.StopAudio(UnityCore.Audio.AudioType.Bomb_Player_2, "Bomb_FuseBurning", false, 0.0f);
                AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Bomb_Player_2, "Bomb_Explosion", false, 0.0f);
            }
            else if (ownerNumber == 2)
            {
                AudioManager.Instance.StopAudio(UnityCore.Audio.AudioType.Bomb_Player_3, "Bomb_FuseBurning", false, 0.0f);
                AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Bomb_Player_3, "Bomb_Explosion", false, 0.0f);
            }
            else if (ownerNumber == 3)
            {
                AudioManager.Instance.StopAudio(UnityCore.Audio.AudioType.Bomb_Player_4, "Bomb_FuseBurning", false, 0.0f);
                AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Bomb_Player_4, "Bomb_Explosion", false, 0.0f);
            }
        }
        else 
        {
            Debug.LogError("There is an audio manager attached");
        }


        bombPosition = bomb.transform.position;
        GameObject explosionClone =  Instantiate(explosion, bombPosition, Quaternion.identity);
        // hits all lthe players within the range
        Collider[] colliders = Physics.OverlapSphere(bomb.transform.position, 4.5f);
        foreach (var collider in colliders)
        {
            if(collider.GetComponent<Rigidbody>() != null)
            collider.GetComponent<Rigidbody>().AddExplosionForce(110, bomb.transform.position, 7.5f/2, 0.1f, ForceMode.Impulse);

            if (collider.GetComponent<MyPlayer>() != null && collider.GetComponent<MyPlayer>().isHit == false)
                if (!collider.GetComponent<MyPlayer>().GetInvul())
                {
                    collider.GetComponent<MyPlayer>().StartCoroutine("InvulFrames", 4f);
                    collider.GetComponent<MyPlayer>().hitPoints -= 1;
                }
        }
        //destroy bomb clone when exploded
        bomb.GetComponent<MeshRenderer>().enabled = false;
        Destroy(FuseSpark);
        yield return new WaitForSeconds(2f);
        Destroy(explosionClone);
       
        Destroy(bomb);
    }
    //plays the gun soundeffect for each player
    public IEnumerator GunShot()
    {
        Debug.Log("kek shooty");
        GameObject gunFireClone = Instantiate(gunFire, gunFire.transform.position, gunFire.transform.rotation);
        gunFireClone.SetActive(true);

        
            if (ownerNumber == 0)
                AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Player_1, "Gunshot", false, 0.0f);
            else if (ownerNumber == 1)
                AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Player_2, "Gunshot", false, 0.0f);
            else if (ownerNumber == 2)
                AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Player_3, "Gunshot", false, 0.0f);
            else if (ownerNumber == 3)
                AudioManager.Instance.PlayAudio(UnityCore.Audio.AudioType.Player_4, "Gunshot", false, 0.0f);
        

        yield break;
    }
    // sword particles
    public IEnumerator SwordSlash()
    {
        if (swordParticles != null)
        {
            swordParticles.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            swordParticles.SetActive(false);
        }
    }
    // list of checks for animation reference
    public bool HasSwung() { return hasSwung; }
    public void SetSwung(bool a_swing) { hasSwung = a_swing; }
    public bool HasFired() { return hasFired; }
    public void SetFired(bool a_fired) { hasFired = a_fired; }
    public bool HasThrown() { return hasThrown; }
    public void SetThrown(bool a_thrown) { hasThrown = a_thrown; }
    public float Timer() { return timer; }
    public void SetTimer(float a_time) { timer = a_time; }
}

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
    //public GameObject bombRadius;

    public GameObject myPlayer;
    Animator animator;

    public int ownerNumber;
    //private AudioManager audioManager;

    private void Start()
    {
        if(myPlayer != null)
        animator = myPlayer.GetComponent<MyPlayer>().characterAnimator;
        //audioManager = GameObject.Find("InGameAudioManager").GetComponent<AudioManager>();

    }

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;

        string name = GetComponent<MeshFilter>().sharedMesh.name;

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


    public void ReloadFinish()
    {
        gunSmoke.SetActive(false);
        hasFired = false;
        timer = 0;
    }

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
            animator.SetTrigger("Gun_Use");
            if (bulletPrefab == null)
            {
                RaycastHit hit;
                if (Physics.SphereCast(myPlayer.transform.position + Vector3.up, 2, transform.TransformDirection(Vector3.forward), out hit, 10.0f))
                    if (hit.transform.gameObject.GetComponent<MyPlayer>() != null)
                    {
                        hit.transform.GetComponent<Rigidbody>().MovePosition(hit.transform.position + player.transform.forward * 2);
                        hit.transform.GetComponent<MyPlayer>().StartCoroutine("InvulFrames", 3f);
                        hit.transform.GetComponent<MyPlayer>().hitPoints -= 1;
                    }
            }
            else
            {
                GameObject bullet = Instantiate(bulletPrefab, myPlayer.transform.position + myPlayer.transform.forward * 2, myPlayer.transform.rotation);
                bullet.GetComponent<Rigidbody>().AddForce(myPlayer.transform.forward * 12, ForceMode.Force);
            }

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
        }
    }


    public IEnumerator Explode(GameObject bomb)
    {
        GetComponent<MeshRenderer>().material.color = Color.black;
        //Instantiate(bombRadius, transform, true);

        GameObject FuseSpark = Instantiate(fuse, bomb.GetComponent<Weapon>().fusePoint.transform.position, 
            bomb.GetComponent<Weapon>().fusePoint.transform.rotation, bomb.GetComponent<Weapon>().fusePoint.transform);
        yield return new WaitForSeconds(2f);

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
        bomb.GetComponent<MeshRenderer>().enabled = false;
        Destroy(FuseSpark);
        yield return new WaitForSeconds(2f);
        Destroy(explosionClone);
       
        Destroy(bomb);
    }

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

    public IEnumerator SwordSlash()
    {
        if (swordParticles != null)
        {
            swordParticles.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            swordParticles.SetActive(false);
        }
    }

    public bool HasSwung() { return hasSwung; }
    public void SetSwung(bool a_swing) { hasSwung = a_swing; }
    public bool HasFired() { return hasFired; }
    public void SetFired(bool a_fired) { hasFired = a_fired; }
    public bool HasThrown() { return hasThrown; }
    public void SetThrown(bool a_thrown) { hasThrown = a_thrown; }
    public float Timer() { return timer; }
    public void SetTimer(float a_time) { timer = a_time; }
}

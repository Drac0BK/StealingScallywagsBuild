using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore;
using UnityCore.Audio;

public class WeaponSolo : MonoBehaviour
{ // NOT FOR FINAL RELEASE ***************************************************
    public bool isCutlass = false;
    public bool isBlunder = false;
    public bool isBomb = false;
    [SerializeField] bool hasFired;
    [SerializeField] bool hasThrown;
    [SerializeField] bool hasSwung;
    public float timer;
    public float swingTimer;

    public Vector3 bombPosition;
    public GameObject explosion;
    public GameObject gunFire;
    public GameObject gunSmoke;
    public GameObject bombRange;

    public GameObject bulletPrefab;

    public GameObject fuse;
    public GameObject fusePoint;

    public GameObject myPlayer;

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;

        string name = GetComponent<MeshFilter>().sharedMesh.name;
        // turns on the corresponding weapon mesh to be seen and used
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
        // timer to tell when they can swing again. 
        if (timer <= 0)
        {
            // once the timer is complete tells the weapon it can swing again
            if (hasSwung == true)
            {
                hasSwung = false;
            }
            else if (hasFired == true && hasThrown == false)
            {
                Debug.Log(hasFired);
                gunSmoke.SetActive(false);
                hasFired = false;
                Debug.Log(hasFired);

            }
            else if (hasThrown == true)
            {
                Debug.Log(hasThrown);
                GetComponent<MeshRenderer>().enabled = true;
                hasThrown = false;
                Debug.Log(hasThrown);

            }
        }
    }

    public void Attack(GameObject player)
    {
        if (isCutlass && !hasSwung)
        {
            //swings the players sword and if collided with, hits the collided object if player
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.GetComponent<SoloPlayer>() != null && hitCollider.GetComponent<SoloPlayer>().isHit == false && hitCollider.gameObject != player.gameObject)
                {
                    if (!hitCollider.transform.GetComponent<SoloPlayer>().GetInvul())
                    {
                        hitCollider.GetComponent<Rigidbody>().MovePosition(hitCollider.transform.position + player.transform.forward * 2);
                        hitCollider.GetComponent<SoloPlayer>().StartCoroutine("InvulFrames", 1f);
                        hitCollider.GetComponent<SoloPlayer>().hitPoints -= 1;
                        hasSwung = true;
                        timer = 1.5f;
                    }
                }
            }
        }
        else if (isBlunder && !hasFired)
        {
            //fires a bullet and begins reloading
            GameObject bullet = Instantiate(bulletPrefab, myPlayer.transform.position + myPlayer.transform.forward * 2, myPlayer.transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(myPlayer.transform.forward * 12, ForceMode.Force);

            hasFired = true;
            myPlayer.GetComponent<SoloPlayer>().reloadStart = hasFired;
            timer = 3.5f;
            myPlayer.GetComponent<SoloPlayer>().reloadStart = hasFired;
            StartCoroutine(GunShot());
        }
        else if (isBomb && !hasThrown)
        {
            // throws a bomb clone and turns off the current one
            hasThrown = true;
            timer = 7f;
            GameObject thrownBomb = Instantiate(gameObject, player.transform.position + player.transform.up * 2, player.transform.rotation);
            thrownBomb.AddComponent<Rigidbody>();
            thrownBomb.GetComponent<Rigidbody>().AddForce(player.transform.up * 2 + player.transform.forward * 10, ForceMode.Impulse);
            thrownBomb.GetComponent<MeshCollider>().enabled = true;
            thrownBomb.GetComponent<WeaponSolo>().SetThrown(false);
            thrownBomb.GetComponent<WeaponSolo>().StartCoroutine(Explode(thrownBomb));
            myPlayer.GetComponent<SoloPlayer>().bombStop = hasThrown;
            myPlayer.GetComponent<SoloPlayer>().bombTimer = timer;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
    // tells the player it has finished reloading and turns off particles
    public void ReloadFinish()
    {
        gunSmoke.SetActive(false);
        hasFired = false;
        timer = 0;
    }
    // tells the player it has completed the throw
    public void ThrowFinish()
    {
        GetComponent<MeshRenderer>().enabled = true;
        hasThrown = false;
        timer = 0;
    }
    // sets the explosion of the bomb and animation with it
    public IEnumerator Explode(GameObject bomb)
    {
        GetComponent<MeshRenderer>().material.color = Color.black;

        GameObject FuseSpark = Instantiate(fuse, bomb.GetComponent<WeaponSolo>().fusePoint.transform.position,
            bomb.GetComponent<WeaponSolo>().fusePoint.transform.rotation, bomb.GetComponent<WeaponSolo>().fusePoint.transform);
        yield return new WaitForSeconds(2f);
        if (bombRange != null)
            bomb.GetComponent<WeaponSolo>().bombRange.SetActive(true);

        bombPosition = bomb.transform.position;
        GameObject explosionClone = Instantiate(explosion, bombPosition, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(bomb.transform.position, 7.0f);
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Rigidbody>() != null)
                collider.GetComponent<Rigidbody>().AddExplosionForce(110, bomb.transform.position, 7.5f, 0.1f, ForceMode.Impulse);
            if (collider.GetComponent<SoloPlayer>() != null && collider.GetComponent<SoloPlayer>().isHit == false)
            {
                if (!collider.GetComponent<SoloPlayer>().GetInvul())
                {
                    collider.GetComponent<SoloPlayer>().StartCoroutine("InvulFrames", 4f);
                    collider.GetComponent<SoloPlayer>().hitPoints -= 1;
                }
            }
        }
        bomb.GetComponent<MeshRenderer>().enabled = false;

        Destroy(FuseSpark);
        Destroy(bomb.GetComponent<WeaponSolo>().bombRange);
        yield return new WaitForSeconds(2f);
        Destroy(explosionClone);     

        Destroy(bomb);
    }
    //spawns particles of a gun shot
    public IEnumerator GunShot()
    {
        GameObject gunFireClone = Instantiate(gunFire, gunFire.transform.position, gunFire.transform.rotation);
        gunFireClone.SetActive(true);

        yield break;
    }
    // collection of checks to see if player has done a action for animation reference
    public bool HasSwung() { return hasSwung; }
    public void SetSwung(bool a_swing) { hasSwung = a_swing; }
    public bool HasFired() { return hasFired; }
    public void SetFired(bool a_fired) { hasFired = a_fired; }
    public bool HasThrown() { return hasThrown; }
    public void SetThrown(bool a_thrown) { hasThrown = a_thrown; }
    public float Timer() { return timer; }
    public void SetTimer(float a_time) { timer = a_time; }
}

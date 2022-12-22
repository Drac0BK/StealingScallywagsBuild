using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.EventSystems;
using UnityCore;
using UnityCore.Audio;

public class MyPlayer : MonoBehaviour
{
    public float movementSpeed = 7.0f, jumpVelocity = 7.0f;
    Rigidbody rb;
    private float swordUseTime;
    public int hitPoints = 4;
    int currentHitPoints;

    public GameObject hitPointImage;
    public GameObject circleIndicator;
    public GameObject carryPointChest;
    public GameObject carryPointBag;
    public GameObject treasureCarried;
    public GameObject groundChecker;
    public GameManager gameManager;
    public GameObject sicknessSpirals;
    public GameObject powerArrows;
    public GameObject stunEffect;

    public Animator characterAnimator;
    public Animator hpAnimator;

    private Vector2 movementInput = Vector2.zero;

    private bool jumped = false;
    private bool isCarrying = false;
    public bool groundedPlayer;
    public bool isHit = false;
    bool isInvul = false;
    public bool isMoving = false;

    public Color ringColor;
    public Color flash;
    public Color original;

    public GameObject currentWeapon;
    Weapon currentWeaponScript;
    public List<GameObject> weaponList = new List<GameObject>();
    public List<GameObject> hitPointIcons = new List<GameObject>();
    public Vector3 startPos;
    int weaponSpot;

    public Image swordIcon = null, gunIcon = null, bombIcon = null, playerIcon = null;
    private PlayerControls controls;
    PlayerConfiguration playerConfig;

    private int playerNumber;

    float blunderTimer = 3.5f;
    float bombTimer = 7.0f;
    public bool reloadStart = false;
    public bool bombStop = false;
    bool canMove = false;

    public void SetMove(bool a_move) { canMove = a_move; }

    private void Start()
    {
        //set all coresponding data of the character to the player config
        currentHitPoints = hitPoints;
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        SetWeapon(weaponSpot);
        circleIndicator.GetComponent <SpriteRenderer>().color = new Vector4(ringColor.r,ringColor.g, ringColor.b, 255.0f);
        controls = new PlayerControls();
        for(int i = 0; i < weaponList.Count; i++)
        {
            weaponList[i].GetComponent<Weapon>().myPlayer = gameObject;
        }
    }

    void FixedUpdate()
    {
        //timer of weapons
        if (reloadStart)
            blunderTimer -= Time.deltaTime;
        else if (!reloadStart && blunderTimer < 0)
        {
            weaponList[1].GetComponent<Weapon>().ReloadFinish();
            blunderTimer = 3.5f;
        }

        if (bombStop)
            bombTimer -= Time.deltaTime;
        else if (!bombStop && bombTimer < 0)
        {
            weaponList[2].GetComponent<Weapon>().ReloadFinish();
            currentWeaponScript.ThrowFinish();
            bombTimer = 7.0f;
        }

        if (blunderTimer < 0)
            reloadStart = false;

        if (bombTimer < 0)
            bombStop = false;


        swordUseTime += Time.deltaTime;

        if(currentWeapon == weaponList[0] || currentWeapon == weaponList[1])
            currentWeapon.GetComponent<Renderer>().enabled = !isCarrying;
        else if (!bombStop)
            currentWeapon.GetComponent<Renderer>().enabled = !isCarrying;


        groundedPlayer = IsGrounded();

        if(isHit)
            jumped = false;

        if (!isHit)
        {
            Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
            Vector3 aniMove = move;



            characterAnimator.SetFloat("MoveBlendX", aniMove.x);

            characterAnimator.SetFloat("MoveBlendZ", aniMove.z);
            //move player and animation
            rb.MovePosition((Vector3)transform.position + (move * Time.deltaTime * movementSpeed));
            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;// makes the model rotate to the forward
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            if (isMoving == true)
            {
                characterAnimator.SetBool("IsMoving", true);
            }
            else
            {
                characterAnimator.SetBool("IsMoving", false);
            }
        }
       // changes ui of hearts based on hp
        if(currentHitPoints > hitPoints)
        {
            currentHitPoints -= 1;
            hitPointIcons[currentHitPoints].gameObject.SetActive(false);
        }

        // Changes the height position of the player..
        if (jumped && groundedPlayer)
        {
            rb.velocity = Vector3.up * jumpVelocity;
        }
        //play jump animation
        if (jumped)
            characterAnimator.SetBool("IsJumping", true);
        if (!jumped)
            characterAnimator.SetBool("IsJumping", false);
        // respawn the player if they fall below the map or hit 0 hitpoints
        if (transform.position.y < -1)
        {
            Respawn();
            Destroy(treasureCarried);
        }

        if (hitPoints <= 0)
        {
            Respawn();
        }

        PowerUpCheck();

        if (weaponSpot == 0)
        {
            //Idle animation setting
            characterAnimator.SetLayerWeight(1, 1);
            characterAnimator.SetLayerWeight(2, 0);
            characterAnimator.SetLayerWeight(3, 0);
        }
        else if (weaponSpot == 1)
        {
            //Idle animation setting
            characterAnimator.SetLayerWeight(1, 0);
            characterAnimator.SetLayerWeight(2, 1);
            characterAnimator.SetLayerWeight(3, 0);
        }
        else if (weaponSpot == 2)
        {
            //Idle animation setting
            characterAnimator.SetLayerWeight(1, 0);
            characterAnimator.SetLayerWeight(2, 0);
            characterAnimator.SetLayerWeight(3, 1);
        }

    }

    bool IsGrounded()
    {
        // checks if grounded by a raycast
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, -Vector3.up);

        Physics.Raycast(downRay, out hit);

        if (hit.distance > 1.2)
        {
            if (circleIndicator != null)
            {
                circleIndicator.gameObject.SetActive(true);
                circleIndicator.transform.position = hit.point + new Vector3 (0, 0.1f, 0);
            }

            return false;
        }
        else if (hit.distance < 1.2 && hit.distance != 0)
        {
            if (circleIndicator != null)
            {
                circleIndicator.gameObject.SetActive(true);
                circleIndicator.transform.position = hit.point + new Vector3(0, 0.1f, 0);
            }
            return true;
        }

        circleIndicator.gameObject.SetActive(false);
        return false;
    }

    public void InitializePlayer(PlayerConfiguration pc)
    {
        //sets the corresponding player config and inputs
        if (playerConfig == null)
        {
            playerConfig = pc;
            playerConfig.Input.onActionTriggered += Input_onActionTriggered;
            playerNumber = playerConfig.Input.playerIndex;

            foreach (GameObject weapon in weaponList)
            {
                weapon.GetComponent<Weapon>().ownerNumber = playerNumber;
            }
        }
    }

    private void Input_onActionTriggered(CallbackContext obj)
    {
        //checks the input from the player input to the characters 
        if (obj.action == null)
            return;

        if (obj.action.name == controls.Player.Move.name)
            MovePlayer(obj);
        if (obj.action.name == controls.Player.Jump.name)
            Jump(obj);
        if (obj.action.name == controls.Player.Pick.name)
            Pick(obj);
        if (obj.action.name == controls.Player.Attack.name)
            Attack(obj);
        if (obj.action.name == controls.Player.Pause.name)
            PauseGame(obj);
        if (obj.action.name == controls.Player.WeaponSwap1.name)
            SwapWeapon1(obj);
        if (obj.action.name == controls.Player.WeaponSwap2.name)
            SwapWeapon2(obj);
    }

    public void SetWeapon(int weaponValue)
    {
        //sets the weapon and mesh on
        currentWeapon= weaponList[weaponValue];
        currentWeaponScript = currentWeapon.GetComponent<Weapon>();
        int weaponScore = 0;
        foreach (var weapon in weaponList)
        {
            if (currentWeapon != weapon)
            {
                weaponList[weaponScore].gameObject.SetActive(false);
            }
            if (currentWeapon == weapon)
            {
                weaponList[weaponScore].gameObject.SetActive(true);
            }
            weaponScore += 1;
        }
        SetWeaponIcon();
    }
    public void SetWeaponIcon()
    {
     //changes the ui icon with the corresponding item   
        if (swordIcon != null || gunIcon != null || bombIcon != null)
        {
            if (currentWeapon.name.Contains("Sword"))
            {
                //Weapon prefab setting
                swordIcon.gameObject.SetActive(true);
                gunIcon.gameObject.SetActive(false);
                bombIcon.gameObject.SetActive(false);
                return;
            }
            if (currentWeapon.name.Contains("Blunder"))
            {
                //Weapon prefab setting
                swordIcon.gameObject.SetActive(false);
                gunIcon.gameObject.SetActive(true);
                bombIcon.gameObject.SetActive(false);
                return;
            }
            if (currentWeapon.name.Contains("Bomb"))
            {
                //Weapon prefab setting
                swordIcon.gameObject.SetActive(false);
                gunIcon.gameObject.SetActive(false);
                bombIcon.gameObject.SetActive(true);
                return;
            }
        }
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        // move the player when they are not hit
        if (isHit)
            movementInput = new Vector2(0, 0);
        if (canMove)
            movementInput = context.action.ReadValue<Vector2>();
    }


    public void Jump(InputAction.CallbackContext context)
    {
        //allows the player to jump when they are not hit
        if(canMove)
        if(!isHit)
            jumped = context.action.triggered;
    }

    public void Pick(InputAction.CallbackContext context)
    {
        if (!gameManager.GetPause())
        {
            //pick up treasure that is within range and drop treasure
            if (context.started && !isHit)
            {
                if (!isCarrying)
                {
                    Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1);
                    foreach (var hitCollider in hitColliders)
                    {
                        if (hitCollider.GetComponent<SpawnableObjects>() != null && treasureCarried == null)
                        {
                            StartCoroutine("pickADrop");
                            treasureCarried = hitCollider.gameObject;
                            characterAnimator.SetLayerWeight(4, 1);
                            if (treasureCarried.GetComponent<SpawnableObjects>().IsChest())
                            {
                                characterAnimator.SetBool("IsBag", false);
                                treasureCarried.transform.position = carryPointChest.transform.position;
                                treasureCarried.transform.rotation = carryPointChest.transform.rotation;
                            }
                            else
                            {
                                characterAnimator.SetBool("IsBag", true);
                                treasureCarried.transform.position = carryPointBag.transform.position;
                                treasureCarried.transform.rotation = carryPointChest.transform.rotation;
                            }
                            treasureCarried.GetComponent<SpawnableObjects>().isCarried = true;
                            treasureCarried.GetComponent<Rigidbody>().isKinematic = true;
                            treasureCarried.GetComponent<Collider>().enabled = false;
                            treasureCarried.GetComponent<MeshCollider>().enabled = false;
                            treasureCarried.transform.rotation = transform.rotation;
                            isCarrying = true;
                            treasureCarried.transform.parent = transform;
                        }
                    }
                }
                else if (isCarrying)
                {
                    treasureCarried.transform.parent = null;
                    treasureCarried.transform.position += transform.forward;
                    StartCoroutine("pickADrop");
                    characterAnimator.SetLayerWeight(4, 0);
                    treasureCarried.GetComponent<Rigidbody>().isKinematic = false;
                    treasureCarried.GetComponent<MeshCollider>().enabled = true;
                    treasureCarried.GetComponent<Collider>().enabled = true;
                    isCarrying = false;
                    treasureCarried = null;
                }
            }
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        //determines if can hit or not else it throws a object
        if (!isCarrying)
        {
            if (context.started && !isHit)
            {
                currentWeapon.GetComponent<Weapon>().Attack(gameObject);

                AnimCheck();
            }
            
        }
        else
        {
            characterAnimator.SetLayerWeight(4, 0);
            treasureCarried.GetComponent<Rigidbody>().isKinematic = false;
            treasureCarried.GetComponent<Collider>().enabled = true;
            treasureCarried.GetComponent<MeshCollider>().enabled = true;
            treasureCarried.transform.parent = null;
            isCarrying = false;
            treasureCarried.GetComponent<Rigidbody>().AddForce(transform.forward * 10 * movementSpeed + transform.up * 9, ForceMode.Impulse);
            treasureCarried = null;
        }

    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        // pause the game
        if(context.started && !gameManager.GetPause())
            gameManager.SetPause(true);

    }

    public void SwapWeapon1(InputAction.CallbackContext context)
    {
        // swap the weapon in the right direction
        if (context.started && !gameManager.GetPause())
        {
            weaponSpot += 1;
            if (weaponSpot > 2)
                weaponSpot = 0;
            SetWeapon(weaponSpot);
            if (weaponSpot == 0)
                currentWeaponScript.SetSwung(true);
            else if (weaponSpot == 1)
                currentWeaponScript.SetFired(true);
            else if (weaponSpot == 2)
                currentWeaponScript.SetThrown(true);
        }
   
    }
    public void SwapWeapon2(InputAction.CallbackContext context)
    { // swap the weapon in the left direction.
        if (context.started && !gameManager.GetPause())
        {
            weaponSpot -= 1;
            if (weaponSpot < 0)
                weaponSpot = 2;
            SetWeapon(weaponSpot);
            if (weaponSpot == 0)
                currentWeaponScript.SetSwung(true);
            else if (weaponSpot == 1)
                currentWeaponScript.SetFired(true);
            else if (weaponSpot == 2)
                currentWeaponScript.SetThrown(true);
        }
    }

    // stops the player from moving 
    public IEnumerator pickADrop()
    {
        isHit = true;
        yield return new WaitForSeconds(0.01f);
        isHit = false;
    }

    public bool GetInvul() { return isInvul; }
    // turns on their stun animation, can't be hit until complete and unable to move till completed
    public IEnumerator InvulFrames(float time)
    {
        isInvul = true;
        characterAnimator.ResetTrigger("Stun_End");
        stunEffect.SetActive(true);
        isHit = true;
        hpAnimator.SetTrigger("TakeDamage");
        characterAnimator.SetTrigger("Stun_Start");

        float temp = 0;
        while (temp <= time)
        {
            if (treasureCarried != null)
            {
                treasureCarried.transform.parent = null;
                treasureCarried.GetComponent<Rigidbody>().isKinematic = false;
                treasureCarried.GetComponent<MeshCollider>().enabled = true;
                treasureCarried.GetComponent<Collider>().enabled = true;
                characterAnimator.SetLayerWeight(4, 0);
                isCarrying = false;
                treasureCarried = null;
            }

            yield return new WaitForSeconds(1);

            temp++;
        }

        characterAnimator.SetTrigger("Stun_End");
        stunEffect.SetActive(false);
        yield return new WaitForSeconds(1f);
        isHit = false;
        foreach (var item in hitPointIcons)
            item.GetComponent<SpriteRenderer>().color = new Vector4(0, 0, 0, 1.0f);
        yield return new WaitForSeconds(3f);
        foreach (var item in hitPointIcons)
            item.GetComponent<SpriteRenderer>().color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        isInvul = false;
    }
    //respawns the player on their corresponding scorezone
    public void Respawn()
    {
        if (!isHit)
        {
            isHit = true;
            transform.position = startPos;
            isCarrying = false;
            StartCoroutine("InvulFrames", 1f);
            characterAnimator.SetLayerWeight(4, 0);
            hitPoints = 4;
            currentHitPoints = hitPoints;
            foreach (var item in hitPointIcons)
                item.gameObject.SetActive(true);
            characterAnimator.ResetTrigger("Stun_End");
        }
        else
        {
            isHit = true;
            transform.position = startPos;
            isCarrying = false;
            StartCoroutine("InvulFrames", 3f);
            characterAnimator.SetLayerWeight(4, 0);
            hitPoints = 4;
            currentHitPoints = hitPoints;
            foreach (var item in hitPointIcons)
                item.gameObject.SetActive(true);
            characterAnimator.ResetTrigger("Stun_End");
        }
    }
    // checks the players values with the powerups in their scorezone and change it
    public void PowerUpCheck()
    {
        if (movementSpeed < 6 || jumpVelocity < 5)
        {
            sicknessSpirals.SetActive(true);
            powerArrows.SetActive(false);
        }
        else if (movementSpeed > 7 || jumpVelocity > 5)
        {
            sicknessSpirals.SetActive(false);
            powerArrows.SetActive(true);
        }
        else
        {
            sicknessSpirals.SetActive(false);
            powerArrows.SetActive(false);
        }
    }
    //sets the animation based on the weapon
    public void AnimCheck()
    {
        if (weaponSpot == 0)
            characterAnimator.SetTrigger("Sword_Idle");
        else if (weaponSpot == 1)
            characterAnimator.SetTrigger("Gun_Idle");
        else if (weaponSpot == 2)
            characterAnimator.SetTrigger("Bomb_Idle");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityCore;
using UnityCore.Audio;

public class EventScript : MonoBehaviour
{
    public GameObject krakens;

    public Kraken kraken;
    public FallingEvent fallEvent;
    public Wave wave;

    [SerializeField]
    bool Lv1, Lv2;

    [SerializeField]
    private Image focus, krakenAnnounce, tsunamiAnnounce, cursedAnnounce, collapseAnnounce;
    private Image imageInAction;
    private float announcementTimer;

    //public ShipCannon cannon;
    List<GameObject> scoreZones = new List<GameObject>();
    public List<GameObject> cursedTreasure = new List<GameObject>();
    public float eventTimer = 100;

    public float minTimeInclusive = 5;
    public float maxTimeExclusive = 12;
    bool hasOccured = false;
    bool isKraken = false;
    bool isWave = false;
    bool isCursed = false;
    bool isStalactite = false;

    public AudioManager audioManager;
    [SerializeField]
    private AudioSource bgmAudioSource;
    private bool toggle;

    void Start()
    {
        // based on the level, the corresponding event is chosen by a random number
        scoreZones = GetComponent<GameManager>().scoreZones;
        int chance = Random.Range(0, 100);
        if (Lv1)
        {
           if (chance >= 0 && chance <= 32)
               isKraken = true;
           else if (chance >= 33 && chance <= 65)
               isWave = true;
           else if (chance >= 66 && chance <= 99)
               isCursed = true;
        }
        else if (Lv2)
        {
            if (chance >= 0 && chance <= 32)
                isKraken = true;
            else if (chance >= 33 && chance <= 65)
                isStalactite = true;
            else if (chance >= 66 && chance <= 99)
                isCursed = true;
        }
        eventTimer = Random.Range(minTimeInclusive, maxTimeExclusive);
    }

    void Update()
    {
        //destroy the events when they are complete
        if(GetComponent<GameManager>().GetTimer() < 0)
        {
            Destroy(kraken);
            if(Lv1)
            Destroy(wave);
            if(Lv2)
            Destroy(fallEvent);
        }
        eventTimer -= Time.deltaTime;
        

        // when the event timer by the editor is reached, the corresponding event is called, turning them on
        if (eventTimer <= 0 && !hasOccured)
        {
            if (isKraken)
            {
                krakens.SetActive(true);
                imageInAction = krakenAnnounce;
                announcementTimer = 3f;
                new WaitForSeconds(5.5f);
                kraken.FindPlayerSpot();
            }
            else if (isWave)
            {
                wave.WaveBegin();
                imageInAction = tsunamiAnnounce;
                announcementTimer = 3f;
            }
            else if (isCursed)
            {
                SpawnCursedTreasure();
                imageInAction = cursedAnnounce;
                announcementTimer = 3f;
            }
            else if(isStalactite)
            {
                imageInAction = collapseAnnounce;
                fallEvent.gameObject.SetActive(true);
                announcementTimer = 3f;
            }
            hasOccured = true;  
        }

        //plays a sound effect when a event is called
        if(bgmAudioSource != null)
        {
            if (bgmAudioSource.isPlaying == false)
            {
                if (toggle == true)
                {
                    if (!isKraken && !isWave && !isCursed)
                    {
                        audioManager.PlayAudio(UnityCore.Audio.AudioType.BGM, "Main_Theme", false, 0.0f);
                    }
                    else if (isKraken)
                    {
                        audioManager.PlayAudio(UnityCore.Audio.AudioType.BGM, "Kraken_Theme", false, 0.0f);
                    }
                    else if (isWave || isStalactite)
                    {
                        audioManager.PlayAudio(UnityCore.Audio.AudioType.BGM, "Tsunami_Theme", false, 0.0f);
                    }
                    else if (isCursed)
                    {
                        audioManager.PlayAudio(UnityCore.Audio.AudioType.BGM, "Cursed_Treasure_Theme", false, 0.0f);
                    }
                    else
                    {
                        Debug.LogError("System is in event time: " + hasOccured + ". System is not registering any song to play.");
                    }
                    toggle = false;
                }
                else
                {
                    toggle = true;
                }

            }
        }
        else
            Debug.LogError("No bgm Audio Source");
        
        // turns on ui according to the event that is called and turns them off after 3 seconds
        if (imageInAction != null)
        {

            if (announcementTimer > 0)
            {
                announcementTimer -= Time.deltaTime;
                imageInAction.gameObject.SetActive(true);
                focus.gameObject.SetActive(true);
            }
            else if (imageInAction.gameObject.activeInHierarchy == true)
            {
                imageInAction.gameObject.SetActive(false);
                focus.gameObject.SetActive(false);
            }
        }
    }
    //spawn treasure event
    void SpawnCursedTreasure()
    {
        int cursedTreasureSpot = 0;
        if (cursedTreasure.Count != 0)
        {
            // for each scorezone spawn a cursed treasure
            for(int i = 0; i < GetComponent<GameManager>().players.Count; i++)
            {
                cursedTreasureSpot = Random.Range(0, cursedTreasure.Count);
                Instantiate(cursedTreasure[cursedTreasureSpot], scoreZones[i].transform.position + (Vector3.up * 20), scoreZones[i].transform.rotation, transform);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public GameObject targetPoint;
    public GameObject startPoint;
    public GameObject wavePrefab;
    public GameObject waveWarnings;
    GameObject wave;
    [SerializeField]
    private float speed = 20.0f;
    [SerializeField] bool waveStarted = false;
    [SerializeField] bool waveActive = false;
    [SerializeField] float timer;
    
    
    private void Update()
    {
        // begin the wave event when the timer reaches 0
        if (waveActive && !waveStarted)
            timer += Time.deltaTime;

        if (timer >= 20)
        {
            WaveBegin();
            timer = 0;
        }
            
        // move the wave
        if (waveStarted)
            wave.transform.position += -transform.right * speed * Time.deltaTime;
        

        if (wave != null)
            if (wave.transform.position.x < targetPoint.transform.position.x)
                WaveEnd();


    }

    public void WaveBegin()
    {
        //spawn the wave object, and start moving the wave
        waveWarnings.SetActive(true);
        waveActive = true;
        waveStarted = true;
        wave = Instantiate(wavePrefab, startPoint.transform.position, wavePrefab.transform.rotation, startPoint.transform);
        StartCoroutine(WarningStop());
    }

    public void WaveEnd()
    {
        //destroy the wave object
        waveStarted = false;
        Destroy(wave.gameObject);
    }

    public IEnumerator WarningStop()
    {
        // ui element to tell players
        yield return new WaitForSeconds(5);
        waveWarnings.SetActive(false);
    }
}

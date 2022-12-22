using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

using System.IO;
using System.Linq;
using UnityEngine.Rendering;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TMPro.TMP_Dropdown resolutionDropdown;
    public Slider audioSlider;
    public Slider musicSlider;
    public Slider effectSlider;

    public Toggle isFull;
    public TMPro.TMP_Dropdown qualityDropdown;
    public RenderPipelineAsset[] qualityLevels;

    Resolution[] resolutions;
    Resolution[] myResolutions = new Resolution[6];
    int[] widthRes = new int[6] { 800, 1024, 1280, 1440, 1680,1920 };
    int[] heightRes = new int[6] { 600, 768, 800, 900, 1050,1080 };

    string fullScreen = "false";
    int quality = 0;
    int resolutionInd = 0;
    public float setVolume;
    public float musicVolume;
    public float effectsVolume;

    bool isFullScreen = false;
    int resIndex = 0;

    // gets all the relevent information for the game and sets them accordingly
    void Start()
    {

        setVolume = PlayerPrefs.GetFloat("Volume");

        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        effectsVolume = PlayerPrefs.GetFloat("EffectsVolume");

        resolutionInd = PlayerPrefs.GetInt("SetResolution");
        quality = PlayerPrefs.GetInt("SetQuality");
        fullScreen = PlayerPrefs.GetString("IsFullScreen");

        audioSlider.value = setVolume;
        musicSlider.value = musicVolume;
        effectSlider.value = effectsVolume;


        if (fullScreen == "true")
        {
            isFull.isOn = true;
            isFullScreen = true;
        }
        else if (fullScreen == "false")
        {
            isFull.isOn = false;
            isFullScreen = false;
        }

        for (int i = 0; i < 6; i++)
        {
            myResolutions[i].width = widthRes[i];
            myResolutions[i].height = heightRes[i];
            myResolutions[i].refreshRate = 60;
        }

        resolutions = myResolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].width == Screen.currentResolution.height)
            {
                resolutionInd = i;
            }

        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = resolutionInd;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetResolution(int resolutionIndex)
    {
        resIndex = resolutionIndex;
    }

    void ApplyFullRes(bool isFullScreen, int resIndex)
    {
        Screen.fullScreen = isFullScreen;
        resolutionInd = resIndex;
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen); //Get Size of Window

    }

    public void SetFullscreen(bool a_isFullscreen)
    {
        
        //Get if fullscreen
        if (a_isFullscreen == false)
        {
            isFullScreen = false;
            fullScreen = "false";
        }
        else
        {
            isFullScreen = true;
            fullScreen = "true";
        }
    }

    public void SetVolume(float volume)
    {
        setVolume = volume;
        audioMixer.SetFloat("Volume", setVolume);
    }
    public void SetEffectVolume(float volume)
    {
        effectsVolume = volume;
        audioMixer.SetFloat("EffectVolume", effectsVolume);
    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        audioMixer.SetFloat("MusicVolume", musicVolume);
    }

    public void SetQuality(int qualityIndex)
    {   
        QualitySettings.SetQualityLevel(qualityIndex); //Get Quality of Window
        quality = qualityIndex;
        QualitySettings.renderPipeline = qualityLevels[qualityIndex];
    }

    public void SaveSettings()
    {
        ApplyFullRes(isFullScreen, resIndex);
        // saves all the settings
        PlayerPrefs.SetFloat("Volume", setVolume);

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);

        PlayerPrefs.SetInt("SetResolution", resolutionInd);
        PlayerPrefs.SetInt("SetQuality", quality);
        PlayerPrefs.SetString("IsFullScreen", fullScreen);
    }

}
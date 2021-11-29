using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public CinemachineFreeLook cineCam;
    public AudioMixer audioMixer;

    public Slider volumeSlider;
    public Dropdown resDropdown;
    public Toggle fullscreenToggle;
    public Slider sensSlider;

    private Resolution[] resolutions;
    private List<string> resList = new List<string>();
    private List<string> resOptions = new List<string>();

    private void Start() {
        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            string res = resolutions[i].width + " x " + resolutions[i].height + " : " + resolutions[i].refreshRate + "Hz";
            resList.Add(res);
            if (resolutions[i].refreshRate <= Screen.currentResolution.refreshRate && resolutions[i].refreshRate >= Screen.currentResolution.refreshRate - 1) {
                string option = resolutions[i].width + " x " + resolutions[i].height + " : " + resolutions[i].refreshRate + "Hz";
                resOptions.Add(option);

                if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height && !PlayerPrefs.HasKey("Resolution")) {
                    currentResolutionIndex = i;
                }
            }
        }

        resDropdown.AddOptions(resOptions);
        resDropdown.value = currentResolutionIndex;
        resDropdown.RefreshShownValue();

        if (PlayerPrefs.HasKey("Volume")) {
            if (PlayerPrefs.GetFloat("Volume") > 0) {
                audioMixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("Volume")) * 40);
            } else {
                audioMixer.SetFloat("MasterVolume", -80f);
            }
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        }

        if (PlayerPrefs.HasKey("Resolution")) {
            Resolution resolution = resolutions[PlayerPrefs.GetInt("Resolution")];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            resDropdown.value = PlayerPrefs.GetInt("Resolution");
        }

        if (PlayerPrefs.HasKey("Fullscreen")) {
            Screen.fullScreen = PlayerPrefs.GetInt("Fullscreen") == 1 ? true : false;
            fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1 ? true : false;
        }

        if (PlayerPrefs.HasKey("Sensitivity")) {
            cineCam.m_XAxis.m_MaxSpeed = 600 * PlayerPrefs.GetFloat("Sensitivity");
            cineCam.m_YAxis.m_MaxSpeed = 4 * PlayerPrefs.GetFloat("Sensitivity");
            CameraController.sensitivity = 150f * PlayerPrefs.GetFloat("Sensitivity");
            sensSlider.value = PlayerPrefs.GetFloat("Sensitivity");
        }
    }

    public void SetVolume(float volume) {
        if (volume > 0) {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 40);
        } else {
            audioMixer.SetFloat("MasterVolume", -80f);
        }
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[FindResolutionID(resolutionIndex)];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", FindResolutionID(resolutionIndex));
    }

    public int FindResolutionID(int index) {
        for (int i = 0; i < resList.Count; i++) {
            if (resList[i] == resOptions[index]) {
                return i;
            }
        }
        return 0;
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void SetSensitivity(float sens) {
        cineCam.m_XAxis.m_MaxSpeed = 600 * sens;
        cineCam.m_YAxis.m_MaxSpeed = 4 * sens;
        CameraController.sensitivity = 150f * sens;
        PlayerPrefs.SetFloat("Sensitivity", sens);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    public Dropdown resolutionDropdown;

    public Dropdown qualityDropdown;
    	
    public Toggle fullscreenToggle;
	
    public AudioMixer masterMixer;

	private Resolution[] resolutions;

	private int currentResolution;


    void Start() {
		
        // Resolution
		resolutionDropdown.ClearOptions ();
		var resOptions = new List<Dropdown.OptionData> ();
		resolutions = Screen.resolutions;
		for (int i = 0; i < resolutions.Length; i++) {
			var option = new Dropdown.OptionData ();
			option.text = resolutions [i].width + "x" + resolutions [i].height;
			resOptions.Add (option);
			if (resolutions[i].width == Screen.width 
				&& resolutions[i].height == Screen.height) {
				currentResolution = i;
			}
		}

		resolutionDropdown.AddOptions (resOptions);
		resolutionDropdown.value = currentResolution;

        // Quality

		qualityDropdown.ClearOptions ();
		var qualOptions = new List<Dropdown.OptionData> ();
		var qualityLevel = QualitySettings.GetQualityLevel();
		string[] names = QualitySettings.names;
		int currentQuality = 0;
		for (int i = 0; i < names.Length; i++) {
			var option = new Dropdown.OptionData ();
			option.text = names [i];
			qualOptions.Add (option);
			if (i == qualityLevel) {
				currentQuality = i;
			}
		}
		qualityDropdown.AddOptions(qualOptions);
		qualityDropdown.value = currentQuality;

        // Fullscreen
		fullscreenToggle.isOn = Screen.fullScreen;
	}

	public void SetResolution(int index) {
		
		Resolution res = resolutions [index];

		if (resolutionDropdown.value != currentResolution) {
			Screen.SetResolution (res.width, res.height, Screen.fullScreen);
		}
	}
	
    public void SetFullScreen(bool fullscreen) {
		Screen.fullScreen = fullscreen;
	}

	public void SetQualityLevel(int level) {
		QualitySettings.SetQualityLevel (level);
	}

    public void LoadByIndex(int sceneIndex) {
		SceneManager.LoadScene (sceneIndex);
	}

	public void SetVolumeMaster(float soundLevel)
	{
		masterMixer.SetFloat ("masterVol", soundLevel);
	}

	public void SetVolumeMusic(float soundLevel)
	{
		masterMixer.SetFloat ("musicVol", soundLevel);
	}

	public void SetVolumeSfx(float soundLevel)
	{
		masterMixer.SetFloat ("sfxVol", soundLevel);
	}

	public void Quit() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

}

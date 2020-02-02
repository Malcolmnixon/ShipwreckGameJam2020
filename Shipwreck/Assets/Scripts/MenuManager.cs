using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;
using System.Linq;

public class MenuManager : MonoBehaviour
{

	[Header("Main Screen")]
	
	public Animator mainAnimator;
    public Dropdown resolutionDropdown;

    public Dropdown qualityDropdown;

    public Dropdown gameTypeDropdown;
    	
    public Toggle fullscreenToggle;
	
    public AudioMixer masterMixer;

	public Slider musicSlider;
	public Slider sfxSlider;

	[Header("Join Screen")]
	public Animator joinAnimator;
	
	public InputField NameInput;
	

	[Header("Wait Screen")]

	public Animator waitAnimator;

	public Text countdownText;

	public Slider countdownSlider;

	[Header("Other")]

	private WorldBuilder worldBuilder;

    public GameObject[] hideOnMobile;

	private Resolution[] resolutions;

	private int currentResolution;


    void Start() {

        #if !UNITY_STANDALONE

        foreach (GameObject item in hideOnMobile) {
            item.SetActive(false);
        }

        #endif
		
		worldBuilder = GameObject.FindObjectOfType<WorldBuilder>();

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
		SetFullScreen(PlayerPrefs.HasKey("Fullscreen"));

		// Volume
		musicSlider.value = PlayerPrefs.GetFloat("MusicVol", 0.75f);
		SetVolumeMusic(musicSlider.value);

		sfxSlider.value = PlayerPrefs.GetFloat("SfxVol", 0.75f);
		SetVolumeSfx(sfxSlider.value);
	}

	public void Update() {
		if (worldBuilder == null) 
		{
			worldBuilder = GameObject.FindObjectOfType<WorldBuilder>(); // try to find it
            if (worldBuilder == null)
            {
				Quit();
                return;
            }
		}
		else if (worldBuilder.HasWorld())
		{
			if (worldBuilder.World.State.Mode == Shipwreck.WorldData.GameMode.Waiting) {
				countdownSlider.value = 1 - (worldBuilder.World.State.RemainingTime / Shipwreck.World.GameConstants.PlayerWaitTime);
				countdownText.text = $"Loading in {worldBuilder.World.State.RemainingTime:0}s";
			} 
			else if ( waitAnimator.GetBool("Shown") && worldBuilder.World.State.Mode == Shipwreck.WorldData.GameMode.Playing) 
			{
				var menuMusic = GameObject.FindObjectOfType<MenuMusicSingleton>();
				Destroy(menuMusic.gameObject);
				SceneManager.LoadScene(2); // Load Game
			}
		}

		DiscoverGamesList();
	}

	public void DiscoverGamesList() 
	{
        var games = worldBuilder.GetGames();
		
		foreach (var option in gameTypeDropdown.options.Skip(3).ToList())
		{
			if (!games.ContainsKey(option.text)) 
			{
				gameTypeDropdown.options.Remove(option);
			}
		}

		foreach (var name in games.Keys) 
		{			
			if (gameTypeDropdown.options.All(option => option.text != name)) 
			{
				gameTypeDropdown.options.Add(new Dropdown.OptionData(name));
			}
		}
	}

	public void InitializeWorldBuilder() 
	{
		if (gameTypeDropdown.value == 0) // Web
        {
            worldBuilder.CreateWeb();
        }
		else if (gameTypeDropdown.value == 1) // Private
		{
			worldBuilder.CreatePrivate();
		}
		else if (gameTypeDropdown.value == 2) // LAN
		{
			worldBuilder.CreateLAN(Guid.NewGuid());
		}
		else //  Join LAN
		{
			var games = worldBuilder.GetGames();
			var name = gameTypeDropdown.options[gameTypeDropdown.value].text;
			if ( gameTypeDropdown.options.Any(option => option.text == name) )
			{
				worldBuilder.JoinLAN(games[name]);
			}
		}
	}

	public void NewPlayer() {
		if (NameInput.text == string.Empty)
		{
			NameInput.text = "Unnamed";
		}
		worldBuilder.NewPlayer(NameInput.text);
	}

	public void SetResolution(int index) {
		
		Resolution res = resolutions [index];

		if (resolutionDropdown.value != currentResolution) {
			Screen.SetResolution (res.width, res.height, Screen.fullScreen);
		}
	}
	
    public void SetFullScreen(bool fullscreen) {
		Screen.fullScreen = fullscreen;
		if (fullscreen) 
		{
 			PlayerPrefs.SetString("Fullscreen", "true");
		} 
		else
		{
			PlayerPrefs.DeleteKey("Fullscreen");
		}
	}

	public void SetQualityLevel(int level) {
		QualitySettings.SetQualityLevel (level);
	}

	public void SetVolumeMaster(float soundLevel)
	{
		masterMixer.SetFloat("MasterVol", Mathf.Log10(soundLevel) * 20);
	}

	public void SetVolumeMusic(float soundLevel)
	{
		masterMixer.SetFloat("MusicVol", Mathf.Log10(soundLevel) * 20);
        PlayerPrefs.SetFloat("MusicVol", soundLevel);
	}

	public void SetVolumeSfx(float soundLevel)
	{
		masterMixer.SetFloat("SfxVol", Mathf.Log10(soundLevel) * 20);
        PlayerPrefs.SetFloat("SfxVol", soundLevel);
	}

	public void ShowMainMenu() {
		mainAnimator.SetBool("Shown", true);
		joinAnimator.SetBool("Shown", false);
		waitAnimator.SetBool("Shown", false);
	}

	public void ShowJoinMenu() {
		mainAnimator.SetBool("Shown", false);
		joinAnimator.SetBool("Shown", true);
		waitAnimator.SetBool("Shown", false);
	}

	public void ShowWaitMenu() {
		mainAnimator.SetBool("Shown", false);
		joinAnimator.SetBool("Shown", false);
		waitAnimator.SetBool("Shown", true);
	}

	public void Quit() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
using System.IO;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public SaveDataList Save;
    [SerializeField] private TextAsset savefile;
    public bool firstGame;
    public bool paused = false;
    public bool isMainMenu = true;
    public Slider loadingBar;
    public TextMeshProUGUI loadingText;
    public GameObject loadingcamera;
    private Canvas PlayerUI;
    private Canvas PauseMenu;
    private Slider MusicSlider;
    private Slider SFXSlider;
    public static GameManager Instance 
    {
        get
        {
            if (_instance == null)
                Debug.Log("Game Manager is NULL");
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
        Save = JsonUtility.FromJson<SaveDataList>(savefile.text);
        SceneManager.LoadScene(1,LoadSceneMode.Additive);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetUI();
        SoundManager.Instance.masterVolume = Save.data[0].music;
        SoundManager.Instance.sfxVolume = Save.data[0].sfx;
        UpdateUI(Save.data[0].music, Save.data[0].sfx);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void NewGame()
    {
        firstGame = true;
        isMainMenu = false;
        removelisenar();
        SceneManager.UnloadSceneAsync("MainMenu");
        loadingBar.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(true);
        StartCoroutine(LoadSceneAsync("Game"));
        SoundManager.Instance.StopuiBackgroundMusic();
        SoundManager.Instance.PlayBackgroundMusic();
        Cursor.lockState = CursorLockMode.Locked;
        if (Save.data.Count == 1) Save.data.Add(new SaveData());
    }
    public void Resume()
    {
        firstGame = false;
        isMainMenu = false;
        paused = false;
        removelisenar();
        SceneManager.UnloadSceneAsync("MainMenu");
        loadingBar.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(true);
        StartCoroutine(LoadSceneAsync("Game"));
        SoundManager.Instance.StopuiBackgroundMusic();
        SoundManager.Instance.PlayBackgroundMusic();
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void MainMenu()
    {
        isMainMenu = true;
        Time.timeScale = 1.0f;
        removelisenar();
        SceneManager.UnloadSceneAsync("Game");
        loadingcamera.gameObject.SetActive(true);
        loadingBar.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(true);
        StartCoroutine(LoadSceneAsync("MainMenu"));
        SoundManager.Instance.PlayBackgroundMusic();
        SoundManager.Instance.StopuiBackgroundMusic();
        Cursor.lockState = CursorLockMode.None;
        SaveToFile();
    }
    void OnPause(InputValue value)
    {
        if (!isMainMenu)
        {
            PlayerUI.gameObject.SetActive(false);
            PauseMenu.gameObject.SetActive(true);
            paused = true;
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void unpause()
    {
        Time.timeScale = 1.0f;
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Start loading the scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);

        // Prevent the scene from activating immediately
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Update the loading bar
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            loadingBar.value = progress;

            // Update the loading text (if applicable)
            if (loadingText != null)
                loadingText.text = (progress * 100).ToString("F0") + "%";

            // Activate the scene once loading is complete
            if (operation.progress >= 0.9f)
            {
                // Wait for a second for a smoother transition (optional)
                yield return new WaitForSeconds(1f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
        loadingBar.gameObject.SetActive(false);
        loadingText.gameObject.SetActive(false);
        loadingcamera.SetActive(false);
        GetUI();
        UpdateUI(Save.data[0].music, Save.data[0].sfx);
    }
    void GetUI()
    {
        Canvas[] canvas = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Canvas c in canvas)
        {
            if (c.name == "PlayerUI")
            {
                PlayerUI = c;
            }
            else if (c.name == "PauseMenu")
            {
                PauseMenu = c;
                Slider[] sliders = c.GetComponentsInChildren<Slider>(true);
                foreach (var item in sliders)
                {
                    if (item.name == "Music slider")
                    {
                        MusicSlider = item.GetComponent<Slider>();
                    }
                    else if (item.name == "SFX slider")
                    {
                        SFXSlider = item.GetComponent<Slider>();
                    }
                }
            }
            else if (c.name == "MainMenu")
            {
                Slider[] sliders = c.GetComponentsInChildren<Slider>(true);
                foreach (var item in sliders)
                {
                    if(item.name == "Music slider")
                    {
                        MusicSlider = item.GetComponent<Slider>();
                    }
                    else if(item.name == "SFX slider")
                    {
                        SFXSlider = item.GetComponent<Slider>();
                    }
                }
                Button[] buttons = c.GetComponentsInChildren<Button>();
                foreach (var button in buttons)
                {
                    if(button.name == "Resume")
                    {
                        button.interactable = (Save.data.Count >= 2);
                    }
                }
            }
        }
        if(MusicSlider != null) {
            MusicSlider.onValueChanged.AddListener(MusicChanged);
        }
        if (SFXSlider != null)
        {
            SFXSlider.onValueChanged.AddListener(SFXChanged);
        }
    }
    void MusicChanged(float value)
    {
        Save.data[0].music = value;
        SoundManager.Instance.musicVolume = value;
        SoundManager.Instance.UpdateVolumes();
    }
    void SFXChanged(float value)
    {
        Save.data[0].sfx = value;
        SoundManager.Instance.sfxVolume = value;
        SoundManager.Instance.UpdateVolumes();
    }
    public void UpdateUI(float music, float sfx)
    {
        MusicSlider.value = music;
        SFXSlider.value = sfx;
    }
    private void removelisenar()
    {
        if (MusicSlider != null) MusicSlider.onValueChanged.RemoveListener(MusicChanged);
        if (SFXSlider != null) SFXSlider.onValueChanged.RemoveListener(SFXChanged);
    }
    private void OnDestroy()
    {
        removelisenar();
    }
    private void OnApplicationQuit()
    {
        SaveToFile();
    }
    void SaveToFile()
    {
        File.WriteAllText(Application.dataPath + "/save.json", JsonUtility.ToJson(Save));
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private Image starBar;    // Single image untuk 3 bintang
    [SerializeField] private Button button;


    private int levelID;
    private AudioManager audioManager;

    private void Awake()
    {
        // Otomatis deteksi Level ID dari nama GameObject (Level1, Level2, dst)
        string levelName = gameObject.name;
        if (levelName.StartsWith("Level"))
        {
            string levelNumber = levelName.Substring(5); // Ambil angka setelah "Level"
            if (int.TryParse(levelNumber, out int parsedID))
            {
                levelID = parsedID;
                Debug.Log($"Level ID terdeteksi: {levelID} dari {levelName}");
            }
        }



        // Pastikan Level ID valid
        if (levelID <= 0)
        {
            Debug.LogError($"Gagal mendeteksi Level ID dari {gameObject.name}. Pastikan nama GameObject adalah 'Level1', 'Level2', dst.");
            levelID = 1;
        }

        // Get components if not assigned
        if (button == null)
            button = GetComponent<Button>();

        if (lockIcon == null)
            lockIcon = transform.Find("Lock")?.gameObject;

        if (starBar == null)
            starBar = transform.Find("StarCurrent")?.GetComponent<Image>();

        // Note: do NOT modify PlayerPrefs here. Initialization (unlock defaults) is handled in the
        // SelectLevelController to avoid duplicated writes from multiple LevelButton instances.
    }

    [System.Obsolete]
    private void Start()
    {

        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager tidak ditemukan di scene!");
        }
        // Set button click listener
        if (button != null)
            button.onClick.AddListener(OnLevelButtonClick);

        LoadState();

    }

    void LoadState()
    {
        string unlockKey = $"Level{levelID}_Unlocked";
        string progressKey = $"Level{levelID}_Progress";

        // Debug info
        Debug.Log($"Checking level {levelID} - Unlock key: {unlockKey}");
        Debug.Log($"Current unlock status: {PlayerPrefs.GetInt(unlockKey, 0)}");

        // Level 1 selalu terbuka
        bool isUnlocked = (levelID == 1);

        if (!isUnlocked) // Untuk level > 1
        {
            isUnlocked = PlayerPrefs.GetInt(unlockKey, 0) == 1;
            Debug.Log($"Level {levelID} unlock status: {isUnlocked}");
        }

        // Do not write PlayerPrefs here. SelectLevelController is responsible for
        // initializing the default unlock state (level 1). This prevents duplicated
        // writes and race conditions when multiple LevelButton instances awake.

        // Ambil progress bintang
        float progress = PlayerPrefs.GetFloat(progressKey, 0f);
        Debug.Log($"Level {levelID} progress: {progress}");

        // Update UI dengan debug info
        Debug.Log($"Level {levelID} - Updating UI - Unlocked: {isUnlocked}");

        if (lockIcon != null)
        {
            lockIcon.SetActive(!isUnlocked);
            Debug.Log($"Level {levelID} - Lock Icon Active: {!isUnlocked}");
        }

        if (button != null)
        {
            button.interactable = isUnlocked;
            Debug.Log($"Level {levelID} - Button Interactable: {isUnlocked}");
        }

        if (starBar != null)
        {
            starBar.fillAmount = progress;
        }
    }

    public void OnLevelButtonClick()
    {
        if (!button.interactable) return;

        // Load level scene dengan path yang benar
        if (audioManager != null && audioManager.interactItemGameOverMenu != null)
        {
            audioManager.PlaySfx(audioManager.interactItemGameOverMenu);
        }
        StartCoroutine(FadeOutAndLoadScene(levelID));
        Debug.Log($"Loading scene: Scenes/Level {levelID}");
    }

    private IEnumerator FadeOutAndLoadScene(int sceneIndex)
    {
        // With persistent AudioManager we no longer fade out music here;
        // just load the scene so music can continue playing.
        SceneManager.LoadScene(sceneIndex);
        yield break;
    }

    // // Dipanggil ketika level selesai untuk update progress
    // public void UpdateProgress(float progress)
    // {
    //     if (starBar == null) return;

    //     string progressKey = $"Level{levelID}_Progress";
    //     float prev = PlayerPrefs.GetFloat(progressKey, 0f);

    //     // Simpan hanya jika progress baru lebih besar dari yang tersimpan
    //     float toStore = Mathf.Max(prev, progress);
    //     if (toStore > prev)
    //     {
    //         PlayerPrefs.SetFloat(progressKey, toStore);
    //         PlayerPrefs.Save();
    //         Debug.Log($"Level {levelID} progress updated: {prev} -> {toStore}");
    //     }

    //     // Pastikan UI menampilkan nilai maksimum
    //     starBar.fillAmount = Mathf.Max(starBar.fillAmount, toStore);
    // }

    // Dipanggil untuk membuka level berikutnya
    public void UnlockLevel()
    {
        PlayerPrefs.SetInt("Level" + levelID + "_Unlocked", 1);
        if (lockIcon != null) lockIcon.SetActive(false);
        if (button != null) button.interactable = true;
    }
}

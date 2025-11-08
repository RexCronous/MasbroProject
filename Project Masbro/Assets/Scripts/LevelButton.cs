using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private Image starBar;    // Single image untuk 3 bintang
    [SerializeField] private Button button;

    private int levelID;

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

        // Inisialisasi level 1
        if (levelID == 1)
        {
            Debug.Log("Membuka Level 1...");
            PlayerPrefs.DeleteKey("Level1_Unlocked"); // Hapus key lama
            PlayerPrefs.SetInt("Level1_Unlocked", 1); // Set ulang ke 1
            PlayerPrefs.Save();
        }
    }

    private void Start()
    {
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

        // Pastikan level 1 selalu terbuka
        if (levelID == 1)
        {
            PlayerPrefs.SetInt(unlockKey, 1);
            PlayerPrefs.Save();
            isUnlocked = true;
        }

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

        int tutorialDone = PlayerPrefs.GetInt("Tutorial_Done", 0);

        if (tutorialDone == 0)
        {
            // Buka panel tutorial dari scene controller
            var controller = FindFirstObjectByType<SelectLevelController>();
            if (controller != null)
            {
                controller.ShowTutorialPanel();
            }
        }
        else
        {
            SceneManager.LoadScene($"Scenes/Level {levelID}");
        }
    }

    // Dipanggil ketika level selesai untuk update progress
    public void UpdateProgress(float progress)
    {
        if (starBar != null)
        {
            starBar.fillAmount = progress;
            PlayerPrefs.SetFloat("Level" + levelID + "_Progress", progress);
        }
    }

    // Dipanggil untuk membuka level berikutnya
    public void UnlockLevel()
    {
        PlayerPrefs.SetInt("Level" + levelID + "_Unlocked", 1);
        if (lockIcon != null) lockIcon.SetActive(false);
        if (button != null) button.interactable = true;
    }
}

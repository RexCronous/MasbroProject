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
            }
        }



        // Pastikan Level ID valid
        if (levelID <= 0)
        {
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
        // Set button click listener
        if (button != null)
            button.onClick.AddListener(OnLevelButtonClick);

        LoadState();

    }

    void LoadState()
    {
        string unlockKey = $"Level{levelID}_Unlocked";
        string progressKey = $"Level{levelID}_Progress";



        // Level 1 selalu terbuka
        bool isUnlocked = (levelID == 1);

        if (!isUnlocked) // Untuk level > 1
        {
            isUnlocked = PlayerPrefs.GetInt(unlockKey, 0) == 1;
        }

        // Do not write PlayerPrefs here. SelectLevelController is responsible for
        // initializing the default unlock state (level 1). This prevents duplicated
        // writes and race conditions when multiple LevelButton instances awake.

        // Ambil progress bintang
        float progress = PlayerPrefs.GetFloat(progressKey, 0f);

        if (lockIcon != null)
        {
            lockIcon.SetActive(!isUnlocked);
        }

        if (button != null)
        {
            button.interactable = isUnlocked;
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
    }

    private IEnumerator FadeOutAndLoadScene(int sceneIndex)
    {
        // With persistent AudioManager we no longer fade out music here;
        // just load the scene so music can continue playing.
        SceneManager.LoadScene(sceneIndex);
        yield break;
    }

    // Dipanggil untuk membuka level berikutnya
    public void UnlockLevel()
    {
        PlayerPrefs.SetInt("Level" + levelID + "_Unlocked", 1);
        if (lockIcon != null) lockIcon.SetActive(false);
        if (button != null) button.interactable = true;
    }
}

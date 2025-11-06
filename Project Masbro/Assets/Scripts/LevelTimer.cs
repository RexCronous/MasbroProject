using UnityEngine;
using TMPro; 

public class LevelTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI fastestTimeText;
    private float time;
    private float fastestTime;
    
    public void FinishLevel()
    {
        time = GameManager.Instance.elapsedTime;
        fastestTime = GameManager.Instance.fastestTime;

        if (timerText != null && fastestTimeText != null)
        {
            // Hitung menit dan detik
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            int fastestMinutes = Mathf.FloorToInt(fastestTime / 60f);
            int fastestSeconds = Mathf.FloorToInt(fastestTime % 60f);

            // Format jadi mm:ss
            fastestTimeText.text = $"YOUR RECORD: {fastestMinutes:00}:{fastestSeconds:00}";
            timerText.text = $"YOUR TIME: {minutes:00}:{seconds:00}";
        }
    }
}

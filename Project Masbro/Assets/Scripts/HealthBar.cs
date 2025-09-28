using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Image totalHealhBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {

    }

    private void Update()
    {
        if (gameManager != null && currentHealthBar != null)
        {
            currentHealthBar.fillAmount = gameManager.lives / 10f;
        }
    }
}

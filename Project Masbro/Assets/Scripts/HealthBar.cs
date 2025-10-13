using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image totalHealhBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {

    }

    private void Update()
    {
        if (GameManager.Instance != null && currentHealthBar != null)
        {
            currentHealthBar.fillAmount = GameManager.Instance.lives / 10f;
        }
    }
}

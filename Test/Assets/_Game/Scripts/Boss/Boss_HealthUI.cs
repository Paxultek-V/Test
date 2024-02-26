using UnityEngine;
using UnityEngine.UI;

public class Boss_HealthUI : MonoBehaviour
{
    [SerializeField] private Image m_healthBarUIImage = null;

    private void OnEnable()
    {
        Boss.OnUpdateHealth += UpdateHealthBar;
    }

    private void OnDisable()
    {
        Boss.OnUpdateHealth -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float percent)
    {
        m_healthBarUIImage.fillAmount = percent;
    }
}

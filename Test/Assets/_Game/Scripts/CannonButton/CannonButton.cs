using System.Collections;
using UnityEngine;

public class CannonButton : MonoBehaviour
{
    public static System.Action<CannonButton> OnCannonButtonPressed;

    [SerializeField] private Cannon m_controlledCannon = null;

    [SerializeField] private SkinnedMeshRenderer m_meshRenderer = null;

    [SerializeField] private ParticleSystem m_pressedButtonFx = null;

    [SerializeField] private ParticleSystem m_disapearButtonFx = null;

    [SerializeField] private Collider m_buttonCollider = null;

    private Coroutine m_vfxCoroutine;
    public bool IsActive { get; private set; }

    public void Initialize(Boss boss)
    {
        m_controlledCannon.Initialize(boss);
    }

    public void ActivateButton()
    {
        IsActive = true;
        m_buttonCollider.enabled = IsActive;
        
        if (m_vfxCoroutine != null)
            StopCoroutine(m_vfxCoroutine);
        
        m_meshRenderer.gameObject.SetActive(true);
        m_meshRenderer.SetBlendShapeWeight(0, 0f);
    }

    public void DeactivateButton()
    {
        if (IsActive == false)
            return;

        IsActive = false;
        m_buttonCollider.enabled = IsActive;
    }

    private void PressButton()
    {
        m_pressedButtonFx.Play();
        m_meshRenderer.SetBlendShapeWeight(0, 100f);

        if (m_vfxCoroutine != null)
            StopCoroutine(m_vfxCoroutine);

        m_vfxCoroutine = StartCoroutine(PressedButtonVFXCoroutine());

        DeactivateButton();
    }

    private IEnumerator PressedButtonVFXCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        HideVisuals();
        PlayDisapearFx();
    }

    public void HideVisuals()
    {
        m_meshRenderer.gameObject.SetActive(false);
    }

    private void PlayDisapearFx()
    {
        m_disapearButtonFx.Play();
    }

    public void ShootCannonBall()
    {
        m_controlledCannon.Shoot();
        PressButton();
        OnCannonButtonPressed?.Invoke(this);
    }
}

using UnityEngine;

public class Platform : MonoBehaviour
{
    public static System.Action<Platform> OnPlatformDestroyed;
    
    [SerializeField] private ParticleSystem m_breakPlatformFx = null;

    [SerializeField] private GameObject m_visual = null;
    
    private LevelController_Runner m_levelControllerRunner;
    
    private float m_zPosThresholdBeforeKill;
    
    public void Initialize(LevelController_Runner levelControllerRunner)
    {
        m_levelControllerRunner = levelControllerRunner;
        m_zPosThresholdBeforeKill = m_levelControllerRunner.ZPosThresholdBeforeKill;
    }
    
    public void Bounce()
    {
        m_breakPlatformFx.Play();
        m_visual.SetActive(false);
    }
    
    private void Kill()
    {
        OnPlatformDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
    
    public void MovePlatform()
    {
        transform.position += Vector3.back * (m_levelControllerRunner.CurrentScrollingSpeed * Time.deltaTime);

        if (transform.position.z < m_zPosThresholdBeforeKill)
        {
            Kill();
        }
    }
}

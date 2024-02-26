using UnityEngine;

public class Boss : MonoBehaviour
{
    public static System.Action OnBossDead;
    public static System.Action OnBossKillPlayer;
    public static System.Action<float> OnUpdateHealth;
    
    [SerializeField] private Animator m_animatorController = null;
    
    [SerializeField] private float m_maxHealth = 100f;

    [SerializeField] private float m_movementSpeed = 10f;
    
    [SerializeField] private Transform m_targetPosition = null;
    
    public Transform Target;
    private float m_currentHealth;
    private bool m_isAlive;
    private bool m_canMove;
    
    private void OnEnable()
    {
        GameActions.onAfterGameModeStarted += EnableBoss;
        CannonBall.OnHitBoss += OnHitBoss;
    }

    private void OnDisable()
    {
        GameActions.onAfterGameModeStarted -= EnableBoss;
        CannonBall.OnHitBoss -= OnHitBoss;
    }

    private void Start()
    {
        m_currentHealth = m_maxHealth;
        UpdateHealthBar();
    }

    private void Update()
    {
        if(!m_isAlive)
            return;
        
        ManageMovement();
    }

    
    private void EnableBoss()
    {
        Initialize();
    }

    private void Initialize()
    {
        m_isAlive = true;
        m_canMove = true;
        
        m_animatorController.SetBool("IsMoving", m_canMove);
    }

    private void ManageMovement()
    {
        if(!m_canMove)
            return;
        
        transform.position += transform.forward * m_movementSpeed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, m_targetPosition.position, m_movementSpeed * Time.deltaTime);

        if (transform.position.z < m_targetPosition.position.z)
        {
            m_canMove = false;
            CannonBall.OnHitBoss -= OnHitBoss;
            m_animatorController.SetTrigger("Smash");
            OnBossKillPlayer?.Invoke();
        }
            
    }

    private void OnHitBoss(float damage)
    {
        m_currentHealth -= damage;

        if (m_currentHealth <= 0)
        {
            m_isAlive = false;
            CannonBall.OnHitBoss -= OnHitBoss;
            
            m_animatorController.SetTrigger("Dead");
            
            OnBossDead?.Invoke();
        }

        UpdateHealthBar();
    }


    private void UpdateHealthBar()
    {
        OnUpdateHealth?.Invoke(m_currentHealth / m_maxHealth);
        
    }
    
}

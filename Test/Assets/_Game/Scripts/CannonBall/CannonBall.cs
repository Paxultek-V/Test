using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public static System.Action<float> OnHitBoss;
    
    [SerializeField] private float m_movementSpeed = 25f;

    [SerializeField] private float m_damage = 5;
    
    [SerializeField] private GameObject m_hitBossFx = null;
    
    private Boss m_boss;
    
    private void Update()
    {
        MoveToTarget();
    }


    public void Initialize(Boss boss)
    {
        m_boss = boss;
    }

    private void MoveToTarget()
    {
        if(m_boss == null)
            return;

        transform.position =
            Vector3.MoveTowards(transform.position, m_boss.Target.position, m_movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, m_boss.Target.position) < 1f)
        {
            Instantiate(m_hitBossFx, transform.position, Quaternion.identity);
            
            OnHitBoss?.Invoke(m_damage);
            
            Destroy(gameObject);
        }
        
    }
}

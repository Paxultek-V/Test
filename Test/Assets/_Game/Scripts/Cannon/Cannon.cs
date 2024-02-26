using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private CannonBall m_cannonBallPrefab = null;

    [SerializeField] private Transform m_spawnPosition = null;

    [SerializeField] private ParticleSystem m_cannonShootFx = null;
    
    private Boss m_boss;
    private CannonBall m_instantiatedCannonBall;


    public void Initialize(Boss boss)
    {
        m_boss = boss;
    }

    public void Shoot()
    {
        if(m_boss == null)
            return;

        m_cannonShootFx.Play();
        
        m_instantiatedCannonBall = Instantiate(m_cannonBallPrefab, m_spawnPosition.position, Quaternion.identity);

        m_instantiatedCannonBall.Initialize(m_boss);
    }
    
}

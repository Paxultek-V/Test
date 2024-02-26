using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{

    private LevelScrollingController m_levelScrollingController;
    private Vector3 m_replacingPosition;
    private float m_speed;
    private float m_zPosThresholdBeforeReplacing;


    public void Initialize(LevelScrollingController levelScrollingController)
    {
        m_levelScrollingController = levelScrollingController;

        m_zPosThresholdBeforeReplacing = levelScrollingController.ZPosThresholdBeforeReplacing;
        m_replacingPosition = new Vector3(0f, 0f, levelScrollingController.ZPosForReplacing);
    }


    public void MoveFloorTile()
    {
        transform.position += Vector3.back * (m_levelScrollingController.CurrentScrollingSpeed * Time.deltaTime);

        if (transform.position.z < m_zPosThresholdBeforeReplacing)
            transform.position = m_replacingPosition;
    }
}
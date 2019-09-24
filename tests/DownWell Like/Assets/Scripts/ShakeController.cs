using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeController : MonoBehaviour
{
    private float m_shakeMagnitude = 0f;
    private float m_shakeDuration = 0f;

    public void ShakeShoot()
    {
        m_shakeMagnitude = 0.075f;
        m_shakeDuration = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Mathf.Approximately(m_shakeDuration, 0))
        {
            transform.localPosition = UnityEngine.Random.insideUnitSphere * m_shakeMagnitude;
            m_shakeDuration -= Time.deltaTime;
            if (m_shakeDuration < 0)
            {
                m_shakeDuration = 0;
            }
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugTextComponent : MonoBehaviour
{
    private DestructibleComponent m_destructibleComponent;
    private TMP_Text m_textMeshProUGUI;

    void Awake()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "DebugText")
            {
                child.gameObject.SetActive(true);
                m_textMeshProUGUI = child.GetComponent<TMP_Text>();
            }
        }

        m_destructibleComponent = GetComponent<DestructibleComponent>();
    }

    void Update()
    {
        m_textMeshProUGUI.SetText(m_destructibleComponent.health.ToString());
    }
}

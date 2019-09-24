using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(DownWellPlataformerController))]
public class DownWellPlataformerControl : MonoBehaviour
{
    private DownWellPlataformerController m_Character;
    private bool m_Jump;
    private bool m_JumpPressed;
    private bool m_JumpButtonPressed; //hack to suport UI

    private bool m_LeftButtonPressed = false;
    private bool m_RightButtonPressed = false;


    private void Awake()
    {
        m_Character = GetComponent<DownWellPlataformerController>();
    }


    private void Update()
    {
        if (!m_Jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");

            m_JumpPressed = CrossPlatformInputManager.GetButton("Jump");
        }
    }

    public void PressJump()
    {
        m_Jump = true;
        m_JumpButtonPressed = true;
    }

    public void ReleaseJump()
    {
        m_JumpButtonPressed = false;
    }

    public void PressLeft()
    {
        m_LeftButtonPressed = true;
    }

    public void ReleaseLeft()
    {
        m_LeftButtonPressed = false;
    }

    public void PressRight()
    {
        m_RightButtonPressed = true;
    }

    public void ReleaseRight()
    {
        m_RightButtonPressed = false;
    }

    private void FixedUpdate()
    {
        // Read the inputs.
        float horizontalAxis = CrossPlatformInputManager.GetAxis("Horizontal");

        if (m_LeftButtonPressed)
        {
            horizontalAxis = -1;
        }
        else if (m_RightButtonPressed)
        {
            horizontalAxis = 1;
        }

        // Pass all parameters to the character control script.
        m_Character.Move(horizontalAxis, m_Jump, m_JumpPressed || m_JumpButtonPressed);
        m_Jump = false;
    }


}



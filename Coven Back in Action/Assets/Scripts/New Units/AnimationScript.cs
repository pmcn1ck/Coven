using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Units;

public class AnimationScript : MonoBehaviour
{
    public Animator m_Animator;

    private void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
    }
    public void runAttackAnim()
    {
        m_Animator.SetTrigger("isAttacking");
    }

    public void runDamageAnim()
    {
        m_Animator.SetTrigger("isDamaged");
    }

    public void runDeathAnim()
    {
        m_Animator.SetBool("isDead", true);
    }

    public void toggleMoveAnim()
    {
        m_Animator.SetBool("moveAnim", !m_Animator.GetBool("moveAnim"));
    }

    public void runAttackAnimTwo()
    {
        m_Animator.SetTrigger("isAttacking2");
    }

    public void runCastAnim()
    {
        m_Animator.SetTrigger("isCasting");
    }
    public void toggleIdleTwo()
    {
        if(m_Animator.GetBool("idle2") == false)
        {
            m_Animator.SetTrigger("idle2Start");
        }
        m_Animator.SetBool("idle2", !m_Animator.GetBool("idle2"));
    }

}

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
        m_Animator.SetTrigger("isDead");
    }

    public void toggleMoveAnim()
    {
        m_Animator.SetBool("moveAnim", !m_Animator.GetBool("moveAnim"));
    }



}

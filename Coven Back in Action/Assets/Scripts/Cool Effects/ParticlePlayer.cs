using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    public ParticleSystem attack;
    public ParticleSystem takeDamage;
    public ParticleSystem heal;
    public List<ParticleSystem> extraParticles;

    public void CallAttackParticle()
    {
        attack.Play();
    }

    public void CallDamageParticle()
    {
        takeDamage.Play();
    }

    public void CallHeal()
    {
        heal.Play();
    }

    public void CallExtraParticle(int x)
    {
        extraParticles[x].Play();
    }

    public void EndExtraParticle(int x)
    {
        extraParticles[x].Stop();
    }

}

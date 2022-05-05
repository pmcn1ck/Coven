using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class cOption : MonoBehaviour
{

    public Slider musicSlider;

    void OnStart()
    {
        InitUI();
    }

    public void InitUI()
    {
        musicSlider.value = GameManager.gm.musicVol;
    }


    public void OnMusicVolChanged()
    {
        GameManager.gm.ChangeMusicVolume(musicSlider.value);
    }


    public void OnClickBack()
    {
        GameManager.gm.audioManager.PlaySound(eSound.click);
        Destroy(this.gameObject);
    }

}

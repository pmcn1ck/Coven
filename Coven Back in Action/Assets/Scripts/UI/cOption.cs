using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class cOption : MonoBehaviour
{
    public Slider masterVol;
    public Slider musicSlider;
    public Slider sfxVol;

    private void Start()
    {
        InitUI();
    }

    void OnStart()
    {
        InitUI();
    }

    public void InitUI()
    {
        musicSlider.value = GameManager.gm.musicVol;
    }
    public void OnMasterVolChanged()
    {
        GameManager.gm.ChangeMasterVolume(masterVol.value);
    }
    public void OnSfxVolChanged()
    {
        GameManager.gm.ChangeSoundVolume(sfxVol.value);
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

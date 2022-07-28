using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wTurnBanner : MonoBehaviour
{
    public Text tBanner;
    public Shadow tShadow;

    public void InitUI(string _banner, Color _cBanner)
    {
        tBanner.text = _banner;
        tBanner.color = tShadow.effectColor = _cBanner;
        //gameObject.GetComponent<Image>().color = _cBanner;
        Destroy(gameObject, 4f);
    }

}

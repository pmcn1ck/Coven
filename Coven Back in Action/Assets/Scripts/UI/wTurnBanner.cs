using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wTurnBanner : MonoBehaviour
{
    public Text tBanner;

    public void InitUI(string _banner, Color _cBanner)
    {
        tBanner.text = _banner;
        gameObject.GetComponent<Image>().color = _cBanner;
        Destroy(gameObject, 4f);
    }

}

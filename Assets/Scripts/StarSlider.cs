using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarSlider : MonoBehaviour {
    float threeStarLength, lengthDrawed = 0;
    public void setThreeStarLength(float tsl)
    {
        threeStarLength = tsl;
    }
	public void Drawed(float length)
    {
        lengthDrawed += length;
        if(GetComponent<Image>().fillAmount>0.66&& (threeStarLength * 3 - lengthDrawed) / (threeStarLength * 3) <= 0.66)
        {
            transform.GetChild(2).GetChild(0).GetComponent<Image>().DOFade(0,0.3f);
        }
        if (GetComponent<Image>().fillAmount > 0.33 && (threeStarLength * 3 - lengthDrawed) / (threeStarLength * 3) <= 0.33)
        {
            transform.GetChild(1).GetChild(0).GetComponent<Image>().DOFade(0, 0.3f);
        }
        if (GetComponent<Image>().fillAmount > 0 && (threeStarLength * 3 - lengthDrawed) / (threeStarLength * 3) <= 0)
        {
            transform.GetChild(0).GetChild(0).GetComponent<Image>().DOFade(0, 0.3f);
        }
        GetComponent<Image>().fillAmount = (threeStarLength * 3 - lengthDrawed) / (threeStarLength * 3);
        
    }
}

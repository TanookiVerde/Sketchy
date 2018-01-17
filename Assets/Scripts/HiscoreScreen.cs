using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiscoreScreen : MonoBehaviour {

    [SerializeField] private Text
        currentScore, 
        hiScore;

    string format = "00000";

    public void setCurrentScore(int value)
    {
        currentScore.text = value.ToString(format);
    }

    public void setHiscore(int value)
    {
        int hiscore;
        if (PlayerPrefs.HasKey("Hiscore")) {
            hiscore = PlayerPrefs.GetInt("Hiscore");
            if (hiscore < value) hiscore = value;
        } else {
            hiscore = value;
        }
        PlayerPrefs.SetInt("Hiscore", hiscore);
        hiScore.text = hiscore.ToString(format);
    }
}

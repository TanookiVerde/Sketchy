using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour {
    Text board;
    string format = "00000";
    int value;

	void Start () {
        value = 0;
        board = GetComponent<Text>();
        board.text = value.ToString(format);
	}

    public void AddScore(float similarity, float bonus) {
        /*
         * Roda a equação de montar score
         */
        value += (int)(similarity * (.5f + (bonus * 2)));
        board.text = value.ToString(format);
    }

    public int GetScore() { return value; }
}

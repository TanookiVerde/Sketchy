﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {
	void Update () {
        if (Input.GetKey(KeyCode.Space)) {
            SceneManager.LoadScene(1);
            enabled = false;
        }
	}
}

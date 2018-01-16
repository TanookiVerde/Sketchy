using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour {
    const int MAX_VALUE = 100000;

    float value;
    Coroutine decreaseRoutine;
    Image _image;

    public float getValue(){ return value / MAX_VALUE; }

    public void StartTimer(float decreaseRatio) {
        _image = GetComponent<Image>();
        _image.fillAmount = 1;
        value = MAX_VALUE;
        if (decreaseRoutine != null) StopCoroutine(decreaseRoutine);
        decreaseRoutine = StartCoroutine(decreaseValue(decreaseRatio));
    }

    public void StopTimer() {
        if (decreaseRoutine != null) StopCoroutine(decreaseRoutine);
    }

    IEnumerator decreaseValue(float decreaseRatio) {
        while (value > 0) {
            yield return null;
            value -= decreaseRatio;
            _image.fillAmount = (value / (float)MAX_VALUE);
        }
        value = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {
    [SerializeField] private Renderer paintedImage;
    [SerializeField] private SpriteRenderer originalSprite;

    Texture2D paintedTex;
    StageState stageState;

    void Update () {
        switch (stageState) {
            case StageState.Playing:
                if (Input.GetKeyDown("space")) {
                    stageState = StageState.Analysis;

                    Sprite _sprite = originalSprite.sprite;

                    paintedTex = paintedImage.sharedMaterial.mainTexture as Texture2D;
                    float similarityValue = ImageComparer.CheckSimilarity(paintedTex, _sprite.texture);
                    Debug.Log("similarityValue: " + (similarityValue * 100).ToString("000") + "%");
                }
            break;

            default:
            break;
        }

	}

    private enum StageState { Playing, Analysis, Results, Transition  }
}

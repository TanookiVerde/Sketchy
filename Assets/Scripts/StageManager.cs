using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour {
    [SerializeField] private Renderer paintedImage;
    [SerializeField] private SpriteRenderer originalSprite;

    [SerializeField] private Text resemblanceValue;

    StageState stageState;

    void Update () {
        switch (stageState) {
            case StageState.Playing:
                if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(2)) {
                    Sprite _sprite = originalSprite.sprite;

                    Texture2D drawingTex = paintedImage.sharedMaterial.mainTexture as Texture2D;
                    float similarityValue = ImageComparator.CheckSimilarity(drawingTex, _sprite.texture);

                    int similarityPercent = ProcessSimilarityValue(similarityValue);
                    if(resemblanceValue) resemblanceValue.text = similarityPercent + "%";

                    //stageState = StageState.Analysis;
                }

                /*
                * Input de test pra ver como o jogo está gerando o HandicapMap
                */
                if(Input.GetKeyDown("y")) {
                    Texture2D _aux = paintedImage.sharedMaterial.mainTexture as Texture2D;
                    _aux = ImageComparator.CreateTextureMap(originalSprite.sprite.texture, Color.black);
                    paintedImage.material.mainTexture = _aux;
                    _aux.Apply();
                }

            break;

            default:
            break;
        }

	}

    private int ProcessSimilarityValue(float value) {
        int result = (int)(value * 100);

        /*
        * Valor tirado do testômetro mesmo 
        * Se o valor do "diameter" em CreateTextureMap() de ImageComparer for alterado, precisa mudar aqui
        */
        result *= 4; 

        if (result < 0)   result = 0;
        if (result > 100) result = 100;
        return result;
    }

    private enum StageState { Playing, Analysis, Results, Transition  }
}

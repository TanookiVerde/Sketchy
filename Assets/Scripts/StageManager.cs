using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private Text resemblanceValue;
    [SerializeField] private Text stageTimer;
    [SerializeField] private TimerBar bonusBar;
    [SerializeField] private Scoreboard scoreboard;

    [SerializeField] private Image introScreen;
    [SerializeField] private Text introText;
    [SerializeField] private Text start;
    [SerializeField] private Text timeOut;

    [Header("Paint references")]
    [SerializeField] private PaintManager paintManager;
    [SerializeField] private Renderer paintedImage;
    [SerializeField] private SpriteRenderer originalSprite;
    [SerializeField] private SpriteRenderer overlapImage;

    [Header("Images")]
    [SerializeField] private List<Sprite> originalImages;
    private Sprite currentImage;

    private enum StageState {
        Intro, //antes de apertar F1
        Playing, //gameplay de sessões
        Transition, //transição entre sessões
        Results //pós-timeout
    }
    StageState stageState;

    //melhorar esse nomes porque deus pai
    public float bonusBarDecreaseRatio = 1;
    public int startingStageTimeValue = 30;

    [Header("Animation Preferences")]
    [SerializeField] private float introFadeSpeed;
    [SerializeField] private float startTimeOutAnimationSpeed = 0.025f;
    [SerializeField] private float maxSize = 1.5f;

    void Start() {
        stageState = StageState.Intro;
        introScreen.gameObject.SetActive(true);
    }

    void Update () {
        switch (stageState) {
            case StageState.Intro:
                if (Input.GetKeyDown(KeyCode.Space)) {
                    StartCoroutine(SetStageTimer(startingStageTimeValue));
                    SetSession();
                    StartCoroutine( IntroScreenAnimation() );
                }
            break;

            case StageState.Playing:
                /*
                * Input de teste, aperte Y para ver como o jogo está gerando o HandicapMap (RegionMap) (ProximityMap) (THICCmap)
                */
                if (Input.GetKeyDown(KeyCode.Y)) {
                    Texture2D _aux = paintedImage.sharedMaterial.mainTexture as Texture2D;
                    _aux = ImageComparator.CreateTextureMap(originalSprite.sprite.texture, Color.black);
                    paintedImage.material.mainTexture = _aux;
                    _aux.Apply();
                }


                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(2)) {
                    Sprite _sprite = originalSprite.sprite;
                    Texture2D drawingTex = paintedImage.sharedMaterial.mainTexture as Texture2D;
                    float similarityValue = ImageComparator.CheckSimilarity(drawingTex, _sprite.texture);

                    //Gera e apresenta a porcentagem de semelhança
                    int similarityPercent = ProcessSimilarityValue(similarityValue);
                    if(resemblanceValue) resemblanceValue.text = similarityPercent + "%";

                    //Adiciona pontiação ao score da fase
                    scoreboard.AddScore(similarityPercent / 2, bonusBar.getValue());

                    //Pisca imagem de overlap e resetta session
                    StartCoroutine(SessionTransition(1.2f));

                    stageState = StageState.Transition;
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

    IEnumerator SessionTransition(float time) {
        bonusBar.StopTimer();

        if (overlapImage) {
            overlapImage.sprite = currentImage;
            overlapImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(time);
            overlapImage.gameObject.SetActive(false);
        }

        SetSession();
    }

    private void SetSession() {
        /*
         * Pega uma outra imagem da lista e setta como o sprite do "originalImage"
         */
        Sprite aux;
        aux = currentImage;
        int _rng = Random.Range(0, originalImages.Count);
        currentImage = originalImages[_rng];
        originalImages.Remove(currentImage);
        if (aux) originalImages.Add(aux);
        originalSprite.sprite = currentImage;

        if (paintManager) paintManager.ClearTexture(Color.white);

        if (bonusBar) bonusBar.StartTimer(bonusBarDecreaseRatio);

        stageState = StageState.Playing;
    }

    IEnumerator SetStageTimer(int startValue){
        string format = "000";
        int value = startValue;
        if (stageTimer) {
            stageTimer.text = value.ToString(format);
            while (value > 0) {
                yield return new WaitForSecondsRealtime(1);
                stageTimer.text = (--value).ToString(format);
            }

            SetGameplay(false);
            StartCoroutine( DecreaseScaleAnimation(timeOut,2f) );
            bonusBar.StopTimer();
            stageState = StageState.Results;
        }
    }

    IEnumerator IntroScreenAnimation(){
        while(introScreen.color.a != 0){
            float currentValue = Mathf.MoveTowards( introScreen.color.a,0,introFadeSpeed);
            
            introScreen.color = new Color(introScreen.color.r,
                                          introScreen.color.g,
                                          introScreen.color.b,
                                          currentValue);
            introText.color = new Color (introText.color.r,
                                         introText.color.g,
                                         introText.color.b,
                                         currentValue);
            yield return new WaitForEndOfFrame();
        }
        introScreen.gameObject.SetActive(false);
        yield return IncreaseScaleAnimation(start);
    }
    IEnumerator IncreaseScaleAnimation(Text img, float stopTime = 0){
        float initialScale = img.transform.localScale.x;
        img.gameObject.SetActive(true);
        while(img.transform.localScale.x != maxSize){
            float currentValue = Mathf.MoveTowards(img.transform.localScale.x,maxSize,startTimeOutAnimationSpeed);
            img.transform.localScale = new Vector3(currentValue,currentValue,1);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(stopTime);
        while(img.color.a != 0){
            float currentValue = Mathf.MoveTowards( img.color.a,0,introFadeSpeed);
            img.color = new Color(img.color.r, img.color.g, img.color.b, currentValue);
            yield return new WaitForEndOfFrame();   
        }
        img.gameObject.SetActive(false);
        img.transform.localScale = new Vector3(initialScale,initialScale,1);
    }
    IEnumerator DecreaseScaleAnimation(Text img, float stopTime = 0){
        img.gameObject.SetActive(true);
        img.transform.localScale = new Vector3(maxSize,maxSize,1);
        while(img.transform.localScale.x != 1){
            float currentValue = Mathf.MoveTowards(img.transform.localScale.x,1,startTimeOutAnimationSpeed);
            img.transform.localScale = new Vector3(currentValue,currentValue,1);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(stopTime);
        while(img.color.a != 0){
            float currentValue = Mathf.MoveTowards( img.color.a,0,introFadeSpeed);
            img.color = new Color(img.color.r, img.color.g, img.color.b, currentValue);
            yield return new WaitForEndOfFrame();   
        }
        img.gameObject.SetActive(false);
        img.transform.localScale = new Vector3(maxSize,maxSize,1);
    }
    //solução temporária, organizar melhor depois
    private void SetGameplay(bool value) {
        paintManager.enabled = value;
    }
}

 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class GameManagerr : MonoBehaviour
{
    class AnswerButton
    {
        public Button button;
        public TextMeshProUGUI text;
        public Animator animator;
        public CanvasGroup cg;
    }

    public Queue<Question> questionQ = new Queue<Question>();
    private Question currentQuestion;
  

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float timeNewQuestion = 1f;

    public static float cameraWidth = 24.5f;

    Transform canvas;
    Transform pQ;
    Transform panelCorrect;
    Transform panelWrong;
    TextMeshProUGUI textPanelWrongCorrAns;
    Transform panelQuestion;
    TextMeshProUGUI textQuestion;
    TextMeshProUGUI textQuestionBanner;
    Transform panelAnswersTF;
    Transform panelAnswersMult;
    Transform panelAnswerWrite;
    Transform playerHealthT;
    Button panelAnswerWriteOK;
    TMP_InputField inputField;
    Transform panelPause;
    Slider sliderVolBgm;
    Slider sliderVolSfx;
    Transform panelGameOver;
    Transform starDest;
    TextMeshProUGUI textScore;
    Transform btnNextLevel;
    Transform panelLevelClear;
    Transform panelTimesUp;
    Animator panelTimesUpAnimator;
    TextMeshProUGUI panelTimesUpTextCorrectAns;
    Transform coinDest;
    TextMeshProUGUI textCoin;
    Transform pauseButtonT;
    Transform victoryPanelT;
    TextMeshProUGUI textVictoryPanelScore;
    TextMeshProUGUI textDefeatPanelScore;
    Transform confetti;
    Transform trophyNotifT;
    Transform trophyNotifTemplate;
    Image imgQuestion;
    Animator animatorBtnQImg;
    Transform playerHealthTxtT;

    Transform introTextPanel;
    TextMeshProUGUI introText;

    Animator animatorTransition;
    Animator animatorQuestion;

    Transform worldT;
    Animator animatorPlayer;
    Animator currEnemyAnimator;

    Light2D globalLight;

    float trophyNotifDuration = 2f;

    float cameraZoomSpeed = 3.5f;
    float cameraZoomScale = 0.9f;
    float cameraZoomDur = 1.15f;
    float onKillCameraZoomSpeed = 3.5f;
    float onKillCameraZoomScale = 0.75f;
    float onKillCameraZoomDur = 1.15f;

    float gameStartDelay = 2.2f;
    float playerAttackTriggerTime = 0.2333f;
    float enemyAttackTriggerTime = 0.08333f;

    float onAttackCamYPosModif = -2.0f;
    float onPlayerAttackCamXPosModif = 2f;
    float onEnemyAttackCamXPosModif = -2f;

    float durDeathDissolve = 0.75f; //Duration of the death effect (fade out effect)
    float onKillSlowMoDur = 0.5f;
    float onKillSlowMoTimeScale = 0.4f;
    float onKillSlowMoRecovSpeed = 4f;

    float starFlyAwayTime = 0.5f;
    float starFlySpeed = 4.5f;
    int currLevelSlayCount;

    int currPlayerHealth;
    int tryPerQ = 2;
    int currTries;
    int correctAns;
    int totalTries;
    int currScore;
    int currProjectedScore;
    int currProjectCoins;
    int currCoins;
    int currStreak;

    int questionsPerLevel = 10;
    float timePerQuestion = 10;
    float bonusTimeImageQuestion = 5;
    float currTime;
    bool isTimeCountingDown;
    int currTimeOnClock;

    readonly bool willShowAnswers = true;

    float gameTime;
    
    public static float sfxVol; 
	public static float bgmVol;

    Transform playerT;
    Transform playerSpriteT;
    Vector3 playerDefaultPos;

    List<GameObject> enemyPrefabs;
    List<GameObject> bossPrefabs;
    Vector2 enemyPos = new Vector2(18.5f, -8.5f);
    GameObject objEnemy;
    int currHealthEnemy;

    int currQuestionNum;
    int currLevel;

    public static float fadeInDuration = 0.75f;
    float questionEnterAnimDuration = 1.9f;
    float buttonEnterAnimInterval = 0.15f;

    List<AnswerButton> currQuestAnsBtnList;

    GameObject attackHitParticle;
    GameObject prefabStar;
    GameObject prefabCoin;
    Material onAttackedMat;

    AudioSource asBgm;

    List<AnswerButton> answerButtonsTF = new List<AnswerButton>();
    List<AnswerButton> answerButtonsMult = new List<AnswerButton>();

    Coroutine corShakeScreen;

    Transform clockT;
    TextMeshProUGUI textClock;
    Transform textClockBG;
    Animator animatorClock;
    Material matDefault;

    List<AudioClip> stageBGMs;
    Transform backgroundContainer;

    float playerMoveSpeed = 8.5f;
    float enemyBaseXPos = 18.77f;
    float playerBaseXPos = -19.39f;
    float enemyXPosGap = 20f;
    int currEnemySummCount;

    //events:
    public delegate void VoidEvent();
    public delegate void OnGetFloatEvent(float f);
    public delegate void OnGetIntEvent(int i);
    public static VoidEvent OnCompleteGame;
    public static VoidEvent OnCompletePerfectGame;
    public static OnGetFloatEvent OnCompleteGameGetTime;
    public static OnGetIntEvent OnStreak;
    public static VoidEvent OnSlayEnemy;

    public static GameManagerr gm;

    bool isPaused;
    
    AudioClip soundHit;
    AudioClip soundCorrect;
    AudioClip soundWrong;
    AudioClip soundGameover;
    AudioClip soundExplo;
    AudioClip soundCoin;
    AudioClip bgmForest;
    AudioClip bgmMountain;
    AudioClip bgmGraveyard;
    AudioClip soundTrophy;
    AudioClip soundLevelComplete;
    
    Animator animatorPanelAnsWrite;
    Animator animatorScreenFlash;
    float screenFlashDur = 2.01666f;

    GameObject prefabHeart;
    GameObject prefabHeartHalf;
    GameObject prefabSfx;

    Transform heartContainer;

    GameObject prefabExplosion;
    GameObject prefabFireHit;

    Animator animatorTutorialPanel2;
    Animator animatorTutorialRobot2;
    Image panelTutorial2Image;

    MainMenu.OnVoidEvent OnClickTutorialImage;

    bool isPlayingTutorial2;

    void Awake()
    {
    	prefabSfx = Resources.Load<GameObject>("Sound");
    	soundHit = Resources.Load<AudioClip>("Sounds/SLASH");
       	soundCorrect = Resources.Load<AudioClip>("Sounds/CORRECT3");
    	soundWrong = Resources.Load<AudioClip>("Sounds/WRONG3");
    	soundGameover = Resources.Load<AudioClip>("Sounds/GAMEOVER");
        soundExplo = Resources.Load<AudioClip>("Sounds/EXPLO");
        soundCoin = Resources.Load<AudioClip>("Sounds/COINCOLLECT");
        soundLevelComplete = Resources.Load<AudioClip>("Sounds/LEVEL-COMPLETE");
        bgmForest = Resources.Load<AudioClip>("Sounds/1ST-LEVEL");
        bgmMountain = Resources.Load<AudioClip>("Sounds/MOUNTAIN");
        bgmGraveyard = Resources.Load<AudioClip>("Sounds/GRAVEYARD");
        soundTrophy = Resources.Load<AudioClip>("Sounds/TROPHY");

        prefabHeart = Resources.Load<GameObject>("Hearts/HeartFull");
        prefabHeartHalf = Resources.Load<GameObject>("Hearts/HeartHalf");
        prefabExplosion = Resources.Load<GameObject>("Particles/Explosion/explosion");
        prefabFireHit = Resources.Load<GameObject>("Particles/FireHit/PartFire");
    	
        gm = this;

        stageBGMs = new List<AudioClip>()
        {
            bgmForest,
            bgmMountain,
            bgmGraveyard
        };

        attackHitParticle = Resources.Load<GameObject>("Particles/AttackHit");
        onAttackedMat = Resources.Load<Material>("SpriteFlash");
        prefabStar = Resources.Load<GameObject>("Star");
        prefabCoin = Resources.Load<GameObject>("Coin");
        matDefault = Resources.Load<Material>("Materials/MaterialDefault");

        enemyPrefabs = new List<GameObject>() {
            Resources.Load<GameObject>("Enemies/enemy003"),
            Resources.Load<GameObject>("Enemies/enemy004"),
            Resources.Load<GameObject>("Enemies/enemy001"),
            Resources.Load<GameObject>("Enemies/enemy002"),
            Resources.Load<GameObject>("Enemies/enemy009"),
            Resources.Load<GameObject>("Enemies/enemy008")

        };
        
        bossPrefabs = new List<GameObject>() {
        	Resources.Load<GameObject>("Enemies/enemy006"),
        	Resources.Load<GameObject>("Enemies/enemy007"),
        	Resources.Load<GameObject>("Enemies/enemy005")
        };

        canvas = GameObject.Find("CanvasGame").transform;
        pQ = canvas.Find("QuestionPanel");
        panelQuestion = pQ.Find("Question");
        animatorQuestion = panelQuestion.GetComponent<Animator>();
        textQuestion = panelQuestion.Find("Text").GetComponent<TextMeshProUGUI>();
        textQuestionBanner = panelQuestion.Find("Banner").Find("Title").GetComponent<TextMeshProUGUI>();
        panelAnswersTF = pQ.Find("AnswerButtonTF");
        panelAnswersMult = pQ.Find("AnswerButtonMult");
        panelCorrect = canvas.Find("CorrectPanel");
        panelWrong = canvas.Find("WrongPanel");
        textPanelWrongCorrAns = panelWrong.transform.Find("TextCorrectAns").GetComponent<TextMeshProUGUI>();
        panelAnswerWrite = pQ.Find("AnswerWrite");
        animatorPanelAnsWrite = panelAnswerWrite.GetComponent<Animator>();
        animatorScreenFlash = canvas.Find("WhiteFlash").GetComponent<Animator>();
        playerHealthT = canvas.Find("Hearts").Find("PlayerHealth");
        playerHealthTxtT = canvas.Find("Hearts").Find("PlayerHealthTxtT");
        inputField = panelAnswerWrite.Find("InputField").GetComponent<TMP_InputField>();
        panelAnswerWriteOK = panelAnswerWrite.Find("Button").GetComponent<Button>();
        panelGameOver = canvas.Find("GameOverPanel");
        textDefeatPanelScore = panelGameOver.Find("TextScore").GetComponent<TextMeshProUGUI>();
        textScore = canvas.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        btnNextLevel = canvas.Find("BtnNextLevel");
        panelLevelClear = canvas.Find("LevelClearPanel");
        animatorTransition = canvas.Find("Transition").GetComponent<Animator>();
        clockT = canvas.Find("Clock");
        animatorClock = clockT.GetComponent<Animator>();
        textClockBG = clockT.Find("BG");
        textClock = clockT.Find("Text").GetComponent<TextMeshProUGUI>();
        panelTimesUp = canvas.Find("TimesUpPanel");
        panelTimesUpAnimator = panelTimesUp.GetComponent<Animator>();
        panelTimesUpTextCorrectAns = panelTimesUp.Find("TextCorrectAns").GetComponent<TextMeshProUGUI>();
        textCoin = canvas.Find("CoinText").GetComponent<TextMeshProUGUI>();
        victoryPanelT = canvas.Find("VictoryPanel");
        textVictoryPanelScore = victoryPanelT.Find("TextScore").GetComponent<TextMeshProUGUI>();
        pauseButtonT = canvas.Find("PauseButton");
        trophyNotifT = canvas.Find("TrophyNotif");
        trophyNotifTemplate = trophyNotifT.Find("Template");
        imgQuestion = canvas.Find("QuestionImage").Find("Image").GetComponent<Image>();
        animatorBtnQImg = panelQuestion.Find("ButtonImage").GetComponent<Animator>();
        heartContainer = canvas.Find("Hearts").Find("PlayerHealth");
        animatorTutorialPanel2 = canvas.Find("TutorialPanel").GetComponent<Animator>();
        animatorTutorialRobot2 = animatorTutorialPanel2.transform.Find("Robot").GetComponent<Animator>();
        panelTutorial2Image = animatorTutorialPanel2.transform.Find("Image").GetComponent<Image>();

        Button btnCompNextLevel = btnNextLevel.Find("Btn").GetComponent<Button>();
        btnCompNextLevel.onClick.RemoveAllListeners();
        btnCompNextLevel.onClick.AddListener(GoToNextLevel);

        panelPause = canvas.Find("PauseMenu");
        sliderVolBgm = panelPause.Find("Volume").Find("Music").Find("Slider").GetComponent<Slider>();
        sliderVolSfx = panelPause.Find("Volume").Find("Sfx").Find("Slider").GetComponent<Slider>();

        sliderVolBgm.onValueChanged.AddListener((float val) => { SetAudioSourceVol(asBgm, val); bgmVol = val; PlayerData.data.volBGM = val; });
        sliderVolSfx.onValueChanged.AddListener((float val) => { sfxVol = val; PlayerData.data.volSFX = val; });

        introTextPanel = canvas.Find("IntroText");
        introText = introTextPanel.Find("Text").GetComponent<TextMeshProUGUI>();

        worldT = GameObject.Find("World").transform;
        coinDest = worldT.Find("CoinDestination");
        starDest = worldT.Find("StarDestination");
        SpawnPlayerCharacter();
        playerSpriteT = playerT.Find("Sprite");
        animatorPlayer = playerT.GetComponent<Animator>();
        asBgm = canvas.Find("Background Music").GetComponent<AudioSource>();

        backgroundContainer = worldT.Find("Background");
        confetti = worldT.Find("Confetti");

        playerDefaultPos = playerSpriteT.transform.localPosition;
        Debug.Log(playerDefaultPos);

        globalLight = worldT.Find("Global Light 2D").GetComponent<Light2D>();

        foreach (Transform t in panelAnswersTF)
        {
            if (t.Find("Btn").TryGetComponent(out Button btn))
            {
                AnswerButton ab = new AnswerButton();
                ab.button = btn;
                ab.text = t.Find("Text").GetComponent<TextMeshProUGUI>();
                ab.animator = t.GetComponent<Animator>();
                ab.cg = t.GetComponent<CanvasGroup>();
                answerButtonsTF.Add(ab);
            }
        }

        foreach (Transform t in panelAnswersMult)
        {
            if (t.Find("Btn").TryGetComponent(out Button btn))
            {
                AnswerButton ab = new AnswerButton();
                ab.button = btn;
                ab.text = t.Find("Text").GetComponent<TextMeshProUGUI>();
                ab.animator = t.GetComponent<Animator>();
                ab.cg = t.GetComponent<CanvasGroup>();
                answerButtonsMult.Add(ab);
            }
        }

        MainMenu.LoadPlayerData();
    }

    public static void SetAudioSourceVol(AudioSource audS, float vol)
    {
        audS.volume = vol;
    }

    void Start()
    {
    	sliderVolBgm.value = PlayerData.data.volBGM;
		sliderVolSfx.value = PlayerData.data.volSFX;
		asBgm.loop = true;
		asBgm.volume = PlayerData.data.volBGM;
    	
        animatorTransition.Play("FadeOut", 0, 0f);
        MainMenu.InitializeQuestionDictionary();
        Camera.main.orthographicSize = cameraWidth / Camera.main.aspect;
        SetPlayerHealth(3);
        currLevel = 1;
        LoadLevel(currLevel);
        if (!PlayerData.data.hasFinishedTutorial2) PlayTutorial2();
        else StartGame();
    }

    void Update()
    {
        if (isPlayingTutorial2 && Input.GetMouseButtonUp(0))
        {
            OnClickTutorialImage?.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.Space)) AcquireTrophy(new Trophy000());

        gameTime += Time.deltaTime;
        PlayerData.data.playTime += Time.deltaTime;

        if (isTimeCountingDown)
        {
            currTime -= Time.deltaTime;
            int currTimeInt = (int)currTime;

            if (currTimeInt < currTimeOnClock)
            {
                currTimeOnClock = currTimeInt;

                string timeStr = "";
                if (currTimeInt <= 3) timeStr = "<color=red><size=+8>" + currTimeOnClock + "</size></color>";
                else timeStr = currTimeOnClock.ToString();

                textClock.SetText(timeStr);
                animatorClock.Play("Shake", 0, 0f);

                if (currTime <= 0f)
                {
                    clockT.gameObject.SetActive(false);
                    OnTimeOut();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            MainMenu.SaveData();
        }
    }

    void SpawnPlayerCharacter()
    {
        playerT = Instantiate(PlayerData.data.equippedChar.prefab, worldT).transform;
        Vector3 pos = playerT.position;
        pos.x = playerBaseXPos;
        playerT.position = pos;

        //apply weapon augment:
        if (PlayerData.data.equippedAugment != null)
        {          
            Instantiate(PlayerData.data.equippedAugment.particlesPrefab, playerT.Find("Sprite").Find("Weapon"));
            Material mat = Instantiate(matDefault);
            Debug.Log(mat == null);
            playerT.Find("Sprite").GetComponent<SpriteRenderer>().material = mat;
            mat.SetColor("_EmissionColor", PlayerData.data.equippedAugment.emissionColor);
            mat.SetFloat("_NoiseSpeed", PlayerData.data.equippedAugment.emissionNoiseSpeed);
            mat.SetFloat("_NoiseScale", PlayerData.data.equippedAugment.emissionNoiseScale);
        }

        coinDest.parent = playerT;
        starDest.parent = playerT;
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            panelPause.gameObject.SetActive(true);
            textQuestion.gameObject.SetActive(false);

            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            panelPause.gameObject.SetActive(false);
            textQuestion.gameObject.SetActive(true);

            Time.timeScale = 1f;
            MainMenu.SaveData();
            isPaused = false;
        }
    }

    void SetPlayerHealth(int amount)
    {
        foreach(Transform heartT in heartContainer)
        {
            heartT.gameObject.SetActive(false);
            Destroy(heartT.gameObject);
        }

        currPlayerHealth = amount;
        for (int i = 0; i < amount; i++) Instantiate(prefabHeart, heartContainer);       
    }

    void DimBackground(float duration)
    {
        IEnumerator Dim()
        {
            float origInten = globalLight.intensity;
            float elapsedTime = 0f;

            while (true)
            {
                if (elapsedTime < duration)
                {
                    float newInten = Mathf.Lerp(globalLight.intensity, 0f, Time.deltaTime * 4f);
                    globalLight.intensity = newInten;
                }
                else
                {
                    float newIten = Mathf.Lerp(globalLight.intensity, origInten, Time.deltaTime * 4f);
                    globalLight.intensity = newIten;

                    if (Mathf.Abs(origInten - globalLight.intensity) < 0.1f)
                    {
                        globalLight.intensity = origInten;
                        break;
                    }
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        StartCoroutine(Dim());
    }

    void SetBGM(AudioClip clip)
    {
        asBgm.clip = clip;
    }
    
    public static void PlaySoundEffect(AudioClip clip)
	{

		AudioSource audSrc = Instantiate(gm.prefabSfx).GetComponent<AudioSource>();
		audSrc.clip = clip;
		audSrc.volume = sfxVol;
		audSrc.Play();
	
		IEnumerator PlaySfx()
		{
			yield return new WaitForSeconds(clip.length);
			Destroy(audSrc.gameObject);
		}
	
		gm.StartCoroutine(PlaySfx());
	}

    void ZoomCamera(float cameraScale, float dur, float cameraZoomSpeed, float xPosModif, float yPosModif)
    {
        IEnumerator Zoom()
        {
            float origCamScale = Camera.main.orthographicSize;
            float targetScale = origCamScale * cameraScale;
            float elapsedTime = 0f;

            float currCamPosXModif = 0f;
            float targetCamPosXModif = xPosModif;

            float currCamPosYModif = 0f;
            float targetCamPosYModif = yPosModif;

            while (true)
            {
                if (elapsedTime < dur)
                {
                    float newScale = Mathf.Lerp(Camera.main.orthographicSize, targetScale, Time.deltaTime * cameraZoomSpeed);
                    Camera.main.orthographicSize = newScale;

                    float newPosX = Mathf.Lerp(currCamPosXModif, targetCamPosXModif, Time.deltaTime * cameraZoomSpeed);
                    float xModif = newPosX - currCamPosXModif;
                    currCamPosXModif = newPosX;

                    float newPosY = Mathf.Lerp(currCamPosYModif, targetCamPosYModif, Time.deltaTime * cameraZoomSpeed);
                    float modif = newPosY - currCamPosYModif;
                    currCamPosYModif = newPosY;

                    Vector3 newPos = new Vector3(Camera.main.transform.position.x + xModif, Camera.main.transform.position.y + modif, Camera.main.transform.position.z);
                    Camera.main.transform.position = newPos;
                }
                else
                {
                    float newScale = Mathf.Lerp(Camera.main.orthographicSize, origCamScale, Time.deltaTime * cameraZoomSpeed);
                    Camera.main.orthographicSize = newScale;

                    float newPosX = Mathf.Lerp(currCamPosXModif, 0f, Time.deltaTime * cameraZoomSpeed);
                    float xModif = newPosX - currCamPosXModif;
                    currCamPosXModif = newPosX;

                    float newPosY = Mathf.Lerp(currCamPosYModif, 0f, Time.deltaTime * cameraZoomSpeed);
                    float modif = newPosY - currCamPosYModif;
                    currCamPosYModif = newPosY;

                    Vector3 newPos = new Vector3(Camera.main.transform.position.x + xModif, Camera.main.transform.position.y + modif, Camera.main.transform.position.z);
                    Camera.main.transform.position = newPos;

                    if (Mathf.Abs(newScale - origCamScale) < 0.01f && Mathf.Abs(0f - currCamPosYModif) < 0.01f && Mathf.Abs(0f - currCamPosXModif) < 0.01f)
                    {
                        Camera.main.orthographicSize = origCamScale;
                        newPos.y += currCamPosYModif;
                        newPos.x += currCamPosXModif;
                        Camera.main.transform.position = newPos;
                        break;
                    }
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        StartCoroutine(Zoom());
    }

    void SpawnEnemy()
    {
        if (objEnemy == null)
        {
        	GameObject enemyPrefab = null;
        	
        	if (questionQ.Count <= 4) {
        		enemyPrefab = bossPrefabs[currLevel - 1];
        		currHealthEnemy = questionQ.Count;
        	}
        	else
        	{
                int index = ((currLevel - 1) * 2) + currLevelSlayCount;
                //enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                enemyPrefab = enemyPrefabs[index];
        		currHealthEnemy = 3;
        	}
        	
            objEnemy = Instantiate(enemyPrefab, worldT);
            Material mat = Instantiate(matDefault);
            objEnemy.transform.Find("Sprite").GetComponent<SpriteRenderer>().material = mat;
            currEnemyAnimator = objEnemy.GetComponent<Animator>();
            

            Vector3 pos = new Vector3(objEnemy.transform.position.x, objEnemy.transform.position.y, objEnemy.transform.position.z);
            Debug.Log(pos);
            if (currEnemySummCount == 0) pos.x = enemyBaseXPos;
            else pos.x = enemyBaseXPos + (currEnemySummCount * enemyXPosGap);
            Debug.Log(pos);
            objEnemy.transform.position = pos;
            Debug.Log(currEnemySummCount);
            currEnemySummCount++;
        }
    }

    void SlowMotion(float speedMultiplier, float duration, float recoverySpeed)
    {
        IEnumerator SlowMo()
        {
            float elapsedTime = 0f;
            Time.timeScale = speedMultiplier;
            float currTimeScale = speedMultiplier;

            while (true)
            {
                if (!isPaused)
                {
                    if (elapsedTime > duration)
                    {
                        currTimeScale = Mathf.Lerp(currTimeScale, 1f, Time.deltaTime * recoverySpeed);
                        Time.timeScale = currTimeScale;

                        if (Mathf.Abs(10f - currTimeScale) < 0.35f)
                        {
                            Time.timeScale = 1f;
                            break;
                        }
                    }
                    elapsedTime += Time.deltaTime;
                }

                yield return null;
            }
        }

        StartCoroutine(SlowMo());
    }

    void SlayUnit(Transform unitT, float deathEffectDuration)
    {
        Material mat = unitT.Find("Sprite").GetComponent<SpriteRenderer>().material;

        IEnumerator PlayDissolveEffect()
        {
            float elapsedTime = 0f;
            mat.SetInt("_WillDissolve", 1);

            while (true)
            {
                float dissolveAmount = elapsedTime / deathEffectDuration;
                mat.SetFloat("_DissolveAmount", dissolveAmount);

                if (elapsedTime >= deathEffectDuration) break;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Destroy(unitT.gameObject);
        }

        StartCoroutine(PlayDissolveEffect());
    }

    void AttackEnemy()
    {
        animatorPlayer.Play("Attack", 0, 0f);

        if (currHealthEnemy == 1) ZoomCamera(onKillCameraZoomScale, onKillCameraZoomDur, cameraZoomSpeed, onPlayerAttackCamXPosModif, onAttackCamYPosModif);
        else ZoomCamera(cameraZoomScale, cameraZoomDur, onKillCameraZoomSpeed, onPlayerAttackCamXPosModif, onAttackCamYPosModif);

        DimBackground(cameraZoomDur);

        IEnumerator TriggerAttack()
        {
            yield return new WaitForSeconds(playerAttackTriggerTime);

            GameObject attackHitPart = PlayerData.data.equippedAugment != null && PlayerData.data.equippedAugment.attackHitPrefab != null ? PlayerData.data.equippedAugment.attackHitPrefab : attackHitParticle;

            Transform particle = PlayParticle(attackHitPart, objEnemy.transform);
            if (PlayerData.data.equippedAugment != null && PlayerData.data.equippedAugment.sfx != null) PlaySoundEffect(PlayerData.data.equippedAugment.sfx);

            ShakeScreen(0.2f, 0.65f, 26f);
            currHealthEnemy -= 1;

            AudioClip attackSfx = (PlayerData.data.equippedChar.sfxAttack != null) ? PlayerData.data.equippedChar.sfxAttack : soundHit;
            PlaySoundEffect(attackSfx);
            if (currHealthEnemy <= 0 || questionQ.Count == 0)
            {
                particle.parent = worldT;
                SlayUnit(objEnemy.transform, durDeathDissolve);
                SlowMotion(onKillSlowMoTimeScale, onKillSlowMoDur, onKillSlowMoRecovSpeed);

                PlayerData.data.enemyKilled += 1;
                currLevelSlayCount += 1;
                OnSlayEnemy?.Invoke();

                if (questionQ.Count == 0)
                {
                    //spawn explosion
                    PlayParticle(prefabExplosion, objEnemy.transform);
                    //flash
                    animatorScreenFlash.gameObject.SetActive(true);
                    animatorScreenFlash.Play("FadeIn", 0, 0f);


                    IEnumerator PlayExploSound(float delay)
                    {
                        yield return new WaitForSeconds(delay);
                        PlaySoundEffect(soundExplo);
                    }

                    int i = 0;
                    float intervalExplo = 0.15f;
                    while (true)
                    {
                        StartCoroutine(PlayExploSound(i * intervalExplo));
                        if (i * intervalExplo >= screenFlashDur) break;
                        i++;
                    }
                    
                    yield return new WaitForSeconds(screenFlashDur);

                    //after flash show panel level clear.
                    panelLevelClear.gameObject.SetActive(true);
                    panelCorrect.gameObject.SetActive(false);

                    yield return new WaitForSeconds(2f);

                    //show next level button
                    panelLevelClear.gameObject.SetActive(false);
                    btnNextLevel.gameObject.SetActive(true);
                    animatorScreenFlash.gameObject.SetActive(false);
                }
            }

            PlayFlashEffect(0.2f, objEnemy.transform.Find("Sprite").GetComponent<SpriteRenderer>());

            if (currHealthEnemy <= 0 || questionQ.Count == 0) objEnemy = null;
        }

        StartCoroutine(TriggerAttack());
    }

    void AttackPlayer()
    {
        currEnemyAnimator.Play("Attack", 0, 0f);
        ZoomCamera(cameraZoomScale, cameraZoomDur, cameraZoomSpeed, onEnemyAttackCamXPosModif, onAttackCamYPosModif);
        DimBackground(cameraZoomDur);

        IEnumerator TriggerAttack()
        {
            yield return new WaitForSeconds(enemyAttackTriggerTime);

            GameObject attackHit = objEnemy.tag != "Caster" ? attackHitParticle : prefabFireHit;
            Transform particle = PlayParticle(attackHit, playerT);          
            ShakeScreen(0.2f, 0.35f, 26f);

            Transform reducedHeart = playerHealthT.GetChild(playerHealthT.childCount - 1);
            reducedHeart.gameObject.SetActive(false);
            Destroy(reducedHeart.gameObject);
            currPlayerHealth -= 1;
            PlaySoundEffect(soundHit);

            foreach (Transform heartT in playerHealthT)
            {
                if (heartT.gameObject.activeSelf && heartT.TryGetComponent(out Animator animator))
                {
                    animator.Play("Flash", 0, 0f);
                }
            }

            if (currPlayerHealth <= 0)
            {
                ShowGameOverScreen();
                panelWrong.gameObject.SetActive(false);
                particle.parent = worldT;
                SlayUnit(playerT, durDeathDissolve);
            }

            PlayFlashEffect(0.2f, playerT.Find("Sprite").GetComponent<SpriteRenderer>());
        }

        StartCoroutine(TriggerAttack());
    }

    void MoveTowardsNextEnemy()
    {
        if (objEnemy != null)
        {
            IEnumerator Move()
            {
                animatorPlayer.Play("Run", 0, 0f);
                Vector3 targetPos = new Vector3(playerBaseXPos + ((currEnemySummCount - 1) * enemyXPosGap), playerT.position.y, playerT.position.z);
                while (true)
                {
                    if (playerT.position.x == targetPos.x)
                    {
                        animatorPlayer.Play("Idle", 0, 0f);
                        SetCurrentQuestion();
                        break;
                    }
                    else
                    {
                        float currXPos = playerT.position.x; 
                        Vector3 newPos = Vector3.MoveTowards(playerT.position, targetPos, Time.deltaTime * playerMoveSpeed);
                        float camXPosModif = newPos.x - currXPos;
                        Vector3 camPos = Camera.main.transform.position;
                        camPos.x += camXPosModif;
                        Camera.main.transform.position = camPos;
                        playerT.position = newPos;
                    }
                    yield return null;
                }
            }

            StartCoroutine(Move());
        }
    }

    void OnClickCorrectAnswer()
    {
        if (currQuestAnsBtnList != null)
        {
            foreach (AnswerButton btn in currQuestAnsBtnList)
            {
                btn.cg.alpha = 0f;
                btn.button.interactable = false;
            }
        }

        animatorPlayer.Play("MoveForward", 1, 0f);
        //currEnemyAnimator.Play("MoveForward", 1, 0f);
        AttackEnemy();
        if (questionQ.Count > 0) PlaySoundEffect(soundCorrect);
        else PlaySoundEffect(soundLevelComplete);

        correctAns += 1;
        IncreaseScore(1);
        GainCoin(1);
        pQ.gameObject.SetActive(false);

        PlayerData.data.currStreak += 1;
        if (PlayerData.data.currStreak > PlayerData.data.highestStreak) PlayerData.data.highestStreak = PlayerData.data.currStreak;
        OnStreak?.Invoke(PlayerData.data.currStreak);

        HideClock();
      
        if (questionQ.Count == 0 && currLevel == 3) ShowVictoryScreen();
        else panelCorrect.gameObject.SetActive(true);

        IEnumerator ShowNextQuestion()
        {
            //flash

            float delayTime = timeNewQuestion;
            if (questionQ.Count == 0) delayTime *= 2; //flash dur + fade out

            yield return new WaitForSeconds(delayTime);

            if (questionQ.Count > 0)
            {
                animatorPlayer.Play("MoveBackward", 1, 0f);
                if (objEnemy == null)
                {
                    yield return new WaitForSeconds(0.33f);
                    panelCorrect.gameObject.SetActive(false);
                    SpawnEnemy();
                    MoveTowardsNextEnemy();
                }
                else
                {
                    panelCorrect.gameObject.SetActive(false);
                    SetCurrentQuestion();
                }
            }
        }

        if (questionQ.Count > 0) StartCoroutine(ShowNextQuestion());
    }

    void OnTimeOut()
    {
        foreach (AnswerButton btn in currQuestAnsBtnList)
        {
            btn.cg.alpha = 0f;
            btn.button.interactable = false;
        }

        //animatorPlayer.Play("MoveForward", 1, 0f);
        currEnemyAnimator.Play("MoveForward", 1, 0f);
        AttackPlayer();
        PlaySoundEffect(soundWrong);

        pQ.gameObject.SetActive(false);       
        HideClock();

        IEnumerator ShowLevelClearPanel()
        {
            yield return new WaitForSeconds(timeNewQuestion * 2);

            panelLevelClear.gameObject.SetActive(false);
            btnNextLevel.gameObject.SetActive(true);
        }

        IEnumerator ShowQuestion()
        {
            float delay = timeNewQuestion * 2;

            yield return new WaitForSeconds(delay);

            panelTimesUp.gameObject.SetActive(false);

            if (questionQ.Count > 0)
            {
                //animatorPlayer.Play("MoveBackward", 1, 0f);
                if (objEnemy != null)
                {
                    currEnemyAnimator.Play("Idle", 0, 0f);
                    currEnemyAnimator.Play("MoveBackward", 1, 0f);
                }
                SetCurrentQuestion();
            }
            else
            {
                if (questionQ.Count == 0 && currLevel == 3) ShowVictoryScreen();
                else
                {
                    panelLevelClear.gameObject.SetActive(true);
                    StartCoroutine(ShowLevelClearPanel());
                }
            }
        }

        if (currPlayerHealth > 1)
        {
            panelTimesUp.gameObject.SetActive(true);
            panelTimesUpAnimator.Play("Shake", 0, 0f);
            panelTimesUpTextCorrectAns.SetText("the correct answer is:\n<color=red><size=+30>" + currentQuestion.answer + "</size></color>");
            StartCoroutine(ShowQuestion());
        }
    } 

    void OnClickWrongAnswer()
    {
        //animatorPlayer.Play("MoveForward", 1, 0f);
        currEnemyAnimator.Play("MoveForward", 1, 0f);
        AttackPlayer();
        PlaySoundEffect(soundWrong);

        currTries -= 1;

        pQ.gameObject.SetActive(false);
        textPanelWrongCorrAns.gameObject.SetActive(false);

        HideClock();

        IEnumerator ShowLevelClearPanel()
        {
            yield return new WaitForSeconds(timeNewQuestion * 2);

            panelLevelClear.gameObject.SetActive(false);
            btnNextLevel.gameObject.SetActive(true);
        }

        IEnumerator ShowQuestion()
        {
            float delay = timeNewQuestion;
            if (currTries == 0) delay *= 2;

            yield return new WaitForSeconds(delay);
            panelWrong.gameObject.SetActive(false);
            currEnemyAnimator.Play("Idle", 0, 0f);
            currEnemyAnimator.Play("MoveBackward", 1, 0f);
            //animatorPlayer.Play("MoveBackward", 1, 0f);

            if (currTries > 0)
            {
                ShowClock();
                StartClockCountdown(Mathf.RoundToInt(timePerQuestion));
                totalTries += 1;
                pQ.gameObject.SetActive(true);
            }
            else
            {
                if (currQuestAnsBtnList != null)
                {
                    foreach (AnswerButton btn in currQuestAnsBtnList)
                    {
                        btn.cg.alpha = 0f;
                        btn.button.interactable = false;
                    }
                }

                if (questionQ.Count > 0)
                {
                    SetCurrentQuestion();
                }
                else
                {
                    if (currLevel == 3) ShowVictoryScreen();
                    else
                    {
                        panelLevelClear.gameObject.SetActive(true);
                        if (objEnemy != null) SlayUnit(objEnemy.transform, durDeathDissolve);
                        StartCoroutine(ShowLevelClearPanel());
                    }
                }
            }
        }

        if (currPlayerHealth > 1)
        {
            panelWrong.gameObject.SetActive(true);
            if (currTries == 0)
            {
                textPanelWrongCorrAns.gameObject.SetActive(true);
                textPanelWrongCorrAns.SetText("the correct answer is:\n<color=red><size=+30>" + currentQuestion.answer + "</size></color>");
            }

            StartCoroutine(ShowQuestion());
        }
    }

    void ShowGameOverScreen()
    {
    	PlaySoundEffect(soundGameover);
        panelGameOver.gameObject.SetActive(true);
        playerHealthT.gameObject.SetActive(false);
        textCoin.gameObject.SetActive(false);
        textScore.gameObject.SetActive(false);
        pauseButtonT.gameObject.SetActive(false);

        textDefeatPanelScore.SetText("your score: <color=yellow><size=+8>" + currProjectedScore + "</size></color>");

        PlayerData.data.coins += currProjectCoins;
        PlayerData.data.totalScore += currProjectedScore;
        MainMenu.SaveData();
    }

    void ShowVictoryScreen()
    {
        textCoin.gameObject.SetActive(false);
        textScore.gameObject.SetActive(false);
        playerHealthT.gameObject.SetActive(false);
        playerHealthTxtT.gameObject.SetActive(false);
        pauseButtonT.gameObject.SetActive(false);
        confetti.gameObject.SetActive(true);

        victoryPanelT.gameObject.SetActive(true);
        textVictoryPanelScore.SetText("your score: <color=yellow><size=+8>" + currProjectedScore + "</size></color>");

        OnCompleteGame?.Invoke();
        OnCompleteGameGetTime?.Invoke(gameTime);
        if (totalTries == correctAns) OnCompletePerfectGame?.Invoke();

        PlayerData.data.coins += currProjectCoins;
        PlayerData.data.totalScore += currProjectedScore;
        MainMenu.SaveData();
    }

    void ShowClock()
    {
        clockT.gameObject.SetActive(true);
        textClockBG.gameObject.SetActive(false);
        textClock.gameObject.SetActive(false);

        animatorClock.Play("FadeIn", 1, 0f);
        textClockBG.gameObject.SetActive(true);
    }

    void HideClock()
    {
        isTimeCountingDown = false;
        clockT.gameObject.SetActive(false);
    }

    void StartClockCountdown(int time)
    {
        if (!isTimeCountingDown)
        {
            currTime = time;
            currTimeOnClock = time;
            textClock.gameObject.SetActive(true);
            textClock.SetText(time.ToString());

            isTimeCountingDown = true;
        }
    }

    void IncreaseScore(int amount)
    {
        currProjectedScore += amount;

        for (int i = 0; i < amount; i++)
        {
            Transform star = Instantiate(prefabStar, worldT).transform;
            Vector2 pos = Random.insideUnitCircle * 5f;
            pos.x += enemyXPosGap * currLevelSlayCount;
            star.position = pos;
            Animator starAnimator = star.GetComponent<Animator>();
            starAnimator.Play("Puff", 1, 0f);

            IEnumerator MoveTowardsScore()
            {
                yield return new WaitForSeconds(starFlyAwayTime);

                while (true)
                {
                    Vector3 newPos = Vector3.Lerp(star.position, starDest.position, Time.deltaTime * starFlySpeed);
                    star.position = newPos;

                    if (Vector3.Distance(star.position, starDest.position) < 0.5f)
                    {
                        StartCoroutine(DestroyStar());
                        break;
                    }

                    yield return null;
                }
            }

            IEnumerator DestroyStar()
            {
                starAnimator.Play("FadeAway", 1, 0f);
                yield return new WaitForSeconds(0.33f);
                Destroy(star.gameObject);

                currScore += 1;
                textScore.SetText("SCORE: <color=yellow><b><size=+12>" + currScore + "</size></b></color>");
            }

            StartCoroutine(MoveTowardsScore());
        }
    }

    public static void AcquireTrophy(Trophy trophy)
    {
        if (!PlayerData.data.acquiredTrophies.ContainsKey(trophy.id))
        {
            PlayerData.data.acquiredTrophies.Add(trophy.id, trophy);
            gm.ShowTrophyNotification(trophy, gm.trophyNotifDuration);
            PlaySoundEffect(gm.soundTrophy);
        }
    }

    void GainCoin(int amount)
    {
        currProjectCoins += amount;

        for (int i = 0; i < amount; i++)
        {
            Transform coin = Instantiate(prefabCoin, worldT).transform;
            Vector2 pos = Random.insideUnitCircle * 5f;
            pos.x += enemyXPosGap * currLevelSlayCount;
            coin.position = pos;
            Animator coinAnimator = coin.GetComponent<Animator>();
            coinAnimator.Play("Puff", 1, 0f);

            IEnumerator MoveTowardsScore()
            {
                yield return new WaitForSeconds(starFlyAwayTime);

                while (true)
                {
                    Vector3 newPos = Vector3.Lerp(coin.position, coinDest.position, Time.deltaTime * starFlySpeed);
                    coin.position = newPos;

                    if (Vector3.Distance(coin.position, coinDest.position) < 0.5f)
                    {
                        StartCoroutine(DestroyStar());
                        break;
                    }

                    yield return null;
                }
            }

            IEnumerator DestroyStar()
            {
                coinAnimator.Play("FadeOut", 1, 0f);
                PlaySoundEffect(soundCoin);
                yield return new WaitForSeconds(0.33f);
                Destroy(coin.gameObject);

                currCoins += 1;
                textCoin.SetText("COINS: <color=yellow><b><size=+12>" + currCoins + "</size></b></color>");
            }

            StartCoroutine(MoveTowardsScore());
        }
    }

    void SetCurrentLevelQuestions()
    {
        Question.Type qType;
        switch (currLevel)
        {
            case 1:
                qType = Question.Type.TrueOrFalse;
                break;
            case 2:
                qType = Question.Type.MultipleChoice;
                break;
            case 3:
                qType = Question.Type.Identification;
                break;
            default:
                qType = Question.Type.TrueOrFalse;
                Debug.Log("Game exceeded number of levels!");
                break;
        }

        List<Question> questionList = new List<Question>(MainMenu.QuestionDictionary[MainMenu.gameSubject][qType]);
        for (int i = 0; i < questionsPerLevel; i++)
        {
            if (questionList.Count == 0) break;

            int randomIndex = Random.Range(0, questionList.Count);
            questionQ.Enqueue(questionList[randomIndex]);
            questionList.RemoveAt(randomIndex);
        }
    }

    void SetBackground(int level)
    {
        if (level > 0 && level <= MainMenu.stagePrefabs.Count)
        {
            if (backgroundContainer.childCount > 0)
            {
                foreach (Transform childT in backgroundContainer)
                {
                    childT.gameObject.SetActive(false);
                    Destroy(childT.gameObject);
                }
            }

            Instantiate(MainMenu.stagePrefabs[level - 1], backgroundContainer);
        }
    }

    void LoadLevel(int count)
    {
        currEnemySummCount = 0;
        currLevel = count;
        questionQ.Clear();
        SetCurrentLevelQuestions();

        SetBackground(count);
        SetBGM(stageBGMs[count - 1]);
        SpawnEnemy();
        InitializeLevel();
    }

    void InitializeLevel()
    {
        animatorPlayer.Play("Idle", 1, 0f);
        playerSpriteT.localPosition = playerDefaultPos;
        Vector3 pos = playerT.position;
        pos.x = playerBaseXPos;
        playerT.position = pos;

        Vector3 camPos = Camera.main.transform.position;
        camPos.x = 0f;
        Camera.main.transform.position = camPos;
        currLevelSlayCount = 0;
    }

    void StartGame()
    {
        introTextPanel.gameObject.SetActive(true);
        introText.SetText("Level " + currLevel + "\n<color=white><size=100>START!</size></color>");

        IEnumerator StartGame()
        {
            yield return new WaitForSeconds(gameStartDelay);
            introTextPanel.gameObject.SetActive(false);
            SetCurrentQuestion();
        }

        StartCoroutine(StartGame());
    }

    public void GoToNextLevel()
    {
        animatorTransition.Play("FadeIn", 0, 0f);
        //delay
        IEnumerator Go()
        {
            yield return new WaitForSeconds(fadeInDuration);
            panelLevelClear.gameObject.SetActive(false);
            btnNextLevel.gameObject.SetActive(false);

            currLevel += 1;
            LoadLevel(currLevel);
            StartGame();
        }

        StartCoroutine(Go());
    }

    void SetCurrentQuestion()
    {
        totalTries += 1;
        currTries = tryPerQ;
        pQ.gameObject.SetActive(true);
        panelQuestion.gameObject.SetActive(true);
        animatorQuestion.Play("Enter", 0, 0f);
        currentQuestion = questionQ.Dequeue();

        panelAnswersTF.gameObject.SetActive(false);
        panelAnswersMult.gameObject.SetActive(false);
        panelAnswerWrite.gameObject.SetActive(false);

        currQuestAnsBtnList = null;

        string txtQuestion = "";

        if (currentQuestion is QuestionTF qTF)
        {
            panelAnswersTF.gameObject.SetActive(true);
            bool ans = GetTrueBoolByChance(50f);
            currentQuestion.answer = ans.ToString();

            answerButtonsTF[0].button.onClick.RemoveAllListeners();
            answerButtonsTF[1].button.onClick.RemoveAllListeners();

            if (ans)
            {
                txtQuestion = qTF.questionT;
                answerButtonsTF[0].button.onClick.AddListener(OnClickCorrectAnswer);
                answerButtonsTF[1].button.onClick.AddListener(OnClickWrongAnswer);
            }
            else
            {
                txtQuestion = qTF.questionF;
                answerButtonsTF[1].button.onClick.AddListener(OnClickCorrectAnswer);
                answerButtonsTF[0].button.onClick.AddListener(OnClickWrongAnswer);
            }

            currQuestAnsBtnList = answerButtonsTF;
        }
        else if (currentQuestion is QuestionMult qMult)
        {
            panelAnswersMult.gameObject.SetActive(true);
            string ans = qMult.answer;
            txtQuestion = qMult.question;

            for (int i = 0; i < qMult.choices.Count; i++)
            {
                answerButtonsMult[i].button.onClick.RemoveAllListeners();
                if (qMult.willJumble) answerButtonsMult[i].text.SetText(JumbleString(qMult.choices[i]));
                else answerButtonsMult[i].text.SetText(qMult.choices[i]);

                if (ans.ToLower() == qMult.choices[i].ToLower())
                {
                    answerButtonsMult[i].button.onClick.AddListener(OnClickCorrectAnswer);
                }
                else
                {
                    answerButtonsMult[i].button.onClick.AddListener(OnClickWrongAnswer);
                }
            }

            currQuestAnsBtnList = answerButtonsMult;
        }
        else if (currentQuestion is QuestionJumbleWrite qJ)
        {
            //panelAnswerWrite.gameObject.SetActive(true);
            string ans = qJ.answer;
            string hint = "\n\n<color=#ff0000>" + JumbleString(qJ.answer).ToUpper() + "</color>";
            txtQuestion = qJ.question + hint;
            panelAnswerWriteOK.onClick.RemoveAllListeners();
            
            void OnClickOK()
            {
                string input = inputField.text.ToLower();
                if (input == ans.ToLower())
                {
                    OnClickCorrectAnswer();
                }
                else
                {
                    OnClickWrongAnswer();
                }
            }

            panelAnswerWriteOK.onClick.AddListener(OnClickOK);
        }

        if (willShowAnswers) textQuestion.SetText(txtQuestion + "\n\nAnswer: " + currentQuestion.answer);
        else textQuestion.SetText(txtQuestion);

        currQuestionNum += 1;
        textQuestionBanner.SetText("Question #" + currQuestionNum);

        animatorBtnQImg.gameObject.SetActive(false);
        if (currentQuestion.img != null)
        {
            imgQuestion.sprite = currentQuestion.img;
            
            IEnumerator ShowImageButton()
            {
                yield return new WaitForSeconds(1.61f);
                animatorBtnQImg.gameObject.SetActive(true);
                animatorBtnQImg.Play("FadeIn", 0, 0f);
            }

            StartCoroutine(ShowImageButton());
        }
        
        
        float qTime = timePerQuestion;
        if (currentQuestion.img != null) qTime += bonusTimeImageQuestion;

        if (objEnemy == null)
        {
            SpawnEnemy();
        }
        
        if (currentQuestion is QuestionJumbleWrite)
        {
        	IEnumerator PlayAnim(){
        		
        		yield return new WaitForSeconds(questionEnterAnimDuration);
        		panelAnswerWrite.gameObject.SetActive(true);
        		animatorPanelAnsWrite.Play("FadeIn", 0, 0f);
        		inputField.text = "";

        		StartClockCountdown(Mathf.RoundToInt(qTime));
        	}
        	
        	StartCoroutine(PlayAnim());
        }

        if (currQuestAnsBtnList != null)
        {
            IEnumerator PlayBtnAnim(float delay, Animator animator)
            {
                if (delay > 0f) yield return new WaitForSeconds(delay);
                animator.Play("Enter", 0, 0f);
            }

            IEnumerator StartPlayBtnAnim()
            {
                yield return new WaitForSeconds(questionEnterAnimDuration);
                ShowClock();

                for (int i = 0; i < currQuestAnsBtnList.Count; i++)
                {
                    StartCoroutine(PlayBtnAnim(i * buttonEnterAnimInterval, currQuestAnsBtnList[i].animator));
                }
            }

            IEnumerator EnableButtons()
            {
                yield return new WaitForSeconds(questionEnterAnimDuration + buttonEnterAnimInterval * (currQuestAnsBtnList.Count - 1));
                foreach (AnswerButton btn in currQuestAnsBtnList)
                {
                    btn.button.interactable = true;
                }

                StartClockCountdown(Mathf.RoundToInt(qTime));
            }

            StartCoroutine(StartPlayBtnAnim());
            StartCoroutine(EnableButtons());
        }

        Canvas.ForceUpdateCanvases();
    }

    string JumbleString(string str)
    {
        List<char> strList = new List<char>();
        string origStr = "";
        bool hasDiffLet = false;
        if (str.Length > 1)
        {
            while (origStr != str && !hasDiffLet)
            {
                if (origStr == "") origStr = str;
                strList.Clear();
                foreach (char c in str)
                {
                    strList.Add(c);
                }

                for (int i = 0; i < str.Length; i++)
                {
                    if (!hasDiffLet && str[0] != str[i]) hasDiffLet = true;
                    int randomIndex = Random.Range(0, str.Length);
                    char temp = strList[randomIndex];
                    strList[randomIndex] = strList[i];
                    strList[i] = temp;
                }

                str = "";
                foreach (char c in strList) str += c;
            }
        }
        return str;
    }

    Transform PlayParticle(GameObject prefab, Transform target)
    {
        Transform particle = Instantiate(prefab, target.Find("Sprite")).transform;
        SpriteRenderer sr = target.Find("Sprite").GetComponent<SpriteRenderer>();
        Vector3 pos = new Vector3(0f, sr.bounds.center.y - target.position.y, transform.position.z);
        Vector3 srPos = sr.transform.position;
        //particle.localPosition = Vector3.zero;
        //particle.localPosition += srPos - pos;
        particle.localPosition = pos;
        //particle.parent = worldT;

        float dur = 0f;
        if (particle.TryGetComponent(out ParticleSystem ps)) dur = ps.main.duration;

        foreach (Transform childPart in particle)
        {
            if (childPart.TryGetComponent(out ParticleSystem pss))
            {
                float totalDuration = pss.main.duration + pss.main.startDelay.constant + pss.main.startLifetime.constant;
                if (totalDuration > dur)
                {
                    dur = totalDuration;
                }
            }
        }

        IEnumerator DestroyParticle()
        {
            yield return new WaitForSeconds(dur);
            Destroy(particle.gameObject);
        }

        StartCoroutine(DestroyParticle());

        return particle;
    }

    protected void PlayFlashEffect(float duration, SpriteRenderer targetSR)
    {
        Material defaultMat = targetSR.material;
        float damagedEffectElapsedTime = 0f;

        IEnumerator PlayFlashEffect()
        {
            while (true)
            {
                if (damagedEffectElapsedTime >= duration)
                {
                    if (targetSR != null) targetSR.material = defaultMat;
                    break;
                }
                else damagedEffectElapsedTime += Time.deltaTime;

                yield return null;
            }
        }

        targetSR.material = onAttackedMat;
        StartCoroutine(PlayFlashEffect());
    }

    public static bool GetTrueBoolByChance(float chance)
    {
        if (chance > 100f)
        {
            chance = 100f;
        }

        float randomNumber = Random.Range(0, 100f);

        if (randomNumber < chance)
        {
            return true;
        }

        return false;
    }

    void ShowTrophyNotification(Trophy trophy, float duration)
    {
        confetti.gameObject.SetActive(true);
        Transform trophyNotif = Instantiate(trophyNotifTemplate, trophyNotifT).transform;
        trophyNotif.gameObject.SetActive(true);
        Animator animator = trophyNotif.GetComponent<Animator>();
        animator.Play("Puff", 0, 0f);
        TextMeshProUGUI text = trophyNotif.Find("Text").GetComponent<TextMeshProUGUI>();
        text.SetText("trophy unlocked: <color=yellow>" + trophy.name + "</color>");

        IEnumerator DestroyNotif()
        {
            float elapsedTime = 0f;
            bool hasFadeout = false;

            while (true)
            {
                if (elapsedTime >= duration && !hasFadeout)
                {
                    animator.Play("Fadeout", 0, 0f);
                    hasFadeout = true;
                }
                else if (elapsedTime >= duration + 0.5f)
                {
                    trophyNotif.gameObject.SetActive(false);
                    Destroy(trophyNotif.gameObject);

                    if (!victoryPanelT.gameObject.activeSelf) confetti.gameObject.SetActive(false);
                    break;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        StartCoroutine(DestroyNotif());
    }

    void ShakeScreen(float duration, float magnitude, float intensity)
    {
        float elapsedTime = 0f;
        float screenShakeDuration = duration;

        IEnumerator ShakeScreen()
        {
            //Vector3 originalPos = Camera.main.transform.position;
            Vector3 currPosModif = new Vector3(0f, 0f, Camera.main.transform.position.z);
            Vector3 center = currPosModif;

            Vector3 GetNextStep()
            {
                Vector2 randomOffset = Random.insideUnitCircle * magnitude;
                return new Vector3(randomOffset.x, randomOffset.y, Camera.main.transform.position.z);
            }

            Vector3 nextStep = GetNextStep();

            while (true)
            {
                if (elapsedTime < screenShakeDuration) //Shake screen
                {
                    if (currPosModif.Equals(nextStep)) nextStep = GetNextStep();

                    Vector3 newPos = Vector3.MoveTowards(currPosModif, nextStep, Time.deltaTime * intensity);
                    Vector3 modif = newPos - currPosModif;
                    currPosModif = newPos;
                    Camera.main.transform.position += modif;
                }
                else //Reposition to original pos
                {
                    Vector3 newPos = Vector3.Lerp(currPosModif, center, Time.deltaTime * intensity);
                    Vector3 modif = newPos - currPosModif;
                    currPosModif = newPos;
                    Camera.main.transform.position += modif;

                    if (Vector3.Distance(currPosModif, center) < 0.01f)
                    {
                        Camera.main.transform.position += new Vector3(currPosModif.x, currPosModif.y, 0f);
                        Debug.Log("end");
                        break;
                    }
                }

                elapsedTime += Time.deltaTime;

                yield return null;
            }
        }

        if (corShakeScreen != null) StopCoroutine(corShakeScreen);
        corShakeScreen = StartCoroutine(ShakeScreen());
    }

    public void PlayTutorial2()
    {
        if (!PlayerData.data.hasFinishedTutorial2)
        {
            isPlayingTutorial2 = true;
            animatorTutorialPanel2.gameObject.SetActive(true);
            animatorTutorialPanel2.Play("Fade In", 0, 0f);
            animatorTutorialRobot2.Play("Float", 1, 0f);

            int currIndex = 0;

            void OnClickTutorialImage()
            {
                currIndex++;
                if (currIndex < MainMenu.tutorialImages2.Count)
                {
                    panelTutorial2Image.sprite = MainMenu.tutorialImages2[currIndex];
                    animatorTutorialPanel2.Play("PuffImage", 1, 0f);
                }
                else
                {
                    PlayerData.data.hasFinishedTutorial2 = true;
                    isPlayingTutorial2 = false;
                    animatorTutorialPanel2.Play("Fade Out", 0, 0f);

                    IEnumerator DisableObj()
                    {
                        yield return new WaitForSeconds(0.25f);

                        animatorTutorialPanel2.gameObject.SetActive(false);
                    }

                    StartCoroutine(DisableObj());
                    this.OnClickTutorialImage -= OnClickTutorialImage;
                    MainMenu.SaveData();
                    StartGame();
                }
            }

            panelTutorial2Image.sprite = MainMenu.tutorialImages2[currIndex];
            animatorTutorialPanel2.Play("PuffImage", 1, 0f);

            this.OnClickTutorialImage += OnClickTutorialImage;
        }
    }
}
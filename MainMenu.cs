using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
using System.Linq;

public class MainMenu : MonoBehaviour
{
    Transform canvas;
    Transform panelStudy;
    Transform panelDiscuss;
    Transform panelStudySubjects;
    Transform panelChooseSubj;
    TextMeshProUGUI textDiscussTitle;
    TextMeshProUGUI textDiscussBody;
    Image imgDiscuss;
    Transform templateBtnDiscussTopic;
    Transform discussTopicContent;
    Transform discussContainer;

    Animator animatorTransition;
    Button subjSelectClose;
    List<Button> subjSelectButtons = new List<Button>();

    public static Question.Subject gameSubject; //temp
    public static Dictionary<Question.Subject, Dictionary<Question.Type, List<Question>>> QuestionDictionary;

    Transform panelTrophies;
    Transform panelTrophiesTemplate;
    Transform panelTrophiesContent;
    Transform textTrophyNameT;
    TextMeshProUGUI textTrophyName;
    TextMeshProUGUI textTrophyReq;

    static List<Trophy> trophies;

    Transform panelShop;
    Transform panelShopCharsWindow;
    Transform panelShopAugsWindow;
    Transform panelShopCharsContainer;
    Transform panelShopCharsTemplate;
    Transform panelShopAugsContainer;
    Transform panelShopAugsTemplate;
    TextMeshProUGUI textShopCoin;

    static List<Character> charList;
    static List<Character> shopCharacters;
    static List<WeaponAugment> weaponAugments;

    Transform panelProfile;
    TextMeshProUGUI textStats;
    Button btnLoadoutChar;
    Button btnLoadoutAug;
    Image imageLoadoutChar;
    Image imageLoadoutAug;
    TextMeshProUGUI textLoadoutChar;
    TextMeshProUGUI textLoadoutAug;

    Transform panelLoadoutSel;
    Transform panelLoadoutSelTempChar;
    Transform panelLoadoutSelTempAug;
    Transform panelLoadoutSelContainer;
    TextMeshProUGUI panelLoadoutTitleText;

    Sprite spriteAugNone;

    AudioSource asBGM;

    Transform panelPause;
    Slider sliderBGM;
    Slider sliderSFX;

    string subj1Name = "Logic Formulation";
    string subj2Name = "Intro to Computing and Problem Solving";

    static string saveFile;

    public static List<Sprite> tutorialImages1;
    public static List<Sprite> tutorialImages2;

    Transform panelTutorial1;
    Image panelTutorial1Image;
    Animator animatorPanelTutorial;
    Animator animatorTutorialRobot;

    public delegate void OnVoidEvent();
    OnVoidEvent OnClickTutorialImage;

    bool isPlayingTutorial1;

    static MainMenu mm;

    Transform worldT;
    public static List<GameObject> stagePrefabs;
    Animator bgAnimator;
    Coroutine corScrollCam;
    Coroutine corCycleBackground;
    float camScrollSpeed = 1f;
    float bgCycleTime = 6f;

    void Awake()
    {
        mm = this;
        saveFile = Path.Combine(Application.dataPath, "gamedata.json");

        worldT = GameObject.Find("World").transform;

        if (stagePrefabs == null)
        {
            stagePrefabs = new List<GameObject>()
            {
                Resources.Load<GameObject>("Stage/Forest/BGForest"),
                Resources.Load<GameObject>("Stage/Riverside/BGRiverside"),
                Resources.Load<GameObject>("Stage/Graveyard/BGGraveyard")
            };
        }

        if (weaponAugments == null)
        {
            weaponAugments = new List<WeaponAugment>();
            spriteAugNone = Resources.LoadAll<Sprite>("icon-aug")[0];

            weaponAugments.Add(new WeaponAugment000());
            weaponAugments.Add(new WeaponAugment001());
            weaponAugments.Add(new WeaponAugment002());
            weaponAugments.Add(new WeaponAugment003());
        }

        if (shopCharacters == null)
        {
            shopCharacters = new List<Character>();

            shopCharacters.Add(new Character001());
            shopCharacters.Add(new Character002());
        }

        if (charList == null)
        {
            charList = new List<Character>();
            charList.Add(new Character000());
            charList.Add(new Character001());
            charList.Add(new Character002());
        }

        if (trophies == null)
        {
            trophies = new List<Trophy>();

            trophies.Add(new Trophy000());
            GameManagerr.OnCompleteGame += () => { GameManagerr.AcquireTrophy(trophies[0]); };

            trophies.Add(new Trophy001());
            GameManagerr.OnCompletePerfectGame += () => { GameManagerr.AcquireTrophy(trophies[1]); };

            Trophy002 trophy002 = new Trophy002();
            trophies.Add(trophy002);
            GameManagerr.OnCompleteGameGetTime += (float seconds) => { if (seconds / 60f <= trophy002.minutes) GameManagerr.AcquireTrophy(trophy002); };

            Trophy003 trophy003 = new Trophy003();
            trophies.Add(trophy003);
            GameManagerr.OnStreak += (int currStreak) => { if (currStreak >= trophy003.consec) GameManagerr.AcquireTrophy(trophy003); };

            Trophy004 trophy004 = new Trophy004();
            trophies.Add(trophy004);
            GameManagerr.OnSlayEnemy += () => { if (PlayerData.data.enemyKilled >= trophy004.slayCount) GameManagerr.AcquireTrophy(trophy004); };
        }
    
        InitializeQuestionDictionary();

        canvas = GameObject.Find("Canvas").transform;
        Transform mainMenu = canvas.Find("MainMenu");
        bgAnimator = canvas.Find("Background").GetComponent<Animator>();
        panelChooseSubj = mainMenu.Find("Subjects");
        panelStudy = mainMenu.Find("CheatPanel");
        panelStudySubjects = panelStudy.Find("Window").Find("Subjects");
        panelDiscuss = panelStudy.Find("Window").Find("StudyPanel");
        discussTopicContent = panelDiscuss.Find("Lessons").Find("Scroll View").Find("Viewport").Find("Content");
        templateBtnDiscussTopic = discussTopicContent.Find("Template");
        textDiscussTitle = panelDiscuss.Find("Discuss").Find("Title").Find("Text").GetComponent<TextMeshProUGUI>();
        discussContainer = panelDiscuss.Find("Discuss").Find("Discuss");
        textDiscussBody = discussContainer.Find("Viewport").Find("Content").Find("Desc").GetComponent<TextMeshProUGUI>();
        imgDiscuss = textDiscussBody.transform.parent.Find("Image").GetComponent<Image>();
        panelTutorial1 = canvas.Find("TutorialPanel");
        panelTutorial1Image = panelTutorial1.Find("Image").GetComponent<Image>();
        animatorPanelTutorial = panelTutorial1.GetComponent<Animator>();
        animatorTutorialRobot = panelTutorial1.Find("Robot").GetComponent<Animator>();

        asBGM = canvas.Find("Background").GetComponent<AudioSource>();

        panelPause = canvas.Find("PauseMenu");
        sliderBGM = panelPause.Find("Volume").Find("Music").Find("Slider").GetComponent<Slider>();
        sliderSFX = panelPause.Find("Volume").Find("Sfx").Find("Slider").GetComponent<Slider>();
       // sliderBGM.onValueChanged.AddListener((float value) => { GameManagerr.SetAudioSourceVol(asBGM, value); });

        panelTrophies = canvas.Find("PanelTrophies");
        panelTrophiesContent = panelTrophies.Find("Window").Find("Items").Find("Trophies").Find("Scroll View").Find("Viewport").Find("Content");
        panelTrophiesTemplate = panelTrophiesContent.GetChild(0);
        textTrophyNameT = panelTrophies.Find("Window").Find("Items").Find("Trophies").Find("Title");
        textTrophyName = textTrophyNameT.Find("Text").GetComponent<TextMeshProUGUI>();
        textTrophyReq = panelTrophies.Find("Window").Find("Items").Find("Trophies").Find("Desc").GetComponent<TextMeshProUGUI>();

        panelShop = canvas.Find("PanelShop");
        panelShopCharsWindow = panelShop.Find("Window").Find("Items").Find("Chars");
        panelShopCharsContainer = panelShopCharsWindow.Find("Content");
        panelShopCharsTemplate = panelShopCharsContainer.Find("Template");
        panelShopAugsWindow = panelShop.Find("Window").Find("Items").Find("Augs");
        panelShopAugsContainer = panelShopAugsWindow.Find("Content");
        panelShopAugsTemplate = panelShopAugsContainer.Find("Template");
        textShopCoin = panelShop.Find("Window").Find("Coin").Find("Text").GetComponent<TextMeshProUGUI>();

        panelProfile = canvas.Find("PanelProfile");
        textStats = panelProfile.Find("Window").Find("Items").Find("Stats").Find("Content").Find("Stats").GetComponent<TextMeshProUGUI>();
        btnLoadoutChar = panelProfile.Find("Window").Find("Items").Find("Loadout").Find("Char").Find("Button").GetComponent<Button>();
        imageLoadoutChar = btnLoadoutChar.transform.Find("Image").GetComponent<Image>();
        textLoadoutChar = btnLoadoutChar.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        btnLoadoutAug = panelProfile.Find("Window").Find("Items").Find("Loadout").Find("Aug").Find("Button").GetComponent<Button>();
        imageLoadoutAug = btnLoadoutAug.transform.Find("Image").GetComponent<Image>();
        textLoadoutAug = btnLoadoutAug.transform.Find("Name").GetComponent<TextMeshProUGUI>();

        panelLoadoutSel = canvas.Find("PanelLoadoutSelect");
        panelLoadoutSelContainer = panelLoadoutSel.Find("Window").Find("Items").Find("Content");
        panelLoadoutSelTempChar = panelLoadoutSelContainer.Find("TemplateChar");
        panelLoadoutSelTempAug = panelLoadoutSelContainer.Find("TemplateAug");
        panelLoadoutTitleText = panelLoadoutSel.Find("Window").Find("Title").Find("Text").GetComponent<TextMeshProUGUI>();

        subjSelectClose = panelChooseSubj.Find("Close").GetComponent<Button>();
        animatorTransition = canvas.Find("Transition").GetComponent<Animator>();
        
        

        int i = 0;
        foreach (Transform buttonT in panelStudySubjects.Find("Content").Find("Buttons"))
        {
        	
        	
            if (buttonT.TryGetComponent(out Button btn))
            {
            	Question.Subject subj = Question.Subject.None;
        	if (i == 0) subj = Question.Subject.LogicFormulation;
        	else if (i == 1) subj = Question.Subject.ProblemSolving;
            	
                void OpenDiscuss()
                {
                    OpenPanelDiscussion(subj);
                }

                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(OpenDiscuss);
                
                TextMeshProUGUI txt = btn.transform.Find("Text").GetComponent<TextMeshProUGUI>();
                if (i == 0) txt.SetText(subj1Name);
                else if (i == 1) txt.SetText(subj2Name);
                i++;
            }
        }
        i = 0;

        foreach(Transform buttonT in panelChooseSubj)
        {
            if (buttonT.tag == "Button" && buttonT.TryGetComponent(out Button btn))
            {
            	Question.Subject subj = Question.Subject.None;

        	if (i == 0) subj = Question.Subject.LogicFormulation;

        	else if (i == 1) subj = Question.Subject.ProblemSolving;
            	
                void ChooseSubj()
                {
                	gameSubject = subj;
                    PlayGame();
                }
                subjSelectButtons.Add(btn);

                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(ChooseSubj);
                btn.onClick.AddListener(DisableSubjSelectButtons);
                
                TextMeshProUGUI txt = btn.transform.Find("Text").GetComponent<TextMeshProUGUI>();
                if (i == 0) txt.SetText(subj1Name);
                else if (i == 1) txt.SetText(subj2Name);
                i++;
            }
        }
        subjSelectButtons.Add(subjSelectClose);

        LoadPlayerData();
        GameManagerr.SetAudioSourceVol(mm.asBGM, PlayerData.data.volBGM);

        asBGM.loop = true;
        sliderBGM.onValueChanged.AddListener((float val) => { GameManagerr.SetAudioSourceVol(asBGM, val); GameManagerr.bgmVol = val; PlayerData.data.volBGM = val; });
        sliderSFX.onValueChanged.AddListener((float val) => { GameManagerr.sfxVol = val; PlayerData.data.volSFX = val; });
        sliderBGM.value = PlayerData.data.volBGM;
        sliderSFX.value = PlayerData.data.volSFX;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        if (tutorialImages1 == null)
        {
            tutorialImages1 = new List<Sprite>()
            {
                Resources.Load<Sprite>("Tutorial/1"),
                Resources.Load<Sprite>("Tutorial/2")
            };
        }

        if (tutorialImages2 == null)
        {
            tutorialImages2 = new List<Sprite>()
            {
                Resources.Load<Sprite>("Tutorial/3"),
                Resources.Load<Sprite>("Tutorial/4"),
                Resources.Load<Sprite>("Tutorial/5"),
                Resources.Load<Sprite>("Tutorial/6"),
                Resources.Load<Sprite>("Tutorial/7"),
                Resources.Load<Sprite>("Tutorial/8"),
                Resources.Load<Sprite>("Tutorial/9"),
                Resources.Load<Sprite>("Tutorial/10")
            };
        }

        Debug.Log(Application.dataPath);
        PlayTutorial1();
        IntroAnimation();
    }
    
    void Update()
    {
        Camera.main.orthographicSize = GameManagerr.cameraWidth / Camera.main.aspect;

        if (isPlayingTutorial1)
        {
            if (Input.GetMouseButtonUp(0))
            {
                OnClickTutorialImage?.Invoke();
            }
        }
    }

    public static void LoadPlayerData()
    {
        if (!File.Exists(saveFile))
        {
            PlayerData.data = new PlayerData();
            Character000 defaultChar = new Character000();
            PlayerData.data.AddCharacter(defaultChar);
            PlayerData.data.EquipCharacter(defaultChar);
            PlayerData.data.coins = 100;
            PlayerData.data.volSFX = 0.5f;
            GameManagerr.sfxVol = 0.5f;
            PlayerData.data.volBGM = 0.5f;
            GameManagerr.bgmVol = 0.5f;
        }
        else
        {
            LoadData();
            GameManagerr.bgmVol = PlayerData.data.volBGM;
            GameManagerr.sfxVol = PlayerData.data.volSFX;
        }

        SaveData();
    }

    void DisableSubjSelectButtons()
    {
        foreach (Button btn in subjSelectButtons) btn.interactable = false;
    }

    public static void InitializeQuestionDictionary()
    {
        if (QuestionDictionary == null)
        {
            int i = 0;
            int subj1Qtf = 0, subj1Mult = 0, subj1Ident = 0;

            QuestionDictionary = new Dictionary<Question.Subject, Dictionary<Question.Type, List<Question>>>();

            while (true)
            {
                System.Type type = System.Type.GetType(ConcatIDToStr("Question", i));
                if (type == null) break;

                Question q = System.Activator.CreateInstance(type) as Question;
                Question.Type qType = Question.Type.TrueOrFalse;

                if (!QuestionDictionary.ContainsKey(q.subj)) QuestionDictionary.Add(q.subj, new Dictionary<Question.Type, List<Question>>());

                if (q is QuestionTF) qType = Question.Type.TrueOrFalse;
                else if (q is QuestionMult) qType = Question.Type.MultipleChoice;
                else if (q is QuestionJumbleWrite) qType = Question.Type.Identification;

                if (!QuestionDictionary[q.subj].ContainsKey(qType)) QuestionDictionary[q.subj].Add(qType, new List<Question>());

                QuestionDictionary[q.subj][qType].Add(q);

                i++;

                if (!q.isSample && q.subj == Question.Subject.LogicFormulation)
                {
                    if (q is QuestionTF) subj1Qtf += 1;
                    else if (q is QuestionMult) subj1Mult += 1;
                    else if (q is QuestionJumbleWrite) subj1Ident += 1;
                }
            }

            Debug.Log("Logic Formulation Question Count: " + subj1Qtf + " " + subj1Mult + " " + subj1Ident);
        }
    }

    static string ConcatIDToStr(string str, int id)
    {
        string idStr = id.ToString();
        while (idStr.Length < 3)
        {
            idStr = "0" + idStr;
        }
        return str + idStr;
    }

    public void PlayGame()
    {
        animatorTransition.Play("FadeIn", 0, 0f);
        IEnumerator Play()
        {
            yield return new WaitForSeconds(GameManagerr.fadeInDuration);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        StartCoroutine(Play());
    }

    public void KodigoGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void OpenPanelStudy()
    {
        panelStudy.gameObject.SetActive(true);
        panelStudySubjects.gameObject.SetActive(true);
        panelDiscuss.gameObject.SetActive(false);
    }

    public void ClosePanelStudy()
    {
        panelStudy.gameObject.SetActive(false);
    }

    public void OpenPanelDiscussion(Question.Subject subj)
    {
        panelStudySubjects.gameObject.SetActive(false);
        panelDiscuss.gameObject.SetActive(true);

        foreach (Transform t in discussTopicContent)
        {
            if (t.gameObject.activeSelf)
            {
                t.gameObject.SetActive(false);
                Destroy(t.gameObject);              
            }
        }

        textDiscussTitle.transform.parent.gameObject.SetActive(false);
        discussContainer.gameObject.SetActive(false);

        foreach (Question.Type qType in QuestionDictionary[subj].Keys)
        {
            foreach (Question q in QuestionDictionary[subj][qType])
            {
                Transform btnT = Instantiate(templateBtnDiscussTopic, discussTopicContent);
                btnT.gameObject.SetActive(true);

                TextMeshProUGUI txt = btnT.Find("Text").GetComponent<TextMeshProUGUI>();
                txt.SetText(q.title);

                Button btn = btnT.GetComponent<Button>();
                
                void ShowDiscussion()
                {
                    textDiscussTitle.transform.parent.gameObject.SetActive(true);
                    textDiscussTitle.SetText(q.title);

                    discussContainer.gameObject.SetActive(true);
                    textDiscussBody.SetText(q.explanation);

                    if (q.img != null)
                    {
                        imgDiscuss.gameObject.SetActive(true);
                        imgDiscuss.sprite = q.img;
                    }
                    else imgDiscuss.gameObject.SetActive(false);
                }

                btn.onClick.AddListener(ShowDiscussion);
            }
        }
    }

    public void OpenPanelChooseSubj()
    {
        panelChooseSubj.gameObject.SetActive(true);
    }

    public void ClosePanelChooseSubj()
    {
        panelChooseSubj.gameObject.SetActive(false);
    }

    public void OpenPanelTrophies()
    {
        panelTrophies.gameObject.SetActive(true);
        textTrophyNameT.gameObject.SetActive(false);
        textTrophyReq.gameObject.SetActive(false);

        foreach (Transform t in panelTrophiesContent)
        {
            if (t.gameObject.activeSelf)
            {
                t.gameObject.SetActive(false);
                Destroy(t.gameObject);
            }
        }

        foreach (Trophy t in trophies)
        {
            Transform trophyT = Instantiate(panelTrophiesTemplate, panelTrophiesContent);
            trophyT.gameObject.SetActive(true);
            Image image = trophyT.GetComponent<Image>();
            image.sprite = t.sprite;

            if (PlayerData.data.acquiredTrophies.ContainsKey(t.id))
            {
                image.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                image.color = new Color(0.33f, 0.33f, 0.33f, 1f);
            }           

            void OnClick()
            {
                textTrophyNameT.gameObject.SetActive(true);
                textTrophyName.SetText(t.name);

                textTrophyReq.gameObject.SetActive(true);
                textTrophyReq.SetText(t.Condition);
            }

            Button btn = trophyT.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnClick);
        }
    }

    public void OpenPanelPause()
    {
        panelPause.gameObject.SetActive(true);
    }

    public void ClosePanelPause()
    {
        panelPause.gameObject.SetActive(false);
        SaveData();
    }

    public void OpenPanelShop()
    {
        panelShop.gameObject.SetActive(true);
        panelShopCharsWindow.gameObject.SetActive(false);
        panelShopAugsWindow.gameObject.SetActive(false);
        textShopCoin.SetText(PlayerData.data.coins.ToString());

        void DespawnTemplate(Transform t)
        {
            if (t.gameObject.activeSelf)
            {
                foreach (Transform childT in t)
                {
                    DespawnTemplate(childT);
                }

                t.gameObject.SetActive(false);
                Destroy(t.gameObject);
            }
        }

        foreach (Transform shopCharT in panelShopCharsContainer)
        {
            DespawnTemplate(shopCharT);
        }

        foreach(Transform shopAugT in panelShopAugsContainer)
        {
            DespawnTemplate(shopAugT);
        }

        foreach (Character c in shopCharacters)
        {
            if (!panelShopCharsWindow.gameObject.activeSelf) panelShopCharsWindow.gameObject.SetActive(true);

            Transform shopCharT = Instantiate(panelShopCharsTemplate, panelShopCharsContainer);
            shopCharT.gameObject.SetActive(true);

            Image image = shopCharT.Find("Image").GetComponent<Image>();
            image.sprite = c.portrait;

            TextMeshProUGUI name = shopCharT.Find("Name").GetComponent<TextMeshProUGUI>();
            name.SetText(c.name);

            Button btn = shopCharT.Find("ButtonBuy").GetComponent<Button>();
            TextMeshProUGUI cost = btn.transform.Find("Cost").GetComponent<TextMeshProUGUI>();

            Transform coinIconT = btn.transform.Find("Coin");

            void DisableBuyButton()
            {
                btn.interactable = false;
                cost.SetText("Owned");
                cost.rectTransform.sizeDelta = new Vector2(260f, cost.rectTransform.sizeDelta.y);
                cost.alignment = TextAlignmentOptions.Center;
                cost.fontSize = 42f;
                coinIconT.gameObject.SetActive(false);
            }

            void OnBuy()
            {
                if (PlayerData.data.coins >= c.cost)
                {
                    PlayerData.data.coins -= c.cost;
                    PlayerData.data.AddCharacter(c);
                    Debug.Log(c.id);
                    textShopCoin.SetText(PlayerData.data.coins.ToString());
                    DisableBuyButton();
                    SaveData();
                }
                else
                {
                    //show warning message
                }
            }

            if (PlayerData.data.ownedCharacters.ContainsKey(c.id)) //if character is already owned
            {
                DisableBuyButton();
            }
            else
            {
                cost.SetText(c.cost.ToString());
                btn.onClick.AddListener(OnBuy);
            }
        }

        foreach (WeaponAugment a in weaponAugments)
        {
            if (!panelShopAugsWindow.gameObject.activeSelf) panelShopAugsWindow.gameObject.SetActive(true);

            Transform shopAugT = Instantiate(panelShopAugsTemplate, panelShopAugsContainer);
            shopAugT.gameObject.SetActive(true);

            Image image = shopAugT.Find("Image").GetComponent<Image>();
            image.sprite = a.icon;

            TextMeshProUGUI name = shopAugT.Find("Name").GetComponent<TextMeshProUGUI>();
            name.SetText(a.name);

            Button btn = shopAugT.Find("ButtonBuy").GetComponent<Button>();
            TextMeshProUGUI cost = btn.transform.Find("Cost").GetComponent<TextMeshProUGUI>();

            Transform coinIconT = btn.transform.Find("Coin");

            void DisableBuyButton()
            {
                btn.interactable = false;
                cost.SetText("Owned");
                cost.rectTransform.sizeDelta = new Vector2(260f, cost.rectTransform.sizeDelta.y);
                cost.alignment = TextAlignmentOptions.Center;
                cost.fontSize = 42f;
                coinIconT.gameObject.SetActive(false);
            }

            void OnBuy()
            {
                if (PlayerData.data.coins >= a.cost)
                {
                    PlayerData.data.coins -= a.cost;
                    PlayerData.data.AddAugment(a);
                    textShopCoin.SetText(PlayerData.data.coins.ToString());
                    DisableBuyButton();
                    SaveData();
                }
                else
                {
                    //show warning message
                }
            }

            if (PlayerData.data.ownedAugments.ContainsKey(a.id)) //if character is already owned
            {
                DisableBuyButton();
            }
            else
            {
                cost.SetText(a.cost.ToString());
                btn.onClick.AddListener(OnBuy);
            }
        }
    }

    public void OpenPanelProfile()
    {
        panelProfile.gameObject.SetActive(true);

        textStats.SetText("Encountered questions: <color=yellow>" + PlayerData.data.encounteredQuestions + "</color>\nCorrects answers: <color=yellow>" + PlayerData.data.correctAnswers + "</color>\nEnemies killed: <color=yellow>" + PlayerData.data.enemyKilled + "</color>\nhighest streak <color=yellow>" + PlayerData.data.highestStreak + "</color>\ngames played: <color=yellow>" + PlayerData.data.gamePlayed + "</color>\n<color=yellow>" + (PlayerData.data.playTime / 60f) + " minute/s</color>");

        imageLoadoutChar.sprite = PlayerData.data.equippedChar.portrait;
        textLoadoutChar.SetText(PlayerData.data.equippedChar.name);

        if (PlayerData.data.equippedAugment != null)
        {
            imageLoadoutAug.sprite = PlayerData.data.equippedAugment.icon;
            textLoadoutAug.SetText(PlayerData.data.equippedAugment.name);
        }
        else
        {
            imageLoadoutAug.sprite = spriteAugNone;
            textLoadoutAug.SetText("None");
        }
    }

    public void OpenAugSelect()
    {
        panelLoadoutSel.gameObject.SetActive(true);
        panelLoadoutTitleText.SetText("Select your weapon augment");

        foreach (Transform t in panelLoadoutSelContainer)
        {
            if (t.gameObject.activeSelf)
            {
                t.gameObject.SetActive(false);
                Destroy(t.gameObject);
            }
        }

        Transform augNoneT = Instantiate(panelLoadoutSelTempAug, panelLoadoutSelContainer);
        augNoneT.gameObject.SetActive(true);

        Image imageAugNone = augNoneT.Find("Image").GetComponent<Image>();
        imageAugNone.sprite = spriteAugNone;

        TextMeshProUGUI nameAugNone = augNoneT.Find("Name").GetComponent<TextMeshProUGUI>();
        string augNoneName = "None";
        if (PlayerData.data.equippedAugment == null) augNoneName += " (Equipped)";
        nameAugNone.SetText(augNoneName);

        Button btnAugNone = augNoneT.GetComponent<Button>();

        void EquipNone()
        {
            PlayerData.data.equippedAugment = null;
            imageLoadoutAug = imageAugNone;
            textLoadoutAug.SetText("None");
            SaveData();
        }

        if (PlayerData.data.equippedAugment == null)
        {
            btnAugNone.interactable = false;
        }
        else
        {
            btnAugNone.interactable = true;
            btnAugNone.onClick.AddListener(EquipNone);
        }

        foreach (WeaponAugment a in PlayerData.data.ownedAugments.Values)
        {
            Transform augT = Instantiate(panelLoadoutSelTempAug, panelLoadoutSelContainer);
            augT.gameObject.SetActive(true);

            Image image = augT.Find("Image").GetComponent<Image>();
            image.sprite = a.icon;

            TextMeshProUGUI name = augT.Find("Name").GetComponent<TextMeshProUGUI>();
            string augName = a.name;
            if (PlayerData.data.equippedAugment != null && PlayerData.data.equippedAugment.id == a.id) augName += " (Equipped)";
            name.SetText(augName);

            Button btn = augT.GetComponent<Button>();

            void EquipChar()
            {
                PlayerData.data.EquipAugment(a);
                imageLoadoutAug.sprite = a.icon;
                textLoadoutAug.SetText(a.name);
                panelLoadoutSel.gameObject.SetActive(false);
                SaveData();
            }

            if (PlayerData.data.equippedAugment != null && PlayerData.data.equippedAugment.id == a.id)
            {
                btn.enabled = false;
            }
            else if (PlayerData.data.ownedAugments.ContainsKey(a.id))
            {
                btn.interactable = true;
                btn.onClick.AddListener(EquipChar);
            }
            else
            {
                btn.interactable = false;
            }
        }
    }

    public void OpenCharSelect()
    {
        panelLoadoutSel.gameObject.SetActive(true);
        panelLoadoutTitleText.SetText("Select your character");

        foreach (Transform t in panelLoadoutSelContainer)
        {
            if (t.gameObject.activeSelf)
            {
                t.gameObject.SetActive(false);
                Destroy(t.gameObject);
            }
        }

        foreach (Character c in PlayerData.data.ownedCharacters.Values)
        {
            Transform charT = Instantiate(panelLoadoutSelTempChar, panelLoadoutSelContainer);
            charT.gameObject.SetActive(true);

            Image image = charT.Find("Image").GetComponent<Image>();
            image.sprite = c.portrait;

            TextMeshProUGUI name = charT.Find("Name").GetComponent<TextMeshProUGUI>();
            string charName = c.name;
            if (PlayerData.data.equippedChar.id == c.id) charName += " (Equipped)";
            name.SetText(charName);

            Button btn = charT.GetComponent<Button>();

            void EquipChar()
            {
                PlayerData.data.EquipCharacter(c);
                imageLoadoutChar.sprite = c.portrait;
                textLoadoutChar.SetText(c.name);
                panelLoadoutSel.gameObject.SetActive(false);
                SaveData();
            }

            if (PlayerData.data.equippedChar.id == c.id)
            {
                btn.enabled = false;
            }
            else if (PlayerData.data.ownedCharacters.ContainsKey(c.id))
            {
                btn.interactable = true;
                btn.onClick.AddListener(EquipChar);
            }
            else
            {
                btn.interactable = false;
            }

            Debug.Log("Has " + charName + " " + PlayerData.data.ownedCharacters.ContainsKey(c.id));
        }
    }

    public static void SaveData()
    {
        PlayerData.data.ownedCharactersID = new int[PlayerData.data.ownedCharacters.Count];
        List<int> charID = new List<int>(PlayerData.data.ownedCharacters.Keys);
        for (int i = 0; i < charID.Count; i++)
        {
            PlayerData.data.ownedCharactersID[i] = charID[i];
        }

        PlayerData.data.ownedAugmentsID = new int[PlayerData.data.ownedAugments.Count];
        List<int> augID = new List<int>(PlayerData.data.ownedAugments.Keys);
        for (int i = 0; i < augID.Count; i++)
        {
            PlayerData.data.ownedAugmentsID[i] = augID[i];
        }

        PlayerData.data.acquiredTrophiesID = new int[PlayerData.data.acquiredTrophies.Count];
        List<int> trophID = new List<int>(PlayerData.data.acquiredTrophies.Keys);
        for (int i = 0; i < trophID.Count; i++)
        {
            PlayerData.data.acquiredTrophiesID[i] = trophID[i];
        }

        string jsonString = JsonUtility.ToJson(PlayerData.data);
        File.WriteAllText(saveFile, jsonString);
    }

    public static void LoadData()
    {
        string fileContents = File.ReadAllText(saveFile);
        Debug.Log(fileContents);
        PlayerData.data = JsonUtility.FromJson<PlayerData>(fileContents);
        Debug.Log(PlayerData.data.coins);

        PlayerData.data.ownedCharacters.Clear();
        PlayerData.data.ownedAugments.Clear();
        PlayerData.data.acquiredTrophies.Clear();

        if (PlayerData.data.ownedCharactersID != null)
        {
            foreach (int i in PlayerData.data.ownedCharactersID)
            {
                Character _char = Activator.CreateInstance(System.Type.GetType(ConcatIDToStr("Character", i))) as Character;
                if (_char != null)
                {
                    PlayerData.data.AddCharacter(_char);
                    if (PlayerData.data.equippedCharacterID == i) PlayerData.data.EquipCharacter(_char);
                }
            }
        }

        if (PlayerData.data.ownedAugmentsID != null)
        {
            foreach (int i in PlayerData.data.ownedAugmentsID)
            {
                WeaponAugment aug = Activator.CreateInstance(System.Type.GetType(ConcatIDToStr("WeaponAugment", i))) as WeaponAugment;
                if (aug != null)
                {
                    PlayerData.data.AddAugment(aug);
                    if (PlayerData.data.hasEquippedAugment && PlayerData.data.equippedAugmentID == i) PlayerData.data.EquipAugment(aug);
                }
            }
        }

        if (PlayerData.data.acquiredTrophiesID != null)
        {
            foreach (int i in PlayerData.data.acquiredTrophiesID)
            {
                Trophy troph = Activator.CreateInstance(System.Type.GetType(ConcatIDToStr("Trophy", i))) as Trophy;
                if (troph != null)
                {
                    PlayerData.data.AddTrophy(troph);
                }
            }
        }
    }

    public void PlayTutorial1()
    {
        if (!PlayerData.data.hasFinishedTutorial1)
        {
            isPlayingTutorial1 = true;
            animatorPanelTutorial.gameObject.SetActive(true);
            animatorPanelTutorial.Play("Fade In", 0, 0f);
            animatorTutorialRobot.Play("Float", 1, 0f);

            int currIndex = 0;

            void OnClickTutorialImage()
            {
                currIndex++;
                if (currIndex < tutorialImages1.Count)
                {
                    panelTutorial1Image.sprite = tutorialImages1[currIndex];
                    animatorPanelTutorial.Play("PuffImage", 1, 0f);
                }
                else
                {
                    PlayerData.data.hasFinishedTutorial1 = true;
                    isPlayingTutorial1 = false;
                    animatorPanelTutorial.Play("Fade Out", 0, 0f);

                    IEnumerator DisableObj()
                    {
                        yield return new WaitForSeconds(0.25f);
                        panelTutorial1.gameObject.SetActive(false);
                    }

                    StartCoroutine(DisableObj());
                    this.OnClickTutorialImage -= OnClickTutorialImage;
                    SaveData();
                }
            }

            panelTutorial1Image.sprite = tutorialImages1[currIndex];
            animatorPanelTutorial.Play("PuffImage", 1, 0f);

            this.OnClickTutorialImage += OnClickTutorialImage;
        }
    }

    void IntroAnimation()
    {
        bgAnimator.Play("Black", 0, 0f);
        //Play anim
        //
      
        Vector3 startPos = Camera.main.transform.position;
        startPos.x = 0f;
        startPos.y = 0f;      

        IEnumerator CycleBackground()
        {
            int currIndex = 0;

            while (true)
            {
                if (currIndex >= stagePrefabs.Count) currIndex = 0;
                foreach (Transform bgT in worldT) Destroy(bgT.gameObject);
                Instantiate(stagePrefabs[currIndex], worldT);
                currIndex++;
                ScrollCamToRight(startPos, camScrollSpeed);

                bgAnimator.Play("FadeOut", 0, 0f);
                //change BG
                yield return new WaitForSeconds(bgCycleTime);
                bgAnimator.Play("FadeIn", 0, 0f);
                yield return new WaitForSeconds(1f);
            }
        }

        if (corCycleBackground == null) corCycleBackground = StartCoroutine(CycleBackground());
    }

    void ScrollCamToRight(Vector2 startPos, float scrollSpeed)
    {
        Vector3 camPos = Camera.main.transform.position;
        camPos.x = startPos.x;
        camPos.y = startPos.y;
        Camera.main.transform.position = camPos;

        IEnumerator ScrollCam()
        {
            while (true)
            {
                yield return null;
                Vector3 currCamPos = Camera.main.transform.position;
                currCamPos.x += scrollSpeed * Time.deltaTime;
                Camera.main.transform.position = currCamPos;
            }
        }

        if (corScrollCam == null)
        {
            corScrollCam = StartCoroutine(ScrollCam());
        }
    }
}
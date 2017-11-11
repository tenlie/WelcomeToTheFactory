using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class TutorialCharacterAI : MonoBehaviour
{
    public Animator animator;
    public AudioSource bgmAudio;
    public PlayerController2D playerController { get; private set; }
    public Rigidbody2D rb2d { get; set; }
    public Collider2D col2d { get; set; }

    public GameObject quote;
    public UILabel quoteLabel;
    public UISprite quoteTop;

    public GameObject tutorialGradient;

    public UISprite sp_TutorialGradientVertical;
    public UISprite sp_TutorialGradientHorizontal;
    private UISprite _sp_TutorialGradient;


    [SerializeField]
    private float subtractor;

    private float _bgmTime
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android) 
                return (bgmAudio.time - subtractor);
            else
                return bgmAudio.time;
        } 
    }

    private Action event1, event2, event3, event4, event5, event6, event7, event8, event9, event10;
    private Action event11, event12, event13, event14, event15, event16, event17, event18, event19, event20;
    private Action event21, event22, event23, event24, event25, event26, event27;

    public void Awake()
    {
        animator.speed = 1.0f;
        playerController = GetComponent<PlayerController2D>();
        col2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        quote.SetActive(false);
        tutorialGradient.SetActive(false);

        if (SaveData.Controls.Equals("Vertical"))
        {
            _sp_TutorialGradient = sp_TutorialGradientVertical;
        }
        else
        {
            _sp_TutorialGradient = sp_TutorialGradientHorizontal;
        }
        NGUITools.SetActive(_sp_TutorialGradient.gameObject, true);
    }

    public void Start()
    {
        event1 = Event1; event2 = Event2; event3 = Event3; event4 = Event4; event5 = Event5; event6 = Event6; event7 = Event7; event8 = Event8; event9 = Event9; event10 = Event10;
        event11 = Event11; event12 = Event12; event13 = Event13; event14 = Event14; event15 = Event15; event16 = Event16; event17 = Event17; event18 = Event18; event19 = Event19; event20 = Event20;
        event21 = Event21; event22 = Event22; event23 = Event23; event24 = Event24; event25 = Event25; event26 = Event26; event27 = Event27;
    }

    void PlayTutorial()
    {
        if (event1 != null && 3f <= _bgmTime) { event1(); event1 = null; return; }

        if (event2 != null && 6.233f <= _bgmTime) { event2(); event2 = null; return; }

        if (event3 != null && 10.366f <= _bgmTime) { event3(); event3 = null; return; }

        if (event4 != null && 14.5f <= _bgmTime) { event4(); event4 = null; return; }

        if (event5 != null && 19.567f <= _bgmTime) { event5(); event5 = null; return; }

        if (event6 != null && 22.033f <= _bgmTime) { event6(); event6 = null; return; }

        if (event7 != null && 22.4f <= _bgmTime) { event7(); event7 = null; return; }

        if (event8 != null && 23.7f <= _bgmTime) { event8(); event8 = null; return; }

        if (event9 != null && 27.467f <= _bgmTime) { event9(); event9 = null; return; }

        if (event10 != null && 30.567f <= _bgmTime) { event10(); event10 = null; return; }

        if (event11 != null && 35.167f <= _bgmTime) { event11(); event11 = null; return; }

        if (event12 != null && 36.433f <= _bgmTime) { event12(); event12 = null; return; }

        if (event13 != null && 39.667f <= _bgmTime) { event13(); event13 = null; return; }

        if (event14 != null && 44f <= _bgmTime) { event14(); event14 = null; return; }

        if (event15 != null && 47.027f <= _bgmTime) { event15(); event15 = null; return; }

        if (event16 != null && 48.533f <= _bgmTime) { event16(); event16 = null; return; }

        if (event17 != null && 51.167f <= _bgmTime) { event17(); event17 = null; return; }

        if (event18 != null && 51.433f <= _bgmTime) { event18(); event18 = null; return; }

        if (event19 != null && 52.233f <= _bgmTime) { event19(); event19 = null; return; }

        if (event20 != null && 55.167f <= _bgmTime) { event20(); event20 = null; return; }

        if (event21 != null && 59.333f <= _bgmTime) { event21(); event21 = null; return; }

        if (event22 != null && 59.6f <= _bgmTime) { event22(); event22 = null; return; }

        if (event23 != null && 62.2f <= _bgmTime) { event23(); event23 = null; return; }

        if (event24 != null && 62.233f <= _bgmTime) { event24(); event24 = null; return; }

        if (event25 != null && 70.367f <= _bgmTime) { event25(); event25 = null; return; }

        if (event26 != null && 71.167f <= _bgmTime) { event26(); event26 = null; return; }

        if (event27 != null && 71.9f <= _bgmTime) { event27(); event27 = null; return; }
    }

    public void Update()
    {
        PlayTutorial();


        animator.SetBool("IsGrounded", playerController.State.IsGrounded);
        animator.SetBool("IsJumping", playerController.State.IsJumpingUp);
        animator.SetBool("IsDoubleJumping", playerController.State.IsDoubleJumping);
    }

    public bool IsInputJump(Vector2 touchPosition)
    {
        bool result = false;

        if (SaveData.Controls.Equals("Vertical"))
        {
            result = Camera.main.ScreenToWorldPoint(touchPosition).y < 0.0f;
        }
        else
        {
            result = Camera.main.ScreenToWorldPoint(touchPosition).x < 0.0f;
        }

        return result;
    }

    public void HideQuote()
    {
        quote.SetActive(false);
    }

    public void DisableTutorialGradient()
    {
        tutorialGradient.GetComponent<TweenAlpha>().enabled = false;
        tutorialGradient.SetActive(false);
    }

    public void FallBack()
    {
        iTween.MoveBy(gameObject, iTween.Hash("x", -0.5, "easeType", iTween.EaseType.linear, "time", 0.2f));
    }

    public void Event1()
    {
        //화면 밖으로부터 달려옴
        iTween.MoveBy(gameObject, iTween.Hash("x", 7.8, "easeType", iTween.EaseType.linear, "time", 2f, "oncomplete", "FallBack"));
    }

    public void Event2()
    {
        quoteLabel.text = "깨어났구나 리츠";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
    }

    public void Event3()
    {
        quoteLabel.GetComponent<UILabel>().text = "폭동이 일어났어!";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
    }

    public void Event4()
    {
        quoteLabel.GetComponent<UILabel>().text = "무슨 일인지 알아봐야 해!";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
    }

    public void Event5()
    {
        playerController.Attack();
    }

    public void Event6()
    {
        playerController.Attack();
    }

    public void Event7()
    {
        playerController.Attack();
    }

    public void Event8()
    {
        quoteLabel.GetComponent<UILabel>().text = "공장 안 쪽까지 침입하다니!";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
    }

    public void Event9()
    {
        quoteLabel.GetComponent<UILabel>().text = "퇴치하자!";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 3f);
    }

    public void Event10()
    {
        quoteLabel.GetComponent<UILabel>().text = "앞장 설테니 따라와!";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
    }

    public void Event11()
    {
        playerController.Attack();
    }

    public void Event12()
    {
        playerController.Attack();
    }

    public void Event13()
    {
        quoteLabel.GetComponent<UILabel>().text = "화면 오른쪽/ 아래쪽(설정값을 따름)을 눌러서 점프할 수 있어.";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);

        tutorialGradient.SetActive(true);
        tutorialGradient.GetComponent<TweenAlpha>().enabled = true;
        Invoke("DisableTutorialGradient", 6f);

    }

    public void Event14()
    {
        quoteLabel.GetComponent<UILabel>().text = "장애물이야! 뛰어!";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
    }

    public void Event15()
    {
        playerController.Jump();
    }

    public void Event16()
    {
        //점멸
        quoteLabel.GetComponent<UILabel>().text = "화면 왼쪽/위쪽(설정값을 따름)을 누르면 공격할 수 있어.";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);

        if (SaveData.Controls.Equals("Vertical"))
        {
            _sp_TutorialGradient.transform.localScale = new Vector3(_sp_TutorialGradient.transform.localScale.x, -(_sp_TutorialGradient.transform.localScale.y), _sp_TutorialGradient.transform.localScale.z);
        }
        else
        {
            _sp_TutorialGradient.transform.localScale = new Vector3(-_sp_TutorialGradient.transform.localScale.x, _sp_TutorialGradient.transform.localScale.y, _sp_TutorialGradient.transform.localScale.z);
        }

        tutorialGradient.SetActive(true);
        tutorialGradient.GetComponent<TweenAlpha>().enabled = true;
        Invoke("DisableTutorialGradient", 6f);
    }

    public void Event17()
    {
        playerController.Jump();
    }

    public void Event18()
    {
        playerController.Jump();
    }

    public void Event19()
    {
        quoteLabel.GetComponent<UILabel>().text = "적이 온다!공격해!";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
    }

    public void Event20()
    {
        playerController.Jump();
    }

    public void Event21()
    {
        playerController.Jump();
    }

    private void Event22()
    {
        playerController.Jump();
    }

    private void Event23()
    {
        quoteLabel.GetComponent<UILabel>().text = "좋아, 돌파하자.";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
    }

    private void Event24()
    {
        quoteLabel.GetComponent<UILabel>().text = "놓치지 말고 따라와!";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
    }

    private void Event25()
    {
        //화면 밖으로 달려감
        iTween.MoveBy(gameObject, iTween.Hash("x", 8.4, "easeType", iTween.EaseType.linear, "time", 1.9f));
    }

    private void Event26()
    {
        playerController.Jump();
    }

    private void Event27()
    {
        Destroy(gameObject);
    }
}

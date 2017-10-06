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

    private bool[] progressed = new bool[26];
    private int index = 0;

    public GameObject tutorialGradient;
    public UISprite sp_TutorialGradient;

    private Action<Action> event1, event2, event3, event4, event5, event6, event7, event8, event9, event10;
    private Action<Action> event11, event12, event13, event14, event15, event16, event17, event18, event19, event20;
    private Action<Action> event21, event22, event23, event24, event25, event26, event27;

    private bool _hasEnded;

    public void Awake()
    {
        animator.speed = 1.0f;
        _hasEnded = false;

        playerController = GetComponent<PlayerController2D>();
        col2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        quote.SetActive(false);
        tutorialGradient.SetActive(false);
    }

    public void Start()
    {
        event1 = Event1; event2 = Event2; event3 = Event3; event4 = Event4; event5 = Event5; event6 = Event6; event7 = Event7; event8 = Event8; event9 = Event9; event10 = Event10;
        event11 = Event11; event12 = Event12; event13 = Event13; event14 = Event14; event15 = Event15; event16 = Event16; event17 = Event17; event18 = Event18; event19 = Event19; event20 = Event20;
        event21 = Event21; event22 = Event22; event23 = Event23; event24 = Event24; event25 = Event25;
    }

    void PlayTutorial()
    {
        if (event1 != null && 3f <= bgmAudio.time) { return; }

        if (event2 != null && 6.233f <= bgmAudio.time) { return; }

        if (event3 != null && 10.366f <= bgmAudio.time) { return; }

        if (event4 != null) { return; }

        if (event5 != null) { return; }

        if (event6 != null) { return; }

        if (event7 != null) { return; }

        if (event8 != null) { return; }

        if (event9 != null) { return; }

        if (event10 != null) { return; }

        if (event11 != null) { return; }

        if (event12 != null) { return; }

        if (event13 != null) { return; }

        if (event14 != null) { return; }

        if (event15 != null) { return; }

        if (event16 != null) { return; }

        if (event17 != null) { return; }

        if (event18 != null) { return; }

        if (event19 != null) { return; }

        if (event20 != null) { return; }

        if (event21 != null) { return; }

        if (event22 != null) { return; }

        if (event23 != null) { return; }

        if (event24 != null) { return; }

        if (event25 != null) { return; }

        if (event26 != null) { return; }
    }

    public void Update()
    {
        //PlayTutorial();

        //Debug.Log("bgmAudio.time : " + bgmAudio.time);
        //Debug.Log("progressed : " + progressed[0]);
        if ( 3f <= bgmAudio.time && bgmAudio.time <= 3.020f)
        {
            if (progressed[0]==true) return;

            //화면 밖으로부터 달려옴
            iTween.MoveBy(gameObject, iTween.Hash("x", 7.8, "easeType", iTween.EaseType.linear, "time", 2f, "oncomplete","FallBack"));

            progressed[0] = true;
        }
        else if (6.233f <= bgmAudio.time && bgmAudio.time <= 6.253f)
        {
            if (progressed[1] == true) return;

            Debug.Log("here");

            quoteLabel.text = "깨어났구나 리츠";
            quoteLabel.AssumeNaturalSize();
            quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
            quote.SetActive(true);
            Invoke("HideQuote", 2f);

            progressed[1] = true;
        }
        else if(10.366f <= bgmAudio.time && bgmAudio.time <= 10.386f)
        {
            if (progressed[2] == true) return;

            quoteLabel.GetComponent<UILabel>().text = "폭동이 일어났어!";
            quoteLabel.AssumeNaturalSize();
            quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
            quote.SetActive(true);
            Invoke("HideQuote", 2f);

            progressed[2] = true;
        }
        else if(14.5f <= bgmAudio.time && bgmAudio.time <= 14.520f)
        {
            if (progressed[3] == true) return;

            quoteLabel.GetComponent<UILabel>().text = "무슨 일인지 알아봐야 해!";
            quoteLabel.AssumeNaturalSize();
            quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
            quote.SetActive(true);
            Invoke("HideQuote", 2f);

            progressed[3] = true;
        }
        else if(19.567f <= bgmAudio.time && bgmAudio.time <= 19.587f)
        {
            if (progressed[4] == true) return;

            playerController.Attack();

            progressed[4] = true;
        }
        else if (22.033f <= bgmAudio.time && bgmAudio.time <= 22.053f)
        {
            if (progressed[5] == true) return;

            playerController.Attack();

            progressed[5] = true;
        }
        else if (22.4f <= bgmAudio.time && bgmAudio.time <= 22.420f)
        {
            if (progressed[6] == true) return;

            playerController.Attack();

            progressed[6] = true;
        }
        else if (23.7f <= bgmAudio.time && bgmAudio.time <= 23.720f)
        {
            if (progressed[7] == true) return;

            quoteLabel.GetComponent<UILabel>().text = "공장 안 쪽까지 침입하다니!";
            quoteLabel.AssumeNaturalSize();
            quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
            quote.SetActive(true);
            Invoke("HideQuote", 2f);

            progressed[7] = true;
        }
        else if (27.467f <= bgmAudio.time && bgmAudio.time <= 27.487f)
        {
            if (progressed[8] == true) return;

            quoteLabel.GetComponent<UILabel>().text = "퇴치하자!";
            quoteLabel.AssumeNaturalSize();
            quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
            quote.SetActive(true);
            Invoke("HideQuote", 3f);

            progressed[8] = true;
        }
        else if (30.567f <= bgmAudio.time && bgmAudio.time <= 30.587f)
        {
            if (progressed[9] == true) return;

            quoteLabel.GetComponent<UILabel>().text = "앞장 설테니 따라와!";
            quoteLabel.AssumeNaturalSize();
            quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
            quote.SetActive(true);
            Invoke("HideQuote", 2f);

            progressed[9] = true;
        }
        else if (35.167f <= bgmAudio.time && bgmAudio.time <= 35.187f)
        {
            if (progressed[10] == true) return;

            playerController.Attack();

            progressed[10] = true;
        }
        else if (36.433f <= bgmAudio.time && bgmAudio.time <= 36.453f)
        {
            if (progressed[11] == true) return;

            playerController.Attack();

            progressed[11] = true;
        }
        else if (39.667f <= bgmAudio.time && bgmAudio.time <= 39.687f)
        {
            if (progressed[12] == true) return;

            //점멸
            quoteLabel.GetComponent<UILabel>().text = "화면 오른쪽/ 아래쪽(설정값을 따름)을 눌러서 점프할 수 있어.";
            quoteLabel.AssumeNaturalSize();
            quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
            quote.SetActive(true);
            Invoke("HideQuote", 2f);

            tutorialGradient.SetActive(true);
            tutorialGradient.GetComponent<TweenAlpha>().enabled = true;
            Invoke("DisableTutorialGradient", 6f);

            progressed[12] = true;
        }
        else if (44f <= bgmAudio.time && bgmAudio.time <= 44.020f)
        {
            if (progressed[13] == true) return;

            quoteLabel.GetComponent<UILabel>().text = "장애물이야 뛰어!";
            quoteLabel.AssumeNaturalSize();
            quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
            quote.SetActive(true);
            Invoke("HideQuote", 2f);

            progressed[13] = true;
        }
        else if (47.027f <= bgmAudio.time && bgmAudio.time <= 47.047f)
        {
            if (progressed[14] == true) return;

            playerController.Jump();

            progressed[14] = true;
        }
        else if (48.533f <= bgmAudio.time && bgmAudio.time <= 48.553f)
        {
            if (progressed[15] == true) return;

            //점멸
            quoteLabel.GetComponent<UILabel>().text = "화면 왼쪽/위쪽(설정값을 따름)을 누르면 공격할 수 있어.";
            quoteLabel.AssumeNaturalSize();
            quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
            quote.SetActive(true);
            Invoke("HideQuote", 2f);

            sp_TutorialGradient.transform.localPosition = new Vector3(-sp_TutorialGradient.transform.localPosition.x, sp_TutorialGradient.transform.localPosition.y, sp_TutorialGradient.transform.localPosition.z);
            tutorialGradient.SetActive(true);
            tutorialGradient.GetComponent<TweenAlpha>().enabled = true;
            Invoke("DisableTutorialGradient", 6f);

            progressed[15] = true;
        }
        else if (51.167f <= bgmAudio.time && bgmAudio.time <= 51.187f)
        {
            if (progressed[16] == true) return;

            playerController.Jump();

            progressed[16] = true;
        }
        else if (51.433f <= bgmAudio.time && bgmAudio.time <= 51.453f)
        {
            if (progressed[17] == true) return;

            playerController.Jump();

            progressed[17] = true;
        }
        else if (52.233f <= bgmAudio.time && bgmAudio.time <= 52.253f)
        {
            if (progressed[18] == true) return;

            quoteLabel.GetComponent<UILabel>().text = "적이 온다! 공격해!";
            quoteLabel.AssumeNaturalSize();
            quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
            quote.SetActive(true);
            Invoke("HideQuote", 2f);

            progressed[18] = true;
        }
        else if (55.167f <= bgmAudio.time && bgmAudio.time <= 55.187f)
        {
            if (progressed[19] == true) return;

            playerController.Jump();

            progressed[19] = true;
        }
        else if (59.333f <= bgmAudio.time && bgmAudio.time <= 59.353f)
        {
            if (progressed[20] == true) return;

            playerController.Jump();

            progressed[20] = true;
        }
        else if (59.6f <= bgmAudio.time && bgmAudio.time <= 59.620f)
        {
            if (progressed[21] == true) return;

            playerController.Jump();

            progressed[21] = true;
        }
        else if (62.2f <= bgmAudio.time && bgmAudio.time <= 62.220f)
        {
            if (progressed[22] == true) return;

            quoteLabel.GetComponent<UILabel>().text = "좋아, 돌파하자.";
            quoteLabel.AssumeNaturalSize();
            quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
            quote.SetActive(true);
            Invoke("HideQuote", 2f);

            progressed[22] = true;
        }
        else if (66.233f <= bgmAudio.time && bgmAudio.time <= 66.253f)
        {
            if (progressed[23] == true) return;

            quoteLabel.GetComponent<UILabel>().text = "놓치지 말고 따라와!";
            quoteLabel.AssumeNaturalSize();
            quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
            quote.SetActive(true);
            Invoke("HideQuote", 2f);

            progressed[23] = true;
        }
        else if (70.367f <= bgmAudio.time && bgmAudio.time <= 70.387f)
        {
            if (progressed[24] == true) return;

            //화면 밖으로 달려감
            iTween.MoveBy(gameObject, iTween.Hash("x", 8.4, "easeType", iTween.EaseType.linear, "time", 1.9f));

            progressed[24] = true;
        }
        else if (71.167f <= bgmAudio.time && bgmAudio.time <= 71.187f)
        {
            if (progressed[25] == true) return;

            playerController.Jump();
        }
        else if ( 71.9 <= bgmAudio.time )
        {
            Destroy(gameObject);
            return;
        }

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

    public void Event1(Action next)
    {
        //화면 밖으로부터 달려옴
        iTween.MoveBy(gameObject, iTween.Hash("x", 7.8, "easeType", iTween.EaseType.linear, "time", 2f, "oncomplete", "FallBack"));
        if (next != null) next();
    }

    public void Event2(Action next)
    {
        quoteLabel.text = "깨어났구나 리츠";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
        if (next != null) next();
    }

    public void Event3(Action next)
    {
        quoteLabel.GetComponent<UILabel>().text = "폭동이 일어났어!";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
        if (next != null) next();
    }

    public void Event4(Action next)
    {
        quoteLabel.GetComponent<UILabel>().text = "무슨 일인지 알아봐야 해!";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
        if (next != null) next();
    }

    public void Event5(Action next)
    {
        playerController.Attack();
        if (next != null) next();
    }

    public void Event6(Action next)
    {
        playerController.Attack();
        if (next != null) next();
    }

    public void Event7(Action next)
    {
        playerController.Attack();
        if (next != null) next();
    }

    public void Event8(Action next)
    {
        quoteLabel.GetComponent<UILabel>().text = "공장 안 쪽까지 침입하다니!";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
        if (next != null) next();
    }

    public void Event9(Action next)
    {
        quoteLabel.GetComponent<UILabel>().text = "퇴치하자!";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 3f);
        if (next != null) next();
    }

    public void Event10(Action next)
    {
        quoteLabel.GetComponent<UILabel>().text = "앞장 설테니 따라와!";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
        if (next != null) next();
    }

    public void Event11(Action next)
    {
        playerController.Attack();
        if (next != null) next();
    }

    public void Event12(Action next)
    {
        playerController.Attack();
        if (next != null) next();
    }

    public void Event13(Action next)
    {
        quoteLabel.GetComponent<UILabel>().text = "화면 오른쪽/ 아래쪽(설정값을 따름)을 눌러서 점프할 수 있어.";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);

        tutorialGradient.SetActive(true);
        tutorialGradient.GetComponent<TweenAlpha>().enabled = true;
        Invoke("DisableTutorialGradient", 6f);

        if (next != null) next();
    }

    public void Event14(Action next)
    {
        quoteLabel.GetComponent<UILabel>().text = "장애물이야! 뛰어!";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
        if (next != null) next();
    }

    public void Event15(Action next)
    {
        playerController.Jump();
        if (next != null) next();
    }

    public void Event16(Action next)
    {
        //점멸
        quoteLabel.GetComponent<UILabel>().text = "화면 왼쪽/위쪽(설정값을 따름)을 누르면 공격할 수 있어.";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);

        sp_TutorialGradient.transform.localPosition = new Vector3(-sp_TutorialGradient.transform.localPosition.x, sp_TutorialGradient.transform.localPosition.y, sp_TutorialGradient.transform.localPosition.z);
        tutorialGradient.SetActive(true);
        tutorialGradient.GetComponent<TweenAlpha>().enabled = true;
        Invoke("DisableTutorialGradient", 6f);
        if (next != null) next();
    }

    public void Event17(Action next)
    {
        playerController.Jump();
        if (next != null) next();
    }

    public void Event18(Action next)
    {
        playerController.Jump();
        if (next != null) next();
    }

    public void Event19(Action next)
    {
        quoteLabel.GetComponent<UILabel>().text = "적이 온다!공격해!";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
        if (next != null) next();
    }

    public void Event20(Action next)
    {
        playerController.Jump();
        if (next != null) next();
    }

    public void Event21(Action next)
    {
        playerController.Jump();
        if (next != null) next();
    }

    public void Event22(Action next)
    {
        playerController.Jump();
        if (next != null) next();
    }

    public void Event23(Action next)
    {
        quoteLabel.GetComponent<UILabel>().text = "좋아, 돌파하자.";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
        if (next != null) next();
    }

    public void Event24(Action next)
    {
        quoteLabel.GetComponent<UILabel>().text = "놓치지 말고 따라와!";
        quoteLabel.AssumeNaturalSize();
        quoteTop.GetComponent<UIWidget>().width = quoteLabel.width + 40;
        quote.SetActive(true);
        Invoke("HideQuote", 2f);
        if (next != null) next();
    }

    public void Event25(Action next)
    {
        //화면 밖으로 달려감
        iTween.MoveBy(gameObject, iTween.Hash("x", 8.4, "easeType", iTween.EaseType.linear, "time", 1.9f));
        if (next != null) next();
    }
}

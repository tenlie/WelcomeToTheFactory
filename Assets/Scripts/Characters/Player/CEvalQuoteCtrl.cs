using UnityEngine;
using System.Collections;

public enum EvalState
{
    PERFECT,
    GREAT,
    GOOD,
    MISS
}

public class CEvalQuoteCtrl : CFollowCtrl
{
    private UIWidget _widget;
    private Transform _tr;
    private bool _isReset;
    private float _time;

    private string _perfectText;
    private string _greatText;
    private string _goodText;
    private string _missText;

    private Color _yellowColor;
    private Color _whiteColor;
    private Color _redColor;

    public float Duration;
    public UISprite QuoteSprite;
    public UILabel QuoteLabel;

    public override void Start()
    {
        base.Start();
        _time = 0f;
        _perfectText = Localization.Get("Perfect") + "!";
        _greatText = Localization.Get("Great") + "!";
        _goodText = Localization.Get("Good") + "!";
        _missText = Localization.Get("Miss") + "!";
        _yellowColor = new Color(255f / 255f, 255f / 255f, 153f / 255f, 255f / 255f);
        _whiteColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
        _redColor = new Color(255f / 255f, 153f / 255f, 153f / 255f, 255f / 255f);
        _widget = GetComponent<UIWidget>();
        _widget.alpha = 0f;
        _tr = transform;
    }

    public override void Update()
    {
        //Debug.Log("Target.Pos : " + Target.transform.position);
        //base.Update();
        HandleDuration();
    }

    public override void Follow()
    {
        base.Follow();
    }

    void HandleDuration()
    {
        _time += Time.smoothDeltaTime;
        if (_time > Duration)
        {
            _widget.alpha = 0f;
        }
    }

    void FollowPivot()
    {
        _tr.position = Target.position;
    }

    public void ResetQuote(EvalState state)
    {
        Debug.Log("State : " + state);
        switch (state)
        {
            case EvalState.PERFECT:
                Debug.Log("Perfect");
                QuoteSprite.color = _yellowColor;
                QuoteLabel.text = _perfectText;
                break;
            case EvalState.GREAT:
                Debug.Log("Great");
                QuoteSprite.color = _whiteColor;
                QuoteLabel.text = _greatText;
                break;
            case EvalState.GOOD:
                Debug.Log("Good");
                QuoteSprite.color = _whiteColor;
                QuoteLabel.text = _goodText;
                break;
            case EvalState.MISS:
                Debug.Log("Miss");
                QuoteSprite.color = _redColor;
                QuoteLabel.text = _missText;
                break;
            default:
                break;
        }
        _widget.alpha = 1f;
        //Debug.Log("_time : " + _time);
        _time = 0f;
        //Debug.Log("_time : " + _time);
    }
}

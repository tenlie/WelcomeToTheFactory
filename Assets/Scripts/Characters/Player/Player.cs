using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private bool _isAttacking;
    private bool _isRunning;
    private bool _isSynced;
    private bool _hasNewRecord;
    private bool _speedChangeStopped;
    private int _speedChangeCount;
    private float _attackTimer = 0;
    private float _attackTime;

    public float AttackFrequency;
    public double SpeedAcceleration;
    public float SpeedforSlowingDown;
    public int StageBGMID;
    public int Score { get; set; }
    public int HighScore { get; set; }
    public bool IsDead { get; private set; }
    public int DeadJumpCount { get; set; }
    public bool AttackedRight { get; private set; }
    public bool IsFullyCharged { get; set; }
    public bool HasTriggeredFullyCharged { get; private set; }

    public PlayerController2D _controller { get; private set; }
    public Rigidbody2D _rigidbody2D { get; set; }
    public Collider2D _collider2D { get; set; }
    public AudioSource StageBGM;
    public Transform CameraFollow;
    public Collider2D CeilingBounds;
    public Transform Foregrounds;
    public ForegroundScrolling foregroundScrolling;
    public Collider2D AttackTrigger;
    public GameObject Player_Body;
    public GameObject Player_Hair;

    public Animator Animator;
    public readonly static int ANISTS_chr_litz_chg = Animator.StringToHash("Attack Layer.chr_litz_chg");

    public void Awake()
    {
        _isSynced = false;
        _isAttacking = false;
        _isRunning = false;
        _speedChangeStopped = false;
        _speedChangeCount = 1;
        _attackTimer = 0;

        Score = 0;
        IsDead = false;
        AttackedRight = false;
        IsFullyCharged = false;
        HasTriggeredFullyCharged = false;

        DeadJumpCount = 0;
        Animator.speed = 1.0f;

        _controller = GetComponent<PlayerController2D>();
        _collider2D = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        Player_Hair = GameObject.FindWithTag("Player_Hair");
        Player_Hair.SetActive(true);
        AttackTrigger.enabled = false;
    }

    public void Update()
    {
        /*
        if (_controller.State.IsGrounded)
        {
            Debug.Log("JUMP END : " + Foregrounds.transform.position.x);
        }
        */

        if (!IsDead)
        {
            if (LevelManager.Instance.IsGameStarted)
            {
                //배경 스크롤
                //var movement = (SpeedAcceleration * TimeMgr.Instance.DeltaTime);
                //foregroundScrolling.Scroll(CameraFollow.position, (float)movement);
                foregroundScrolling.Scroll(CameraFollow.position, (float)SpeedAcceleration * Time.smoothDeltaTime);
            }

            //인풋 처리
            HandleKeyboardInput();
        }
        else
        {
            //사망모션 처리
            _controller.AnimateDeathMotion();
        }

        Animator.SetBool("IsGrounded", _controller.State.IsGrounded);
        Animator.SetBool("IsJumping", _controller.State.IsJumpingUp);
        Animator.SetBool("IsDoubleJumping", _controller.State.IsDoubleJumping);
        Animator.SetBool("IsDead", IsDead);
    }

    void LateUpdate()
    {
        if (IsDead || !LevelManager.Instance.IsGameStarted)
            return;

        //배경 스크롤
        //foregroundScrolling.Scroll(CameraFollow.position, (float)SpeedAcceleration * TimeMgr.Instance.DeltaTime);
        //var movement = (SpeedAcceleration * Time.smoothDeltaTime);
        //foregroundScrolling.Scroll(CameraFollow.position, (float)SpeedAcceleration * Time.smoothDeltaTime);
    }

    //사망 처리
    public void Kill()
    {
        IsDead = true;
        Player_Hair.SetActive(false);
        CeilingBounds.enabled = false;
        Instantiate(_controller.DeathEffect, new Vector2(transform.position.x, transform.position.y + 1f), Quaternion.identity);
        GetComponent<BoxCollider2D>().offset = new Vector2(0, 0.33f);
        GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 0.7f);
        _controller.CalculateDistanceBetweenRays();
        _controller.PlatformMask = (1 << LayerMask.NameToLayer("NGUI"))
             | (1 << LayerMask.NameToLayer("Ground"));
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

    //인풋 처리
    public void HandleTouchInput(Vector2 touchPos)
    {
        if (IsDead)
            return;

        //터치 인풋
        //if (!UICamera.isOverUI && Input.touchCount > 0)
        //{
        //   Touch touch = Input.GetTouch(0);

        //  if (touch.phase == TouchPhase.Began)
        // {
        if (IsInputJump(touchPos))
        {
            _controller.Jump();
        }
        else
        {
            _controller.Attack();
        }
        //}
        return;
    }

    void HandleKeyboardInput()
    {
        if (IsDead)
            return;

        //키보드 인풋
        if (Input.GetButtonDown("Jump"))
        {
            _controller.Jump();
        }

        if (Input.GetButtonDown("Attack"))
        {
            _controller.Attack();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LevelManager.Instance.OnApplicationPause(!LevelManager.Instance.IsPaused);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("CLEAR"))
        {
            LevelManager.Instance.LevelClear();
        }
    }

    //노트 판정 및 효과음 재생
    void OnTriggerStay2D(Collider2D col)
    {
        if (!col.CompareTag("NoteCollider") || IsDead)
            return;

        Debug.Log("InputType: " + _controller.InputType);

        if (_controller.InputType.Equals(""))
            return;

        if (col.GetComponent<Note>()!=null)
        {
            col.GetComponent<Note>().PlayNote(_controller.InputType);
            col.enabled = false;
        }
        else if (col.GetComponent<MissNote>() != null)
        {
            Debug.Log("haha");
            Debug.Log("_controller.InputType : " + _controller.InputType);
            col.GetComponent<MissNote>().PlayNote(_controller.InputType);
        }

        _controller.InputType = "";
    }

    //노트 판정 및 효과음 재생
    void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("NoteCollider") || IsDead)
            return;

        Note note = col.GetComponent<Note>();

        if (note==null)
        {
            return;
        }

        if (note.isRealNote && !note.hadInput)
        {
            LevelManager.Instance.ShowEvalQuote(3);
        }
    }
}

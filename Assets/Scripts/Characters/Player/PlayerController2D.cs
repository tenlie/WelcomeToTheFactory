using UnityEngine;
using System.Collections;
using System;

public class PlayerController2D : MonoBehaviour
{
    private const float SkinWidth = 0.02f;
    private const int TotalHorizontalRays = 8;
    private const int TotalVerticalRays = 4;

    private static readonly float SlopeLimitTangant = Mathf.Tan(75f * Mathf.Deg2Rad);
    private static readonly int ANISTS_CHARGE = Animator.StringToHash("Attack Layer.CHARGE");

    public LayerMask PlatformMask;
    public Animator MainAnim;
    public Animator RightAttackAnim;
    public Animator LeftAttackAnim;
    public Animator DoubleJumpAnim;
    public Collider2D AttackCol;
    public ControllerParameters2D DefaultParameters;

    public ControllerState2D State { get; private set; }
    public ControllerParameters2D Parameters { get { return _overrideParameters ?? DefaultParameters; } }
    public GameObject StandingOn { get; private set; }
    public Vector2 Velocity { get { return _velocity; } }
    public Vector3 PlatformVelocity { get; private set; }
    public GameObject PlayerBody;
    public Transform Foregrounds;

    public bool HandleCollisions { get; set; }
    public bool JumpInputSaved { get; set; }
    public bool IsFullyCharged { get; private set; }
    public bool HasTriggeredFullyCharged { get; private set; }
    public string InputType { get; set; }
    public float SpeedForSlowingDown;
    public DateTime JumpInputSavedTime { get; set; }
    public GameObject JumpEffect;
    public GameObject DeathEffect;
    //public GameObject[] DustEffects;

    public Vector2 _velocity;
    private Transform _transform;
    private Vector3 _localScale;
    private BoxCollider2D _boxCollider;
    private ControllerParameters2D _overrideParameters;
    public bool IsDead;
    private float _jumpCooltime;
    private int _jumpCount;
    private int _deadJumpCount;
    private bool _isAttacking;
    private bool _attackedRight;
    private float _attackCooltime;
    private float _attackColCooltime;
    private float _attackTime;
    private GameObject _lastStandingOn;

    private Vector3
        _activeLocalPlatformPoint,
        _activeGlobalPlatformPoint;

    private Vector3
        _raycastTopLeft,
        _raycastBottomRight,
        _raycastBottomLeft;

    private float
        _verticalDistanceBetweenRays,
        _horizontalDistanceBetweenRays,
        _colliderWidth,
        _colliderHeight;

    public void Awake()
    {
        State = new ControllerState2D();
        _boxCollider = GetComponent<BoxCollider2D>();
        _transform = transform;
        _localScale = transform.localScale;
        _jumpCount = 0;
        _deadJumpCount = 0;
        _isAttacking = false;
        _attackedRight = false;
        IsDead = false;

        InputType = "";
        HandleCollisions = true;
        IsFullyCharged = false;
        HasTriggeredFullyCharged = false;

        CalculateDistanceBetweenRays();
    }

    public void CalculateDistanceBetweenRays()
    {
        _colliderWidth = _boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2 * SkinWidth);
        _horizontalDistanceBetweenRays = _colliderWidth / (TotalVerticalRays - 1);

        _colliderHeight = _boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2 * SkinWidth);
        _verticalDistanceBetweenRays = _colliderHeight / (TotalHorizontalRays - 1);
    }

    public void AddForce(Vector2 force)
    {
        _velocity += force;
    }

    public void SetForce(Vector2 force)
    {
        _velocity = force;
    }

    public void SetHorizontalForce(float x)
    {
        _velocity.x = x;
    }

    public void SetVerticalForce(float y)
    {
        _velocity.y = y;
    }

    public void SlowDownToStopForward(float deltatime)
    {
        _velocity.x -= deltatime;
        if (_velocity.x < 0)
            _velocity.x = 0;
    }

    public void SlowDownToStopBackward(float deltatime)
    {
        _velocity.x += deltatime;
        if (_velocity.x > 0)
            _velocity.x = 0;
    }

    //사망
    public void Kill()
    {
        //Player_Hair.SetActive(false);
        //CeilingBounds.enabled = false;
        IsDead = true;
        Instantiate(DeathEffect, new Vector2(_transform.position.x, _transform.position.y + 1f), Quaternion.identity);
        _boxCollider.offset = new Vector2(0, 0.33f);
        _boxCollider.size = new Vector2(0.5f, 0.7f);
        CalculateDistanceBetweenRays();
        PlatformMask = (1 << LayerMask.NameToLayer("Platforms"))
                     | (1 << LayerMask.NameToLayer("Enemies"))
                     | (1 << LayerMask.NameToLayer("Obstacles"));
    }

    //점프
    public void Jump()
    {
        //점프 쿨타임
        if (_jumpCooltime > 0) return;

        if (State.IsGrounded)
        {
            //Debug.Log("JUM PPOS : " + Foregrounds.transform.position.x);
            SetVerticalForce(Parameters.JumpMagnitude);
            _jumpCount = 1;
            _jumpCooltime = 0.01f;
            GameObject jumpEffect = Instantiate(JumpEffect, new Vector3(_transform.position.x-0.6f, _transform.position.y, _transform.position.z), Quaternion.identity) as GameObject;
            InputType = "jump";
        }
        else
        {
            if (_jumpCount < 2)
            {
                State.IsDoubleJumping = true;
                SetVerticalForce(0);
                SetVerticalForce(Parameters.JumpMagnitude);
                _jumpCount++;
                _jumpCooltime = 0.01f;
                DoubleJumpAnim.Play("DoubleJumpEffect");
                InputType = "jump";
            }
            else
            {
                Debug.Log("JumpInputSaved");
                JumpInputSaved = true;
                JumpInputSavedTime = DateTime.UtcNow;
            }
        }
    }

    //공격
    public void Attack()
    {
        if (!_isAttacking)
        {
            AttackCol.enabled = true;
            _isAttacking = true;
            _attackCooltime = Parameters.AttackFrequency;
            _attackColCooltime = 0.3f;

            if (IsFullyCharged)
            {
                ChargeAttack();
            }
            else
            {
                if (Time.time - _attackTime > 0.7f)
                {
                    _attackedRight = false;
                }
                else
                {
                    _attackedRight = !_attackedRight;
                }

                if (!_attackedRight)
                {
                    AttackRight();
                }
                else
                {
                    AttackLeft();
                }
            }
        }
        _attackTime = Time.time;
    }

    public void LateUpdate()
    {
        //중력 처리
        _velocity.y += Parameters.Gravity * Time.deltaTime;

        //점프 쿨타임 처리
        if (_jumpCooltime > 0)
            _jumpCooltime -= Time.smoothDeltaTime;

        //공격 쿨타임 처리
        if (_isAttacking)
        {
            if (_attackCooltime > 0)
            {
                _attackCooltime -= Time.smoothDeltaTime;
            }
            else
            {
                _isAttacking = false;
            }
        }

        //공격 컬라이더 쿨타임 처리
        if (_attackColCooltime > 0)
        {
            _attackColCooltime -= Time.smoothDeltaTime;
        }
        else
        {
            AttackCol.enabled = false;
        }

        if (IsFullyCharged && !HasTriggeredFullyCharged) FullyCharged();

        //이동 처리
        Move(Velocity * Time.deltaTime);
    }

    private void Move(Vector2 deltaMovement)
    {
        var wasGrounded = State.IsCollidingBelow;
        State.Reset();

        if (HandleCollisions)
        {
            //Ray 원점 계산
            CalculateRayOrigins();

            if (deltaMovement.y < 0 && wasGrounded)
                HandleVerticalSlope(ref deltaMovement);

            //수평이동 처리
            if (Mathf.Abs(deltaMovement.x) > 0.001f)
                MoveHorizontally(ref deltaMovement);

            //수직이동 처리
            MoveVertically(ref deltaMovement);

            //위치 조정
            CorrectHorizontalPlacement(ref deltaMovement, true);
            CorrectHorizontalPlacement(ref deltaMovement, false);
        }

        if (Time.deltaTime > 0)
            _velocity = deltaMovement / Time.deltaTime;

        _transform.Translate(deltaMovement, Space.World);

        _velocity.x = Mathf.Min(_velocity.x, Parameters.MaxVelocity.x);
        _velocity.y = Mathf.Min(_velocity.y, Parameters.MaxVelocity.y);

        if (State.IsMovingUpSlope) _velocity.y = 0;

        if (StandingOn != null)
        {
            _activeGlobalPlatformPoint = transform.position;
            _activeLocalPlatformPoint = StandingOn.transform.InverseTransformPoint(transform.position);

            if (_lastStandingOn != StandingOn)
            {
                if (_lastStandingOn != null)
                    _lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);

                StandingOn.SendMessage("ControllerEnter2D", this, SendMessageOptions.DontRequireReceiver);
                _lastStandingOn = StandingOn;
            }
            else if (_lastStandingOn != null)
                StandingOn.SendMessage("ControllerStay2D", this, SendMessageOptions.DontRequireReceiver);
        }
        else if (_lastStandingOn != null)
        {
            _lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
            _lastStandingOn = null;
        }
    }

    //이동하는 플랫폼의 경우 플레이어가 플랫폼과 같이 이동하도록 처리
    private void HandlePlatforms()
    {
        if (StandingOn != null)
        {
            var newGlobalPlatformPoint = StandingOn.transform.TransformPoint(_activeLocalPlatformPoint);
            var moveDistance = newGlobalPlatformPoint - _activeGlobalPlatformPoint;

            if (moveDistance != Vector3.zero)
            {
                transform.Translate(moveDistance, Space.World);
            }

            PlatformVelocity = (newGlobalPlatformPoint - _activeGlobalPlatformPoint) / Time.deltaTime;
        }
        else
        {
            PlatformVelocity = Vector3.zero;
        }

        StandingOn = null;
    }

    private void CalculateRayOrigins()
    {
        var size = new Vector2(_boxCollider.size.x * Mathf.Abs(_localScale.x), _boxCollider.size.y * Mathf.Abs(_localScale.y)) / 2;
        var center = new Vector2(_boxCollider.offset.x * _localScale.x, _boxCollider.offset.y * _localScale.y);

        _raycastTopLeft = _transform.position + new Vector3(center.x - size.x + SkinWidth, center.y + size.y - SkinWidth);
        _raycastBottomRight = _transform.position + new Vector3(center.x + size.x - SkinWidth, center.y - size.y + SkinWidth);
        _raycastBottomLeft = _transform.position + new Vector3(center.x - size.x + SkinWidth, center.y - size.y + SkinWidth);
    }

    private void MoveHorizontally(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        var rayDirection = isGoingRight ? Vector2.right : Vector2.left;
        var rayOrigin = isGoingRight ? _raycastBottomRight : _raycastBottomLeft;

        for (var i = 0; i < TotalHorizontalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * _verticalDistanceBetweenRays));
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var rayCastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
            if (!rayCastHit)
                continue;

            if (i == 0 && HandleHorizontalSlope(ref deltaMovement, Vector2.Angle(rayCastHit.normal, Vector2.up), isGoingRight))
                break;

            deltaMovement.x = rayCastHit.point.x - rayVector.x;
            rayDistance = Mathf.Abs(deltaMovement.x);

            if (isGoingRight)
            {
                deltaMovement.x -= SkinWidth;
                State.IsCollidingRight = true;
            }
            else
            {
                deltaMovement.x += SkinWidth;
                State.IsCollidingLeft = true;
            }

            if (rayDistance < SkinWidth + 0.0001f)
                break;
        }
    }

    private void MoveVertically(ref Vector2 deltaMovement)
    {
        if (deltaMovement.y < 0)
        {
            State.IsJumpingUp = false;
            State.IsDoubleJumping = false;
        }
        else
        {
            State.IsJumpingUp = true;
        }

        var isGoingUp = deltaMovement.y > 0;
        var rayDistance = Mathf.Abs(deltaMovement.y) + SkinWidth;
        var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
        var rayOrigin = isGoingUp ? _raycastTopLeft : _raycastBottomLeft;

        rayOrigin.x += deltaMovement.x;

        var standingOnDistance = float.MaxValue;

        for (var i = 0; i < TotalVerticalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x + (i * _horizontalDistanceBetweenRays), rayOrigin.y);
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
            if (!raycastHit)
                continue;

            if (!isGoingUp)
            {
                var verticalDistanceToHit = _transform.position.y - raycastHit.point.y;
                if (verticalDistanceToHit < standingOnDistance)
                {
                    standingOnDistance = verticalDistanceToHit;
                    StandingOn = raycastHit.collider.gameObject;
                }
            }

            deltaMovement.y = raycastHit.point.y - rayVector.y;
            rayDistance = Mathf.Abs(deltaMovement.y);

            if (isGoingUp)
            {
                deltaMovement.y -= SkinWidth;
                State.IsCollidingAbove = true;
            }
            else
            {
                deltaMovement.y += SkinWidth;
                State.IsCollidingBelow = true;
            }

            if (!isGoingUp && deltaMovement.y > 0.0001f)
            {
                State.IsMovingUpSlope = true;
            }

            if (rayDistance < SkinWidth + 0.0001f)
            {
                break;
            }
        }
    }

    private void CorrectHorizontalPlacement(ref Vector2 deltaMovement, bool isRight)
    {
        var halfWidth = (_boxCollider.size.x * _localScale.x) / 2f;
        var rayOrigin = isRight ? _raycastBottomRight : _raycastBottomLeft;

        if (isRight)
        {
            rayOrigin.x -= (halfWidth - SkinWidth);
        }
        else
        {
            rayOrigin.x += (halfWidth - SkinWidth);
        }

        var rayDirection = isRight ? Vector2.right : Vector2.left;
        var offset = 0f;

        for (var i = 1; i < TotalHorizontalRays - 1; i++)
        {
            var rayVector = new Vector2(deltaMovement.x + rayOrigin.x, deltaMovement.y + rayOrigin.y + (i * _verticalDistanceBetweenRays));
            Debug.DrawRay(rayVector, rayDirection * halfWidth, isRight ? Color.cyan : Color.magenta);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, halfWidth, PlatformMask);
            if (!raycastHit)
                continue;

            offset = isRight ? ((raycastHit.point.x - _transform.position.x) - halfWidth) : (halfWidth - (_transform.position.x - raycastHit.point.x));
        }

        deltaMovement.x += offset;
    }

    //경사 처리
    private void HandleVerticalSlope(ref Vector2 deltaMovement)
    {
        var center = (_raycastBottomLeft.x + _raycastBottomRight.x) / 2;
        var direction = -Vector2.up;

        var slopeDistance = SlopeLimitTangant * (_raycastBottomRight.x - center);
        var slopeRayVector = new Vector2(center, _raycastBottomLeft.y);

        Debug.DrawRay(slopeRayVector, direction * slopeDistance, Color.yellow);

        var raycastHit = Physics2D.Raycast(slopeRayVector, direction, slopeDistance, PlatformMask);
        if (!raycastHit)
            return;

        var isMovingDownSlope = Mathf.Sign(raycastHit.normal.x) == Mathf.Sign(deltaMovement.x);
        if (!isMovingDownSlope)
            return;

        var angle = Vector2.Angle(raycastHit.normal, Vector2.up);
        if (Mathf.Abs(angle) < 0.0001f)
            return;

        State.IsMovingUpSlope = true;
        State.SlopeAngle = angle;
        deltaMovement.y = raycastHit.point.y - slopeRayVector.y;
    }

    //경사 처리
    private bool HandleHorizontalSlope(ref Vector2 deltaMovement, float angle, bool isGoingRight)
    {
        if (Mathf.RoundToInt(angle) == 90) return false;

        if (angle > Parameters.SlopeLimit)
        {
            deltaMovement.x = 0;
            return true; 
        }
         
        if (deltaMovement.y > 0.07f)
            return true;

        deltaMovement.x += isGoingRight ? -SkinWidth : SkinWidth;
        deltaMovement.y += Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * deltaMovement.x);
        State.IsMovingUpSlope = true;
        State.IsCollidingBelow = true;
        return true;
    }

    //오른손 공격
    private void AttackRight()
    {
        InputType = "attack";
        MainAnim.SetTrigger("RightAttack");
        RightAttackAnim.Play("RightAttackEffect");
    }

    //왼손 공격
    private void AttackLeft()
    {
        InputType = "attack";
        MainAnim.SetTrigger("LeftAttack");
        LeftAttackAnim.Play("LeftAttackEffect");
    }

    //차징 공격
    private void ChargeAttack()
    {
        InputType = "attack";

        if (MainAnim.GetCurrentAnimatorStateInfo(1).fullPathHash != ANISTS_CHARGE)
        {
            MainAnim.SetTrigger("ChargeAttack");
        }
        else
        {
            AttackRight();
        }

        IsFullyCharged = false;
        HasTriggeredFullyCharged = false;
    }

    //차징 준비완료
    private void FullyCharged()
    {
        MainAnim.SetTrigger("FullyCharged");
        HasTriggeredFullyCharged = true;
    }

    //사망 모션 처리
    public void AnimateDeathMotion()
    {
        //Debug.Log("_deadJumpCount:" + _deadJumpCount);
        //Debug.Log("velocity.x: " + Velocity.x);

        if (_deadJumpCount == 0)
        {
            if (Velocity.x > 0)
            {
                SetForce(new Vector2(7, 13));
            }
            else
            {
                SetForce(new Vector2(-7, 13));
            }

            _deadJumpCount++;
        }
        else if (_deadJumpCount == 1)
        {
            if (Velocity.x > 0)
            {
                SlowDownToStopForward(Time.deltaTime * Parameters.SpeedForSlowingDown);
            }
            else if (Velocity.x < 0)
            {
                SlowDownToStopBackward(Time.deltaTime * Parameters.SpeedForSlowingDown);
            }

            _deadJumpCount++;
        }
        else if (_deadJumpCount == 2)
        {
            if (Velocity.x > 0)
            {
                SlowDownToStopForward(Time.deltaTime * Parameters.SpeedForSlowingDown);
            }
            else if (Velocity.x < 0)
            {
                SlowDownToStopBackward(Time.deltaTime * Parameters.SpeedForSlowingDown);
            }

            _deadJumpCount++;
        }
        else
        {
            if (Velocity.x > 0)
            {
                SlowDownToStopForward(Time.deltaTime * Parameters.SpeedForSlowingDown);
            }
            else if (Velocity.x < 0)
            {
                //SetHorizontalForce(0);
                SlowDownToStopBackward(Time.deltaTime * Parameters.SpeedForSlowingDown);
            }
        }

        if (Velocity.x > 0.1f)
        {
            PlayerBody.transform.Rotate(0, 0, -7);
        }
        else if (Velocity.x < -0.1f)
        {
            PlayerBody.transform.Rotate(0, 0, 7);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var parameters = other.gameObject.GetComponent<ControllerPhysicsVolume2D>();
        if (parameters == null)
            return;
        _overrideParameters = parameters.Parameters;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        var parameters = other.gameObject.GetComponent<ControllerPhysicsVolume2D>();
        if (parameters == null)
            return;
        _overrideParameters = null;
    }
}

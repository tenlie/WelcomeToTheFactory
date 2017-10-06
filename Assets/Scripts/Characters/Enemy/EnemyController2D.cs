using UnityEngine;

public class EnemyController2D : CharacterController2D 
{
    //private const float SkinWidth = 0.02f;
    //private const int TotalHorizontalRays = 8;
    //private const int TotalVerticalRays = 4;
    //private static readonly float SlopeLimitTangant = Mathf.Tan(75f * Mathf.Deg2Rad);

    //public LayerMask PlatformMask;
    //public ControllerParameters2D DefaultParameters;

    //public ControllerState2D State { get; private set; }
    //public ControllerParameters2D Parameters { get { return _overrideParameters ?? DefaultParameters; } }
    //public GameObject StandingOn { get; private set; }
    //public Vector2 Velocity { get { return _velocity; } }
    //public Vector3 PlatformVelocity { get; private set; }

    //public bool HandleCollisions { get; set; }

    //private Vector2 _velocity;
    //private Transform _transform;
    //private Vector3 _localScale;
    //private BoxCollider2D _boxCollider;
    //private ControllerParameters2D _overrideParameters;
    //private float _jumpIn;
    //private GameObject _lastStandingOn;

    //private Vector3
    //    _activeLocalPlatformPoint,
    //    _activeGlobalPlatformPoint;

    //private Vector3
    //    _raycastTopLeft,
    //    _raycastBottomRight,
    //    _raycastBottomLeft;

    //private float
    //    _verticalDistanceBetweenRays,
    //    _horizontalDistanceBetweenRays,
    //    _colliderWidth,
    //    _colliderHeight;

    /*
    public override void Awake()
    {
        //_boxCollider = GetComponent<BoxCollider2D>();
        //_transform = transform;
        //_localScale = transform.localScale;
        //State = new ControllerState2D();
        //HandleCollisions = true;
        //CalculateDistanceBetweenRays();
    }
    public override void CalculateDistanceBetweenRays()
    {
        _colliderWidth = _boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2 * SkinWidth);
        _horizontalDistanceBetweenRays = _colliderWidth / (TotalVerticalRays - 1);

        _colliderHeight = _boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2 * SkinWidth);
        _verticalDistanceBetweenRays = _colliderHeight / (TotalHorizontalRays - 1);
    }

    public override void SetForce(Vector2 force)
    {
        _velocity = force;
    }

    public override void AddForce(Vector2 force)
    {
        _velocity += force;
    }

    public override void SetHorizontalForce(float x)
    {
        _velocity.x = x;
    }

    public override void SetVerticalForce(float y)
    {
        _velocity.y = y;
    }
    public override void Jump()
    {
        // TODO: Moving platform support
        SetVerticalForce(Parameters.JumpMagnitude);

        //점프 주가기 설정되어 있으면 이단점프를 자유롭게 할 수 없다.
        //점프 주기
        //_jumpIn = Parameters.JumpFrequency;
    }
    */

    public override void LateUpdate()
    {
        //_velocity.y += Parameters.Gravity * Time.smoothDeltaTime;
        //_jumpIn -= Time.deltaTime;
        //Debug.Log("Parameters.Gravity: "+Parameters.Gravity);
        //Debug.Log("velocity.x: " + _velocity.x + " velocity.y: " + _velocity.y);

        //if (!Player.IsDead)
        //{
            //Move(Velocity * Time.smoothDeltaTime);
        //}
    }
    /*
    private void Move(Vector2 deltaMovement)
    {
        var wasGrounded = State.IsCollidingBelow;
        State.Reset();

        if (HandleCollisions)
        {
            // This is what makes JUMP ACTION weird!!!
            //if (CanHandlePlatforms)
            HandlePlatforms();

            CalculateRayOrigins();

            if (deltaMovement.y < 0 && wasGrounded)
                HandleVerticalSlope(ref deltaMovement);

            // Casts rays to calculates how far the Character can move horizontally
            if (Mathf.Abs(deltaMovement.x) > 0.001f)
                MoveHorizontally(ref deltaMovement);

            // Casts rays to calculates how far the Character can move vertically
            MoveVertically(ref deltaMovement);

            CorrectHorizontalPlacement(ref deltaMovement, true);
            CorrectHorizontalPlacement(ref deltaMovement, false);
        }

        // This is the Function that Actually Moves the Character
        _transform.Translate(deltaMovement, Space.World);

        // TODO: Additional moving platform code

        //if (Time.deltaTime > 0)
        //    _velocity = deltaMovement / Time.deltaTime;

        // Limits the maximum speed
        _velocity.x = Mathf.Min(_velocity.x, Parameters.MaxVelocity.x);
        _velocity.y = Mathf.Min(_velocity.y, Parameters.MaxVelocity.y);

        if (State.IsMovingUpSlope)
            _velocity.y = 0;

        if (StandingOn != null)
        {
            _activeGlobalPlatformPoint = transform.position;
            _activeLocalPlatformPoint = StandingOn.transform.InverseTransformPoint(transform.position);

            //Debug.DrawLine(transform.position, _activeGlobalPlatformPoint);
            //Debug.DrawLine(transform.position, _activeLocalPlatformPoint);

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
            rayOrigin.x -= (halfWidth - SkinWidth);
        else
            rayOrigin.x += (halfWidth - SkinWidth);

        var rayDirection = isRight ? Vector2.right : Vector2.left;
        var offset = 0f;

        for (var i = 1; i < TotalHorizontalRays - 1; i++)
        {
            var rayVector = new Vector2(deltaMovement.x + rayOrigin.x, deltaMovement.y + rayOrigin.y + (i * _verticalDistanceBetweenRays));
            Debug.DrawRay(rayVector, rayDirection * halfWidth, isRight ? Color.cyan : Color.magenta);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, halfWidth, PlatformMask);
            if (!raycastHit)
                continue;

            //offset = isRight ? ((raycastHit.point.x - _transform.position.x) - halfWidth) : (halfWidth - (_transform.position.x - raycastHit.point.x));		                                                                                              ));
            offset = isRight ? ((raycastHit.point.x - _transform.position.x) - halfWidth) : (halfWidth - (_transform.position.x - raycastHit.point.x));
        }

        deltaMovement.x += offset;
    }

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

        // ReSharper diaable CompareOfFloatsByEqualityOperator

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

    private bool HandleHorizontalSlope(ref Vector2 deltaMovement, float angle, bool isGoingRight)
    {
        if (Mathf.RoundToInt(angle) == 90)
            return false;

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
    */
}


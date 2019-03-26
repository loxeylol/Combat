using Neox.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IHittable
{
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [Header("Input")]
    [SerializeField] private string _horizontalAxis = "Horizontal";
    [SerializeField] private string _verticalAxis = "Vertical";
    [SerializeField] private KeyCode _fireKey = KeyCode.Space;

    [Header("Movement Values")]
    [SerializeField, Range(0f, 2f)] private float _movementSpeed;
    [SerializeField] private bool _rotateFree;
    [SerializeField, Range(0, 720)] private int _freeRotationSpeed;
    [SerializeField, Range(0f, 1f)] private float _intervalRotationDelay = .25f;

    [Header("Bullet and Spawner")]
    [SerializeField] private BulletBehaviour _bulletPrefab;
    [SerializeField] private Transform _bulletSpawn;

    [Header("Knockback")]
    [SerializeField, Range(0f, 2f)] private float _knockbackDuration = 1f;
    [SerializeField, Range(0f, 20f)] private float _knockbackPower = 5f;
    [SerializeField] private InterpolationTypes _knockbackInterpolation;
    [SerializeField] private int _hitRotationSpeed = 360;

    private BoxCollider _collider;
    private Rigidbody _rb;
    private MeshRenderer[] _meshRenderers;
    private BulletBehaviour _currentBullet;

    private Vector2 _input;
    private Vector3 _startPos;

    private bool _isHitting;
    private float _bulletTimer;
    private float _rotateTimer;

    private int _score = 0;
    public Action<int> scoreChanged;

    private const float MIN_SPEED = 0f;
    private const float MAX_SPEED = 2f;

    // --- Properties -------------------------------------------------------------------------------------------------
    public bool IsInvincible { get; set; }
    public bool CanMove { get; set; }
    //for bullet rotation and spawn
    public Transform BulletSpawn { get => _bulletSpawn; }
    public float RotationInput { get { return _input.x; } }
    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            scoreChanged?.Invoke(_score);
        }
    }
    private float RotationInterval { get { return 360f / SettingsManager.PlayerRotationSteps; } }
    private float Speed { get { return _movementSpeed; } set { _movementSpeed = Mathf.Clamp(value, MIN_SPEED, MAX_SPEED); } }
    private bool CanShoot { get { return CanMove && _currentBullet == null; } }

    private bool IsInvisible
    {
        get { return _meshRenderers[0].enabled; }
        set
        {
            foreach (MeshRenderer mr in _meshRenderers)
                mr.enabled = !value;
        }
    }

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
        _meshRenderers = GetComponentsInChildren<MeshRenderer>(true);

        IsInvincible = false;
        IsInvisible = SettingsManager.InvisibleTankMode;
        _rotateTimer = 0f;
        _bulletTimer = 0f;
        CanMove = true;
    }

    private void Update()
    {
        if (CanMove)
        {
            _input = new Vector2(
                x: SignZero(Input.GetAxisRaw(_horizontalAxis)),
                y: SignZero(Input.GetAxisRaw(_verticalAxis)));

            Vector3 rotation = SettingsManager.FreePlayerRotation
                ? GetFreeRotation(_input.x * _freeRotationSpeed)
                : GetIntervalRotation(_input.x);

            transform.Rotate(rotation);

            MovePlayer(_input.y);
        }


        //if (CanShoot && Input.GetKey(_fireKey))
        if (Input.GetKeyDown(_fireKey)&& CanShoot)
        {
            FireWithMode(SettingsManager.SelectedFireMode);
        }

        if (SettingsManager.InvisibleTankMode)
        {
            IsInvisible = CanShoot == false || CanMove == false;
        }
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    void IHittable.OnHit(BulletBehaviour bullet, Collision collision)
    {
        if (IsInvincible)
            return;

        if (bullet.Player == this && SettingsManager.FriendlyFire)
        {
            Score--;
        }
        else
        {
            bullet.Player.Score++;
        }

        Vector3 playerNormal = collision.contacts[0].normal;
        Vector3 inBetweenDirection = (bullet.transform.forward + -playerNormal) / 2;
        Debug.DrawRay(playerNormal, inBetweenDirection, Color.red);

        StartCoroutine(GotHitRoutine(inBetweenDirection));
    }

    public void ClearBullet()
    {
        _currentBullet = null;
    }

    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private Vector3 GetFreeRotation(float rotationSpeed)
    {
        return Vector3.up * rotationSpeed * Time.deltaTime;
    }

    private Vector3 GetIntervalRotation(float rotationInput)
    {
        _rotateTimer -= Time.deltaTime;

        if (_rotateTimer > 0f)
            return Vector3.zero;

        _rotateTimer += _intervalRotationDelay;
        return Vector3.up * RotationInterval * rotationInput;
    }

    // --------------------------------------------------------------------------------------------
    private IEnumerator GotHitRoutine(Vector3 direction)
    {
        CanMove = false;
        IsInvincible = true;

        float startTime = Time.time;
        float t = 0f;

        float startRotation = transform.rotation.y;
        float endRotation = startRotation + (_hitRotationSpeed * _knockbackDuration);

        while (t < 1f)
        {
            t = (Time.time - startTime) / _knockbackDuration;
            float rot = NeoxMath.Lerp(startRotation, endRotation, t, _knockbackInterpolation);
            transform.rotation = Quaternion.Euler(0f, rot, 0f);
            float power = NeoxMath.Lerp(_knockbackPower, 0f, t, _knockbackInterpolation);
            transform.position += direction * power * Time.deltaTime;

            yield return null;
        }

        CanMove = true;
        yield return new WaitForSeconds(1f);
        IsInvincible = false;
    }

    private void FireWithMode(FireModes fireMode)
    {
        switch (fireMode)
        {
            case FireModes.Straight:
            default:
                FireStraight();
                break;
        }
    }

    private void MovePlayer(float input)
    {
        if (input > 0f)
        {
            transform.position += transform.forward * _movementSpeed * Time.deltaTime;
        }
    }

    private void FireStraight()
    {
        
        _currentBullet = MonoFactory.GetFactoryObject<BulletBehaviour>();
        _currentBullet.Player = this;
        _currentBullet.transform.position = _bulletSpawn.position;
        _currentBullet.transform.rotation = transform.rotation;
    }

    private float SignZero(float f)
    {
        return f > 0f ? 1f
            : f < 0f ? -1f
            : 0f;
    }



    // --------------------------------------------------------------------------------------------
}


// **************************************************************************************************************************************************
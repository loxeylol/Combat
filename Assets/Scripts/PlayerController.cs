using Neox.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [Header("Input")]
    [SerializeField] private string _horizontalAxis = "Horizontal";
    [SerializeField] private string _verticalAxis = "Vertical";
    [SerializeField] private KeyCode _fireKey = KeyCode.Space;

    [Header("Movement Values")]
    [SerializeField, Range(0f, 20f)] private float _movementSpeed;
    [SerializeField] private bool _rotateFree;
    [SerializeField, Range(0, 720)] private int _freeRotationSpeed;
    [SerializeField, Range(0f, 1f)] private float _intervalRotationDelay = .25f;
    [SerializeField] private float _gotHitRotationSpeed = 20;

    [Header("Bullet and Spawner")]
    [SerializeField] private BulletBehaviour _bulletPrefab;
    [SerializeField] private Transform _bulletSpawn;

    private BoxCollider _collider;
    private Rigidbody _rb;
    private MeshRenderer _playerMesh;
    [SerializeField] private MeshRenderer _turretMesh;
    [SerializeField] private MeshRenderer _playerTextMesh;
    private BulletBehaviour _currentBullet;




    private Vector2 _input;
    private Vector3 _startPos;

    private bool _isHitting;
    private bool _canShoot;
    private float _bulletTimer;
    private float _rotateTimer;

    private const float MIN_SPEED = 1f;
    private const float MAX_SPEED = 20f;

    // --- Properties -------------------------------------------------------------------------------------------------
    public delegate void PlayerScoreDelegate(int score);
    public PlayerScoreDelegate getScore;
    public bool IsInvincible { get; set; }
    public bool WasHit { get; set; }
    public bool CanMove { get; set; }
    public bool IsPlayerMeshVisible { get => !SettingsManager.InvisibleTankMode; }
    //for bullet rotation
    public float RotationInput { get { return _input.x; } }
    public int Score { get; set; }
    private float RotationInterval { get { return 360f / SettingsManager.PlayerRotationSteps; } }
    private float Speed { get { return _movementSpeed; } set { _movementSpeed = Mathf.Clamp(value, MIN_SPEED, MAX_SPEED); } }
    private bool CanShoot { get { return _currentBullet == null; } }

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {

        _collider = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
        _playerMesh = GetComponent<MeshRenderer>();

        IsInvincible = false;
        _playerMesh.enabled = IsPlayerMeshVisible ? true : false;
        _turretMesh.enabled = IsPlayerMeshVisible ? true : false;
        _playerTextMesh.enabled = IsPlayerMeshVisible ? true : false;
        _rotateTimer = 0f;
        _bulletTimer = 0f;
        CanMove = true;
        _canShoot = true;
    }

    private void Update()
    {


        if (CanMove || !WasHit)
        {
            _input = new Vector2(
                x: SignZero(Input.GetAxisRaw(_horizontalAxis)),
                y: SignZero(Input.GetAxisRaw(_verticalAxis)));

            Vector3 rotation = SettingsManager.FreePlayerRotation
                ? GetFreeRotation(_input.x)
                : GetIntervalRotation(_input.x);

            transform.Rotate(rotation);

            MovePlayer(_input.y);

            if (CanShoot && Input.GetKey(_fireKey))
            {
                SwitchFireMode((int)SettingsManager.SelectedFireMode);
            }


        }
        else
        {
            transform.Rotate(GetFreeRotation(_gotHitRotationSpeed));
        }



        if (SettingsManager.InvisibleTankMode)
        {
            _playerMesh.enabled = !CanShoot || WasHit ? true : false;
            _turretMesh.enabled = !CanShoot || WasHit ? true : false;
            _playerTextMesh.enabled = !CanShoot || WasHit ? true : false;
        }
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public Vector3 GetFreeRotation(float rotationInput)
    {
        return Vector3.up * rotationInput * _freeRotationSpeed * Time.deltaTime;
    }
    public Vector3 GetIntervalRotation(float rotationInput)
    {
        _rotateTimer -= Time.deltaTime;

        if (_rotateTimer > 0f)
            return Vector3.zero;

        _rotateTimer += _intervalRotationDelay;
        return Vector3.up * RotationInterval * rotationInput;
    }
    public IEnumerator GotHitRoutine()
    {
        CanMove = false;
        yield return new WaitForSeconds(1f);
        CanMove = true;
        _isHitting = false;
        WasHit = false;

    }
    public void GetHitInDirecTionFromBullet(Vector3 bulletDirection, Collision playerCollision)
    {
        Vector3 playerNormal = playerCollision.contacts[0].normal;
        Vector3 lerpedVector = Vector3.Lerp(bulletDirection, -playerNormal, .5f);
        WasHit = true;
        StartCoroutine(InvincibleRoutine());
        StartCoroutine(GotHitRoutine());
        GettingHitByBullet(lerpedVector);


    }
    public void StartCoroutineWhenHitting()
    {
        StartCoroutine(GotHitRoutine());
    }
    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private IEnumerator InvincibleRoutine()
    {
        IsInvincible = true;
        yield return new WaitForSeconds(2f);
        IsInvincible = false;
    }
    private Vector3 RotatePlayerAfterHitting()
    {
        if (_isHitting)
        {
            return Vector3.zero;
        }
        _isHitting = true;
        return Vector3.up * 45;
    }
    private void GettingHitByBullet(Vector3 dir)
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + dir, .5f);
    }
    private void SwitchFireMode(int fireMode)
    {
        switch (fireMode)
        {
            case 0:
                ShootNormalBullet();
                break;
            default:
                ShootNormalBullet();
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
    private void ShootNormalBullet()
    {
        _currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, transform.rotation);
        _currentBullet.Player = this;
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
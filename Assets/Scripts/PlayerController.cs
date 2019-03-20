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
    //[SerializeField] private bool _rotateFree;
    [SerializeField, Range(0, 720)] private int _freeRotationSpeed;
    [SerializeField, Range(2, 24)] private int _intervallRange = 16;
    [SerializeField, Range(0f, 1f)] private float _intervalRotationDelay = .25f;
    [SerializeField] private float _gotHitRotationSpeed = 20;
    [Header("Bullet and Spawner")]
    [SerializeField] private BulletBehaviour _bulletPrefab;
    [SerializeField] private Transform _bulletSpawn;
    //[SerializeField] private SettingsManager.FireModes _fireModeEnum;

    [SerializeField] private bool _isInvisibleTankMode = false;

    private BoxCollider _collider;
    private Rigidbody _rb;
    private MeshRenderer _playerMesh;
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
    public SettingsManager.FireModes FireMode { get => SettingsManager.SelectedFireMode; }
    public bool WasHit { get; set; }
    public bool CanMove { get; set; }
    public bool IsPlayerMeshVisible { get; set; }
    public float RotationInput { get { return _input.x; } }
    public int Score { get; set; }
    private float RotationInterval { get { return 360f / _intervallRange; } }
    private float Speed { get { return _movementSpeed; } set { _movementSpeed = Mathf.Clamp(value, MIN_SPEED, MAX_SPEED); } }
    private bool CanShoot { get { return _currentBullet == null; } }

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        
        _collider = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
        _playerMesh = GetComponent<MeshRenderer>();
        IsPlayerMeshVisible = true;
        
        
        if (SettingsManager.InvisibleTankMode)
        {
            _playerMesh.enabled = TogglePlayerMesh();
        }
        
        _rotateTimer = 0f;
        _bulletTimer = 0f;
        CanMove = true;
        _canShoot = true;
    }

    private void Update()
    {
       
        
        if (CanMove)
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
                ShootNormalBullet();
            }
           
            
        }
        else
        {
            
            if (WasHit)
            {
                transform.Rotate(GetFreeRotation(_gotHitRotationSpeed));
            }
            else
            {
                Vector3 rotation = RotatePlayerAfterHitting();
                transform.Rotate(rotation);
                MovePlayer(_input.y);
            }

        }
        


        if (SettingsManager.InvisibleTankMode)
        {
            _playerMesh.enabled = CanShoot ? false : true;
        }
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public Vector3 GetFreeRotation(float rotationInput)
    {
        return Vector3.up * rotationInput * _freeRotationSpeed * Time.deltaTime;
    }

    public IEnumerator GotHitRoutine()
    {
        CanMove = false;
        yield return new WaitForSeconds(1f);
        CanMove = true;
        _isHitting = false;
        WasHit = false;

    }

    public Vector3 GetIntervalRotation(float rotationInput)
    {
        _rotateTimer -= Time.deltaTime;

        if (_rotateTimer > 0f)
            return Vector3.zero;

        _rotateTimer += _intervalRotationDelay;
        return Vector3.up * RotationInterval * rotationInput;
    }

    public void GetHit(Collision col)
    {
        WasHit = true;
        StartCoroutine(GotHitRoutine());
        GettingHitByBullet(-col.contacts[0].normal);
    }

    public void StartCoroutineWhenHitting()
    {
        StartCoroutine(GotHitRoutine());
    }
    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private void SelectRightFireMode(SettingsManager.FireModes selectedMode)
    {
       
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
    private bool TogglePlayerMesh()
    {
        return !IsPlayerMeshVisible;
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

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.gameObject.tag == "Bullet")
        {
            
            if (collision.gameObject.GetComponent<BulletBehaviour>().Player != this)
            {
                //GettingHitByBullet(collision.contacts[0].normal);
                StartCoroutine(GotHitRoutine());
                collision.gameObject.GetComponent<BulletBehaviour>().Player.Score++;
                Debug.Log("Enemy hit me" + collision.gameObject.GetComponent<BulletBehaviour>().Player.Score);
                
            }
            Destroy(collision.gameObject);
        }
        */
    }
    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
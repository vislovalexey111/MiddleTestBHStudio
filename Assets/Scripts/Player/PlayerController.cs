using System;
using System.Collections;
using UnityEngine;
using Cinemachine;
using TMPro;
using Mirror;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput), typeof(MeshRenderer))]
public class PlayerController : NetworkBehaviour
{
    //private Color _color;
    private CinemachineFreeLook _camera;

    static private bool s_CountdownStarted;

    [Header("Player Properties")]
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _dashDistance = 3f;
    [SerializeField] private float _damageDuration = 3f;
    [SerializeField] private int _pointsToWin = 3;

    [Header("Player Components")]
    [SerializeField] private TextMeshPro _displayName;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterController _characterController;

    [Header("SyncVars")]
    [SyncVar(hook = nameof(HandleNameChange))] private string _name;
    [SyncVar(hook = nameof(HandleSetPoints))] private int _points;

    [SyncVar(hook = nameof(HandleTakeDamage))] public bool IsTakingDamage;
    [SyncVar(hook = nameof(HandleDashing))] public bool IsDashing;

    // Commands
    [Command] private void CmdSetName(string newName) => _name = newName;
    [Command] private void CmdSetPoints(int newPoints) => _points = newPoints;
    [Command] private void CmdDash(bool isDashing) => IsDashing = isDashing;
    [Command] private void CmdTakeDamage(PlayerController playerController, bool isTakingDamage) => playerController.IsTakingDamage = isTakingDamage;

    // Actions
    public Action<string> OnWin;
    public Action<int> OnUpdatePoints;

    // SyncVar Handlers
    private void HandleNameChange(string _, string newValue) => _displayName.text = newValue;
    private void HandleDashing(bool _, bool newValue) { if (newValue) StartCoroutine(DashCoroutine()); }

    private void HandleTakeDamage(bool _, bool newValue)
    {
        if (newValue) StartCoroutine(TakeDamageCoroutine());

        _animator.SetBool("GotDamage", newValue);
    }

    private void HandleSetPoints(int _, int newPoints)
    {
        if(newPoints == _pointsToWin)
        {
            s_CountdownStarted = true;
            OnWin?.Invoke(_name);
        }

        OnUpdatePoints?.Invoke(newPoints);
    }

    // Coroutines
    private IEnumerator DashCoroutine()
    {
        Vector3 currentPosition = transform.position;

        while (Vector3.Distance(transform.position, currentPosition) < _dashDistance && IsDashing)
        {
            _characterController.Move(_movementSpeed * 2f * Time.deltaTime * transform.forward);
            yield return null;
        }

        if (isLocalPlayer) CmdDash(false);
    }

    private IEnumerator TakeDamageCoroutine()
    {
        yield return new WaitForSeconds(_damageDuration);
        if (isLocalPlayer) CmdTakeDamage(this, false);
    }

    // Gameplay functions
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsDashing && isLocalPlayer)
        {
            CmdDash(false);

            if (hit.gameObject.TryGetComponent(out PlayerController playerController))
            {
                if (playerController.IsTakingDamage || playerController.IsDashing || s_CountdownStarted) return;

                CmdSetPoints(_points + 1);
                CmdTakeDamage(playerController, true);
            }
        }
    }

    private void Move(Vector2 input)
    {
        if (IsDashing) return;

        Vector3 movement = transform.right * input.x + transform.forward * input.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, _camera.m_XAxis.Value, 0f), _rotationSpeed * Time.deltaTime);

        _characterController.Move(_movementSpeed * Time.deltaTime * movement);
    }

    private void Dash()
    {
        if (IsDashing || IsTakingDamage) return;

        CmdDash(true);
    }

    // Standart Functions
    private void Start()
    {
        //_color = _meshRenderer.material.color;
        s_CountdownStarted = false;
        OnUpdatePoints?.Invoke(_points);
    }

    public override void OnStartLocalPlayer()
    {
        _camera = FindObjectOfType<CinemachineFreeLook>();

        _camera.Follow = transform;
        _camera.LookAt = transform;
        _playerInput.OnMove += Move;
        _playerInput.OnDash += Dash;

        CmdSetPoints(0);
        CmdSetName(PlayerPrefs.GetString("PlayerName"));
    }

    public override void OnStopLocalPlayer()
    {
        _playerInput.OnDash -= Dash;
        _playerInput.OnMove -= Move;
    }
}
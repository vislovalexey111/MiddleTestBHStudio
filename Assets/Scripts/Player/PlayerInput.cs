using System;
using UnityEngine;
using Mirror;

public class PlayerInput : NetworkBehaviour
{
    private Vector2 _direction;

    public Action<Vector2> OnMove;
    public Action OnDash;

    private void OnEnable()
    {
       Cursor.lockState = CursorLockMode.Locked;
       Cursor.visible = false;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        _direction.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (_direction.magnitude >= 0.1f)
            OnMove?.Invoke(_direction.normalized);

        if (Input.GetButtonDown("Fire1"))
            OnDash?.Invoke();
    }
}

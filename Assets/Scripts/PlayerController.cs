using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Variables: Movement

    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;

    [SerializeField] private Movement movement;

    #endregion

    #region Variables: Rotation

    [SerializeField] private float rotationSpeed = 500f;
    private Camera _mainCamera;

    #endregion

    #region Variables: Interaction

    [SerializeField] private Transform _holdArea;
    private GameObject _heldObject;
    private Rigidbody _heldObjectRB;

    [SerializeField] private float interactRange;
    [SerializeField] private float pickUpForce = 150.0f;

    #endregion

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        ApplyRotation();
        ApplyMovement();
        ProcessPickUpObject();
    }

    private void ProcessPickUpObject()
    {
        if (_heldObject != null)
        {
            MoveObject();
        }
    }

    private void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0)
        {
            return;
        }

        _direction = Quaternion.Euler(0.0f, _mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(_input.x, 0.0f, _input.y);
        var targetRotation = Quaternion.LookRotation(_direction, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        movement.currentSpeed = Mathf.MoveTowards(movement.currentSpeed, movement.speed, movement.acceleration * Time.deltaTime);

        _characterController.Move(_direction * movement.currentSpeed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
    }


    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray r = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
            Debug.DrawLine(r.origin, r.origin + r.direction * interactRange, new Color(1f, 0f, 0f), 500f);

            if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange)
                && hitInfo.collider.gameObject.CompareTag("Interactable"))
            {
                PickUpObject(hitInfo.collider.gameObject);
            }
        }
        else if (context.canceled)
        {
            DropObject();
        }
    }

    private void MoveObject()
    {
        if (Vector3.Distance(_heldObject.transform.position, _holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (_holdArea.position - _heldObject.transform.position);
            _heldObjectRB.AddForce(moveDirection * pickUpForce);
        }
    }

    private void PickUpObject(GameObject pickedObject)
    {
        if (pickedObject.TryGetComponent<Rigidbody>(out Rigidbody pickRB))
        {
            Debug.Log($"PICKING UP {pickedObject.name}");
            _heldObjectRB = pickRB;
            _heldObjectRB.useGravity = false;
            _heldObjectRB.drag = 10;
            _heldObjectRB.constraints = RigidbodyConstraints.FreezeRotation;

            _heldObject = pickedObject; 
            _heldObject.transform.SetParent(_holdArea);
        }
    }

    private void DropObject()
    {
        if (_heldObject != null)
        {
            _heldObjectRB.useGravity = true;
            _heldObjectRB.drag = 1;
            _heldObjectRB.constraints = RigidbodyConstraints.None;
            _heldObjectRB = null;

            _heldObject.transform.parent = null;
            _heldObject = null;
        }
    }
}

[Serializable]
public struct Movement
{
    public float speed;
    public float acceleration;

    [HideInInspector] public float currentSpeed;
}

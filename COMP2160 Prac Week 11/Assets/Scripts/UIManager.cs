/**
 * A singleton class to allow point-and-click movement of the marble.
 * 
 * It publishes a TargetSelected event which is invoked whenever a new target is selected.
 * 
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using System;
using UnityEngine;
using UnityEngine.InputSystem;

// note this has to run earlier than other classes which subscribe to the TargetSelected event
[DefaultExecutionOrder(-100)]
public class UIManager : MonoBehaviour
{
#region UI Elements
    [SerializeField] private Transform crosshair;
    [SerializeField] private Transform target;
#endregion 

#region Singleton
    static private UIManager instance;
    static public UIManager Instance
    {
        get { return instance; }
    }
#endregion 

#region Actions
    private Actions actions;
    private InputAction mouseAction;
    private InputAction deltaAction;
    private InputAction selectAction;
    private InputAction cameraAction;
#endregion

#region Events
    public delegate void TargetSelectedEventHandler(Vector3 worldPosition);
    public event TargetSelectedEventHandler TargetSelected;
#endregion

#region Init & Destroy
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one UIManager in the scene.");
        }

        instance = this;

        actions = new Actions();
        mouseAction = actions.mouse.position;
        deltaAction = actions.mouse.delta;
        selectAction = actions.mouse.select;
        cameraAction = actions.camera.zoom;

        Cursor.visible = false;
        target.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        actions.mouse.Enable();
        actions.camera.Enable();
    }

    void OnDisable()
    {
        actions.mouse.Disable();
        actions.camera.Disable();
    }
#endregion Init

#region Update
    void Update()
    {
        MoveCrosshair();
        SelectTarget();
        CheckZoom();
    }

    private void MoveCrosshair() 
    {
        Vector2 mousePos = mouseAction.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if( Physics.Raycast(ray, out RaycastHit hit))
        {
            
            crosshair.position = hit.point;
        }
       
        // FIXME: Move the crosshair position to the mouse position (in world coordinates)
    }

    private void SelectTarget()
    {
        if (selectAction.WasPerformedThisFrame())
        {
            // set the target position and invoke 
            target.gameObject.SetActive(true);
            target.position = crosshair.position;     
            TargetSelected?.Invoke(target.position);       
        }
    }
    private void CheckZoom()
    {
        if(cameraAction.WasPerformedThisFrame())
        {
            float zoom = cameraAction.ReadValue<float>();
            float newSize = Camera.main.orthographicSize + zoom / 100;

            if (newSize > 0.1f && newSize < 100f)
            {
                Camera.main.orthographicSize = newSize;
            }
        }
    }

#endregion Update

}
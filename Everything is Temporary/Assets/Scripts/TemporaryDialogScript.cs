using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An interactive object that displays a dialog upon interaction. Use
/// the down arrow key or walk away to stop the dialog and regain camera
/// control.
/// </summary>
public class TemporaryDialogScript : MonoBehaviour, IInteractiveObject
{
    public bool CanInteractWith(IInteractor interactor)
    {
        return enabled && m_cameraFocus == null;
    }

    public float GetDistanceTo(IInteractor interactor)
    {
        return Mathf.Abs(transform.position.x - interactor.GetPosition().x);
    }

    public void HideSelectionVisual()
    {
        m_selectionVisual.SetActive(false);
    }

    public void ShowSelectionVisual()
    {
        // Only show the selection visual again if we are not currently
        // interacting with the player.
        if (m_cameraFocus == null)
            m_selectionVisual.SetActive(true);
    }

    public bool TryInteractWith(IInteractor interactor)
    {
        if (m_cameraFocus == null)
        {
            var townCamera = GameManager.Singleton.TownCamera;
            m_cameraFocus = townCamera.GrabCameraFocus(m_cameraFocusPosition);
            m_dialogVisual.SetActive(true);
            m_interactor = interactor;

            HideSelectionVisual();

            return true;
        }

        return false;
    }

    private void Awake()
    {
        m_selectionVisual.SetActive(false);
        m_dialogVisual.SetActive(false);

        m_interactionSystemRegistration = InteractionSystem.Singleton.RegisterInteractiveObject(this);
    }

    private void FixedUpdate()
    {
        if (m_cameraFocus != null)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || GetDistanceTo(m_interactor) > 1)
            {
                m_cameraFocus.Unsubscribe();
                m_cameraFocus = null;

                m_interactor = null;

                m_dialogVisual.SetActive(false);
            }
        }
    }

    [SerializeField]
    [Tooltip("When the player interacts with this interactive object, the camera" +
             " will begin to follow this Transform.")]
    private Transform m_cameraFocusPosition;

    [SerializeField]
    [Tooltip("When the player is near this interactive object, the selection" +
             " visual will be enabled. Otherwise, it will be disabled.")]
    private GameObject m_selectionVisual;

    [SerializeField]
    [Tooltip("When the player begins an interaction, this dialog object" +
             " will be enabled. It will otherwise be disabled.")]
    public GameObject m_dialogVisual;

    private IUnsubscriber m_cameraFocus = null;
    private IInteractor m_interactor = null;

    private IUnsubscriber m_interactionSystemRegistration;
}

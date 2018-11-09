using UnityEngine;
using UnityEngine.UI;

public class PageScript : MonoBehaviour
{
    public void EnableCameras()
    {
        leftCamera.enabled = true;
        rightCamera.enabled = true;
    }

    public void DisableCameras()
    {
        leftCamera.enabled = false;
        rightCamera.enabled = false;
    }

    public void SetCameraRenderTargets(RenderTexture left, RenderTexture right)
    {
        leftCamera.targetTexture = left;
        rightCamera.targetTexture = right;
    }

    public void EnableRaycasters()
    {
        leftRaycaster.enabled = true;
        rightRaycaster.enabled = true;
    }

    public void DisableRaycasters()
    {
        leftRaycaster.enabled = false;
        rightRaycaster.enabled = false;
    }


#if UNITY_EDITOR
    [ContextMenu("Setup Page")]
    public void SetupPage()
    {
        Camera[] cameras = GetComponentsInChildren<Camera>();
        GraphicRaycaster[] raycasters = GetComponentsInChildren<GraphicRaycaster>();

        Debug.Assert(cameras.Length == 2);
        Debug.Assert(raycasters.Length == 2);

        // TODO: This code should alert the user when there are potential errors.
        if (cameras[0].gameObject.name.Contains("Left"))
        {
            leftCamera = cameras[0];
            rightCamera = cameras[1];
        }
        else
        {
            leftCamera = cameras[1];
            rightCamera = cameras[0];
        }

        if (raycasters[0].gameObject.name.Contains("Left"))
        {
            leftRaycaster = raycasters[0];
            rightRaycaster = raycasters[1];
        }
        else
        {
            leftRaycaster = raycasters[1];
            rightRaycaster= raycasters[0];
        }
    }
#endif

    [SerializeField]
    private Camera leftCamera;
    [SerializeField]
    private Camera rightCamera;
    [SerializeField]
    private GraphicRaycaster leftRaycaster;
    [SerializeField]
    private GraphicRaycaster rightRaycaster;
}

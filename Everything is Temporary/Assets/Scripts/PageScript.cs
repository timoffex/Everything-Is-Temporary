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
        leftRaycaster = null;
        rightRaycaster = null;
        leftCamera = null;
        rightCamera = null;

        Camera[] cameras = GetComponentsInChildren<Camera>();
        GraphicRaycaster[] raycasters = GetComponentsInChildren<GraphicRaycaster>();

        if (cameras.Length != 2)
        {
            Debug.LogFormat("There must be exactly two cameras in a page.");
            return;
        }

        if (raycasters.Length != 2)
        {
            Debug.LogFormat("There must be exactly two raycasters in a page.");
            return;
        }

        foreach (var cam in cameras)
            ConfigureCamera(cam);

        foreach (var raycaster in raycasters)
            ConfigureRaycaster(raycaster);
    }

    private void ConfigureCamera(Camera cam)
    {
        GameObject go = cam.gameObject;

        if (go.name.ToLower().Contains("left"))
        {
            leftCamera = cam;
        }
        else if (go.name.ToLower().Contains("right"))
        {
            rightCamera = cam;
        }
        else
        {
            Debug.LogFormat("Child {0} of {1} does not have 'left' or 'right'" +
                            " in its name.", go.name, gameObject.name);
        }
    }

    private void ConfigureRaycaster(GraphicRaycaster raycaster)
    {

        if (!(raycaster is PageRaycaster))
        {
            Debug.LogFormat("Raycaster on child {0} of {1} is a GraphicRaycaster" +
                            " but not a PageRaycaster. Replacing with a" +
                            " PageRaycaster.",
                            raycaster.gameObject.name,
                            gameObject.name);

            GameObject go = raycaster.gameObject;
            DestroyImmediate(raycaster);
            PageRaycaster pageRaycaster = go.AddComponent<PageRaycaster>();

            if (go.name.ToLower().Contains("left"))
            {
                pageRaycaster.side = PageCoordinates.PageSide.Left;
                leftRaycaster = pageRaycaster;
            }
            else if (go.name.ToLower().Contains("right"))
            {
                pageRaycaster.side = PageCoordinates.PageSide.Right;
                rightRaycaster = pageRaycaster;
            }
            else
            {
                Debug.LogFormat("Child {0} of {1} does not have 'left' or 'right'" +
                                " in its name.", go.name, gameObject.name);
            }
        }
        else
        {
            PageRaycaster pageRaycaster = raycaster as PageRaycaster;

            switch (pageRaycaster.side)
            {
                case PageCoordinates.PageSide.Left:
                    leftRaycaster = pageRaycaster;
                    break;
                case PageCoordinates.PageSide.Right:
                    rightRaycaster = pageRaycaster;
                    break;
            }
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

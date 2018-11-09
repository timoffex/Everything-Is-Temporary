using UnityEngine;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

public partial class GameManager : MonoBehaviour
{

    /// <summary>
    /// Applies a fade effect to the camera.
    /// </summary>
    /// <param name="cam">Camera.</param>
    /// <param name="initialValue">Initial fade value.</param>
    /// <param name="toFadeValue">Final fade value.</param>
    /// <param name="duration">Fade duration.</param>
    /// <param name="color">Fade color.</param>
    /// <param name="endAction">Action to invoke when fade completes.</param>
    /// <param name="cancelAction">Action to invoke if fade is canceled.</param>
    private void FadeCamera(Camera cam, float initialValue,
                            float toFadeValue, float duration,
                            Color color, System.Action endAction = null,
                            System.Action cancelAction = null)
    {
        CameraFadeScript fader = cam.gameObject.GetComponent<CameraFadeScript>();

        if (fader == null)
            fader = cam.gameObject.AddComponent<CameraFadeScript>();

        if (fader.IsFading)
        {
            fader.CancelAndBeginNewFade(fader.FadeValue, toFadeValue, color,
                                        duration, endAction, cancelAction);
        }
        else
        {
            fader.BeginFade(initialValue, toFadeValue, color, duration, endAction,
                            cancelAction);
        }
    }

    /// <summary>
    /// Helper method to wait for Unity's AsyncOperation.
    /// </summary>
    /// <param name="op">An operation to wait for.</param>
    public async Task WaitForAsyncOperation(AsyncOperation op)
    {
        SemaphoreSlim sema = new SemaphoreSlim(0);

        op.completed += (_) => sema.Release();

        await sema.WaitAsync();
    }
}

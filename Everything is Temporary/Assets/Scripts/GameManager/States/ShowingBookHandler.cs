using UnityEngine;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

public partial class GameManager : MonoBehaviour
{
    private class ShowingBookHandler : StateHandler
    {
        public ShowingBookHandler(GameManager gameManager) : base(gameManager)
        {
        }

        public override State GetState()
        {
            return State.ShowingBook;
        }

        protected override async Task<StateHandler> SwitchTo_Impl(State state)
        {
            switch (state)
            {
                case State.ShowingTown:
                    return await SwitchToTown();

                default:
                    return null;
            }
        }

        private async Task<StateHandler> SwitchToTown()
        {
            GameManager gm = GameManagerParent;

            // Can only switch to town view if a town is loaded.
            if (gm.m_loadedTown == null)
                return null;

            // Since a town is loaded, there must be a town camera registered.
            Debug.Assert(gm.m_townCamera != null);

            // Disable mouse inputs to pages.
            gm.m_townInputBlocker = MainRaycastHelper.Singleton.BlockPageInput();

            // Fade out main camera.
            {
                bool success = false;
                SemaphoreSlim fadeSema = new SemaphoreSlim(0);

                gm.FadeCamera(gm.BookCamera, 0, 1, 1, Color.black, () =>
                {
                    success = true;
                    fadeSema.Release();
                }, () =>
                {
                    success = false;
                    fadeSema.Release();
                });

                await fadeSema.WaitAsync();

                // If fading was canceled, bail out!
                if (!success)
                {
                    gm.m_townInputBlocker.Unsubscribe();
                    gm.m_townInputBlocker = null;
                    return null;
                }
            }

            // Begin fade in town camera.
            gm.FadeCamera(gm.m_townCamera, 1, 0, 1, Color.black);

            gm.m_townCamera.enabled = true;
            gm.BookCamera.enabled = false;

            return new ShowingTownHandler(gm);
        }
    }
}

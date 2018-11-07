using UnityEngine;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

public partial class GameManager : MonoBehaviour
{
    private class ShowingTownHandler : StateHandler
    {
        public ShowingTownHandler(GameManager gameManager) : base(gameManager)
        {
        }

        public override State GetState()
        {
            return State.ShowingTown;
        }

        protected override async Task<StateHandler> SwitchTo_Impl(State state)
        {
            switch (state)
            {
                case State.ShowingBook:
                    return await SwitchToBook();
                default:
                    return null;
            }
        }

        private async Task<StateHandler> SwitchToBook()
        {
            GameManager gm = GameManagerParent;

            // There must be a town camera in this state.
            Debug.Assert(gm.m_townCamera != null);

            // There must similarly be an input blocker token.
            Debug.Assert(gm.m_townInputBlocker != null);

            // Fade town camera.
            {
                bool success = false;
                SemaphoreSlim sema = new SemaphoreSlim(0);

                gm.FadeCamera(gm.m_townCamera, 0, 1, 1, Color.black, () =>
                {
                    success = true;
                    sema.Release();
                }, () =>
                {
                    success = false;
                    sema.Release();
                });

                await sema.WaitAsync();

                // If fading is canceled, bail.
                if (!success)
                    return null;
            }

            // Begin fading in book camera.
            gm.FadeCamera(gm.BookCamera, 1, 0, 1, Color.black);

            gm.BookCamera.enabled = true;
            gm.m_townCamera.enabled = false;

            // Enable book event system.
            gm.m_townInputBlocker.Unsubscribe();
            gm.m_townInputBlocker = null;

            gm.TownEventSystem.enabled = false;
            gm.BookEventSystem.enabled = true;

            return new ShowingBookHandler(gm);
        }
    }
}

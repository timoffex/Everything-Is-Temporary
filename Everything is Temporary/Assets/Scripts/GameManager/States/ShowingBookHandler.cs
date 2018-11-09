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

        protected override Task<StateHandler> SwitchTo_Impl(State state)
        {
            switch (state)
            {
                case State.ShowingTown:
                    return Task.FromResult(SwitchToTown());

                default:
                    return null;
            }
        }

        private StateHandler SwitchToTown()
        {
            GameManager gm = GameManagerParent;

            // Can only switch to town view if a town is loaded.
            if (gm.m_loadedTown == null)
                return null;

            // Since a town is loaded, there must be a town camera registered.
            Debug.Assert(gm.m_townCamera != null);

            gm.m_townCamera.SetRenderTarget(gm.m_sidescrollingQuad.GetRenderTexture());
            gm.FadeCamera(gm.m_townCamera, 1, 0, 1, Color.black);
            gm.m_townCamera.EnableCamera();

            return new ShowingTownHandler(gm);
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

public partial class GameManager : MonoBehaviour
{
    /// <summary>
    /// Represents the various states in which the GameManager can be.
    /// </summary>
    private enum State
    {
        ShowingBook, ShowingTown
    };

    /// <summary>
    /// Switches to the required state, if possible. Returns false if this
    /// is not possible.
    /// </summary>
    private async Task<bool> SwitchToState(State state)
    {
        if (m_stateHandler == null)
        {
            Debug.LogWarning("Cannot switch GameManager states because there " +
                             "is no state handler!");
            return false;
        }

        return await m_stateHandler.SwitchTo(state);
    }

    private abstract class StateHandler
    {
        public StateHandler(GameManager gameManager)
        {
            m_gameManager = new WeakReference<GameManager>(gameManager);
        }

        /// <summary>
        /// Returns the State that this represents.
        /// </summary>
        /// <returns>The state.</returns>
        public abstract State GetState();


        public async Task<bool> SwitchTo(State state)
        {
            // Don't do anything if we are in the correct state.
            if (state == GetState())
                return true;

            // Put GameManager into state limbo to prevent state transitions.
            GameManagerParent.m_stateHandler = null;

            StateHandler result = await SwitchTo_Impl(state);

            if (result != null)
            {
                Debug.Assert(result.GetState() == state);
                GameManagerParent.m_stateHandler = result;
                return true;
            }
            else
            {
                GameManagerParent.m_stateHandler = this;
                return false;
            }
        }

        /// <summary>
        /// Switches to a new State, if possible.
        /// </summary>
        /// <returns>The new StateHandler, or null on failure.</returns>
        /// <param name="state">The target State.</param>
        protected abstract Task<StateHandler> SwitchTo_Impl(State state);


        protected GameManager GameManagerParent
        {
            get
            {
                GameManager gm;
                bool success = m_gameManager.TryGetTarget(out gm);

                // Should never fail!
                Debug.Assert(success);

                return gm;
            }
        }

        private WeakReference<GameManager> m_gameManager;
    }

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
            // TODO: Disable MouseInput too.
            gm.m_townInputBlocker = gm.m_pageInputModule.IgnoreInput();

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

            // Enable town event system.
            gm.BookEventSystem.enabled = false;
            gm.TownEventSystem.enabled = true;

            return new ShowingTownHandler(gm);
        }
    }

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

    private StateHandler m_stateHandler;
}

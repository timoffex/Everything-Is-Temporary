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
        /// <summary>
        /// This state means the GameManager is showing only the book.
        /// </summary>
        ShowingBook,

        /// <summary>
        /// This state means the GameManager is showing the book and a "town"
        /// (a sidescrolling environment).
        /// </summary>
        ShowingTown
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

    private StateHandler m_stateHandler;
}

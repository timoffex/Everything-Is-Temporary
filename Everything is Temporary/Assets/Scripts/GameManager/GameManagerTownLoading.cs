using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public partial class GameManager : MonoBehaviour
{
    /*
     * Loading a town is a chain of asynchronous
     * */

    public async Task LoadTown(string townScene)
    {
        /*
        // If this is the loaded town, don't do anything.
        if (m_loadedTown == townScene)
            return;
        */

        // Transition to the state where only the book is shown. If this
        // is not possible, cancel.
        if (!await SwitchToState(State.ShowingBook))
            return;

        // Unload previous town, if any, and then proceed.
        if (m_loadedTown != null)
            await WaitForAsyncOperation(SceneManager.UnloadSceneAsync(m_loadedTown));

        // Load town scene.
        m_loadedTown = townScene;
        await WaitForAsyncOperation(SceneManager.LoadSceneAsync(townScene, LoadSceneMode.Additive));

        // The town should have a town camera. It should use
        // a script like TownCamera to register itself on Awake().
        Debug.Assert(m_townCamera != null, "Town scenes must have a Camera with" +
                     " a TownCamera script.");

        await SwitchToState(State.ShowingTown);
    }

    /// <summary>
    /// The town that is currently loaded.
    /// </summary>
    private string m_loadedTown = null;
}

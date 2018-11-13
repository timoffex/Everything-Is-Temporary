using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

public class TownVerifierEditor : EditorWindow
{

    [MenuItem("Window/Verify Town Scene")]
    public static void Initialize()
    {
        TownVerifierEditor editor = GetWindow<TownVerifierEditor>();
        editor.Show();
    }

    private void OnEnable()
    {
        ScanScene();
    }

    private void OnHierarchyChange()
    {
        ScanScene();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Refresh"))
            ScanScene();

        bool foundProblem = false;

        GUILayout.Label("Town Cameras:");

        foreach (var camera in m_townCameras)
            EditorGUILayout.ObjectField(camera, typeof(TownCamera), true);

        if (m_townCameras.Count > 1)
        {
            EditorGUILayout.HelpBox("You have more than one TownCamera. Only one will end" +
                                    " up being used. You should remove one of these.",
                                    MessageType.Warning);
            foundProblem = true;
        }
        else if (m_townCameras.Count < 1)
        {
            EditorGUILayout.HelpBox("You don't have a TownCamera in this scene. Every" +
                                    " town scene must have an enabled camera with a" +
                                    " TownCamera component, as that is what gets rendered" +
                                    " in the quad above the book.",
                                    MessageType.Error);
            foundProblem = true;
        }

        if (m_badCameras.Count > 0)
        {
            GUILayout.Label("Bad Cameras:");

            foreach (var camera in m_badCameras)
                EditorGUILayout.ObjectField(camera, typeof(Camera), true);

            EditorGUILayout.HelpBox("You have cameras in your town scene that will overwrite" +
                                    " screen contents. Disable or remove these.",
                                    MessageType.Warning);
            foundProblem = true;
        }

        GUILayout.Label("Sidescroll Raycasters:");

        foreach (var wrapper in m_wrappedRaycasters)
            EditorGUILayout.ObjectField(wrapper, typeof(SidescrollRaycaster), true);

        if (m_badRaycasters.Count > 0)
        {
            GUILayout.Label("Bad Raycasters:");

            foreach (var raycaster in m_badRaycasters)
                EditorGUILayout.ObjectField(raycaster, typeof(BaseRaycaster), true);

            EditorGUILayout.HelpBox("All raycasters in a town scene (e.g. GraphicRaycaster," +
                                    " PhysicsRaycaster, Physics2DRaycaster) must be 'wrapped'" +
                                    " in a SidescrollRaycaster so that mouse input correctly" +
                                    " comes from the sidescroller quad above the book.",
                                    MessageType.Warning);

            if (GUILayout.Button(new GUIContent("Fix by wrapping in SidescrollRaycasters")))
            {
                FixBadRaycasters();
            }

            foundProblem = true;
        }

        if (m_eventSystems.Count > 0)
        {
            GUILayout.Label("Event Systems:");

            foreach (var eventSystem in m_eventSystems)
                EditorGUILayout.ObjectField(eventSystem, typeof(EventSystem), true);

            EditorGUILayout.HelpBox("You have EventSystems in your town scene. These will" +
                                    " conflict with the EventSystem in the main game scene" +
                                    " when the town scene is loaded. Please remove all" +
                                    " EventSystems before loading this scene from the" +
                                    " book view.", MessageType.Error);

            foundProblem = true;
        }

        if (!foundProblem)
            EditorGUILayout.HelpBox("Couldn't find any issues! If your town scene doesn't" +
                                    " work properly, it might be for a reason that this" +
                                    " code does not detect. See if you can figure it out," +
                                    " and if you cannot, then ask Tima.", MessageType.Info);
        else
            EditorGUILayout.HelpBox("Found some problems with your town scene! This means" +
                                    " that when you try to load it in the book view, weird" +
                                    " things might happen. Fix the issues above before trying" +
                                    " to use this scene.", MessageType.Error);
    }

    private void ScanScene()
    {
        ScanCameras();
        ScanRaycasters();
        ScanEventSystems();
    }

    private void ScanCameras()
    {
        m_townCameras = new List<TownCamera>();
        m_townCameras.AddRange(FindObjectsOfType<TownCamera>());

        m_badCameras = new List<Camera>();
        var allCameras = FindObjectsOfType<Camera>();

        foreach (var camera in allCameras)
        {
            bool hasTownCamera = camera.gameObject.GetComponent<TownCamera>() != null;

            if (!hasTownCamera && camera.enabled && camera.targetTexture == null)
                m_badCameras.Add(camera);
        }
    }

    private void ScanRaycasters()
    {
        m_wrappedRaycasters = new List<SidescrollRaycaster>();
        m_wrappedRaycasters.AddRange(FindObjectsOfType<SidescrollRaycaster>());

        m_badRaycasters = new List<BaseRaycaster>();

        var allRaycasters = FindObjectsOfType<BaseRaycaster>();
        foreach (var raycaster in allRaycasters)
        {
            if (raycaster is SidescrollRaycaster)
                continue;

            var wrapper = m_wrappedRaycasters.Find((w) => w.DelegateRaycaster == raycaster);

            if (wrapper == null)
                m_badRaycasters.Add(raycaster);
        }
    }

    private void ScanEventSystems()
    {
        m_eventSystems = new List<EventSystem>();
        m_eventSystems.AddRange(FindObjectsOfType<EventSystem>());
    }

    private void FixBadRaycasters()
    {
        foreach (var raycaster in m_badRaycasters)
        {
            var wrapper = raycaster.gameObject.AddComponent<SidescrollRaycaster>();
            wrapper.DelegateRaycaster = raycaster;
        }
    }

    private List<TownCamera> m_townCameras;
    private List<SidescrollRaycaster> m_wrappedRaycasters;

    /// <summary>
    /// List of raycasters that are not wrapped in a SidescrollRaycaster.
    /// </summary>
    private List<BaseRaycaster> m_badRaycasters;

    /// <summary>
    /// List of cameras without a TownCamera component that are enabled and
    /// not drawing to a texture.
    /// </summary>
    private List<Camera> m_badCameras;

    /// <summary>
    /// List of event systems in the scene. There should not be event systems.
    /// </summary>
    private List<EventSystem> m_eventSystems;
}

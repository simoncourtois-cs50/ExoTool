using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildManagerWindow : EditorWindow
{

    [MenuItem("Tools/BuildManagerWindow")]
    public static void ShowWindow()
    {
        BuildManagerWindow window = GetWindow<BuildManagerWindow>();
        window.titleContent = new GUIContent("Build Manager");
        
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        //VERSION
        _versionTitle = new Label($"Current Version : {_version}");
        root.Add(_versionTitle);

        //Major
        root.Add(new Label("Major"));
        _majorField = new TextField { value = _major };
        _majorField.RegisterValueChangedCallback(evt =>
        {
            _major = evt.newValue;
        });
        root.Add(_majorField);

        //Minor
        root.Add(new Label("Minor"));
        _minorField = new TextField { value = _minor };
        _minorField.RegisterValueChangedCallback(evt =>
        {
            _minor = evt.newValue;
        });
        root.Add(_minorField);

        //Patch
        root.Add(new Label("Patch"));
        _patchField = new TextField { value = _patch };
        _patchField.RegisterValueChangedCallback(evt =>
        {
            _patch = evt.newValue;
        });
        root.Add(_patchField);

        //Update Button
        Button updateButton = new Button();
        updateButton.name = "Update Button";
        updateButton.text = "Update Version";
        updateButton.clicked += handleUpdateButtonClick;
        root.Add(updateButton);

        // GIT
        Label branch = new Label($"Branch : {_branch}");
        root.Add(branch);
        Label tag = new Label($"Tag : {_tag}");
        root.Add(tag);
        Label commit = new Label($"Commit : {_commit}");
        root.Add(commit);
        Label status = new Label($"Status : {_status}");
        root.Add(status);
    }
    private void handleUpdateButtonClick()
    {
        _version = $"v{_major}.{_minor}.{_patch}";
        Debug.Log(GitUtility.GetStatus());
    }

    #region Private and Protected

    private TextField _majorField;
    private TextField _minorField;
    private TextField _patchField;

    private string _major;
    private string _minor;
    private string _patch;
    private string _version;
    private Label _versionTitle;
    private string _branch;
    private string _tag;
    private string _commit;
    private string _status;
    #endregion

    
    //buttons
    //help boxes
    //Sections for Git and Build information
    // Branch / Tag / Commit / Status
}

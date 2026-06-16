using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildManagerWindow : EditorWindow
{

    [MenuItem("Tools/BuildManagerWindow")]
    public static void ShowWindow()
    {
        BuildManagerWindow window = GetWindow<BuildManagerWindow>("Build Manager");
        window.titleContent = new GUIContent("Build Manager");
        window.minSize = new Vector2(300, 400);
        window.Show();
    }

    public void CreateGUI()
    {
        UpdateDisplayedVersion();

        VisualElement root = rootVisualElement;

        //VERSION
        _versionTitle = new Label($"Current Version : {_currentVersion}");
        root.Add(_versionTitle);

        //Major

        VisualElement horizontalGroupMajor = new VisualElement();
        horizontalGroupMajor.style.flexDirection = FlexDirection.Row;

        Label majorLabel = new Label("Major");
        majorLabel.style.flexGrow = 2;
        horizontalGroupMajor.Add(majorLabel);

        Label majorNumberLabel = new Label(_majorNumber.ToString());
        majorNumberLabel.style.flexGrow = 2;
        horizontalGroupMajor.Add(majorNumberLabel);

        Button incrementButton = new Button();
        incrementButton.name = "+";
        incrementButton.text = "+";
        incrementButton.style.flexGrow = 1;
        incrementButton.clicked += () =>
        {
            _majorNumber++;
            majorNumberLabel.text = _majorNumber.ToString();
            
        };
        horizontalGroupMajor.Add(incrementButton);

        Button decrementButton = new Button();
        decrementButton.name = "-";
        decrementButton.text = "-";
        decrementButton.style.flexGrow = 1;
        decrementButton.clicked += () =>
        {
            _majorNumber--;
            _majorNumber = _majorNumber < Int32.Parse(_major) ? Int32.Parse(_major) : _majorNumber;
            majorNumberLabel.text = _majorNumber.ToString();
        };
        horizontalGroupMajor.Add(decrementButton);

        root.Add(horizontalGroupMajor);

        //Minor
        VisualElement horizontalGroupMinor = new VisualElement();
        horizontalGroupMinor.style.flexDirection = FlexDirection.Row;

        Label minorLabel = new Label("Minor");
        minorLabel.style.flexGrow = 2;
        horizontalGroupMinor.Add(minorLabel);

        Label minorNumberLabel = new Label(_majorNumber.ToString());
        minorNumberLabel.style.flexGrow = 2;
        horizontalGroupMinor.Add(minorNumberLabel);

        Button incrementButtonMinor = new Button();
        incrementButtonMinor.name = "+";
        incrementButtonMinor.text = "+";
        incrementButtonMinor.style.flexGrow = 1;
        incrementButtonMinor.clicked += () =>
        {
            _minorNumber++;
            minorNumberLabel.text = _minorNumber.ToString();
        };
        horizontalGroupMinor.Add(incrementButtonMinor);

        Button decrementButtonMinor = new Button();
        decrementButtonMinor.name = "-";
        decrementButtonMinor.text = "-";
        decrementButtonMinor.style.flexGrow = 1;
        decrementButtonMinor.clicked += () =>
        {
            _minorNumber--;
            _minorNumber = _minorNumber < Int32.Parse(_minor) ? Int32.Parse(_minor) : _minorNumber;
            minorNumberLabel.text = _minorNumber.ToString();
        };
        horizontalGroupMinor.Add(decrementButtonMinor);

        root.Add(horizontalGroupMinor);

        //Patch
        VisualElement horizontalGroupPatch = new VisualElement();
        horizontalGroupPatch.style.flexDirection = FlexDirection.Row;

        Label patchLabel = new Label("Patch");
        patchLabel.style.flexGrow = 2;
        horizontalGroupPatch.Add(patchLabel);

        Label patchNumberLabel = new Label(_patchNumber.ToString());
        patchNumberLabel.style.flexGrow = 2;
        horizontalGroupPatch.Add(patchNumberLabel);

        Button incrementButtonPatch = new Button();
        incrementButtonPatch.name = "+";
        incrementButtonPatch.text = "+";
        incrementButtonPatch.style.flexGrow = 1;
        incrementButtonPatch.clicked += () =>
        {
            _patchNumber++;
            patchNumberLabel.text = _patchNumber.ToString();
        };
        horizontalGroupPatch.Add(incrementButtonPatch);

        Button decrementButtonPatch = new Button();
        decrementButtonPatch.name = "-";
        decrementButtonPatch.text = "-";
        decrementButtonPatch.style.flexGrow = 1;
        decrementButtonPatch.clicked += () =>
        {
            _patchNumber--;
            _patchNumber = _patchNumber < Int32.Parse(_patch) ? Int32.Parse(_patch) : _patchNumber;
            patchNumberLabel.text = _patchNumber.ToString();
        };
        horizontalGroupPatch.Add(decrementButtonPatch);

        root.Add(horizontalGroupPatch);

        //Update Button
        Button updateButton = new Button();
        updateButton.name = "Update Button";
        updateButton.text = "Update Version";
        updateButton.clicked += handleUpdateButtonClick;
        root.Add(updateButton);

        // GIT
        Label branch = new Label($"Branch : {_branch}");
        root.Add(branch);
        Label commit = new Label($"Commit : {_commit}");
        root.Add(commit);
        Label status = new Label($"Status : {_status}");
        root.Add(status);
    }
    private void handleUpdateButtonClick()
    {
        _newVersion = $"{_majorNumber.ToString()}.{_minorNumber.ToString()}.{_patchNumber.ToString()}";

        if(_status == "Waiting commit")
        {
            EditorUtility.DisplayDialog("Warning", "Commit Before adding a tag", "ok");
            return;
        }

        if (_currentVersion == _newVersion)
        {
            EditorUtility.DisplayDialog("Warning", "Versions are identical, update with a superior version", "ok");
            return;

        }

        bool authorizeVersionUpdate = EditorUtility.DisplayDialog("Warning - Update Tag", $"Do you want to update version tag from {_currentVersion} to {_newVersion}", "yes", "no");

        if (authorizeVersionUpdate)
        {
            GitUtility.SetTag(_newVersion);
            _currentVersion = _newVersion;
            _versionTitle.text = _newVersion;
        }
    }
    private void UpdateDisplayedVersion()
    {
        string tag = GitUtility.GetTag();
        string[] versionsNumber = tag.Split(".");
        _major = versionsNumber[0];
        _minor = versionsNumber[1];
        _patch = versionsNumber[2][0].ToString();
        _currentVersion = $"{_major}.{_minor}.{_patch}";

        _majorNumber = Int32.Parse(_major);
        _minorNumber = Int32.Parse(_minor);
        _patchNumber = Int32.Parse(_patch);

        _branch = GitUtility.GetBranch();
        _commit = GitUtility.GetCommit();
        _status = GitUtility.GetStatus();
      
    }

    #region Private and Protected

    private string _currentVersion;
    private Label _versionTitle;
    private string _branch;
    private string _commit;
    private string _status;

    private string _newVersion;

    private string _major;
    private string _minor;
    private string _patch;

    private int _majorNumber;
    private int _minorNumber;
    private int _patchNumber;


    #endregion

    
    //buttons
    //help boxes
    //Sections for Git and Build information
    // Branch / Tag / Commit / Status
}

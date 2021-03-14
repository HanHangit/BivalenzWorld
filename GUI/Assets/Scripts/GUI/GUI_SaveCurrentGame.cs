using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Assets.Scripts.GUI.World;
using SFB;
using Validator;

public class GUI_SaveCurrentGame : GUI_Button
{
    public const string SENTENCES = "sen";
    public const string WORLD = "wld";
    public const string FOLDER = "Saves";

    private string _lastChoosenDirectory = "";

    [SerializeField]
    private bool _saveWorld = false;
    [SerializeField]
    private GUI_Factory_TabsButtonPanel _worldTabsButtons = default;
    private TabsButtonPanel _buttonPanel = null;

    private void Start()
    {
        _buttonPanel = _worldTabsButtons.Create();
    }

    protected override void ButtonClickedListener()
    {
        if (_saveWorld)
        {
            SaveWorldObjs();
        }
        else
        {
            SaveSentences();
        }
    }

    private void SaveWorldObjs()
    {
        var board = GameManager.Instance.GetCurrentBoard();
        if (board != null)
        {
            List<Field> obj = board.GetFieldElements();
            List<WorldObject> worldObjs = new List<WorldObject>();
            var defaultName = _buttonPanel.GetActiveButton().GetName();

            foreach (Field item in obj)
            {
                if (item.HasPredicateInstance())
                {
                    List<Predicate> predicates = item.GetPredicatesList();
                    var constant = item.GetConstantsList();
                    List<string> worldPredicates = new List<string>();
                    foreach (var pred in predicates)
                    {
                        worldPredicates.Add(pred.PredicateIdentifier);
                    }

                    List<object> coord = new List<object>
                    {
                            item.GetX(),
                            item.GetZ()
                    };
                    worldObjs.Add(new WorldObject(constant, worldPredicates, coord));
                }
            }

            var jsonString = JsonConvert.SerializeObject(worldObjs);
            var path = SaveDataWorld(jsonString, WORLD, defaultName);

            if (!string.IsNullOrEmpty(path))
            {
                _buttonPanel.GetActiveButton().SetName(Path.GetFileNameWithoutExtension(path));
            }
        }
    }

    private string GetRootDirectory()
    {
        if (string.IsNullOrEmpty(_lastChoosenDirectory))
        {
            return Application.dataPath;
        }
        else
        {
            return _lastChoosenDirectory;
        }
    }

    private string SaveDataWorld(string fileContent, string world, string defaultName = "")
    {
        ExtensionFilter[] filter = new ExtensionFilter[]
        {
                new ExtensionFilter("World", world),
        };

        var path = StandaloneFileBrowser.SaveFilePanel("Save World", GetRootDirectory(), defaultName, filter);
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, fileContent);
            _lastChoosenDirectory = Path.GetDirectoryName(path);
        }

        return path;
    }

    private void SaveSentences()
    {
        var manager = GameManager.Instance;
        if (manager == null)
        {
            return;
        }

        var currentButton = manager.NavigationText.GetCurrentSelectedButton();
        //List<GUI_TextInputElement> list = manager.GetTextInputField().GetGuiTextElementsWithText();
        var resultSentences = "";
        foreach (var item in currentButton.GetText())
        {
            resultSentences += item;
        }

        Debug.Log(resultSentences);
        //var jsonString = JsonConvert.SerializeObject(currentButton.GetText());
        string correctData = JsonConvert.SerializeObject(currentButton.GetText(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        var path = SaveDataSentences(correctData, SENTENCES);
        var rawName = Path.GetFileName(path);
        var name = rawName.Split('.')[0];
        currentButton.SetButtonName(name);
    }

    private string SaveDataSentences(string fileContent, string sentences)
    {
        ExtensionFilter[] filter = new ExtensionFilter[]
        {
                new ExtensionFilter("Sentence", sentences),
        };

        var path = StandaloneFileBrowser.SaveFilePanel("Save Sentence", GetRootDirectory(), "", filter);

        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, fileContent);
            _lastChoosenDirectory = Path.GetDirectoryName(path);
        }

        return path;
    }
}

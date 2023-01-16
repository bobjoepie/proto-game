using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIDocManagerOld : MonoBehaviour
{
    public static UIDocManagerOld Instance { get; private set; }
    public UIDocument document;
    public GameMenuManager gameMenuManager;
    public GM_Modals Modals { get; private set; }
    public GM_ShopMenu ShopMenu { get; private set; }
    public GM_EventLogger EventLogger { get; private set; }
    public ListView EventLoggerList;
    public VisualTreeAsset EventLoggerListEntry;
    public GM_Selector Selector { get; private set; }

    private UIDocManagerOld()
    {
        Instance = this;
    }
    
    void Start()
    {
        gameMenuManager = document.rootVisualElement.Q<GameMenuManager>();
        Modals = gameMenuManager.Q<GM_Modals>();
        ShopMenu = gameMenuManager.Q<GM_ShopMenu>();
        EventLogger = gameMenuManager.Q<GM_EventLogger>();
        EventLoggerList = EventLogger.Q<ListView>();
        Selector = gameMenuManager.Q<GM_Selector>();
        FillEventLogger();
    }

    private void FillEventLogger()
    {
        var items = new List<string>(15);
        for (int i = 0; i < 15; i++)
        {
            items.Add("item");
        }
        EventLoggerList.makeItem = () =>
        {
            var newListEntry = EventLoggerListEntry.Instantiate();

            return new Label();
        };

        EventLoggerList.bindItem = (item, index) =>
        {
            (item as Label).text = items[index] + index;
        };

        var listView = new ListView(items, 16, EventLoggerList.makeItem, EventLoggerList.bindItem);
        listView.selectionType = SelectionType.Single;
        listView.onItemsChosen += objects => Debug.Log(objects);
        listView.onSelectionChange += objects => Debug.Log(objects);
        listView.style.position = Position.Relative;
        listView.style.left = StyleKeyword.Auto;
        listView.style.top = StyleKeyword.Auto;
        listView.style.right = StyleKeyword.Auto;
        listView.style.bottom = StyleKeyword.Auto;
        EventLogger.Q("event-logger-window").Add(listView);
    }
}

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class InactiveChildrenCleanerTool
{
    public static List<Transform> objectList;

    public static void GetChildren(Transform parent, List<Transform> childrenList)
    {
        foreach(Transform transform in parent)
        {
            if(!transform.gameObject.activeInHierarchy)
            {
                childrenList.Add(transform);
            }
            GetChildren(transform, childrenList);
        }
    }
    [MenuItem("GameObject/Clean inactive Children")]
    public static void CleanChildren()
    {
        objectList = new();

        if(Selection.transforms.Length == 0)
        {
            EditorUtility.DisplayDialog("Clean Inactive Children", "You must select at least one GameObject", "Ok");
            return;
        }
        if(Selection.transforms.Length != 0)
        {
            foreach (Transform transform in Selection.transforms)
            {
                GetChildren(transform, objectList);
            }
        }
        string childrenList = GetStringList();

        bool authorizeDestroy = EditorUtility.DisplayDialog("Do you want to erase these inactive GameObjects ?", childrenList, "Yes", "No");

        if(authorizeDestroy) DestroyInactiveChildren();
       
    }
    [MenuItem("GameObject/Clean inactive Children", true)]
    public static bool CleanChildrenvalidate()
    {
        return Selection.transforms.Length != 0;
    }
    public static void ClearList()
    {
        objectList.Clear();
    }

    public static void DestroyInactiveChildren()
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            GameObject.DestroyImmediate(objectList[i].gameObject);
            
        }
    }
    public static string GetStringList()
    {
        string childrenList = "";

        for (int i = 0; i < objectList.Count; i++)
        {
            childrenList += "- " + objectList[i].gameObject.name + "\n";
        }
        return childrenList;
    }
}

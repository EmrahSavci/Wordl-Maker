using System.IO;
using System.Linq;
using UnityEditor;

public static class EnumCreator
{
#if UNITY_EDITOR
    public static string BasePath = "Assets/Resources/EnumStorage/";

    public static void CreateEnum(string ItemName, string[] ItemsToEnum)
    {
        if (!Directory.Exists(BasePath))
        {
            Directory.CreateDirectory(BasePath);
        }

        var Item = ItemName + ".cs";
        var AllPath = BasePath + Item;

        var FileInside = "public enum Enum_" + ItemName + "{ Empty,";
        if (ItemsToEnum.Length > 0)
        {
            
            foreach (var item in ItemsToEnum)
            {
                var _item = item.Replace(" ", "_");

                FileInside += " " + _item;
                if (_item != ItemsToEnum.Last())
                    FileInside += ",";
            }
            FileInside += "}";
        }
        else FileInside += "}";
        File.WriteAllText(AllPath, FileInside);
        AssetDatabase.Refresh();

    }
#endif

}
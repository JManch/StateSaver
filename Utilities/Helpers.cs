using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using SLZ.Marrow.Pool;

namespace StateSaver.Utilities {
internal static class Helpers {
    private static readonly HashSet<string> objectBlacklist =
        new HashSet<string> { "MuzzleFX_Void" };

    public static string CleanObjectName(string name) {
        Regex regex = new Regex(@"\[\d+\]|\(\d+\)");
        name = regex.Replace(name, "");
        name = name.Replace("(Clone)", "");
        return name.Trim();
    }

    public static bool IsBlacklisted(GameObject gameObject) {
        return objectBlacklist.Contains(CleanObjectName(gameObject.name));
    }

    public static string GetPath(Transform transform) {
        return GetPathR(transform, transform.name);
    }

    private static string GetPathR(Transform transform, string path) {
        if (transform.parent == null)
            return path;
        return GetPathR(transform.parent, transform.parent.name + "/" + path);
    }

    public static void CleanScene() {
        var poolees = GameObject.FindObjectsOfType<AssetPoolee>();
        foreach (var poolee in poolees) {
            GameObject gameObject = poolee.gameObject;
            if (!IsBlacklisted(gameObject)) {
                poolee.Despawn();
            }
        }
    }
}
}

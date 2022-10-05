using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using MelonLoader;
using SLZ.Marrow.Pool;
using SLZ.Props.Weapons;

using StateSaver.Utilities;

namespace StateSaver {

public static class BuildInfo {
    public const string Name = "StateSaver";
    public const string Author = "Manch";
    public const string Company = null;
    public const string Version = "1.0.0";
}

public class SceneState {
    [XmlArray("SceneObjects"),
     XmlArrayItem(typeof(StateObject), ElementName = "Object")]
    public List<StateObject> sceneObjects { get; }
    [XmlArray("SceneConstraints"),
     XmlArrayItem(typeof(StateConstraint), ElementName = "Constraint")]
    public List<StateConstraint> sceneConstraints { get; }

    public SceneState() {
        sceneObjects = new List<StateObject>();
        sceneConstraints = new List<StateConstraint>();
    }

    public string Serialize() {
        XmlSerializer xmlSerializer = new XmlSerializer(
            typeof(SceneState),
            new Type[] { typeof(StateObject), typeof(StateConstraint) });
        using (StringWriter sw = new StringWriter()) {
            xmlSerializer.Serialize(sw, this);
            return sw.ToString();
        }
    }

    public void SaveScene() {
        var poolees = GameObject.FindObjectsOfType<AssetPoolee>();
        foreach (var poolee in poolees) {
            GameObject gameObject = poolee.gameObject;
            if (gameObject.active && !Helpers.IsBlacklisted(gameObject)) {
                MelonLogger.Msg("Saving object with name " + poolee.name);

                sceneObjects.Add(new StateObject(gameObject));

                // Save constraints
                var constraints =
                    gameObject.GetComponentsInChildren<ConstraintTracker>(true);
                foreach (var constraint in constraints) {
                    if (constraint.isHost) {
                        var stateConstraint = new StateConstraint(constraint);
                        sceneConstraints.Add(stateConstraint);
                    }
                }
            }
        }
    }

    public void LoadScene() {}
}

public class StateObject {
    public int id { get; set; }
    public string name { get; set; }
    public Vector3 position { get; set; }
    public Quaternion rotation { get; set; }

    public StateObject(GameObject gameObject) {
        this.id = gameObject.GetInstanceID();
        this.name = Helpers.CleanObjectName(gameObject.name);
        this.position = gameObject.transform.position;
        this.rotation = gameObject.transform.rotation;
    }

    // For deserialisation
    private StateObject() {}
}

public class StateConstraint {
    public int idA { get; set; }
    public int idB { get; set; }
    public string rbPathA { get; set; }
    public string rbPathB { get; set; }
    public Constrainer.ConstraintMode mode { get; set; }
    public Vector3 anchorA { get; set; }
    public Vector3 anchorB { get; set; }

    public StateConstraint(ConstraintTracker constraint) {
        // Object A is the host
        this.idA = constraint.joint.transform.root.gameObject.GetInstanceID();
        this.idB = constraint.joint.connectedBody.transform.root.gameObject
                       .GetInstanceID();
        this.rbPathA = Utils.GetPath(constraint.joint.transform);
        this.rbPathB = Utils.GetPath(constraint.joint.connectedBody.transform);
        this.mode = constraint.mode;
        this.anchorA = constraint.joint.anchor;
        this.anchorB = constraint.joint.connectedAnchor;
    }

    // For deserialisation
    private StateConstraint() {}
}

public class Main : MelonMod {

    public SceneState debugSceneState;

    public override void OnUpdate() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            MelonLogger.Msg("Space was pressed");
            debugSceneState = new SceneState();
            debugSceneState.SaveScene();
        } else if (Input.GetKeyDown(KeyCode.P)) {
            MelonLogger.Msg("P was pressed");
            string serialized = debugSceneState.Serialize();
            MelonLogger.Msg(serialized);
            MelonLogger.Msg(Utils.Base64Encode(serialized));
        }
    }
}

}

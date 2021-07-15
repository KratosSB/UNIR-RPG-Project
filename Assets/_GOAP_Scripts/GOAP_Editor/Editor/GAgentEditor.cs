using System.Collections.Generic;
using GOAP_Utils;
using GOAP_Core;
using UnityEditor;
using UnityEngine;

namespace GOAP_Editor
{
    [CustomEditor(typeof(GAgentVisual))]
    [CanEditMultipleObjects]
    public class GAgentVisualEditor : Editor 
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            serializedObject.Update();
            GAgentVisual agent = (GAgentVisual) target;
            GUILayout.Label("Name: " + agent.name);
            GUILayout.Label("Current Action: " + agent.gameObject.GetComponent<GAgent>().CurrentAction);
            GUILayout.Label("Actions: ");
            foreach (GAction a in agent.gameObject.GetComponent<GAgent>().ActionList)
            {
                string pre = "";
                string eff = "";

                foreach (KeyValuePair<string, int> p in a.PreConditionsDictionary)
                    pre += p.Key + ", ";
                foreach (KeyValuePair<string, int> e in a.AfterEffectsDictionary)
                    eff += e.Key + ", ";

                GUILayout.Label("====  " + a.ActionName + "(" + pre + ")(" + eff + ")");
            }
            GUILayout.Label("Goals: ");
            foreach (KeyValuePair<SubGoal, int> g in agent.gameObject.GetComponent<GAgent>().GoalsDictionary)
            {
                GUILayout.Label("---: ");
                foreach (KeyValuePair<string, int> sg in g.Key.SGoals)
                    GUILayout.Label("=====  " + sg.Key);
            }
            GUILayout.Label("Beliefs: ");
            foreach (KeyValuePair<string, int> sg in agent.gameObject.GetComponent<GAgent>().Beliefs.GetStates())
            {
                GUILayout.Label("=====  " + sg.Key);
            }

            GUILayout.Label("Inventory: ");
            foreach (GameObject g in agent.gameObject.GetComponent<GAgent>().Inventory.ItemList)
            {
                GUILayout.Label("====  " + g.tag);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
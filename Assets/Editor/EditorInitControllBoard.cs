using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InitControllBoard))]
public class EditorInitControllBoard : Editor
{
    bool cardflod = true;
    bool knapsack = true;
    int cardorder = 0;
    public override void OnInspectorGUI()
    {
        InitControllBoard initcontroll = (InitControllBoard)target;
        EditorGUILayout.LabelField("初始化设置面板");
        EditorGUILayout.Space();

        cardflod = EditorGUILayout.Foldout(cardflod, "初始卡组");
        if (cardflod)
        {
            OnInspectorCarddeck(initcontroll);
        }
        EditorGUILayout.Space();
        knapsack = EditorGUILayout.Foldout(knapsack, "初始背包开放格");
        if (knapsack)
        {
            OnInspectorKnapsack(initcontroll);
        }
        EditorGUILayout.Space();


        if (GUI.changed)
        {
            EditorUtility.SetDirty(initcontroll);
        }
    }

    private void OnInspectorKnapsack(InitControllBoard initcontroll)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("设置初始背包开放格");
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();
        initcontroll.knapsackLaticInit[20] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[20], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[21] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[21], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[22] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[22], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[23] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[23], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[24] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[24], GUILayout.Width(15));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        initcontroll.knapsackLaticInit[15] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[15], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[16] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[16], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[17] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[17], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[18] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[18], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[19] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[19], GUILayout.Width(15));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        initcontroll.knapsackLaticInit[10] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[10], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[11] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[11], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[12] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[12], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[13] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[13], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[14] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[14], GUILayout.Width(15));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        initcontroll.knapsackLaticInit[5] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[5], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[6] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[6], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[7] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[7], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[8] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[8], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[9] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[9], GUILayout.Width(15));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        initcontroll.knapsackLaticInit[0] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[0], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[1] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[1], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[2] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[2], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[3] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[3], GUILayout.Width(15));
        initcontroll.knapsackLaticInit[4] = EditorGUILayout.Toggle(initcontroll.knapsackLaticInit[4], GUILayout.Width(15));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void OnInspectorCarddeck(InitControllBoard initcontroll)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("设置初始卡组");
        EditorGUILayout.Space();
        int toremove = -1;
        for(int i=0;i< initcontroll.carddeckInit.Count;i++)
        {
            EditorGUILayout.BeginHorizontal("box");
            initcontroll.carddeckInit[i] = EditorGUILayout.IntField(initcontroll.carddeckInit[i]);
            if (GUILayout.Button("移除"))
            {
                toremove = i;
            }
            EditorGUILayout.EndHorizontal();
        }
        if (toremove != -1)
        {
            initcontroll.carddeckInit.Remove(initcontroll.carddeckInit[toremove]);
            toremove = -1;
        }
        EditorGUILayout.BeginHorizontal();
        cardorder = EditorGUILayout.IntField(cardorder,GUILayout.Width(50));
        if (GUILayout.Button("添加"))
        {
            initcontroll.carddeckInit.Add(cardorder);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

}

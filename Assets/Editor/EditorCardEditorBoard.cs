using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(CardEditorBoard))]
public class EditorCardEditorBoard : Editor
{
    private editorCardCollect selectcard;
    private bool isSelect = false;
    private ReorderableList reorderList;

    bool foldout_Cards = true;
    Vector2 scrollPos = new Vector2(0, 0);
    int toremove = -1;
    int toremoveeffect = -1;

    private void OnEnable()
    {
        CardEditorBoard cardboard = (CardEditorBoard)target;
        reorderList = new ReorderableList(cardboard.allCards, cardboard.allCards.GetType());
        //绘制元素
        reorderList.drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
          {
              editorCardCollect ECard = cardboard.allCards[index];
              rect.y += 2;
              rect.height = EditorGUIUtility.singleLineHeight;
              rect.width = 20;
              //id
              EditorGUI.LabelField(rect, "ID");
              rect.x += 15;
              EditorGUI.BeginDisabledGroup(true);
              EditorGUI.IntField(rect,ECard.Card.id);
              EditorGUI.EndDisabledGroup();
              //name
              rect.x += 25;
              rect.width = 40;
              EditorGUI.LabelField(rect, "Name");
              rect.x += 35;
              rect.width = 80;
              ECard.Card.name = EditorGUI.TextField(rect, ECard.Card.name);
              //cost
              rect.x += 85;
              rect.width = 40;
              EditorGUI.LabelField(rect, "Cost");
              rect.x += 35;
              rect.width = 20;
              ECard.Card.cost = EditorGUI.IntField(rect, ECard.Card.cost);
              //kind
              rect.x += 25;
              rect.width = 40;
              EditorGUI.LabelField(rect, "Kind");
              rect.x += 35;
              rect.width = 50;
              ECard.Card.Kind = (CardKind)EditorGUI.EnumPopup(rect, ECard.Card.Kind);
              //rank
              rect.x += 55;
              rect.width = 40;
              EditorGUI.LabelField(rect, "Rank");
              rect.x += 35;
              rect.width = 40;
              ECard.Card.Rank = (Rank)EditorGUI.EnumPopup(rect, ECard.Card.Rank);
              //remove
              rect.x += 50;
              rect.width = 60;
              if (GUI.Button(rect,"Remove"))
              {
                  if (ECard == selectcard)
                  {
                      selectcard = null;
                      isSelect = false;
                  }
                  toremove = index;
              }
          };
        //绘制表头
        reorderList.drawHeaderCallback = (Rect rect) =>
        {
            GUI.Label(rect, "卡池总表");
        };

        //添加回调
        reorderList.onAddCallback = (ReorderableList list) =>
          {
              cardboard.AddCard();
          };
        //重新排序
        reorderList.onReorderCallback = (ReorderableList list) =>
          {
              cardboard.AutoIdOrder();
          };
        //选择
        reorderList.onSelectCallback = (ReorderableList list) =>
          {
              selectcard = cardboard.allCards[list.index];
              isSelect = true;
          };
        //移除回调
        reorderList.onRemoveCallback = (ReorderableList list) =>
        {
            if (selectcard != null)
            {
                toremove = list.index;
                selectcard = null;
                isSelect = false;
            }
        };
    }
    public override void OnInspectorGUI()
    {
        CardEditorBoard cardboard = (CardEditorBoard)target;
        //标题
        GUI.skin.label.fontSize = 15;
        GUILayout.Label("数据编辑器");
        GUI.skin.label.fontSize = 12;
        EditorGUILayout.Space();
        cardGUI(cardboard);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(cardboard);
        }
    }

    void cardGUI(CardEditorBoard cardboard)
    {
        //卡牌编辑器
        //标题
        GUI.color = Color.cyan;
        EditorGUILayout.BeginHorizontal("box");
        GUILayout.Space(10);
        foldout_Cards = EditorGUILayout.Foldout(foldout_Cards, "卡池编辑器");
        GUI.color = Color.white;
        if (GUILayout.Button("重新计算id"))
        {
            cardboard.AutoIdOrder();
        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;

        if (foldout_Cards)
        {
            GUILayout.BeginVertical("box");
            #region 绘制卡池列表
            EditorGUILayout.BeginVertical("box", GUILayout.Height(350));
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            reorderList.DoLayoutList();
            //处理删除卡
            if (toremove != -1)
            {
                cardboard.allCards.Remove(cardboard.allCards[toremove]);
                toremove = -1;
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            #endregion
            GUILayout.EndVertical();
            #region 卡牌编辑面板
            GUI.color = new Color(0.8f, 1, 0.9f);
            if (isSelect)
            {
                EditorGUILayout.BeginVertical("box");
                #region 第一行 名称 费用 id
                EditorGUILayout.BeginHorizontal();
                selectcard.showGrade = EditorGUILayout.Toggle(selectcard.showGrade,GUILayout.Width(10));
                if (selectcard.showGrade)
                {
                    EditorGUILayout.LabelField("升级版  卡名", GUILayout.Width(70));
                    selectcard.gradeCard.name = EditorGUILayout.TextField(selectcard.gradeCard.name);
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField("费用", GUILayout.Width(35));
                    selectcard.gradeCard.cost = EditorGUILayout.IntField(selectcard.gradeCard.cost, GUILayout.Width(20));
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField("种类", GUILayout.Width(30));
                    selectcard.gradeCard.Kind = (CardKind)EditorGUILayout.EnumPopup(selectcard.gradeCard.Kind, GUILayout.Width(50));
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField("品级", GUILayout.Width(30));
                    selectcard.gradeCard.Rank = (Rank)EditorGUILayout.EnumPopup(selectcard.gradeCard.Rank, GUILayout.Width(50));
                    EditorGUILayout.LabelField("Id", GUILayout.Width(17));
                    EditorGUI.BeginDisabledGroup(true);
                    selectcard.gradeCard.id = EditorGUILayout.IntField(selectcard.gradeCard.id, GUILayout.Width(20));
                    EditorGUI.EndDisabledGroup();
                }
                else
                {
                    EditorGUILayout.LabelField("升级版  卡名", GUILayout.Width(70));
                    selectcard.Card.name = EditorGUILayout.TextField(selectcard.Card.name);
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField("费用", GUILayout.Width(35));
                    selectcard.Card.cost = EditorGUILayout.IntField(selectcard.Card.cost, GUILayout.Width(20));
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField("种类", GUILayout.Width(30));
                    selectcard.Card.Kind = (CardKind)EditorGUILayout.EnumPopup(selectcard.Card.Kind, GUILayout.Width(50));
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField("品级", GUILayout.Width(30));
                    selectcard.Card.Rank = (Rank)EditorGUILayout.EnumPopup(selectcard.Card.Rank, GUILayout.Width(50));
                    EditorGUILayout.LabelField("Id", GUILayout.Width(17));
                    EditorGUI.BeginDisabledGroup(true);
                    selectcard.Card.id = EditorGUILayout.IntField(selectcard.Card.id, GUILayout.Width(20));
                    EditorGUI.EndDisabledGroup();
                }

                EditorGUILayout.EndHorizontal();
                #endregion
                #region 第二行 效果&演示
                GUI.color = Color.white;
                EditorGUILayout.BeginHorizontal();
                #region 效果表
                if (selectcard.showGrade)
                {
                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.LabelField("效果表", GUILayout.MinWidth(60));
                    for (int i = 0; i < selectcard.gradeCard.playEffects.Count; i++)
                    {
                        EditorGUILayout.BeginVertical("box");
                        editorEffect effect = selectcard.gradeCard.playEffects[i];
                        //名称 数值 移除
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("【" + effect.name + "】", GUILayout.MinWidth(60));
                        EditorGUILayout.LabelField("Num", GUILayout.Width(35));
                        effect.num = EditorGUILayout.IntField(effect.num, GUILayout.Width(20));
                        if (GUILayout.Button("Remove"))
                        {
                            toremoveeffect = i;
                        }
                        EditorGUILayout.EndHorizontal();
                        //子效果表
                        if (effect.b_haveChildEffect || effect.b_haveJudge)
                        {
                            childEffectShow(effect);
                        }
                        EditorGUILayout.EndVertical();
                    }
                    if (toremoveeffect != -1)
                    {
                        selectcard.gradeCard.playEffects.Remove(selectcard.gradeCard.playEffects[toremoveeffect]);
                        toremoveeffect = -1;
                    }
                    //添加效果行
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Add Effect", GUILayout.Width(60));
                    EnumEffect enumEffect = (EnumEffect)EditorGUILayout.EnumPopup(EnumEffect.Default);
                    if (enumEffect != EnumEffect.Default)
                    {
                        selectcard.gradeCard.playEffects.Add(CardEditorBoard.EffectFromEnum(enumEffect));
                        enumEffect = EnumEffect.Default;
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.EndVertical();
                }
                else
                {
                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.LabelField("效果表", GUILayout.MinWidth(60));
                    for (int i = 0; i < selectcard.Card.playEffects.Count; i++)
                    {
                        EditorGUILayout.BeginVertical("box");
                        editorEffect effect = selectcard.Card.playEffects[i];
                        //名称 数值 移除
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("【" + effect.name + "】", GUILayout.MinWidth(60));
                        EditorGUILayout.LabelField("Num", GUILayout.Width(35));
                        effect.num = EditorGUILayout.IntField(effect.num, GUILayout.Width(20));
                        if (GUILayout.Button("Remove"))
                        {
                            toremoveeffect = i;
                        }
                        EditorGUILayout.EndHorizontal();
                        //子效果表
                        if (effect.b_haveChildEffect || effect.b_haveJudge)
                        {
                            childEffectShow(effect);
                        }
                        EditorGUILayout.EndVertical();
                    }
                    if (toremoveeffect != -1)
                    {
                        selectcard.Card.playEffects.Remove(selectcard.Card.playEffects[toremoveeffect]);
                        toremoveeffect = -1;
                    }
                    //添加效果行
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Add Effect", GUILayout.Width(60));
                    EnumEffect enumEffect = (EnumEffect)EditorGUILayout.EnumPopup(EnumEffect.Default);
                    if (enumEffect != EnumEffect.Default)
                    {
                        selectcard.Card.playEffects.Add(CardEditorBoard.EffectFromEnum(enumEffect));
                        selectcard.gradeCard.playEffects.Add(CardEditorBoard.EffectFromEnum(enumEffect));
                        enumEffect = EnumEffect.Default;
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.EndVertical();
                }

                #endregion
                #region 演出表
                EditorGUILayout.BeginVertical("box");
                //帮助信息
                EditorGUILayout.HelpBox("动画：1-待机 2-攻击 3-受击",MessageType.Info);
                //第一行 标题 排序按钮
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("演出表",GUILayout.MinWidth(60));
                EditorGUILayout.LabelField("总时间", GUILayout.Width(40));
                selectcard.alltime = EditorGUILayout.FloatField(selectcard.alltime, GUILayout.Width(40));
                if (GUILayout.Button("Sort by time"))
                {
                    selectcard.performlist.Sort();
                }
                EditorGUILayout.EndHorizontal();
                //演出列表
                int j = 0;
                foreach(editorPerform ep in selectcard.performlist)
                {
                    EditorGUILayout.BeginVertical("box");
                    //1、动画/效果  (主角/敌人   路径)  remove
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("演出", GUILayout.Width(30));
                    ep.performkind =(PerformKind) EditorGUILayout.EnumPopup(ep.performkind,GUILayout.Width(40));
                    if (ep.performkind == PerformKind.动画)
                    {
                        EditorGUILayout.LabelField("Who", GUILayout.Width(30));
                        ep.charactor = (Charactor)EditorGUILayout.EnumPopup(ep.charactor, GUILayout.Width(80));
                        GUILayout.Space(30);
                    }
                    else if(ep.performkind==PerformKind.特效)
                    {
                        EditorGUILayout.LabelField("Way", GUILayout.Width(30));
                        ep.effectWay = (EffectWay)EditorGUILayout.EnumPopup(ep.effectWay, GUILayout.Width(80));
                        GUILayout.Space(30);
                    }
                    if (GUILayout.Button("Remove"))
                    {
                        toremoveeffect = j;
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(10);
                    ep.time = EditorGUILayout.Slider(ep.time, 0, 1);
                    //
                    EditorGUILayout.BeginHorizontal();
                    if (ep.performkind == PerformKind.动画)
                    {
                        EditorGUILayout.LabelField("动画序号", GUILayout.Width(50));
                        ep.animation = EditorGUILayout.IntField(ep.animation, GUILayout.Width(100));
                        GUILayout.Space(30);
                        EditorGUILayout.LabelField("动画倍速", GUILayout.Width(50));
                        ep.animationSpeed = EditorGUILayout.FloatField(ep.animationSpeed, GUILayout.Width(30));
                    }
                    else if (ep.performkind == PerformKind.特效)
                    {
                        EditorGUILayout.LabelField("特效", GUILayout.Width(50));
                        ep.effectGO = (GameObject)EditorGUILayout.ObjectField(ep.effectGO,typeof(GameObject), GUILayout.Width(100));
                        EditorGUILayout.LabelField("time", GUILayout.Width(30));
                        ep.effecttime = EditorGUILayout.FloatField(ep.effecttime, GUILayout.Width(20));
                        if (ep.effectWay == EffectWay.从主角到怪物 | ep.effectWay == EffectWay.从怪物到主角)
                        {
                            EditorGUILayout.LabelField("移动速度", GUILayout.Width(50));
                            ep.movespeed = EditorGUILayout.FloatField(ep.movespeed, GUILayout.Width(30));
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.EndVertical();
                    j++;
                }
                if (toremoveeffect != -1)
                {
                    selectcard.performlist.Remove(selectcard.performlist[toremoveeffect]);
                    toremoveeffect = -1;
                }
                PerformKind newperform = PerformKind.添加演出;
                newperform = (PerformKind)EditorGUILayout.EnumPopup(newperform);
                if (newperform != PerformKind.添加演出)
                {
                    selectcard.performlist.Add(new editorPerform(newperform));
                }

                EditorGUILayout.EndVertical();
                #endregion
                EditorGUILayout.EndHorizontal();
                #endregion
                EditorGUILayout.EndVertical();

            }
            #endregion
            GUI.color = Color.white;
        }
    }

    void childEffectShow(editorEffect effect)
    {
        EditorGUILayout.LabelField("子效果表");
        EditorGUILayout.BeginVertical("box");
        int i = 0;
        int toremovee = -1;
        if (effect.b_haveJudge)
        {
            int j = 0;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("如果", GUILayout.MinWidth(80));

            foreach(editorJudge ej in effect.judges)
            {
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField(ej.judgeKind.ToString(), GUILayout.MinWidth(40));
                EditorGUI.BeginDisabledGroup(!ej.b_UseNum);
                ej.num = EditorGUILayout.IntField(ej.num,GUILayout.Width(20));
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Remove"))
                {
                    toremovee = j;
                }
                EditorGUILayout.EndHorizontal();
                j++;
            }
            if (toremovee != -1)
            {
                effect.judges.Remove(effect.judges[toremovee]);
                toremovee = -1;
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Add Judge", GUILayout.Width(60));
            EnumJudge enumJudge = EnumJudge.默认;
            enumJudge = (EnumJudge)EditorGUILayout.EnumPopup(enumJudge, GUILayout.MinWidth(60));
            if (enumJudge != EnumJudge.默认)
            {
                effect.judges.Add(CardEditorBoard.JudgeFormEnum(enumJudge));
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("执行", GUILayout.MinWidth(80));
        }
        foreach(editorEffect ce in effect.childeffects)
        {
            EditorGUILayout.BeginVertical("box");
            //名称 数值 移除
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("【" + ce.name + "】", GUILayout.MinWidth(60));
            EditorGUILayout.LabelField("Num", GUILayout.Width(35));
            ce.num = EditorGUILayout.IntField(ce.num, GUILayout.Width(20));
            if (GUILayout.Button("Remove"))
            {
                toremovee = i;
            }
            EditorGUILayout.EndHorizontal();

            //子效果
            if (ce.b_haveChildEffect)
            {
                childEffectShow(ce);
            }

            EditorGUILayout.EndVertical();
            i++;
        }
        if (toremovee != -1)
        {
            effect.childeffects.Remove(effect.childeffects[toremovee]);
        }
        //添加效果行
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Add Effect", GUILayout.Width(60));
        EnumEffect enumEffect = (EnumEffect)EditorGUILayout.EnumPopup(EnumEffect.Default);
        if (enumEffect != EnumEffect.Default)
        {
            effect.childeffects.Add(CardEditorBoard.EffectFromEnum(enumEffect));
            enumEffect = EnumEffect.Default;
        }
        EditorGUILayout.EndHorizontal();
        if (effect.b_haveJudge)
        {
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }
}

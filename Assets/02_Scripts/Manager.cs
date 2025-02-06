using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MoveManager))]
public class Manager : Editor
{
    public override void OnInspectorGUI()  //该方法会绘制在Inspector面板上  
    {
        MoveManager moveManager = (MoveManager)target;
        moveManager.Item = EditorGUILayout.Vector2Field("Item:", moveManager.Item);
        EditorGUILayout.HelpBox("确保Content下没有HorizontalLayoutGroup和VerticalLayoutGroup组件", MessageType.None);
        moveManager.Unit = (DicType)EditorGUILayout.EnumPopup("列表类型",moveManager.Unit);//枚举列表
        if (moveManager.Unit == DicType.Horizontal)
        {
            moveManager.isShowHor = EditorGUILayout.Foldout(moveManager.isShowHor, "Horizontal");//展开标题
            if (moveManager.isShowHor)
            {
                EditorGUI.indentLevel++;
                moveManager.isShowHorPadding = EditorGUILayout.Foldout(moveManager.isShowHorPadding, "Padding");//展开标题
                if (moveManager.isShowHorPadding)
                {
                    EditorGUI.indentLevel++;
                    moveManager.Left = EditorGUILayout.IntField("Left", moveManager.Left);
                    moveManager.Right = EditorGUILayout.IntField("Right", moveManager.Right);
                    moveManager.Top = EditorGUILayout.IntField("Top", moveManager.Top);
                    moveManager.Bottom = EditorGUILayout.IntField("Bottom", moveManager.Bottom);
                }
                EditorGUI.indentLevel--;
                moveManager.Spacing =  EditorGUILayout.IntField("    Spacing", moveManager.Spacing);

                moveManager.childAlignmentType = (ChildAlignment)EditorGUILayout.EnumPopup("    Child Alignment", moveManager.childAlignmentType);//枚举列表

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("    Control Child Size");
                moveManager.ControlChildSizeWidth = GUILayout.Toggle(moveManager.ControlChildSizeWidth, "Width");
                moveManager.ControlChildSizeHeight = GUILayout.Toggle(moveManager.ControlChildSizeHeight, "Height");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("    Use Child Scale   ");
                moveManager.UseChildScaleWidth = GUILayout.Toggle(moveManager.UseChildScaleWidth, "Width");
                moveManager.UseChildScaleHeight = GUILayout.Toggle(moveManager.UseChildScaleHeight, "Height");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("    Child Force Expand");
                moveManager.ChildForceExpandWidth = GUILayout.Toggle(moveManager.ChildForceExpandWidth, "Width");
                moveManager.ChildForceExpandHeight = GUILayout.Toggle(moveManager.ChildForceExpandHeight, "Height");
                EditorGUILayout.EndHorizontal();
            }
        }
        if (moveManager.Unit == DicType.Vertical)
        {
            moveManager.isShowVer = EditorGUILayout.Foldout(moveManager.isShowVer, "Vertical");//展开标题
            if (moveManager.isShowVer)
            {
                EditorGUI.indentLevel++;
                moveManager.isShowVerPadding = EditorGUILayout.Foldout(moveManager.isShowVerPadding, "Padding");//展开标题
                if (moveManager.isShowVerPadding)
                {
                    EditorGUI.indentLevel++;
                    moveManager.Left = EditorGUILayout.IntField("Left", moveManager.Left);
                    moveManager.Right = EditorGUILayout.IntField("Right", moveManager.Right);
                    moveManager.Top = EditorGUILayout.IntField("Top", moveManager.Top);
                    moveManager.Bottom = EditorGUILayout.IntField("Bottom", moveManager.Bottom);
                }
                EditorGUI.indentLevel--;
                moveManager.Spacing = EditorGUILayout.IntField("    Spacing", moveManager.Spacing);

                moveManager.childAlignmentType = (ChildAlignment)EditorGUILayout.EnumPopup("    Child Alignment", moveManager.childAlignmentType);//枚举列表

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("    Control Child Size");
                moveManager.ControlChildSizeWidth = GUILayout.Toggle(moveManager.ControlChildSizeWidth, "Width");
                moveManager.ControlChildSizeHeight = GUILayout.Toggle(moveManager.ControlChildSizeHeight, "Height");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("    Use Child Scale   ");
                moveManager.UseChildScaleWidth = GUILayout.Toggle(moveManager.UseChildScaleWidth, "Width");
                moveManager.UseChildScaleHeight = GUILayout.Toggle(moveManager.UseChildScaleHeight, "Height");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("    Child Force Expand");
                moveManager.ChildForceExpandWidth = GUILayout.Toggle(moveManager.ChildForceExpandWidth, "Width");
                moveManager.ChildForceExpandHeight = GUILayout.Toggle(moveManager.ChildForceExpandHeight, "Height");
                EditorGUILayout.EndHorizontal();
            }
        }



    }
}

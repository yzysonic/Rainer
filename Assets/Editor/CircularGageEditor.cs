using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;



[CustomEditor(typeof(CircularGage), true)]
[CanEditMultipleObjects]
public class CircularGageEditor : RawImageEditor
{
    SerializedProperty m_Texture;
    SerializedProperty m_UVRect;
    SerializedProperty m_Radius;
    GUIContent m_UVRectContent;

    protected override void OnEnable()
    {
        base.OnEnable();

        // Note we have precedence for calling rectangle for just rect, even in the Inspector.
        // For example in the Camera component's Viewport Rect.
        // Hence sticking with Rect here to be consistent with corresponding property in the API.
        m_UVRectContent = new GUIContent("UV Rect");

        m_Texture = serializedObject.FindProperty("m_Texture");
        m_UVRect = serializedObject.FindProperty("m_UVRect");
        m_Radius = serializedObject.FindProperty("radius");

        SetShowNativeSize(true);
    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(m_Radius);
        EditorGUILayout.PropertyField(m_Texture);
        AppearanceControlsGUI();
        //RaycastControlsGUI();
        //EditorGUILayout.PropertyField(m_UVRect, m_UVRectContent);
        SetShowNativeSize(false);
        NativeSizeButtonGUI();

        if(EditorGUI.EndChangeCheck())
        {
            var mat = m_Material.objectReferenceValue as Material;
            mat.SetFloat("_Radius", m_Radius.floatValue);
        }

        serializedObject.ApplyModifiedProperties();
    }

    void SetShowNativeSize(bool instant)
    {
        base.SetShowNativeSize(m_Texture.objectReferenceValue != null, instant);
    }


}

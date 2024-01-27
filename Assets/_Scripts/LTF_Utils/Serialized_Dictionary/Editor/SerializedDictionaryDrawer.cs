#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LTF.SerializedDictionary.Editor
{
    [CustomPropertyDrawer(typeof(SerializedDictionary<,>), true)]
    public class SerializedDictionaryDrawer : PropertyDrawer
    {
        private const float SPACE = 17f;
        private const BindingFlags privateInstanceFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        private static readonly GUIContent r_cachedContent = new();

        private float _elementHeight;
        private bool _isEnabled = false;
        private bool _foldoutRList;
        private bool _isReadOnly;

        private int _selectedIndex;
        private ReorderableList _reorderableList;
        private SerializedProperty _entriesProperty;

        private bool IsDragging => (bool)_reorderableList.GetType().GetField("m_Dragging", privateInstanceFlags).GetValue(_reorderableList);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
            => _foldoutRList ? (_reorderableList != null ? _reorderableList.GetHeight() : 0) + SPACE : SPACE;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (!_isEnabled)
                // this shouldn't be here, but since CanCacheInspectorGUI is apparently not called
                // there is not currently way to call this just on enabled
                OnEnable(property);

            _foldoutRList = EditorGUI.Foldout(new Rect(rect.position, new Vector2(rect.size.x, SPACE)), _foldoutRList, label, true);

            if (_foldoutRList && _reorderableList != null)
            {
                rect.y += SPACE;
                _reorderableList.DoList(rect);
            }
        }

        // TODO: Figure out why this is not getting called
        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            if (!_isEnabled)
                OnEnable(property);

            EditorPrefs.SetBool(property.name, _foldoutRList);
            return true;
        }

        private void OnEnable(SerializedProperty property)
        {
            _isEnabled = true;

            // Enabled
            _foldoutRList = EditorPrefs.GetBool(property.name, false);
            _isReadOnly = Attribute.IsDefined(fieldInfo, typeof(ReadOnlyAttribute));

            // Initialize List
            _entriesProperty = property.FindPropertyRelative("_entries");
            if (_entriesProperty == null)
                _reorderableList = new ReorderableList(new List<object>(), typeof(object), false, true, false, false);
            else
            {
                // Create List
                _reorderableList = new(property.serializedObject, _entriesProperty, !_isReadOnly, true, !_isReadOnly, !_isReadOnly)
                {
                    drawHeaderCallback = rect => EditorGUI.LabelField(rect, fieldInfo.Name),
                    onAddCallback = _ => _entriesProperty.InsertArrayElementAtIndex(_entriesProperty.arraySize),
                    onRemoveCallback = _ => _entriesProperty.DeleteArrayElementAtIndex(_selectedIndex),
                    onReorderCallback = list =>
                    {
                        // TODO: Moving elements sometimes doesn't work
                        _entriesProperty.MoveArrayElement(_selectedIndex, list.index);
                    },
                    elementHeightCallback = index =>
                    {
                        var keyHeight = EditorGUI.GetPropertyHeight(_entriesProperty
                            .GetArrayElementAtIndex(index)
                            .FindPropertyRelative("Key"));
                        var valueHeight = EditorGUI.GetPropertyHeight(_entriesProperty
                            .GetArrayElementAtIndex(index)
                            .FindPropertyRelative("Value"));
                        var height = 8f + Math.Max(keyHeight, valueHeight);

                        if (!IsDragging || (IsDragging && _elementHeight < height))
                            _elementHeight = height;

                        return _elementHeight;
                    },
                    drawElementCallback = (rect, index, istActive, isFocused) =>
                    {
                        // Before DrawProperties
                        var oldWidth = EditorGUIUtility.labelWidth;
                        EditorGUIUtility.labelWidth = 50f;
                        var wasEnabled = GUI.enabled;
                        GUI.enabled = !_isReadOnly;

                        // Draw Properties
                        rect.position = new Vector2(rect.position.x + 10, rect.position.y);

                        var halfSizeX = rect.size.x * .5f;
                        var keyProp = _entriesProperty.GetArrayElementAtIndex(index).FindPropertyRelative("Key");
                        const float LeftOffset = 100f;
                        var sizeKey = new Vector2(halfSizeX - LeftOffset, rect.size.y);
                        EditorGUI.PropertyField(new(rect.position, sizeKey), keyProp, GUIContent(keyProp.type), true);

                        var valueProp = _entriesProperty.GetArrayElementAtIndex(index).FindPropertyRelative("Value");
                        const float RightOffset = 58f;
                        var sizeValue = new Vector2(halfSizeX + RightOffset, rect.size.y);
                        var positionValue = rect.position + new Vector2(sizeKey.x + 25f, 0f);
                        EditorGUI.PropertyField(new(positionValue, sizeValue), valueProp, GUIContent(valueProp.type), true);

                        // After DrawProperies
                        GUI.enabled = wasEnabled;
                        EditorGUIUtility.labelWidth = oldWidth;

                        static GUIContent GUIContent(string text)
                        {
                            r_cachedContent.text = text;
                            return r_cachedContent;
                        }
                    },
                };
                _reorderableList.onSelectCallback += list => _selectedIndex = list.index;
            }
        }
    }
}
#endif

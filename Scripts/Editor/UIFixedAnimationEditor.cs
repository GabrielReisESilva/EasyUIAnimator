using EasyUIAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIFixedAnimation))]
public class UIFixedAnimationEditor : Editor {

    public override void OnInspectorGUI()
    {
        UIFixedAnimation fixedAnim = (UIFixedAnimation)target;
        fixedAnim.animType = (AnimationType) EditorGUILayout.EnumPopup("Animation",fixedAnim.animType);
        if(fixedAnim.animType != AnimationType.GROUP)
        {
            if (GUILayout.Button("Use Current Value"))
                UseCurrentValue(fixedAnim);
        }
        
        switch (fixedAnim.animType)
        {
            case AnimationType.MOVE:
                fixedAnim.startV3 = EditorGUILayout.Vector3Field("Start Position", fixedAnim.startV3);
                fixedAnim.finalV3 = EditorGUILayout.Vector3Field("Final Position", fixedAnim.finalV3);
                fixedAnim.useScreenValues = EditorGUILayout.Toggle("Use Screen Values", fixedAnim.useScreenValues);
                break;
            case AnimationType.SCALE:
                fixedAnim.startV3 = EditorGUILayout.Vector3Field("Start Scale", fixedAnim.startV3);
                fixedAnim.finalV3 = EditorGUILayout.Vector3Field("Final Scale", fixedAnim.finalV3);
                break;
            case AnimationType.ROTATION:
                fixedAnim.startV3 = EditorGUILayout.Vector3Field("Start Rotation", fixedAnim.startV3);
                fixedAnim.finalV3 = EditorGUILayout.Vector3Field("Final Rotation", fixedAnim.finalV3);
                break;
            case AnimationType.IMAGE:
                fixedAnim.startColor = EditorGUILayout.ColorField("Start Color", fixedAnim.startColor);
                fixedAnim.finalColor = EditorGUILayout.ColorField("Final Color", fixedAnim.finalColor);
                break;
            case AnimationType.GROUP:
                SerializedProperty tps = serializedObject.FindProperty("fixedAnimations");
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(tps, true);
                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
                break;
            default:
                break;
        }

        fixedAnim.disableAfter = EditorGUILayout.Toggle("Disable On Finish", fixedAnim.disableAfter);
        if(!fixedAnim.disableAfter)
            fixedAnim.loop = (Loop)EditorGUILayout.EnumPopup("Loop Options", fixedAnim.loop);
        fixedAnim.delay = EditorGUILayout.FloatField("Delay", fixedAnim.delay);
        if (fixedAnim.animType != AnimationType.GROUP)
        {
            fixedAnim.duration = EditorGUILayout.FloatField("Duration", fixedAnim.duration);
            fixedAnim.mod = (Modifiers)EditorGUILayout.EnumPopup("Modifier", fixedAnim.mod);
            if(fixedAnim.animType != AnimationType.IMAGE)
            {
                fixedAnim.effect = (Effects)EditorGUILayout.EnumPopup("Effect", fixedAnim.effect);
                if (fixedAnim.effect != Effects.NONE)
                {
                    fixedAnim.max = EditorGUILayout.FloatField("Max", fixedAnim.max);
                    if (fixedAnim.effect != Effects.EXPLOSION)
                    {
                        fixedAnim.bounce = EditorGUILayout.IntField("Bounce", fixedAnim.bounce);
                        fixedAnim.randomDirection = false;
                    }
                    else
                    {
                        fixedAnim.randomDirection = EditorGUILayout.Toggle("Random Direction", fixedAnim.randomDirection);
                    }
                    if (!fixedAnim.randomDirection && fixedAnim.animType == AnimationType.MOVE)
                    {
                        fixedAnim.effectRotation = EditorGUILayout.Vector3Field("Effect Rotation", fixedAnim.effectRotation);
                    }
                }
            }
        }

        fixedAnim.playOnStart  = EditorGUILayout.Toggle("Play On Start", fixedAnim.playOnStart);
        fixedAnim.playOnEnable = EditorGUILayout.Toggle("Play On Enable", fixedAnim.playOnEnable);
        fixedAnim.playAudioOnPlay = EditorGUILayout.Toggle("Play Audio On Animation Start", fixedAnim.playAudioOnPlay);
        if (GUILayout.Button("Play"))
            fixedAnim.Play();
    }

    private void UseCurrentValue(UIFixedAnimation anim)
    {
        switch (anim.animType)
        {
            case AnimationType.MOVE:
                if(anim.useScreenValues)
                    anim.startV3 = anim.transform.position;
                else
                    anim.startV3 = Vector3.Scale (anim.transform.position, EasyUIAnimator.UIAnimator.InvertedScreenDimension);
                break;
            case AnimationType.SCALE:
                anim.startV3 = anim.transform.localScale;
                break;
            case AnimationType.ROTATION:
                anim.startV3 = anim.transform.localRotation.eulerAngles;
                break;
            case AnimationType.IMAGE:
                anim.startColor = anim.GetComponent<UnityEngine.UI.Graphic>().color;
                break;
        }
    }
}

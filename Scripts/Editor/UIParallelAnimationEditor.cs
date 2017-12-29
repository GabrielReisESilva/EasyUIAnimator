using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EasyUIAnimator;

[CustomEditor(typeof(UIParallelAnimation))]
public class UIParallelAnimationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UIParallelAnimation parallelAnim = (UIParallelAnimation)target;

        //MOVE ANIMATION
        if (parallelAnim.moveAnimation)
        {
            if (GUILayout.Button("- Move Animation", GUILayout.Height(30)))   parallelAnim.moveAnimation = false;
            if (GUILayout.Button("Use Current Value"))  UseCurrentValue(AnimationType.MOVE, parallelAnim);
            parallelAnim.start[0]           = EditorGUILayout.Vector3Field("Start Position", parallelAnim.start[0]);
            parallelAnim.final[0]           = EditorGUILayout.Vector3Field("Final Position", parallelAnim.final[0]);
            parallelAnim.useScreenValues    = EditorGUILayout.Toggle("Use Screen Values", parallelAnim.useScreenValues);
            parallelAnim.isBezier           = EditorGUILayout.Toggle("Bézier Curve", parallelAnim.isBezier);
            if (parallelAnim.isBezier)
            {
                parallelAnim.isCubicBezier = EditorGUILayout.Toggle("Cubic Bézier Curve", parallelAnim.isCubicBezier);
                
                parallelAnim.bezierP1 = EditorGUILayout.Vector2Field("P1", parallelAnim.bezierP1);
                if(parallelAnim.isCubicBezier)
                    parallelAnim.bezierP2 = EditorGUILayout.Vector2Field("P2", parallelAnim.bezierP2);
            }
            else
            {
                parallelAnim.moveModifier = (Modifiers)EditorGUILayout.EnumPopup("Modifier", parallelAnim.moveModifier);
                parallelAnim.moveEffect = (Effects)EditorGUILayout.EnumPopup("Effect", parallelAnim.moveEffect);
                if (parallelAnim.moveEffect != Effects.NONE)
                {
                    parallelAnim.max[0] = EditorGUILayout.FloatField("Max", parallelAnim.max[0]);
                    if (parallelAnim.moveEffect != Effects.EXPLOSION)
                    {
                        parallelAnim.bounce[0] = EditorGUILayout.IntField("Bounce", parallelAnim.bounce[0]);
                        parallelAnim.randomDirection = false;
                    }
                    else
                    {
                        parallelAnim.randomDirection = EditorGUILayout.Toggle("Random Direction", parallelAnim.randomDirection);
                    }
                    if (!parallelAnim.randomDirection)
                    {
                        parallelAnim.effectRotation = EditorGUILayout.Vector3Field("Effect Rotation", parallelAnim.effectRotation);
                    }
                }
            }
        }
        else
            if (GUILayout.Button("+ Move Animation", GUILayout.Height(30))) parallelAnim.moveAnimation = true;

        //SCALE ANIMATION
        if (parallelAnim.scaleAnimation)
        {
            if (GUILayout.Button("- Scale Animation", GUILayout.Height(30))) parallelAnim.scaleAnimation = false;
            if (GUILayout.Button("Use Current Value")) UseCurrentValue(AnimationType.SCALE, parallelAnim);
            parallelAnim.start[1] = EditorGUILayout.Vector3Field("Start Scale", parallelAnim.start[1]);
            parallelAnim.final[1] = EditorGUILayout.Vector3Field("Final Scale", parallelAnim.final[1]);

            parallelAnim.scaleModifier  = (Modifiers)EditorGUILayout.EnumPopup("Modifier", parallelAnim.scaleModifier);
            parallelAnim.scaleEffect    = (Effects)EditorGUILayout.EnumPopup("Effect", parallelAnim.scaleEffect);
            if (parallelAnim.scaleEffect != Effects.NONE)
            {
                parallelAnim.max[1] = EditorGUILayout.FloatField("Max", parallelAnim.max[1]);
                if (parallelAnim.scaleEffect != Effects.EXPLOSION)
                {
                    parallelAnim.bounce[1] = EditorGUILayout.IntField("Bounce", parallelAnim.bounce[1]);
                }
            }
        }
        else
            if (GUILayout.Button("+ Scale Animation", GUILayout.Height(30))) parallelAnim.scaleAnimation = true;

        //ROTATION ANIMATION
        if (parallelAnim.rotationAnimation)
        {
            if (GUILayout.Button("- Rotation Animation", GUILayout.Height(30))) parallelAnim.rotationAnimation = false;
            if (GUILayout.Button("Use Current Value")) UseCurrentValue(AnimationType.ROTATION, parallelAnim);
            parallelAnim.start[2] = EditorGUILayout.Vector3Field("Start Rotation", parallelAnim.start[2]);
            parallelAnim.final[2] = EditorGUILayout.Vector3Field("Final Rotation", parallelAnim.final[2]);

            parallelAnim.rotationModifier   = (Modifiers)EditorGUILayout.EnumPopup("Modifier", parallelAnim.rotationModifier);
            parallelAnim.rotationEffect     = (Effects)EditorGUILayout.EnumPopup("Effect", parallelAnim.rotationEffect);
            if (parallelAnim.rotationEffect != Effects.NONE)
            {
                parallelAnim.max[2] = EditorGUILayout.FloatField("Max", parallelAnim.max[2]);
                if (parallelAnim.rotationEffect != Effects.EXPLOSION)
                {
                    parallelAnim.bounce[2] = EditorGUILayout.IntField("Bounce", parallelAnim.bounce[2]);
                }
            }
        }
        else
            if (GUILayout.Button("+ Rotation Animation", GUILayout.Height(30))) parallelAnim.rotationAnimation = true;

        //GRAPH ANIMATION
        if (parallelAnim.graphicAnimation)
        {
            if (GUILayout.Button("- Graphic Animation", GUILayout.Height(30))) parallelAnim.graphicAnimation = false;
            if (GUILayout.Button("Use Current Value")) UseCurrentValue(AnimationType.IMAGE, parallelAnim);
            parallelAnim.startColor = EditorGUILayout.ColorField("Start Color", parallelAnim.startColor);
            parallelAnim.finalColor = EditorGUILayout.ColorField("Final Color", parallelAnim.finalColor);

            parallelAnim.graphicModifier = (Modifiers)EditorGUILayout.EnumPopup("Modifier", parallelAnim.graphicModifier);
        }
        else
            if (GUILayout.Button("+ Graphic Animation", GUILayout.Height(30))) parallelAnim.graphicAnimation = true;

        GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        EditorGUILayout.LabelField("-Common Values-", style);
        parallelAnim.disableAfter   = EditorGUILayout.Toggle("Disable On Finish", parallelAnim.disableAfter);
        if (!parallelAnim.disableAfter)
            parallelAnim.loop       = (Loop)EditorGUILayout.EnumPopup("Loop Options", parallelAnim.loop);
        parallelAnim.delay          = EditorGUILayout.FloatField("Delay", parallelAnim.delay);
        parallelAnim.duration       = EditorGUILayout.FloatField("Duration", parallelAnim.duration);
        parallelAnim.playOnStart    = EditorGUILayout.Toggle("Play On Start", parallelAnim.playOnStart);
        parallelAnim.playOnEnable   = EditorGUILayout.Toggle("Play On Enable", parallelAnim.playOnEnable);
        parallelAnim.playAudioOnPlay = EditorGUILayout.Toggle("Play Audio On Animation Start", parallelAnim.playAudioOnPlay);
        if (GUILayout.Button("Play"))
            parallelAnim.PlayReverse();
    }

    protected virtual void OnSceneGUI()
    {
        UIParallelAnimation parallelAnim = (UIParallelAnimation)target;

        if (!parallelAnim.isBezier)
            return;

        EditorGUI.BeginChangeCheck();

        Vector2 screenP1 = Vector2.Scale(parallelAnim.bezierP1,UIAnimator.ScreenDimension);
        Vector2 screenP2 = Vector2.Scale(parallelAnim.bezierP2, UIAnimator.ScreenDimension);
        Vector2 newP1 = Handles.PositionHandle(screenP1, Quaternion.identity);
        Vector2 newP2 = screenP2;
        if (parallelAnim.isCubicBezier)
            newP2 = Handles.PositionHandle(screenP2, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            parallelAnim.bezierP1 = Vector2.Scale(newP1,UIAnimator.InvertedScreenDimension);
            if (parallelAnim.isCubicBezier)
                parallelAnim.bezierP2 = Vector2.Scale(newP2, UIAnimator.InvertedScreenDimension);
        }
    }

    private void UseCurrentValue(AnimationType type, UIParallelAnimation anim)
    {
        switch (type)
        {
            case AnimationType.MOVE:
                if (anim.useScreenValues)
                    anim.start[0] = anim.transform.position;
                else
                    anim.start[0] = Vector3.Scale(anim.transform.position, EasyUIAnimator.UIAnimator.InvertedScreenDimension);
                break;
            case AnimationType.SCALE:
                anim.start[1] = anim.transform.localScale;
                break;
            case AnimationType.ROTATION:
                anim.start[2] = anim.transform.localRotation.eulerAngles;
                break;
            case AnimationType.IMAGE:
                anim.startColor = anim.GetComponent<UnityEngine.UI.Graphic>().color;
                break;
        }
    }
}

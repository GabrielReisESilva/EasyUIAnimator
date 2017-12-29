/*
 * Copyright (c) 2017 Gabriel Reis e Silva
 *
 * Simplified it so that it doesn't throw exceptions.
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using EasyUIAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class UIParallelAnimation : MonoBehaviour {

    public bool moveAnimation;
    public bool scaleAnimation;
    public bool rotationAnimation;
    public bool graphicAnimation;
    public bool isBezier;
    public bool isCubicBezier;
    public bool useScreenValues;
    public bool playOnStart;
    public bool playOnEnable;
    public bool playAudioOnPlay;
    public bool randomDirection;
    public bool disableAfter;
    public Vector3[] start = new Vector3[3];
    public Vector3[] final = new Vector3[3];
    public Color startColor;
    public Color finalColor;
    public float delay;
    public float duration = 1;
    public float[] max = new float[3];
    public int[] bounce = new int[3];
    public Vector3 effectRotation;
    public Vector2 bezierP1;
    public Vector2 bezierP2;
    public Modifiers moveModifier;
    public Modifiers scaleModifier;
    public Modifiers rotationModifier;
    public Modifiers graphicModifier;
    public Effects moveEffect;
    public Effects scaleEffect;
    public Effects rotationEffect;
    public Loop loop;
    private UIAnimation uiAnimation;
    private RectTransform rect;
    private Graphic image;
    private AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        if (Application.isPlaying && uiAnimation != null) return;

        rect = GetComponent<RectTransform>();

        List<UIAnimation> animations = new List<UIAnimation>();
        if (moveAnimation)
        {
            UpdateBehaviour mod = EasyUIAnimatorUtils.GetModifier(moveModifier);
            Effect.EffectUpdate eff = EasyUIAnimatorUtils.GetEffect(moveEffect, max[0], bounce[0]);
            effectRotation = (randomDirection) ? Vector3.forward * Random.Range(0, 360) : effectRotation;
            Vector2 startValue = (useScreenValues) ? Vector3.Scale(start[0], (Vector3)UIAnimator.InvertedScreenDimension) : start[0];
            Vector2 finalValue = (useScreenValues) ? Vector3.Scale(final[0], (Vector3)UIAnimator.InvertedScreenDimension) : final[0];
            if (isBezier)
            {
                if (isCubicBezier)
                    animations.Add(UIAnimator.MoveBezier(rect, startValue, finalValue, bezierP1, bezierP2, duration));
                else
                    animations.Add(UIAnimator.MoveBezier(rect, startValue, finalValue, bezierP1, duration));
            }
            else
            {
                animations.Add(UIAnimator.Move(rect, startValue, finalValue, duration).SetModifier(mod).SetEffect(eff, Quaternion.Euler(effectRotation)));
            }
        }
        if (scaleAnimation)
        {
            UpdateBehaviour mod = EasyUIAnimatorUtils.GetModifier(scaleModifier);
            Effect.EffectUpdate eff = EasyUIAnimatorUtils.GetEffect(scaleEffect, max[1], bounce[1]);
            animations.Add(UIAnimator.Scale(rect, start[1], final[1], duration).SetModifier(mod).SetEffect(eff));
        }
        if (rotationAnimation)
        {
            UpdateBehaviour mod = EasyUIAnimatorUtils.GetModifier(rotationModifier);
            Effect.EffectUpdate eff = EasyUIAnimatorUtils.GetEffect(rotationEffect, max[2], bounce[2]);
            if (start[2].x != 0 || start[2].y != 0 || final[2].x != 0 || final[2].y != 0)
                animations.Add(UIAnimator.Rotate(rect, Quaternion.Euler(start[2]), Quaternion.Euler(final[2]), duration).SetModifier(mod).SetEffect(eff));
            else
                animations.Add(UIAnimator.Rotate(rect, start[2].z, final[2].z, duration).SetModifier(mod).SetEffect(eff));
        }
        if (graphicAnimation)
        {
            UpdateBehaviour mod = EasyUIAnimatorUtils.GetModifier(graphicModifier);
            image = GetComponent<Graphic>();
            if (!image)
                Debug.LogError("Please attach an Image/Text component to the gameObject");
            else
                animations.Add(UIAnimator.ChangeColor(image, startColor, finalColor, duration).SetDelay(delay).SetModifier(mod));
        }

        if (animations.Count == 0)
        {
            Debug.Log("No animation");
            return;
        }
        else
        {
            uiAnimation = new UIGroupAnimation(animations.ToArray()).SetDelay(delay);
        }

        switch (loop)
        {
            case Loop.LOOP:
                uiAnimation.SetLoop();
                break;
            case Loop.PING_PONG:
                uiAnimation.SetLoop(true);
                break;
        }

        if (playAudioOnPlay)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                Debug.LogError("No audio source found!");
        }

        if (disableAfter) uiAnimation.SetCallback(() => { gameObject.SetActive(false); });
        if (Application.isPlaying)
            if (playOnStart) Play();
    }

    private void OnEnable()
    {
        if(playOnEnable)
            if(uiAnimation != null)
                Play();
    }

    /*  Play
     *      Play the animation.
     *      If you play in Edit Mode, it updates using the EditorApplication
     */
    public void Play()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            if (uiAnimation == null) Start();
            uiAnimation.Play();
        }
        else
        {
            Start();
            if (uiAnimation != null)
            {
                EditorApplication.update = EditorUpdate;
            }
        }
        if (playAudioOnPlay) audioSource.Play();
#else
        if (uiAnimation == null) Start();
        uiAnimation.Play();
#endif
    }

    /*  Play Reverse
     *      Play the reverse animation.
     *      Plays the animation from the end to the beginning.
     */
    public void PlayReverse()
    {
        if (uiAnimation == null) Start();
        Play();
    }

    public void Pause()
    {
        uiAnimation.Pause(true);
    }

#if UNITY_EDITOR
    private void EditorUpdate()
    {
        float deltaTime = (Time.deltaTime > 0.3f) ? 1 / 60f : (Time.deltaTime == 0.0f) ? 1 / 60f : Time.deltaTime;
        if (graphicAnimation)
            EditorUtility.SetDirty(image);
        if (!uiAnimation.Update(deltaTime))
        {
            uiAnimation.SetDelay(delay);
            uiAnimation.Update(0);
            // The LOOP can cause unexpected behaviour during Edit Mode. Use with caution.
            //if (loop == Loop.NONE)
            EditorApplication.update = null;
            //else
            //uiAnimation.OnFinish();
        }
    }
#endif

    #region GIZMOS
    private void OnDrawGizmosSelected()
    {
        Vector3 rectDimension = Vector3.one;
        try
        {
            rectDimension = Vector3.Scale((rect.anchorMax - rect.anchorMin), UIAnimator.ScreenDimension);
        }
        catch (System.Exception)
        {
            Debug.Log("Please add a UIAnimator script to your scene");
            return;
        }
        if (moveAnimation)
        {
            Gizmos.color = Color.blue;
            Vector3 screenStart = (useScreenValues) ? start[0] : Vector3.Scale(start[0], UIAnimator.ScreenDimension);
            Vector3 screenFinal = (useScreenValues) ? final[0] : Vector3.Scale(final[0], UIAnimator.ScreenDimension);
            Gizmos.DrawWireCube(screenStart, rectDimension);
            Gizmos.DrawWireSphere(screenStart, 5f);
            Gizmos.DrawLine(screenStart, screenFinal);
            if (isBezier)
            {
                Vector3 p1 = Vector3.Scale(bezierP1, UIAnimator.ScreenDimension);
                Vector3 p2 = Vector3.Scale(bezierP2, UIAnimator.ScreenDimension);
                Gizmos.DrawWireSphere(p1, 3f);
                Gizmos.DrawLine(screenStart, p1);
                Gizmos.color = Color.red;
                if (isCubicBezier)
                {
                    Gizmos.DrawWireSphere(p2, 3f);
                    Gizmos.DrawLine(screenFinal, p2);
                }
                else
                {
                    Gizmos.DrawLine(screenFinal, p1);
                }

                float points = 10f;
                Vector3 point = screenStart;
                for (int i = 1; i <= (int)points; i++)
                {
                    float time = i / points;
                    Vector3 otherPoint = point;
                    if (isCubicBezier)
                        otherPoint = (1 - time) * (1 - time) * (1 - time) * screenStart + 3 * (1 - time) * (1 - time) * time * p1 + 3 * (1 - time) * time * time * p2 + time * time * time * screenFinal;
                    else
                        otherPoint = (1 - time) * (1 - time) * screenStart + 2 * (1 - time) * time * p1 + time * time * screenFinal;
                    Gizmos.color = Color.Lerp(Color.blue, Color.red, i / points);
                    //Gizmos.DrawLine(rect.position, otherPoint);
                    Gizmos.DrawLine(point, otherPoint);
                    point = otherPoint;
                }
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(screenFinal, rectDimension);
            Gizmos.DrawSphere(screenFinal, 5f);
        }
        if (scaleAnimation)
        {
            Gizmos.color = Color.blue;
            Vector3 newDimension = Vector3.Scale(rectDimension, final[1]);
            Gizmos.DrawWireCube(rect.position, Vector3.Scale(rectDimension, start[1]));
            Gizmos.DrawLine(rect.position + 0.5f * rectDimension, rect.position + 0.5f * newDimension);
            Gizmos.DrawLine(rect.position + 0.5f * new Vector3(rectDimension.x, -rectDimension.y), rect.position + 0.5f * new Vector3(newDimension.x, -newDimension.y));
            Gizmos.DrawLine(rect.position - 0.5f * rectDimension, rect.position - 0.5f * newDimension);
            Gizmos.DrawLine(rect.position - 0.5f * new Vector3(rectDimension.x, -rectDimension.y), rect.position - 0.5f * new Vector3(newDimension.x, -newDimension.y));
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(rect.position, newDimension);
        }
        if (rotationAnimation)
        {
            Gizmos.color = Color.blue;
            float points = 10f;
            float radius = rectDimension.x * 0.5f;
            Vector3 point = rect.position;
            for (int i = 1; i <= (int)points; i++)
            {
                Gizmos.color = Color.Lerp(Color.blue, Color.red, i / points);
                Vector3 otherPoint = rect.position + (i * radius / points) * (Quaternion.Euler(start[2] + i * (final[2] - start[2]) / points) * Vector3.right);
                Gizmos.DrawLine(rect.position, otherPoint);
                Gizmos.DrawLine(point, otherPoint);
                point = otherPoint;
            }
        }
    }
    #endregion
}

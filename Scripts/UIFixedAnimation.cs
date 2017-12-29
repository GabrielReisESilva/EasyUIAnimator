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
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class UIFixedAnimation : MonoBehaviour {

    [SerializeField]
    public Vector3 startV3;
    public Vector3 finalV3;
    public Color startColor = Color.white;
    public Color finalColor = Color.white;
    public UIFixedAnimation[] fixedAnimations;
    public bool useScreenValues;
    public bool randomDirection;
    public bool disableAfter;
    public bool playOnStart;
    public bool playOnEnable;
    public bool playAudioOnPlay;
    public float duration = 1;
    public float delay;
    public float max = 0.2f;
    public int bounce = 2;
    public Vector3 effectRotation;
    public AnimationType animType;
    public Modifiers mod;
    public Effects effect;
    public Loop loop;
    private UIAnimation uiAnimation;
    private RectTransform rect;
    private Graphic image;
    private AudioSource audioSource;
    
    public UIAnimation UIAnimation { get { return uiAnimation; } }

    void Start()
    {
        if (Application.isPlaying && uiAnimation != null) return;

        rect = GetComponent<RectTransform>();
        UpdateBehaviour mMod = EasyUIAnimatorUtils.GetModifier(mod);
        Effect.EffectUpdate mEff = EasyUIAnimatorUtils.GetEffect(effect, max, bounce);
        effectRotation = (randomDirection) ? Vector3.forward * Random.Range(0, 360) : effectRotation;
        switch (animType)
        {
            case AnimationType.MOVE:
                if (useScreenValues)
                {
                    uiAnimation         = UIAnimator.Move(rect, Vector3.Scale(startV3, (Vector3)UIAnimator.InvertedScreenDimension), Vector3.Scale(finalV3, (Vector3)UIAnimator.InvertedScreenDimension), duration).SetDelay(delay).SetModifier(mMod).SetEffect(mEff, Quaternion.Euler(effectRotation));
                }
                else{
                    uiAnimation         = UIAnimator.Move(rect, startV3, finalV3, duration).SetDelay(delay).SetModifier(mMod).SetEffect(mEff, Quaternion.Euler(effectRotation));
                }
                break;
            case AnimationType.SCALE:
                uiAnimation         = UIAnimator.Scale(rect, startV3, finalV3, duration).SetDelay(delay).SetModifier(mMod).SetEffect(mEff, Quaternion.Euler(effectRotation));
                break;
            case AnimationType.ROTATION:
                if(startV3.x != 0 || startV3.y != 0 || finalV3.x != 0 || finalV3.y != 0)
                {
                    uiAnimation = UIAnimator.Rotate(rect, Quaternion.Euler(startV3), Quaternion.Euler(finalV3), duration).SetDelay(delay).SetModifier(mMod).SetEffect(mEff, Quaternion.Euler(effectRotation));
                }
                else
                {
                    uiAnimation = UIAnimator.Rotate(rect, startV3.z, finalV3.z, duration).SetDelay(delay).SetModifier(mMod).SetEffect(mEff, Quaternion.Euler(effectRotation));
                }
                break;
            case AnimationType.IMAGE:
                image = GetComponent<Graphic>();
                if (!image)
                {
                    Debug.LogError("Please attach an Image component to the gameObject");
                    uiAnimation = null;
                    return;
                }
                uiAnimation         = UIAnimator.ChangeColor(image, startColor, finalColor, duration).SetDelay(delay).SetModifier(mMod);
                break;
            case AnimationType.GROUP:
                UIAnimation[] uiAnimations = new UIAnimation[fixedAnimations.Length];
                for (int i = 0; i < uiAnimations.Length; i++)
                {
                    uiAnimations[i] = fixedAnimations[i].uiAnimation;
                }
                uiAnimation = new UIGroupAnimation(uiAnimations);
                break;
            default:
                break;
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
        if (playOnEnable)
            if (uiAnimation != null)
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
            if(uiAnimation != null)
            {
                EditorApplication.update = EditorUpdate;
            }
        }
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
        uiAnimation.Reverse();
        uiAnimation.Play();
    }

    public void Pause()
    {
        uiAnimation.Pause(true);
    }

#if UNITY_EDITOR
    private void EditorUpdate()
    {
        float deltaTime = (Time.deltaTime > 0.3f) ? 1/60f : Time.deltaTime;
        if (animType == AnimationType.IMAGE)
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
        Gizmos.color            = Color.blue;
        switch (animType)
        {
            case AnimationType.MOVE:
                Vector3 screenStart = (useScreenValues) ? startV3 : Vector3.Scale(startV3, UIAnimator.ScreenDimension);
                Vector3 screenFinal = (useScreenValues) ? finalV3 : Vector3.Scale(finalV3, UIAnimator.ScreenDimension);
                Gizmos.DrawWireCube(screenStart, rectDimension);
                Gizmos.DrawWireSphere(screenStart, 5f);
                Gizmos.DrawLine(screenStart, screenFinal);

                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(screenFinal, rectDimension);
                Gizmos.DrawSphere(screenFinal, 5f);
                break;
            case AnimationType.SCALE:
                Vector3 newDimension = Vector3.Scale(rectDimension, finalV3);
                Gizmos.DrawWireCube(rect.position, Vector3.Scale(rectDimension, startV3));
                Gizmos.DrawLine(rect.position + 0.5f * rectDimension, rect.position + 0.5f * newDimension);
                Gizmos.DrawLine(rect.position + 0.5f * new Vector3(rectDimension.x, - rectDimension.y), rect.position + 0.5f * new Vector3(newDimension.x, -newDimension.y));
                Gizmos.DrawLine(rect.position - 0.5f * rectDimension, rect.position - 0.5f * newDimension);
                Gizmos.DrawLine(rect.position - 0.5f * new Vector3(rectDimension.x, - rectDimension.y), rect.position - 0.5f * new Vector3(newDimension.x, - newDimension.y));
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(rect.position, newDimension);
                break;
            case AnimationType.ROTATION:
                float points = 10f;
                float radius = rectDimension.x * 0.5f;
                Vector3 point = rect.position;
                for (int i = 1; i <= (int)points; i++)
                {
                    Gizmos.color = Color.Lerp(Color.blue, Color.red, i/points);
                    Vector3 otherPoint = rect.position + (i * radius / points) * (Quaternion.Euler(startV3 + i * (finalV3 - startV3) / points) * Vector3.right);
                    Gizmos.DrawLine(rect.position, otherPoint);
                    Gizmos.DrawLine(point, otherPoint);
                    point = otherPoint;
                }
                break;
            case AnimationType.IMAGE:
                break;
            case AnimationType.GROUP:
                break;
            default:
                break;
        }
    }
    #endregion
    /* MOVED TO EasyUIAnimationUtils CLASS
    #region UTILS

    public enum AnimationType { MOVE, SCALE, ROTATION, IMAGE, GROUP }
    public enum Modifiers { LINEAR, QUAD_IN, QUAD_OUT, CUB_IN, CUB_OUT, POLY_IN, POLY_OUT, SIN, TAN, CIRCULAR_IN, CIRCULAR_OUT }
    public enum Effects { NONE, SPRING, WAVE, EXPLOSION }
    public enum Loop { NONE, LOOP, PING_PONG}

    private static UpdateBehaviour GetModifier(Modifiers mod)
    {
        switch (mod)
        {
            case Modifiers.LINEAR:
                return Modifier.Linear;
            case Modifiers.QUAD_IN:
                return Modifier.QuadIn;
            case Modifiers.QUAD_OUT:
                return Modifier.QuadOut;
            case Modifiers.CUB_IN:
                return Modifier.CubIn;
            case Modifiers.CUB_OUT:
                return Modifier.CubOut;
            case Modifiers.POLY_IN:
                return Modifier.PolyIn;
            case Modifiers.POLY_OUT:
                return Modifier.PolyOut;
            case Modifiers.SIN:
                return Modifier.Sin;
            case Modifiers.TAN:
                return Modifier.Tan;
            case Modifiers.CIRCULAR_IN:
                return Modifier.CircularIn;
            case Modifiers.CIRCULAR_OUT:
                return Modifier.CircularOut;
            default:
                return Modifier.Linear;
        }
    }

    private static Effect.EffectUpdate GetEffect(Effects eff, float max, int bounce)
    {
        switch (eff)
        {
            case Effects.NONE:
                return (float time)=> { return 0f; };
            case Effects.SPRING:
                return Effect.Spring(max, bounce);
            case Effects.WAVE:
                return Effect.Wave(max, bounce);
            case Effects.EXPLOSION:
                return Effect.Explosion(max);
            default:
                return (float time) => { return 0f; };
        }
    }

    #endregion
    */
}

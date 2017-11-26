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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace EasyUIAnimator
{
    /*! UI Animator
     *      Main class - private Singleton.
     * 
     *      Updatable in-game object, responsible
     *      for updating all animation. It holds 
     *      an instance of each while animating.
     */
     [ExecuteInEditMode]
    public class UIAnimator : MonoBehaviour {
		private static UIAnimator instance;
		private List<UIAnimation> animations;
		private List<UIAnimation> removeList;
		private Vector2 screenDimension;
		private Vector2 invertedScreenDimension;

		private static UIAnimator Instance 			    { get {return instance;} }
        public static Vector2 ScreenDimension           { get {return instance.screenDimension; } }
        public static Vector2 InvertedScreenDimension   { get {return instance.invertedScreenDimension; } }
        public static List<UIAnimation> Animations 	    { get {return instance.animations;}}
		#region UNITY

        /*  Awake, Start, Update
         *      Unity functions.
         *      Update is called each frame. It
         *      updates all unfinished animations.
         */
		void Awake () 
		{
            if (instance == null){
				instance = this;
				animations = new List<UIAnimation>();
				removeList = new List<UIAnimation>();
                screenDimension = new Vector2(Screen.width, Screen.height);
                invertedScreenDimension = new Vector2(1f / Screen.width, 1f / Screen.height);
            }
		}

		void Start ()
        {
        }
        
        void Update()
        {
#if UNITY_EDITOR
            if (animations == null)
                Awake();
            if (!Application.isPlaying)
            {
                screenDimension = new Vector2(Screen.width, Screen.height);
                invertedScreenDimension = new Vector2(1f / Screen.width, 1f / Screen.height);
            }
#endif
            if (animations.Count > 0){
				for (int i = 0; i < animations.Count; i++) {
					if(!animations[i].Update(Time.deltaTime)){
						removeList.Add(animations[i]);
					}
				}
			}
			RemoveSafely();
		}

		#endregion

		#region PUBLIC
        /*  Get Center
         *      Used to get the anchor center of <transform>.
         */
		public static Vector2 GetCenter(RectTransform transform){
			return Vector2.Scale(transform.position,Instance.invertedScreenDimension);
		}

        /*  Add Animation
         *      Animation uses to add itself to the Animator.
         */
        public static UIAnimation AddAnimation(UIAnimation animation){
			instance.animations.Add(animation);
			return animation;
		}

        /*  Remove Animation
         *      Removes animations from updating list.
         */
        public static void RemoveAnimation(UIAnimation anim){
			foreach(UIAnimation animation in instance.animations){
				if(animation == anim){
					instance.removeList.Add(animation);
					return;
				}
			}
		}

        #endregion

        #region PRIVATE
        /*  Remove Safely.
         *      After finished, the animation cannot be removed
         *      inside the iterator. It is store in another list
         *      to be remove with safety.
         */
        private void RemoveSafely(){
			if(removeList.Count > 0){
				foreach (var item in removeList) {
					animations.Remove(item);
                    if (item.OnFinish != null)
                        item.OnFinish();
                }
				removeList.Clear();
			}
		}

        #endregion

        #region MOVE_ANIMATION
        /*  Move*
         *      Movement animations.
         *      
         *      It creates an animation to move <transform>
         *      from <origin> to <target> in <duration>
         *      seconds.
         *      - MOVE TO: 				From (current position) to (Vector2 target).
         *      - MOVE HORIZONTAL: 		From (float origin) to (float target) with fixed y.
         *      - MOVE HORIZONTAL TO: 	From (current position) to (float target) with fixed y.
         *      - MOVE HORIZONTAL: 		From (float origin) to (float target) with fixed x.
         *      - MOVE HORIZONTAL TO: 	From (current position) to (float target) with fixed x.
         *      - MOVE OFFSET: 			From (current position) to (current position + Vector2 offset).
         */
        public static UIPositionAnimation Move(RectTransform transform, Vector2 origin, Vector2 target, float duration){
			return new UIPositionAnimation(
				transform: 	transform,
				origin: 	origin,
				target:		target,
				duration:	duration
			);
		}

		public static UIPositionAnimation MoveTo(RectTransform transform, Vector2 target, float duration){
			return Move (transform, GetCenter(transform), target, duration);
		}

		public static UIPositionAnimation MoveHorizontal(RectTransform transform, float origin, float target, float duration){
			return Move (transform, new Vector2(origin,GetCenter(transform).y), new Vector2(target,GetCenter(transform).y), duration);
		}

		public static UIPositionAnimation MoveHorizontalTo(RectTransform transform, float target, float duration){
			return Move (transform, GetCenter(transform), new Vector2(target,GetCenter(transform).y), duration);
		}

		public static UIPositionAnimation MoveVertical(RectTransform transform, float origin, float target, float duration){
			return Move (transform, new Vector2(GetCenter(transform).x,origin), new Vector2(GetCenter(transform).x,target), duration);
		}

		public static UIPositionAnimation MoveVerticalTo(RectTransform transform, float target, float duration){
			return Move (transform, GetCenter(transform), new Vector2(GetCenter(transform).x,target), duration);
		}

		public static UIPositionAnimation MoveOffset(RectTransform transform, Vector2 offset, float duration){
			return Move (transform, GetCenter(transform), GetCenter(transform) + offset, duration);
		}

        public static UIPositionAnimation MoveBezier(RectTransform transform, Vector2 origin, Vector2 target, Vector2 p1, Vector2 p2, float duration)
        {
            return new UIBezierAnimation(
                transform: transform,
                origin: origin,
                target: target,
                p1: p1,
                p2: p2,
                duration: duration
            );
        }

        public static UIPositionAnimation MoveBezier(RectTransform transform, Vector2 origin, Vector2 target, Vector2 p1, float duration)
        {
            return new UIBezierAnimation(
                transform: transform,
                origin: origin,
                target: target,
                p1: p1,
                duration: duration
            );
        }

        #endregion
        #region SCALE_ANIMATION
        /*  Scale*
         *      Scale animations.
         *      
         *      It creates an animation to scale <transform>
         *      from <origin> to <target> in <duration>
         *      seconds.
         *      - SCALE TO: 			From (current scale) to (Vector3 target).
         *      - SCALE OFFSET: 		From (current scale) to (current scale + Vector3 offset).
         */

        public static UIScaleAnimation Scale(RectTransform transform, Vector3 origin, Vector3 target, float duration){
			return new UIScaleAnimation(
				transform: 	transform,
				origin: 	origin,
				target:		target,
				duration:	duration
			);
		}

		public static UIScaleAnimation ScaleTo(RectTransform transform, Vector3 target, float duration){
			return Scale(transform, transform.localScale, target, duration);
		}

		public static UIScaleAnimation ScaleOffset(RectTransform transform, Vector3 offset, float duration){
			return Scale(transform, transform.localScale, transform.localScale + offset, duration);
		}

        #endregion
        #region ROTATION_ANIMATION
        /*  Rotation*
         *      Rotation animations.
         *      
         *      It creates an animation to rotate <transform>
         *      from <origin> to <target> in <duration>
         *      seconds.
         *      - ROTATE TO: 		From (current rotation) to (Quaternion target).
         *      - ROTATE OFFSET: 	From (current rotation) to (current rotation + Quaterion offset).
         */

        public static UIRotationAnimation Rotate(RectTransform transform, Quaternion origin, Quaternion target, float duration){
			return new UIRotationAnimation(
				transform: 	transform,
				origin: 	origin,
				target:		target,
				duration:	duration
			);
		}

		public static UIRotationAnimation RotateTo(RectTransform transform, Quaternion target, float duration){
			return Rotate(transform, transform.localRotation, target, duration);
		}

		public static UIRotationAnimation RotateOffset(RectTransform transform, Quaternion offset, float duration){
			return Rotate(transform, transform.localRotation, transform.localRotation * offset, duration);
		}

        //UNCLAMPED : THE ROTATION IS NOT LIMITED TO THE 360 DEGREES, BUT LIMITED TO THE Z AXIS
        public static UIRotationAnimation Rotate(RectTransform transform, float originAngle, float targetAngle, float duration)
        {
            return new UIRotationAnimation(
                transform: transform,
                origin: originAngle,
                target: targetAngle,
                duration: duration
            );
        }

        public static UIRotationAnimation RotateTo(RectTransform transform, float targetAngle, float duration)
        {
            return Rotate(transform, transform.localRotation.eulerAngles.z, targetAngle, duration);
        }

        public static UIRotationAnimation RotateOffset(RectTransform transform, float offsetAngle, float duration)
        {
            return Rotate(transform, transform.localRotation.eulerAngles.z, transform.localRotation.eulerAngles.z + offsetAngle, duration);
        }

        #endregion
        #region IMAGE_ANIMATION
        /*  ChangeColor*, Fade*
         *      Image animations.
         *      
         *      It creates an animation to change color 
         *      of <image> from <originColor> to <targetColor>
         *      in <duration> seconds.
         *      - CHANGE COLOR TO: 		From (current color) to (Color targetColor).
         *      - FADE IN:              Creates a fade in effect.
         *      - FADE OUT:             Creates a fade out effect.
         */

        public static UISpriteAnimation ChangeColor(Graphic image, Color originColor, Color targetColor, float duration){
			return new UISpriteAnimation(
				image:			image,
				originColor:	originColor,
				targetColor:	targetColor,
				duration:		duration
			);
		}

		public static UISpriteAnimation ChangeColorTo(Graphic image, Color targetColor, float duration){
			return ChangeColor(image,image.color,targetColor,duration);
		}

		public static UISpriteAnimation FadeIn(Graphic image, float duration){
			Color originColor = image.color;
			originColor.a = 0;
			Color targetColor = image.color;
			targetColor.a = 1;
			return ChangeColor(image,originColor,targetColor,duration);
		}

		public static UISpriteAnimation FadeOut(Graphic image, float duration){
			Color originColor = image.color;
			originColor.a = 1;
			Color targetColor = image.color;
			targetColor.a = 0;
			return ChangeColor(image,originColor,targetColor,duration);
		}

		#endregion
	}

    /*! UI Animation
     *      Abstract - Animations base class.
     * 
     *      Holds common features for all animations.
     */
    public abstract class UIAnimation{

		public delegate void AnimationCallback();

		private float timer = 0;
		private float delay = 0;
		private bool paused = false;
		private AnimationCallback onFinish;
		protected float duration;
		protected UpdateBehaviour updateBehaviour;

		public float Duration 	{get {return duration;}}
		public float Delay 		{get {return delay;}}
		public AnimationCallback OnFinish {get {return onFinish;}}

        /*  Update
         *  Updates the animation.
         * 
         *  Sums the time variation to the timer. Then
         *  calls the abstract function OnUpdate so each
         *  animation tells what need to be update. 
         *  Timer goes from (-delay/duration) to 1 in 
         *  (delay + duration) seconds.
         */
		public bool Update(float deltaTime){
			if(paused)
				return true;

			timer += deltaTime / duration;
            //Debug.Log("timer: " + timer);

            if (timer < 0)
				OnUpdate(0);
			else if(timer < 1){
				OnUpdate(timer);
			}
			else{
				OnEnd();
				return false;
			}
			return true;
		}

        /*  On Update, On End
         *      Abstract class.
         *      
         *      Must be override by subclass.
         */
		public abstract void OnUpdate(float timer);

		public abstract void OnEnd();

        public abstract void Reverse();

        /*  Set Effect
         *      Add effect to animation
         * 
         *      Effects may behave differently depending
         *      on the animation, so it can be might be
         *      overridden by subclass.
         */
		public virtual UIAnimation SetEffect(Effect.EffectUpdate effect, Quaternion rotation = default(Quaternion)){return this;}

        /*  Set Callback
         *      Add callback to animation.
         * 
         *      Callbacks are called when the animation ends.
         */
        public UIAnimation SetCallback(AnimationCallback callback, bool add = false){
            if (add) onFinish += callback;
            else onFinish = callback;
            return this;
		}

        /*  Set Modifier
         *      Add modifier to animation.
         * 
         *      Modifier change how the timer affects the animation.
         */
        public UIAnimation SetModifier(UpdateBehaviour updateBehaviour){
			this.updateBehaviour = updateBehaviour;
			return this;
		}

        /*  Set Delay
         *      Add delay to animation.
         */
        public UIAnimation SetDelay(float delay){
			this.delay = delay;
			timer = - delay / duration;
			return this;
		}

        /*  Set Loop
         *      Set the animation to be a loop.
         *      The animation can also be PingPong
         *      (replays from the end to beginning).
         *      A value can be set as a delay.
         *      Please notice that overrides any previously setted callback.
         */
        public UIAnimation SetLoop(bool pingPong = false)
        {
            SetCallback(()=> 
            {
                if(pingPong) Reverse();
                Play();
            });
            return this;
        }

        /*  Play
         *      Play animation.
         */
        public void Play(){
			if(paused)
				paused = false;
			else
				Restart();
		}

        /*  Pause
         *      Pause animation.
         */
        public void Pause(bool playIfPaused = false){
            if (playIfPaused)
                if (paused)
                    Play();
                else
                    paused = true;
            else
                paused = true;
        }

        /*  Restart
         *      Restart animation.
         */
        public void Restart(){
			SetDelay(delay);
			if(!UIAnimator.Animations.Contains(this))
				UIAnimator.AddAnimation(this);
		}

        /*  Stop
         *      Stop animation.
         */
        public void Stop(){
			UIAnimator.RemoveAnimation(this);
		}
	}

    /*! UI Position Animation
     *      UI Animation - Movement Animation
     * 
     *      Overrides superclass abstract methods.
     *      Updates transform position.
     */
    public class UIPositionAnimation : UIAnimation{
		protected RectTransform transform;
		private Vector2 originPosition;
		private Vector2 targetPosition;
		protected Effect.EffectBehaviour effectBehaviour;

		public UIPositionAnimation(RectTransform transform, UIPositionAnimation animation) :
		this (transform, animation.originPosition, animation.targetPosition, animation.duration){
			originPosition = animation.originPosition;
			targetPosition = animation.targetPosition;
			updateBehaviour = animation.updateBehaviour;
			effectBehaviour = animation.effectBehaviour;
		}

		public UIPositionAnimation(RectTransform transform, Vector2 origin, Vector2 target, float duration){
			this.transform 		= transform;
			this.duration 		= duration < 0.0000001f ? 0.0000001f : duration;
            this.originPosition = Vector2.Scale(origin,UIAnimator.ScreenDimension) - (Vector2)transform.position + transform.anchoredPosition;
			this.targetPosition = Vector2.Scale(target,UIAnimator.ScreenDimension) - (Vector2)transform.position + transform.anchoredPosition;
			updateBehaviour		= Modifier.Linear;
			effectBehaviour		= Effect.NoEffect;
        }

		public override void OnUpdate (float timer){
			transform.anchoredPosition 	= Vector2.Lerp(originPosition, 	targetPosition, updateBehaviour(timer))	+ effectBehaviour(timer);
		}

		public override void OnEnd (){
			transform.anchoredPosition = targetPosition;
		}

        public override void Reverse()
        {
            Vector3 aux     = originPosition;
            originPosition  = targetPosition;
            targetPosition  = aux;
        }

        /*  Set Effect
         *      Sets effect
         * 
         *      For more on Effects, please see Effects class
         */
        public override UIAnimation SetEffect (Effect.EffectUpdate effect, Quaternion rotation = default(Quaternion)){
			Vector2 direction = (targetPosition-originPosition).normalized;
			direction = (direction == Vector2.zero) ? Vector2.right : direction;
			Vector2 directionVector = rotation * direction;
			directionVector *=  UIAnimator.ScreenDimension.y;
			this.effectBehaviour = Effect.GetBehaviour(effect, directionVector);
			return this;
		}
	}

    public class UIBezierAnimation : UIPositionAnimation
    {
        public UIBezierAnimation(RectTransform transform, Vector2 origin, Vector2 target, float duration, Vector2 p1, Vector2 p2) : base (transform,origin,target,duration)
        {
            Vector2 mP0 = Vector2.Scale(origin, UIAnimator.ScreenDimension) - (Vector2)transform.position + transform.anchoredPosition;
            Vector2 mP1 = Vector2.Scale(p1, UIAnimator.ScreenDimension)     - (Vector2)transform.position + transform.anchoredPosition;
            Vector2 mP2 = Vector2.Scale(p2, UIAnimator.ScreenDimension)     - (Vector2)transform.position + transform.anchoredPosition;
            Vector2 mP3 = Vector2.Scale(target, UIAnimator.ScreenDimension) - (Vector2)transform.position + transform.anchoredPosition;
            effectBehaviour = Effect.BezierEffectBehaviour(mP0, mP1, mP2, mP3);
        }

        public UIBezierAnimation(RectTransform transform, Vector2 origin, Vector2 target, float duration, Vector2 p1) : base(transform, origin, target, duration)
        {
            Vector2 mP0 = Vector2.Scale(origin, UIAnimator.ScreenDimension) - (Vector2)transform.position + transform.anchoredPosition;
            Vector2 mP1 = Vector2.Scale(p1, UIAnimator.ScreenDimension) - (Vector2)transform.position + transform.anchoredPosition;
            Vector2 mP2 = Vector2.Scale(target, UIAnimator.ScreenDimension) - (Vector2)transform.position + transform.anchoredPosition;
            effectBehaviour = Effect.BezierEffectBehaviour(mP0, mP1, mP2);
        }

        public override void OnUpdate(float timer)
        {
            transform.anchoredPosition = effectBehaviour(timer);
        }
    }

        /*! UI Scale Animation
         *      UI Animation - Scale Animation
         * 
         *      Overrides superclass abstract methods.
         *      Updates transform localScale.
         */
    public class UIScaleAnimation : UIAnimation{
		private RectTransform transform;
		private Vector3 originScale;
		private Vector3 targetScale;
		private Effect.EffectBehaviour effectBehaviour;

		public UIScaleAnimation(RectTransform transform, UIScaleAnimation animation) :
		this (transform, animation.originScale, animation.targetScale, animation.duration){}

		public UIScaleAnimation(RectTransform transform, Vector3 origin, Vector3 target, float duration){
			this.transform 		= transform;
			this.duration 		= duration < 0.0000001f ? 0.0000001f : duration;
			this.originScale 	= origin;
			this.targetScale 	= target;
			updateBehaviour		= Modifier.Linear;
			effectBehaviour		= Effect.NoEffect;
		}

		public override void OnUpdate (float timer){
			transform.localScale		= Vector3.Lerp(originScale, 	targetScale, 	updateBehaviour(timer)) + (Vector3)effectBehaviour(timer);
		}

		public override void OnEnd (){
			transform.localScale = targetScale;
		}

        public override void Reverse()
        {
            Vector3 aux = originScale;
            originScale = targetScale;
            targetScale = aux;
        }

        /*  Set Effect
         *      Sets effect
         * 
         *      For more on Effects, please see Effects class
         */
        public override UIAnimation SetEffect (Effect.EffectUpdate effect, Quaternion rotation = default(Quaternion)){
			this.effectBehaviour = Effect.GetBehaviour(effect,rotation * (targetScale-originScale));
			return this;
		}
	}

       /*! UI Rotation Animation
        *      UI Animation - Rotation Animation
        * 
        *      Overrides superclass abstract methods.
        *      Updates transform localRotation.
        */
    public class UIRotationAnimation : UIAnimation{
		private RectTransform transform;
        private float originAngle;
        private float targetAngle;
        private Quaternion originRotation;
        private Quaternion targetRotation;
        private Effect.EffectBehaviour effectBehaviour;
        private bool unclamped = true;

		public UIRotationAnimation(RectTransform transform, UIRotationAnimation animation) :
		this (transform, animation.originAngle, animation.targetAngle, animation.duration){}

		public UIRotationAnimation(RectTransform transform, Quaternion origin, Quaternion target, float duration){
			this.transform 		= transform;
			this.duration 		= duration < 0.0000001f ? 0.0000001f : duration;
			this.originRotation = origin;
			this.targetRotation = target;
			updateBehaviour		= Modifier.Linear;
			effectBehaviour		= Effect.NoEffect;
            unclamped           = false;
		}

        public UIRotationAnimation(RectTransform transform, float origin, float target, float duration)
        {
            this.transform      = transform;
            this.duration       = duration < 0.0000001f ? 0.0000001f : duration;
            this.originAngle    = origin;
            this.targetAngle    = target;
            updateBehaviour     = Modifier.Linear;
            effectBehaviour     = Effect.NoEffect;
            unclamped           = true;
        }

        public override void OnUpdate (float timer){
            if (unclamped)
            {
                transform.localRotation = Quaternion.AngleAxis(Mathf.Lerp(originAngle, targetAngle, timer), Vector3.forward) * Quaternion.Euler(Vector3.forward * effectBehaviour(timer).x);
            }
            else
            {
                transform.localRotation = Quaternion.Lerp(originRotation, targetRotation, updateBehaviour(timer)) * Quaternion.Euler(Vector3.forward * effectBehaviour(timer).x);
            }
        }

		public override void OnEnd (){
            if (unclamped)
            {
                transform.localRotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);
            }
            else
            {
                transform.localRotation = targetRotation;
            }
		}

        public override void Reverse()
        {
            if (unclamped)
            {
                float aux = originAngle;
                originAngle = targetAngle;
                targetAngle = aux;
            }
            else
            {
                Quaternion aux = originRotation;
                originRotation = targetRotation;
                targetRotation = aux;
            }
        }

        public override UIAnimation SetEffect (Effect.EffectUpdate effect, Quaternion rotation = default(Quaternion)){
			this.effectBehaviour = Effect.GetBehaviour(effect, Vector2.right);
			return this;
		}
	}

       /*! UI Sprite Animation
        *      UI Animation - Color Animation
        * 
        *      Overrides superclass abstract methods.
        *      Updates image color.
        */
    public class UISpriteAnimation : UIAnimation{
		private Graphic image;
        private Color originColor;
        private Color targetColor;

        public UISpriteAnimation(Graphic img, UISpriteAnimation animation) :
		this (img, animation.originColor, animation.targetColor, animation.duration){}

		public UISpriteAnimation(Graphic image, Color originColor, Color targetColor, float duration){
			this.image          = image;
			this.duration       = duration;
            this.originColor    = originColor;
            this.targetColor    = targetColor;
			updateBehaviour		= Modifier.Linear;
        }

		public override void OnUpdate (float timer){
			image.color	= Color.Lerp(originColor, targetColor, updateBehaviour(timer));
		}

		public override void OnEnd (){
			image.color = targetColor;
		}

        public override void Reverse()
        {
            Color aux   = originColor;
            originColor = targetColor;
            targetColor = aux;
        }
    }

       /*! UI Group Animation
        *      UI Animation - Group Animation
        * 
        *      Overrides superclass abstract methods.
        *      Updates multiple UI Animation. Used as reusable animation.
        */
    public class UIGroupAnimation : UIAnimation{
		private UIAnimation[] animations;
		private bool[] finished;
		private float lastTime;

		public UIGroupAnimation(RectTransform[] rects, UIPositionAnimation transformAnimation){
			animations = new UIAnimation[rects.Length];
			for (int i = 0; i < animations.Length; i++) {
				animations[i] = new UIPositionAnimation(rects[i],transformAnimation);
			}
			duration = animations[0].Duration;
			finished = new bool[animations.Length];
		}

		public UIGroupAnimation(RectTransform[] rects, UIScaleAnimation transformAnimation){
			animations = new UIAnimation[rects.Length];
			for (int i = 0; i < animations.Length; i++) {
				animations[i] = new UIScaleAnimation(rects[i],transformAnimation);
			}
			duration = animations[0].Duration;
			finished = new bool[animations.Length];
		}

		public UIGroupAnimation(Image[] imgs, UISpriteAnimation spriteAnimation){
			animations = new UIAnimation[imgs.Length];
			for (int i = 0; i < animations.Length; i++) {
				animations[i] = new UISpriteAnimation(imgs[i],spriteAnimation);
			}
			duration = animations[0].Duration;
			finished = new bool[animations.Length];
		}

		public UIGroupAnimation(params UIAnimation[] animations){
			for (int i = 0; i < animations.Length; i++) {
				duration = Mathf.Max(duration,animations[i].Duration + animations[i].Delay);
			}
			this.animations = animations;
			finished = new bool[animations.Length];
		}

		public override void OnUpdate (float timer){
			float deltaTime = (timer - lastTime) * duration;
			for (int i = 0; i < animations.Length; i++) {
				if(!finished[i])
					finished[i] = !animations[i].Update(deltaTime);
			}
			lastTime = timer;
		}

		public override void OnEnd (){
			for (int i = 0; i < animations.Length; i++) {
				animations[i].OnEnd();
				finished[i] = false;
				animations[i].SetDelay(animations[i].Delay);
				lastTime = 0;
			}
		}

        public override void Reverse()
        {
            for (int i = 0; i < animations.Length; i++)
            {
                animations[i].Reverse();
            }
        }

        public override UIAnimation SetEffect (Effect.EffectUpdate effect, Quaternion rotation = default(Quaternion))
		{
			for (int i = 0; i < animations.Length; i++) {
				animations[i].SetEffect(effect,rotation);
			}
			return this;
		}

        /*  Set Group Modifier
         *      Set same modifier for all animations
         */
        public UIGroupAnimation SetGroupModifier(UpdateBehaviour mod){
			for (int i = 0; i < animations.Length; i++) {
				animations[i].SetModifier(mod);
			}
			return this;
		}

        /*  Set Group Effect
         *      Set same effect for all animations
         */
        public UIGroupAnimation SetGroupEffect(Effect.EffectGroup effectGroup, float max = 0.2f, float min = 0.0f, int maxBounce = 2, int minBounce = 1, int minAngle = 0, int maxAngle = 0){
			for (int i = 0; i < animations.Length; i++) {
				animations[i].SetEffect(effectGroup (Random.Range(min,max), Random.Range(minBounce,maxBounce)), Quaternion.Euler(Vector3.forward * Random.Range(minAngle,maxAngle)));
			}
			return this;
		}
	}

	public delegate float UpdateBehaviour(float deltaTime);

    /*!  Modifier
     *      Change animation behaviour.
     * 
     *      Returns a float value used in inside
     *      UIAnimation.OnUpdate to change the timer
     *      growth curve, changing the animation.
     *      To add a new modifier simply create a new
     *      UpdateBehaviour function.
     *      CAUTION: 
     *      1. Functions must attend: f(0) = 0 & f(1) = 1.
     *      2. It is used inside a Lerp function, any 
     *      values above 1 may have unexpected behaviour.
     */
    public static class Modifier{
		public static float Linear	(float time)	{return time;}
		public static float QuadOut	(float time)	{return time * time;}
		public static float QuadIn	(float time)	{return (float)Mathf.Pow(time,0.50f);}
		public static float CubOut	(float time)	{return time * time * time;}
		public static float CubIn	(float time)	{return Mathf.Pow(time,0.33f);}
		public static float PolyOut	(float time)	{return time * time * time * time;}
		public static float PolyIn	(float time)	{return Mathf.Pow(time,0.25f);}
		public static float Sin		(float time)	{return 0.5f + 0.5f * Mathf.Cos((1-time)*Mathf.PI);}
		public static float Tan		(float time)	{return 2 * time - Sin(time);}
		public static float CircularIn	(float time){return Mathf.Sqrt(Mathf.Sin(time*Mathf.PI/2));}
		public static float CircularOut	(float time){return 1 - Mathf.Sqrt(Mathf.Cos(-time*Mathf.PI/2));}
	}

    /*!  Effect
     *      Add new values to the animation.
     * 
     *      Returns a Vector2 from (float time) adding a new behaviour
     *      to the animation.
     *      To add a new effect you must create a new EffectUpdate function
     *      You can use a float and a int parameter to adjust your effect
     *      CAUTION:
     *      1. Functions must attend: f(0) = 0 & f(1) = 0.
     *      2. You must also create a EffectGroup, so the effect can be
     *      used in a GroupAnimation
     */
    public static class Effect{
		public delegate EffectUpdate EffectGroup(float max, int bounce);
		public delegate Vector2 EffectBehaviour(float time);
		public delegate float EffectUpdate(float time);
		public static Vector2 NoEffect(float time){return Vector2.zero;}

		public static EffectUpdate Spring(float max = 0.2f, int bounce = 2)	{return (float time) => {return max * (1- time * time) * Mathf.Sin(Mathf.PI * bounce * time * time);};}
		public static EffectUpdate Wave(float max = 0.2f, int bounce = 2)	{return (float time) => {return max * Mathf.Sin(Mathf.PI * bounce * time);};} 
		public static EffectUpdate Explosion(float max = 0.2f)				{return (float time) => {return max * Mathf.Sqrt(Mathf.Sin(Mathf.Pow(time,0.75f)*Mathf.PI));};}

		public static EffectGroup SpringGroup	(){return (float max, int bounce) => {return Spring(max,bounce);};}
		public static EffectGroup WaveGroup		(){return (float max, int bounce) => {return Wave(max,bounce);};}
		public static EffectGroup ExplosionGroup(){return (float max, int bounce) => {return Explosion(max);};}

        /*  Get Behaviour
        *      NOTE: For movement animations, changing the directionVector can
        *      modify you effect.
        */
        public static EffectBehaviour GetBehaviour(EffectUpdate update, Vector2 directionVector){
			return ((float time) => {return directionVector * update(time);});
        }
        public static EffectBehaviour BezierEffectBehaviour(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (float time) => { return (1 - time) * (1 - time) * (1 - time) * p0 + 3 * (1 - time) * (1 - time) * time * p1 + 3 * (1 - time) * time * time * p2 + time * time * time * p3;};
        }
        public static EffectBehaviour BezierEffectBehaviour(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            return (float time) => { return (1 - time) * (1 - time) * p0 + 2 * (1 - time) * time * p1 + time * time * p2;};
        }
    }
}
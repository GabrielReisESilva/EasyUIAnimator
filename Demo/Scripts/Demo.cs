using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyUIAnimator;

public class Demo : MonoBehaviour {

	public RectTransform[] demos;

	public RectTransform origin;
	public RectTransform target;
	public RectTransform moveDemo;
	public RectTransform modDemo1;
	public RectTransform modDemo2;
	public RectTransform effDemo1;
	public RectTransform effDemo2;
	public RectTransform effDemo3;
	public RectTransform effDemo4;
	public RectTransform[] groupEffDemo;
	public RectTransform scaleDemo1;
	public RectTransform scaleDemo2;
	public Image[] colorDemo;

	private int selectedDemos;

	private UIAnimation linear1Animation;
	private UIAnimation linear2Animation;
	private UIAnimation quadInAnimation;
	private UIAnimation quadOutAnimation;
	private UIAnimation cubInAnimation;
	private UIAnimation cubOutAnimation;
	private UIAnimation polyInAnimation;
	private UIAnimation polyOutAnimation;
	private UIAnimation sinAnimation;
	private UIAnimation tanAnimation;
	private UIAnimation circularInAnimation;
	private UIAnimation circularOutAnimation;

	private UIAnimation springEffect1;
	private UIAnimation springEffect2;
	private UIAnimation springEffect3;
	private UIAnimation springEffect4;
	private UIAnimation waveEffect1;
	private UIAnimation waveEffect2;
	private UIAnimation waveEffect3;
	private UIAnimation waveEffect4;
	private UIAnimation explosionEffect1;
	private UIAnimation explosionEffect2;
	private UIAnimation explosionEffect3;
	private UIAnimation explosionEffect4;

	private UIAnimation springGroupEffect;
	private UIAnimation waveGroupEffect;
	private UIGroupAnimation explosionGroupEffect;

	private UIAnimation scale1Animation;
	private UIAnimation scale2Animation;
	private UIAnimation rotation1Animation;
	private UIAnimation rotation2Animation;

	private UIAnimation callback1Animation;
	private UIAnimation callback2Animation;
	private UIAnimation callback3Animation;
	private UIAnimation callback4Animation;
	private UIAnimation callback5Animation;
	private UIAnimation callback6Animation;

    public UIFixedAnimation fixedAnimation1;
    public UIFixedAnimation fixedAnimation2;
    public UIFixedAnimation fixedAnimation3;
    public UIFixedAnimation fixedAnimation4;
    public UIFixedAnimation fixedAnimation5;

    void Awake () {
		
	}

	void Start () {
		//DEMO 2 - MODIFIERS
		linear1Animation 	= UIAnimator.MoveHorizontal(modDemo1, 0.3f, 0.7f, 2f);
		linear2Animation 	= UIAnimator.MoveHorizontal(modDemo2, 0.3f, 0.7f, 2f).SetModifier(Modifier.Linear);
		quadInAnimation 	= UIAnimator.MoveHorizontal(modDemo1, 0.3f, 0.7f, 2f).SetModifier(Modifier.QuadIn);
		quadOutAnimation 	= UIAnimator.MoveHorizontal(modDemo2, 0.3f, 0.7f, 2f).SetModifier(Modifier.QuadOut);
		cubInAnimation		= UIAnimator.MoveHorizontal(modDemo1, 0.3f, 0.7f, 2f).SetModifier(Modifier.CubIn);
		cubOutAnimation		= UIAnimator.MoveHorizontal(modDemo2, 0.3f, 0.7f, 2f).SetModifier(Modifier.CubOut);
		polyInAnimation		= UIAnimator.MoveHorizontal(modDemo1, 0.3f, 0.7f, 2f).SetModifier(Modifier.PolyIn);
		polyOutAnimation	= UIAnimator.MoveHorizontal(modDemo2, 0.3f, 0.7f, 2f).SetModifier(Modifier.PolyOut);
		sinAnimation		= UIAnimator.MoveHorizontal(modDemo1, 0.3f, 0.7f, 2f).SetModifier(Modifier.Sin);
		tanAnimation		= UIAnimator.MoveHorizontal(modDemo2, 0.3f, 0.7f, 2f).SetModifier(Modifier.Tan);
		circularInAnimation = UIAnimator.MoveHorizontal(modDemo1, 0.3f, 0.7f, 2f).SetModifier(Modifier.CircularIn);
		circularOutAnimation= UIAnimator.MoveHorizontal(modDemo2, 0.3f, 0.7f, 2f).SetModifier(Modifier.CircularOut);
		//DEMO 3 - EFFECTS
		springEffect1		= UIAnimator.Move(effDemo1, new Vector2(0.5f,0.7f), new Vector2(0.8f,0.7f), 2f).SetModifier(Modifier.PolyIn).SetEffect(Effect.Spring(0.21f,4));
		springEffect2		= UIAnimator.Move(effDemo2, new Vector2(0.5f,0.5f), new Vector2(0.8f,0.5f), 2f).SetEffect(Effect.Spring(0.2f,6),Quaternion.Euler(0,0,90f));
		springEffect3		= UIAnimator.Move(effDemo3, new Vector2(0.2f,0.6f), new Vector2(0.2f,0.6f), 2f).SetEffect(Effect.Spring(0.1f,4));
		springEffect4		= UIAnimator.Move(effDemo4, new Vector2(0.4f,0.6f), new Vector2(0.4f,0.6f), 2f).SetEffect(Effect.Spring(0.1f,4),Quaternion.Euler(0,0,90f));
		waveEffect1			= UIAnimator.Move(effDemo1, new Vector2(0.5f,0.7f), new Vector2(0.8f,0.7f), 2f).SetModifier(Modifier.CircularIn).SetEffect(Effect.Wave(0.05f,10));
		waveEffect2			= UIAnimator.Move(effDemo2, new Vector2(0.5f,0.5f), new Vector2(0.8f,0.5f), 2f).SetModifier(Modifier.Linear).SetEffect(Effect.Wave(0.1f,6),Quaternion.Euler(0,0,90f));
		waveEffect3			= UIAnimator.Move(effDemo3, new Vector2(0.2f,0.6f), new Vector2(0.2f,0.6f), 2f).SetEffect(Effect.Wave(0.1f,4));
		waveEffect4			= UIAnimator.Move(effDemo4, new Vector2(0.4f,0.6f), new Vector2(0.4f,0.6f), 2f).SetEffect(Effect.Wave(0.1f,4),Quaternion.Euler(0,0,90f));
		explosionEffect1	= UIAnimator.Move(effDemo1, new Vector2(0.5f,0.7f), new Vector2(0.8f,0.7f), 2f).SetModifier(Modifier.CircularOut);
		explosionEffect2	= UIAnimator.Move(effDemo2, new Vector2(0.5f,0.5f), new Vector2(0.8f,0.5f), 2f).SetModifier(Modifier.CircularOut).SetEffect(Effect.Explosion(0.2f));
		explosionEffect3	= UIAnimator.Move(effDemo3, new Vector2(0.2f,0.6f), new Vector2(0.2f,0.6f), 2f).SetEffect(Effect.Explosion(0.1f));
		explosionEffect4	= UIAnimator.Move(effDemo4, new Vector2(0.4f,0.6f), new Vector2(0.4f,0.6f), 2f).SetEffect(Effect.Explosion(0.2f));
		//DEMO 4 - GROUPS
		springGroupEffect	= new UIGroupAnimation(
			UIAnimator.Move(groupEffDemo[0], new Vector2(0.3f,0.6f), new Vector2(0.3f,0.6f), 1f).SetEffect(Effect.Spring(0.2f,4),Quaternion.Euler(0,0,000f)),
			UIAnimator.Move(groupEffDemo[1], new Vector2(0.3f,0.6f), new Vector2(0.3f,0.6f), 1f).SetEffect(Effect.Spring(0.2f,4),Quaternion.Euler(0,0,090f)),
			UIAnimator.Move(groupEffDemo[2], new Vector2(0.3f,0.6f), new Vector2(0.3f,0.6f), 1f).SetEffect(Effect.Spring(0.2f,4),Quaternion.Euler(0,0,180f)),
			UIAnimator.Move(groupEffDemo[3], new Vector2(0.3f,0.6f), new Vector2(0.3f,0.6f), 1f).SetEffect(Effect.Spring(0.2f,4),Quaternion.Euler(0,0,270f))
		);
		waveGroupEffect	= new UIGroupAnimation(
			UIAnimator.Move(groupEffDemo[0], new Vector2(0.3f,0.6f), new Vector2(0.3f,0.6f), 4f).SetEffect(Effect.Wave(0.2f,4),Quaternion.Euler(0,0,090f)),
			UIAnimator.Move(groupEffDemo[1], new Vector2(0.3f,0.6f), new Vector2(0.3f,0.6f), 4f).SetEffect(Effect.Wave(0.2f,4),Quaternion.Euler(0,0,045f)).SetDelay(0.25f),
			UIAnimator.Move(groupEffDemo[2], new Vector2(0.3f,0.6f), new Vector2(0.3f,0.6f), 4f).SetEffect(Effect.Wave(0.2f,4),Quaternion.Euler(0,0,000f)).SetDelay(0.50f),
			UIAnimator.Move(groupEffDemo[3], new Vector2(0.3f,0.6f), new Vector2(0.3f,0.6f), 4f).SetEffect(Effect.Wave(0.2f,4),Quaternion.Euler(0,0,315f)).SetDelay(0.75f)
		);
		explosionGroupEffect = new UIGroupAnimation(groupEffDemo,UIAnimator.Move(groupEffDemo[0], new Vector2(0.3f,0.6f), new Vector2(0.7f,0.6f), 2f));
		//DEMO 5 = SCALE/ROTATE
		scale1Animation		= UIAnimator.Scale		(scaleDemo1, new Vector3(0.0f,0.0f), new Vector3(2.5f,2.5f), 2f).SetModifier(Modifier.PolyIn).SetEffect(Effect.Wave(0.2f,7));
		rotation1Animation	= UIAnimator.Rotate		(scaleDemo2, Quaternion.Euler(0,0,-90), Quaternion.Euler(0,0,90), 2f);
		//DEMO 6 - COLOR
		callback1Animation 	= UIAnimator.Move(colorDemo[0].rectTransform, new Vector2(0.3f,0.45f), new Vector2(0.3f,0.75f), 1f);
		callback2Animation 	= UIAnimator.Move(colorDemo[1].rectTransform, new Vector2(0.3f,0.75f), new Vector2(0.4f,0.55f), 1f);
		callback3Animation 	= UIAnimator.Move(colorDemo[2].rectTransform, new Vector2(0.4f,0.55f), new Vector2(0.5f,0.75f), 1f);
		callback4Animation 	= UIAnimator.Move(colorDemo[2].rectTransform, new Vector2(0.5f,0.75f), new Vector2(0.6f,0.55f), 1f);
		callback5Animation 	= UIAnimator.Move(colorDemo[3].rectTransform, new Vector2(0.6f,0.55f), new Vector2(0.7f,0.75f), 1f);
		callback6Animation 	= UIAnimator.Move(colorDemo[4].rectTransform, new Vector2(0.7f,0.75f), new Vector2(0.3f,0.45f), 1f).SetEffect(Effect.Wave(0.2f,1),Quaternion.Euler(0,0,90f));
		callback1Animation .SetCallback(callback2Animation.Play);
		callback2Animation .SetCallback(callback3Animation.Play);
		callback3Animation .SetCallback(callback4Animation.Play);
		callback4Animation .SetCallback(callback5Animation.Play);
		callback5Animation .SetCallback(callback6Animation.Play);
		callback6Animation .SetCallback(ResetAnimation);

		callback1Animation.Play();
        //DEMO 7 - LOOP
        //PLEASE SEE THE COMPONENTS IN INSPECTOR OR READ THE INSTRUCTIONS
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetMouseButtonDown(0)){
			if(Input.mousePosition.x > 0.9f * Screen.width)
				Next();
			else if(Input.mousePosition.x < 0.1f * Screen.width)
				Previous();
		}
		if(selectedDemos == 0){
			if(Input.GetMouseButton(0)){
				if(Input.mousePosition.y > 0.4f * Screen.height)
					target.position = Input.mousePosition;
			}
			else if(Input.GetMouseButton(1)){
				if(Input.mousePosition.y > 0.4f * Screen.height)
					origin.position = Input.mousePosition;
			}
		}

	}

	public void Next(){
		UIAnimator.MoveHorizontalTo(demos[selectedDemos],-0.5f,0.5f)
			.SetModifier(Modifier.CircularIn)
			.Play();
		
		selectedDemos++;
		selectedDemos = (selectedDemos >= demos.Length) ? 0 : selectedDemos;
		demos[selectedDemos].gameObject.SetActive(true);

		UIAnimator.MoveHorizontal(demos[selectedDemos], 1.5f,0.5f,0.5f)
			.SetModifier(Modifier.CircularIn)
			.Play();
	}

	public void Previous(){
		UIAnimator.MoveHorizontalTo(demos[selectedDemos],1.5f,0.5f)
			.SetModifier(Modifier.CircularIn)
			.Play();
		
		selectedDemos--;
		selectedDemos = (selectedDemos < 0) ? demos.Length-1 : selectedDemos;
		demos[selectedDemos].gameObject.SetActive(true);

		UIAnimator.MoveHorizontal(demos[selectedDemos], -0.5f,0.5f,0.5f)
			.SetModifier(Modifier.CircularIn)
			.Play();
	}

	#region DEMO_1_BUTTONS

	public void MoveDemo(){
		Debug.Log ("origin.anchoredPosition: " + (UIAnimator.GetCenter(origin)));
		Debug.Log ("target.anchoredPosition: " + (UIAnimator.GetCenter(target)));
		//		UIAnimator.Move(moveDemo, UIAnimator.GetAnchorCenter(origin), UIAnimator.GetAnchorCenter(target), 1f);
		UIAnimator.Move(moveDemo, UIAnimator.GetCenter(origin), UIAnimator.GetCenter(target), 1f).Play();
	}

	public void MoveToDemo(){
		UIAnimator.MoveTo(moveDemo, UIAnimator.GetCenter(target), 1f).Play();
	}

	public void MoveHorizontalDemo(){
		UIAnimator.MoveHorizontal(moveDemo, UIAnimator.GetCenter(origin).x, UIAnimator.GetCenter(target).x, 1f).Play();
	}

	public void MoveHorizontalToDemo(){
		UIAnimator.MoveHorizontalTo(moveDemo, UIAnimator.GetCenter(target).x, 1f).Play();
	}

	public void MoveVerticalDemo(){
		UIAnimator.MoveVertical(moveDemo, UIAnimator.GetCenter(origin).y, UIAnimator.GetCenter(target).y, 1f).Play();
	}

	public void MoveVerticalToDemo(){
		UIAnimator.MoveVerticalTo(moveDemo, UIAnimator.GetCenter(target).y, 1f).Play();
	}

	public void MoveOffsetDemo(){
		UIAnimator.MoveOffset(moveDemo, UIAnimator.GetCenter(target) - UIAnimator.GetCenter(origin), 1f).Play();
	}

	#endregion

	#region DEMO_2_BUTTONS

	public void Linear(){
		linear1Animation.Play();
		linear2Animation.Play();
	}

	public void Quad(){
		quadInAnimation.Play();
		quadOutAnimation.Play();
	}

	public void Cub(){
		cubInAnimation.Play();
		cubOutAnimation.Play();
	}

	public void Poly(){
		polyInAnimation.Play();
		polyOutAnimation.Play();
	}

	public void SinTan(){
		sinAnimation.Play();
		tanAnimation.Play();
	}

	public void Circular(){
		circularInAnimation.Play();
		circularOutAnimation.Play();
	}

	#endregion

	#region DEMO_3_BUTTONS

	public void Spring(){
		springEffect1.Play();
		springEffect2.Play();
		springEffect3.Play();
		springEffect4.Play();
	}

	public void Wave(){
		waveEffect1.Play();
		waveEffect2.Play();
		waveEffect3.Play();
		waveEffect4.Play();
	}

	public void Explosion(){
		explosionEffect1.SetEffect(Effect.Explosion(0.1f),Quaternion.Euler(Vector3.forward * Random.Range(0,360))).Play();
		explosionEffect2.SetEffect(Effect.Explosion(0.2f),Quaternion.Euler(Vector3.forward * Random.Range(0,360))).Play();
		explosionEffect3.SetEffect(Effect.Explosion(0.1f),Quaternion.Euler(Vector3.forward * Random.Range(0,360))).Play();
		explosionEffect4.SetEffect(Effect.Explosion(0.2f),Quaternion.Euler(Vector3.forward * Random.Range(0,360))).Play();
	}

	#endregion

	#region DEMO_4_BUTTONS

	public void SpringGroup(){
		springGroupEffect.Play();
	}

	public void WaveGroup(){
		waveGroupEffect.Play();
	}

	public void ExplosionGroup(){
		explosionGroupEffect.SetGroupEffect(Effect.ExplosionGroup(),min:0.1f,maxAngle:360).SetGroupModifier(Modifier.CircularOut).Play();
	}

	#endregion

	#region DEMO_5_BUTTONS

	public void FromTo(){
		scale1Animation.Play();
		rotation1Animation.Play();
	}

	public void To(){
		UIAnimator.ScaleTo	(scaleDemo1, new Vector3(1f,1f), 2f).SetModifier(Modifier.CircularIn).SetEffect(Effect.Spring(0.2f,7)).Play();
		UIAnimator.RotateTo	(scaleDemo2, Quaternion.Euler(0,0,180), 2f).SetModifier(Modifier.CircularIn).SetEffect(Effect.Spring(45f,5)).Play();
	}

	public void Offset(){
		UIAnimator.ScaleOffset(scaleDemo1, new Vector3(0.3f,0.3f), 2f).Play();
		UIAnimator.RotateOffset	(scaleDemo2, Quaternion.Euler(0,0,45), 2f).Play();
	}

	#endregion

	#region DEMO_6_BUTTONS

	public void ChangeColor(){
		new UIGroupAnimation(colorDemo,UIAnimator.ChangeColorTo(colorDemo[0],new Color(Random.Range(0,1f),Random.Range(0,1f),Random.Range(0,1f)),1f)).Play();
	}

	public void FadeIn(){
		new UIGroupAnimation(colorDemo,UIAnimator.FadeIn(colorDemo[0],1f)).Play();
	}

	public void FadeOut(){
		new UIGroupAnimation(colorDemo,UIAnimator.FadeOut(colorDemo[0],1f)).Play();
	}

	public void ResetAnimation(){
		callback1Animation.Play();
	}

    #endregion

    #region DEMO_7_BUTTONS

    public void PlayAndPause()
    {
        fixedAnimation1.Pause();
        fixedAnimation2.Pause();
        fixedAnimation3.Pause();
        fixedAnimation4.Pause();
        fixedAnimation5.Pause();
    }

    #endregion
}
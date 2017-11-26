using EasyUIAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUIAnimator
{
    public enum AnimationType { MOVE, SCALE, ROTATION, IMAGE, GROUP }
    public enum Modifiers { LINEAR, QUAD_IN, QUAD_OUT, CUB_IN, CUB_OUT, POLY_IN, POLY_OUT, SIN, TAN, CIRCULAR_IN, CIRCULAR_OUT }
    public enum Effects { NONE, SPRING, WAVE, EXPLOSION }
    public enum Loop { NONE, LOOP, PING_PONG }

    public static class EasyUIAnimatorUtils
    {
        public static UpdateBehaviour GetModifier(Modifiers mod)
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

    public static Effect.EffectUpdate GetEffect(Effects eff, float max, int bounce)
    {
        switch (eff)
        {
            case Effects.NONE:
                return (float time) => { return 0f; };
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
}


    

    
}

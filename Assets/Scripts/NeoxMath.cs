using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Neox.Helpers
{
    public enum InterpolationTypes
    {
        /// <summary>Regular unsmoothed interpolation.</summary>
        Linear = 0,
        /// <summary>Smoothed curve (Ease-In + Ease-Out).</summary>
        Smooth = 1,
        /// <summary>Even smoother curve (Smooth Ease-In + Ease-Out).</summary>
        Smoother = 2,
        /// <summary>Accelerating function (Ease-In).</summary>
        Squared = 3,
        /// <summary>Decelerating function (Ease-Out).</summary>
        InverseSquared = 4,
        /// <summary>Smoother accelerating function (Strong Ease-In).</summary>
        Cubic = 5,
        /// <summary>Smoother decelerating function (Strong Ease-Out).</summary>
        InverseCubic = 6,
        /// <summary>Softer decelerating function (Subtle Ease-Out).</summary>
        Sin = 7,
        /// <summary>Softer accelerating function (Subtle Ease-In).</summary>
        InverseSin = 8,
        /// <summary>FLOAT/INT ONLY: Accelerating for 0 -> 1, decelerating for 1 -> 0 (Ease-In / Ease-Out).</summary>
        SmartSquared = 20,
        /// <summary>FLOAT/INT ONLY: Decelerating for 0 -> 1, accelerating for 1 -> 0 (Ease-Out / Ease-In).</summary>
        SmartInverseSquared = 21,
        /// <summary>FLOAT/INT ONLY: Smoothly accelerating for 0 -> 1, decelerating for 1 -> 0 (Strong Ease-In / Ease-Out).</summary>
        SmartCubic = 22,
        /// <summary>FLOAT/INT ONLY: Smoothly decelerating for 0 -> 1, accelerating for 1 -> 0 (Strong Ease-Out / Ease-In).</summary>
        SmartInverseCubic = 23
    }

    public enum IntRounding
    {
        Regular = 0,
        Floor = 1,
        Ceiling = 2
    }

    public static class NeoxMath
    {
        public const float TAU = 2f * Mathf.PI;
        public static Vector3 Half3 = new Vector3(0.5f, 0.5f, 0.5f);
        public static Vector2 Half2 = new Vector2(0.5f, 0.5f);

        // ----------------------------------------------------------------------------------------------------------------------------------------------

        public static int RandomPlusMinusOne()
        {
            return Random.Range(0, 2) * 2 - 1;
        }

        // ----------------------------------------------------------------------------------------------------------------------------------------------

        public static float RoundToDecimalPlaces(float value, byte decimalPlaces)
        {
            return (float)System.Math.Round(value, decimalPlaces);
        }

        public static Vector2 RoundToDecimalPlaces(Vector2 value, byte decimalPlaces)
        {
            return new Vector2(
                x: RoundToDecimalPlaces(value.x, decimalPlaces),
                y: RoundToDecimalPlaces(value.y, decimalPlaces)
            );
        }

        public static Vector3 RoundToDecimalPlaces(Vector3 value, byte decimalPlaces)
        {
            return new Vector3(
                x: RoundToDecimalPlaces(value.x, decimalPlaces),
                y: RoundToDecimalPlaces(value.y, decimalPlaces),
                z: RoundToDecimalPlaces(value.z, decimalPlaces)
            );
        }

        // ----------------------------------------------------------------------------------------------------------------------------------------------

        #region Lerping

        /// <summary>
        /// Lerp from float a to b by the factor t (range 0 to 1).
        /// Uses smooth (ease-in and -out) interpolation as a default, but supports many different interpolation curves.
        /// </summary>
        /// <param name="from">The start value (t = 0)</param>
        /// <param name="to">The target value (t = 1)</param>
        /// <param name="t">The interpolation value (range 0 to 1)</param>
        /// <param name="type">The interpolation to use</param>
        /// <returns>The interpolated value between from and to.</returns>
        public static float Lerp(float from, float to, float t, InterpolationTypes type = InterpolationTypes.Smooth)
        {
            type = GetSmartType(from, to, type);
            t = ApplyLerp(t, type);
            return Mathf.Lerp(from, to, t);
        }

        /// <summary>
        /// Lerp from int a to b by the factor t (range 0 to 1) with rounding.
        /// Uses smooth (ease-in and -out) interpolation as a default, but supports many different interpolation curves.
        /// </summary>
        /// <param name="from">The start value (t = 0)</param>
        /// <param name="to">The target value (t = 1)</param>
        /// <param name="t">The interpolation value (range 0 to 1)</param>
        /// <param name="type">The interpolation to use</param>
        /// <param name="rounding">The type of rounding to apply to the result.</param>
        /// <returns>The interpolated value between from and to rounded to the nearest integer.</returns>
        public static int Lerp(int from, int to, float t, InterpolationTypes type = InterpolationTypes.Smooth, IntRounding rounding = IntRounding.Regular)
        {
            type = GetSmartType(from, to, type);
            t = ApplyLerp(t, type);
            return ApplyRounding(Mathf.Lerp(from, to, t), rounding);
        }

        /// <summary>
        /// Lerp from Vector2 a to b by the factor t (range 0 to 1).
        /// Uses smooth (ease-in and -out) interpolation as a default, but supports many different interpolation curves.
        /// </summary>
        /// <param name="from">The start value (t = 0)</param>
        /// <param name="to">The target value (t = 1)</param>
        /// <param name="t">The interpolation value (range 0 to 1)</param>
        /// <param name="type">The interpolation to use</param>
        /// <returns>The interpolated value between from and to.</returns>
        public static Vector2 Lerp(Vector2 from, Vector2 to, float t, InterpolationTypes type = InterpolationTypes.Smooth)
        {
            t = ApplyLerp(t, type);
            return Vector2.Lerp(from, to, t);
        }

        /// <summary>
        /// Lerp from Vector3 a to b by the factor t (range 0 to 1).
        /// Uses smooth (ease-in and -out) interpolation as a default, but supports many different interpolation curves.
        /// </summary>
        /// <param name="from">The start value (t = 0)</param>
        /// <param name="to">The target value (t = 1)</param>
        /// <param name="t">The interpolation value (range 0 to 1)</param>
        /// <param name="type">The interpolation to use</param>
        /// <returns>The interpolated value between from and to.</returns>
        public static Vector3 Lerp(Vector3 from, Vector3 to, float t, InterpolationTypes type = InterpolationTypes.Smooth)
        {
            t = ApplyLerp(t, type);
            return Vector3.Lerp(from, to, t);
        }

        /// <summary>
        /// Lerp from Vector2Int a to b by the factor t (range 0 to 1) with rounding.
        /// Uses smooth (ease-in and -out) interpolation as a default, but supports many different interpolation curves.
        /// </summary>
        /// <param name="from">The start value (t = 0)</param>
        /// <param name="to">The target value (t = 1)</param>
        /// <param name="t">The interpolation value (range 0 to 1)</param>
        /// <param name="type">The interpolation to use</param>
        /// <param name="rounding">The type of rounding to apply to the result.</param>
        /// <returns>The interpolated value between from and to.</returns>
        public static Vector2Int Lerp(Vector2Int from, Vector2Int to, float t, InterpolationTypes type = InterpolationTypes.Smooth, IntRounding rounding = IntRounding.Regular)
        {
            t = ApplyLerp(t, type);
            return ApplyRounding(Vector2.Lerp(from, to, t), rounding);
        }

        /// <summary>
        /// Lerp from Vector3Int a to b by the factor t (range 0 to 1) with rounding.
        /// Uses smooth (ease-in and -out) interpolation as a default, but supports many different interpolation curves.
        /// </summary>
        /// <param name="from">The start value (t = 0)</param>
        /// <param name="to">The target value (t = 1)</param>
        /// <param name="t">The interpolation value (range 0 to 1)</param>
        /// <param name="type">The interpolation to use</param>
        /// <param name="rounding">The type of rounding to apply to the result.</param>
        /// <returns>The interpolated value between from and to.</returns>
        public static Vector3Int Lerp(Vector3Int from, Vector3Int to, float t, InterpolationTypes type = InterpolationTypes.Smooth, IntRounding rounding = IntRounding.Regular)
        {
            t = ApplyLerp(t, type);
            return ApplyRounding(Vector3.Lerp(from, to, t), rounding);
        }

        /// <summary>
        /// Lerp from Quaternion a to b by the factor t (range 0 to 1).
        /// Uses smooth (ease-in and -out) interpolation as a default, but supports many different interpolation curves.
        /// </summary>
        /// <param name="from">The start value (t = 0)</param>
        /// <param name="to">The target value (t = 1)</param>
        /// <param name="t">The interpolation value (range 0 to 1)</param>
        /// <param name="type">The interpolation to use</param>
        /// <returns>The interpolated value between from and to.</returns>
        public static Quaternion Lerp(Quaternion from, Quaternion to, float t, InterpolationTypes type = InterpolationTypes.Smooth)
        {
            t = ApplyLerp(t, type);
            return Quaternion.Lerp(from, to, t);
        }

        // ----------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Apply the given interpolation to the value t (range 0 to 1).
        /// </summary>
        /// <param name="t">The interpolation value (range 0 to 1)</param>
        /// <param name="type">The interpolation to use</param>
        /// <returns>The interpolated value (range 0 to 1)</returns>
        public static float ApplyLerp(float t, InterpolationTypes type)
        {
            t = Mathf.Clamp01(t);
            float i = 1f - t;                                                           // Store inverted t for inverted functions            

            // Calculations based on the exmaples avaliable at http://sol.gfxile.net/interpolation/
            switch(type)
            {
                case InterpolationTypes.Linear:
                    return t;
                case InterpolationTypes.Smooth:                                                  // Regular smoothstep
                    return t * t * (3 - 2 * t);
                case InterpolationTypes.Smoother:                                                // Based on the formula by Ken Perlin.
                    return t = t * t * t * (t * (t * 6 - 15) + 10);
                case InterpolationTypes.Squared:
                case InterpolationTypes.SmartSquared:
                    return t * t;
                case InterpolationTypes.InverseSquared:
                case InterpolationTypes.SmartInverseSquared:
                    return 1f - (i * i);
                case InterpolationTypes.Cubic:
                case InterpolationTypes.SmartCubic:
                    return t * t * t;
                case InterpolationTypes.InverseCubic:
                case InterpolationTypes.SmartInverseCubic:
                    return 1f - (i * i * i);
                case InterpolationTypes.Sin:
                    return t * (Mathf.PI / 2f);
                case InterpolationTypes.InverseSin:
                    return 1f - (i * (Mathf.PI / 2f));
                default:
                    Debug.LogWarningFormat("Unhandled LerpType '{0}' requested!", type);
                    return t;
            }
        }

        /// <summary>
        /// Apply the given interpolation to the value t (range 0 to 1) a given number of times.
        /// </summary>
        /// <param name="t">The interpolation value (range 0 to 1)</param>
        /// <param name="type">The interpolation to use</param>
        /// <param name="repeats">The amount of times the interpolation will be applied (range 0 to 10)</param>
        /// <returns>The interpolated value (range 0 to 1)</returns>
        public static float ApplyLerpRepeatedly(float t, InterpolationTypes type, int repeats)
        {
            repeats = Mathf.Clamp(repeats, 0, 10);

            for(byte i = 0; i < repeats; i++)
            {
                t = ApplyLerp(t, type);
            }

            return t;
        }

        // ----------------------------------------------------------------------------------------------------------------------------------------------

        private static int ApplyRounding(float value, IntRounding rounding)
        {
            switch(rounding)
            {
                default:
                case IntRounding.Regular:
                    return Mathf.RoundToInt(value);
                case IntRounding.Floor:
                    return Mathf.FloorToInt(value);
                case IntRounding.Ceiling:
                    return Mathf.CeilToInt(value);
            }
        }

        private static Vector2Int ApplyRounding(Vector2 value, IntRounding rounding)
        {
            switch(rounding)
            {
                default:
                case IntRounding.Regular:
                    return new Vector2Int(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));
                case IntRounding.Floor:
                    return new Vector2Int(Mathf.FloorToInt(value.x), Mathf.FloorToInt(value.y));
                case IntRounding.Ceiling:
                    return new Vector2Int(Mathf.CeilToInt(value.x), Mathf.CeilToInt(value.y));
            }
        }

        private static Vector3Int ApplyRounding(Vector3 value, IntRounding rounding)
        {
            switch(rounding)
            {
                default:
                case IntRounding.Regular:
                    return new Vector3Int(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y), Mathf.RoundToInt(value.z));
                case IntRounding.Floor:
                    return new Vector3Int(Mathf.FloorToInt(value.x), Mathf.FloorToInt(value.y), Mathf.FloorToInt(value.z));
                case IntRounding.Ceiling:
                    return new Vector3Int(Mathf.CeilToInt(value.x), Mathf.CeilToInt(value.y), Mathf.CeilToInt(value.z));
            }
        }

        private static InterpolationTypes GetSmartType(float from, float to, InterpolationTypes type)
        {
            switch(type)
            {
                case InterpolationTypes.SmartSquared:
                    //Debug.LogFormat("Determining Smart type for {0} -> {1}: '{2}' -> '{3}'!", from, to, type, 
                    //    to > from ? InterpolationTypes.Squared : InterpolationTypes.InverseSquared);
                    return to > from ? InterpolationTypes.Squared : InterpolationTypes.InverseSquared;
                case InterpolationTypes.SmartInverseSquared:
                    //Debug.LogFormat("Determining Smart type for {0} -> {1}: '{2}' -> '{3}'!", from, to, type,
                    //    to > from ? InterpolationTypes.InverseSquared : InterpolationTypes.Squared);
                    return to > from ? InterpolationTypes.InverseSquared : InterpolationTypes.Squared;
                case InterpolationTypes.SmartCubic:
                    //Debug.LogFormat("Determining Smart type for {0} -> {1}: '{2}' -> '{3}'!", from, to, type,
                    //    to > from ? InterpolationTypes.Cubic : InterpolationTypes.InverseCubic);
                    return to > from ? InterpolationTypes.Cubic : InterpolationTypes.InverseCubic;
                case InterpolationTypes.SmartInverseCubic:
                    //Debug.LogFormat("Determining Smart type for {0} -> {1}: '{2}' -> '{3}'!", from, to, type,
                    //    to > from ? InterpolationTypes.InverseCubic : InterpolationTypes.Cubic);
                    return to > from ? InterpolationTypes.InverseCubic : InterpolationTypes.Cubic;
                default:
                    return type;
            }
        }

        // ----------------------------------------------------------------------------------------------------------------------------------------------

        #endregion

        // ----------------------------------------------------------------------------------------------------------------------------------------------
    }
}
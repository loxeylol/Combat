using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class MonoBehaviourExtensions
{
    // --- Enums ------------------------------------------------------------------------------------------------------

	// --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------

	// --- Properties -------------------------------------------------------------------------------------------------

	// --- Constructors -----------------------------------------------------------------------------------------------
	

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public static Coroutine DoAfter(this MonoBehaviour mb, YieldInstruction wait, Action action)
    {
        return mb.StartCoroutine(DoAfterRoutine(wait, action));
    }

    public static Coroutine DoAfter(this MonoBehaviour mb, IEnumerator enumerator, Action action)
    {
        return mb.StartCoroutine(DoAfterRoutine(enumerator, action));
    }

    public static Coroutine DoAfterSeconds(this MonoBehaviour mb, float seconds, bool realTime, Action action)
    {
        if(realTime)
        {
            return mb.DoAfter(new WaitForSecondsRealtime(seconds), action);
        }
        else
        {
            return mb.DoAfter(new WaitForSeconds(seconds), action);
        }
    }

    private static IEnumerator DoAfterRoutine(YieldInstruction wait, Action action)
    {
        yield return wait;
        action?.Invoke();
    }

    private static IEnumerator DoAfterRoutine(IEnumerator wait, Action action)
    {
        yield return wait;
        action?.Invoke();
    }

    // --- Protected/Private Methods ----------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
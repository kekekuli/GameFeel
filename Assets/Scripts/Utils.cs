using UnityEngine;
using System;
using System.Collections;

public static class Utils
{
    public static void RunAfterDelay(MonoBehaviour mono, float delay, Action action)
    {
        mono.StartCoroutine(RunAfterDelayCoroutine(delay, action));
    }

    private static IEnumerator RunAfterDelayCoroutine(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}

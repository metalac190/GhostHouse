using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace Mechanics.Dialog
{
    /// <summary>
    /// A flag used to abort animations or processes early.
    /// </summary>
    public class InterruptionFlag
    {
        public bool Interrupted { get; private set; } = false;
        public void Set() => Interrupted = true;
        public void Clear() => Interrupted = false;
    }

    /// <summary>
    /// A class of animation coroutines.
    /// </summary>
    public static class Tweens
    {
        /// <summary>
        /// A simple typewriter animation for text mesh pro that displays letters one at a time to match the provided rate.
        /// </summary>
        /// <param name="text">The text to animate. It should already have the text to display assigned. </param>
        /// <param name="lettersPerSecond"> The speed of this animation. </param>
        /// <param name="onCharacterTyped"> Callback performed each frame of the animation that is provided the index of the last letter displayed. </param>
        /// <param name="onComplete"> Callback performed at the end of the animation. </param>
        /// <param name="interruption"> An interruption flag that allows an early exit. When <see cref="InterruptionFlag.Set"/> is called, the animation will finish as fast as possible. </param>
        /// <returns></returns>
        public static IEnumerator SimpleTypewriter(TextMeshProUGUI text, float lettersPerSecond, Action<int> onCharacterTyped = null, Action onComplete = null, InterruptionFlag interruption = null)
        {
            bool paused = false;
            PauseMenu.PauseUpdated += Pause;
            text.maxVisibleCharacters = 0;

            // Wait a single frame to let the text component process its
            // content, otherwise text.textInfo.characterCount won't be
            // accurate
            yield return null;

            var characterCount = text.textInfo.characterCount;

            // Early out if letter speed is zero or text length is zero
            if (lettersPerSecond <= 0 || characterCount == 0)
            {
                text.maxVisibleCharacters = characterCount;
                onComplete?.Invoke();
                yield break;
            }

            float secondsPerLetter = 1.0f / lettersPerSecond;

            var accumulator = Time.deltaTime;
            while (text.maxVisibleCharacters < characterCount)
            {
                // pause effect if in the pause menu
                if (paused)
                {
                    yield return null;
                    continue;
                }

                // rush the animation if the interruption flag has been raised
                if (interruption?.Interrupted == true) break;

                while (accumulator >= secondsPerLetter)
                {
                    text.maxVisibleCharacters += 1;
                    onCharacterTyped?.Invoke(text.maxVisibleCharacters - 1);
                    accumulator -= secondsPerLetter;
                }
                accumulator += Time.deltaTime;

                yield return null;
            }

            text.maxVisibleCharacters = characterCount;
            PauseMenu.PauseUpdated -= Pause;
            onComplete?.Invoke();

            void Pause(bool isPaused)
            {
                paused = isPaused;
            }
        }

        /// <summary>
        /// A linear fade animation for <see cref="CanvasGroup"/>'s.
        /// </summary>
        /// <param name="canvasGroup"> The canvas group element to animate. </param>
        /// <param name="from"> The initial alpha (0-1). </param>
        /// <param name="to"> The final alpha (0-1). </param>
        /// <param name="duration"> The length of the animation in seconds. </param>
        /// <param name="onUpdate"> Callback performed after each frame of the animation. </param>
        /// <param name="onComplete"> Callback performed when the animation is complete. </param>
        /// <param name="interruption"> An interruption flag that allows an early exit. When <see cref="InterruptionFlag.Set"/> is called, the animation will finish as fast as possible. </param>
        /// <returns></returns>
        public static IEnumerator LerpAlpha(CanvasGroup canvasGroup, float from, float to, float duration, Action onUpdate = null, Action onComplete = null, InterruptionFlag interruption = null)
        {
            canvasGroup.alpha = from;

            float timeElapsed = 0f;
            while (timeElapsed < duration)
            {
                // rush the animation if the interruption flag has been raised
                if (interruption?.Interrupted == true) break;

                canvasGroup.alpha = Mathf.Lerp(from, to, timeElapsed / duration);
                onUpdate?.Invoke();
                timeElapsed += Time.deltaTime;

                yield return null;
            }

            canvasGroup.alpha = to;

            if (to == 0)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            else
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }

            onComplete?.Invoke();
        }

        /// <summary>
        /// A linear interpolation animation of <paramref name="transform"/> between <paramref name="from"/> and <paramref name="to"/> over <paramref name="duration"/> seconds.
        /// </summary>
        /// <param name="transform"> The transform being animated. </param>
        /// <param name="from"> The initial position of the transform. </param>
        /// <param name="to"> The final position of the transform. </param>
        /// <param name="duration"> The length of time to animate over. </param>
        /// <param name="onUpdate"> A callback performed each frame of the animation. </param>
        /// <param name="onComplete"> A callback performed when the animation is complete. </param>
        /// <param name="interruption"> An interruption flag that allows an early exit. When <see cref="InterruptionFlag.Set"/> is called, the animation will finish as fast as possible. </param>
        /// <returns></returns>
        public static IEnumerator LerpPosition(Transform transform, Vector3 from, Vector3 to, float duration, Action onUpdate = null, Action onComplete = null, InterruptionFlag interruption = null)
        {
            transform.position = from;

            var timeElapsed = 0f;
            while (timeElapsed < duration)
            {
                // rush the animation if the interruption flag has been raised
                if (interruption?.Interrupted == true) break;

                transform.position = Vector3.Lerp(from, to, timeElapsed / duration);
                onUpdate?.Invoke();
                timeElapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = to;
            onComplete?.Invoke();
        }

        public static IEnumerator WaitBefore(float waitTime, Action callback)
        {
            yield return new WaitForSeconds(waitTime);
            callback?.Invoke();
        }
    }
}
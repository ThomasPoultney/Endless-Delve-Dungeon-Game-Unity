using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Responsible for controlling the animation state of the attached object
/// </summary>
public class AnimationController : MonoBehaviour
{
    ///The name of the current animation
    public string currentAnimationState = "";
    ///The animator of the attached object
    public Animator animator;
    /// <summary>
    /// Changes the animation state of the animator attached to the object
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeAnimationState(string newState)
    {
        if (newState == currentAnimationState) return;

        animator.Play(newState);

        currentAnimationState = newState;
    }

    /// <summary>
    /// Retrieves the length of the current animation being played
    /// </summary>
    /// <returns>The length of the current animation</returns>
    public float getCurrentAnimationLength()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }

}

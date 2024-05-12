using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayClip : MonoBehaviour, IEnemyBehaviorInterface
{
    protected new Animation animation;
    protected UnityAction onComplete;

    public void SetAnimation(Animation newAnimation)
    {
        animation = newAnimation;

        AnimationEvent animEnd = new AnimationEvent();
        animEnd.time = animation.clip.length;
        animEnd.functionName = "OnComplete";

        animation.clip.AddEvent(animEnd);
        animation.clip.legacy = true;
        //animation.AddClip(animation.clip, "attack");
    }

    public void Execute(IEnemyInterface enemy, UnityAction onComplete)
    {
        Debug.Log(name + " execute");
        this.onComplete = onComplete;
        animation.Rewind();
        animation.Play();
    }

    public void Cancel()
    {
        // Call remaining animation events on clip.

        // Stop animation
    }

    protected void OnComplete()
    {
        Debug.Log(name + " animation finished.");

        onComplete?.Invoke();
    }
}

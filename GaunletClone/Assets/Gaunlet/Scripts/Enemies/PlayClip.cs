using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayClip : MonoBehaviour, IEnemyBehaviorInterface
{
    protected new Animation animation;
    protected AnimationClip clip;
    protected UnityAction onComplete;

    protected bool isPlaying = false;
    public bool IsPlaying { get { return isPlaying; } }

    protected void Awake()
    {
        if(animation == null)
        {
            animation = GetComponent<Animation>();
            if(animation == null )
            {
                animation = gameObject.AddComponent<Animation>();
                animation.playAutomatically = false;
            }
        }
    }

    public void SetAnimation(AnimationClip animationClip)
    {
        if (animation == null)
        {
            animation = GetComponent<Animation>();
            if (animation == null)
            {
                animation = gameObject.AddComponent<Animation>();
                animation.playAutomatically = false;
            }
        }

        if (clip != null) 
            animation.RemoveClip(clip);
        clip = animationClip;
        animation.AddClip(clip, clip.name);
        animation.clip = clip;
        animation.clip.legacy = true;
    }

    public void Execute(IEnemyInterface enemy, UnityAction onComplete)
    {
        Debug.Log(name + " execute");
        this.onComplete = onComplete;
        animation.clip = clip;
        enemy.Rigidbody.velocity = Vector3.zero;    
        animation.Rewind();
        animation.Play();
        isPlaying = true;
        StartCoroutine(AnimationTimer());
    }

    public void Cancel()
    {
        // Call remaining animation events on clip.

        // Stop animation
    }

    public IEnumerator AnimationTimer()
    {
        yield return new WaitForSeconds(clip.averageDuration);
        OnComplete();
    }

    protected void OnComplete()
    {
        isPlaying = false;
        onComplete?.Invoke();
    }
}

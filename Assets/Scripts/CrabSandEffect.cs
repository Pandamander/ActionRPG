using UnityEngine;

public class CrabSandEffect : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _clipName = "sand_burst";

    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _animator.Play(_clipName, 0, 0f);
        Destroy(gameObject, GetClipLength());
    }

    private float GetClipLength()
    {
        if (_animator.runtimeAnimatorController == null)
            return 0.35f;

        foreach (AnimationClip clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == _clipName)
                return clip.length;
        }

        return 0.35f;
    }
}

using UnityEngine;

public class SequenceLauncher : MonoBehaviour
{
    [SerializeField] private TweenSequencer[] _tweenSequences;

    [SerializeField] private bool _playOnStart = false;

    private void Start()
    {
        if (_playOnStart)
        {
            StartSequences();
        }
    }

    public void StartSequences()
    {
        for (int i = 0; i < _tweenSequences.Length; i++)
        {
            _tweenSequences[i].StartSequence();
        }
    }

}

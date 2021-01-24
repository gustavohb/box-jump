using UnityEngine;
using DG.Tweening;

public class TweenSequencer : MonoBehaviour
{
    [SerializeField] private TweenData[] _tweenDataArray;

    private Sequence _sequence;

    private bool _sequenceTriggered = false;

    private void Awake()
    {
        _sequence = DOTween.Sequence();
        BuildSequence();
    }

    private void BuildSequence()
    {
        _sequence.Pause();
        _sequence.SetAutoKill(false);
        for(int i = 0; i < _tweenDataArray.Length; i++)
        {
            if (_tweenDataArray[i].join)
            {
                _sequence.Join(_tweenDataArray[i].GetTween());
            }
            else
            {
                _sequence.Append(_tweenDataArray[i].GetTween());
            }
        }
    }

    public void StartSequence()
    {
        Debug.Log("Start Sequence on " + gameObject.name);

        if (!_sequenceTriggered)
        {
            _sequence.PlayForward();
        }
        else
        {
            _sequence.PlayBackwards();
        }

        _sequenceTriggered = !_sequenceTriggered;
    }
}

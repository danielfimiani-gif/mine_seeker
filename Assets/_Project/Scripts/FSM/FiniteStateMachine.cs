using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FiniteStateMachine<T>
{
    private DoubleEntryTable<FsmState<T>, UnityEvent, FsmState<T>> _fsmTable;
    private FsmState<T> _currentState;

    public FiniteStateMachine(FsmState<T>[] states, UnityEvent[] transitionEvents, FsmState<T> entryState)
    {
        _fsmTable = new DoubleEntryTable<FsmState<T>, UnityEvent, FsmState<T>>(states, transitionEvents);
        _currentState = entryState;

        _currentState.OnEnter();
    }

    private void OnTriggerTransition(UnityEvent transitionEvent)
    {
        FsmState<T> targetState = _fsmTable[_currentState, transitionEvent];

        if (targetState != null)
        {
            _currentState.OnExit();
            targetState.OnEnter();

            _currentState = targetState;
        }
    }

    public void ConfigureTransition(FsmState<T> sourceState, FsmState<T> targetState, UnityEvent transitionEvent)
    {
        _fsmTable[sourceState, transitionEvent] = targetState;
        transitionEvent.AddListener(() => OnTriggerTransition(transitionEvent));
    }

    public void Update() => _currentState.OnUpdate();
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FiniteStateMachine<T>
{
    private DoubleEntryTable<FsmState<T>, UnityEvent, FsmState<T>> _fsmTable;
    private FsmState<T> _currentState;
    private HashSet<UnityEvent> _subscribedEvents;

    public FiniteStateMachine(FsmState<T>[] states, UnityEvent[] transitionEvents, FsmState<T> entryState)
    {
        _fsmTable = new DoubleEntryTable<FsmState<T>, UnityEvent, FsmState<T>>(states, transitionEvents);
        _currentState = entryState;

        _currentState.OnEnter();
        _subscribedEvents = new();
    }

    private void OnTriggerTransition(UnityEvent transitionEvent)
    {
        var targetState = _fsmTable[_currentState, transitionEvent];
        Debug.Log($"[StateMachine] Attempting transition: {_currentState} -> {targetState} via {transitionEvent}");

        if (targetState == null)
            return;

        _currentState.OnExit();
        targetState.OnEnter();

        Debug.Log($"[StateMachine] Transition completed: {_currentState} -> {targetState} via {transitionEvent}");
        _currentState = targetState;
    }

    public void ConfigureTransition(FsmState<T> sourceState, FsmState<T> targetState, UnityEvent transitionEvent)
    {
        _fsmTable[sourceState, transitionEvent] = targetState;
        if (!_subscribedEvents.Contains(transitionEvent))
        {
            transitionEvent.AddListener(() => OnTriggerTransition(transitionEvent));
            _subscribedEvents.Add(transitionEvent);
        }

    }

    public void Update() => _currentState.OnUpdate();
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StateMachine : MonoBehaviour {
	[HideInInspector] public Enum lastState;

    public class State {
        public Action DoUpdateState = DoNothing;
		public Action DoFixedUpdateState = DoNothing;
        public Action EnterState = DoNothing;
        public Action ExitState = DoNothing;

        public Enum currentState;
    }

    public Enum currentState {
        get {return state.currentState;}
        set {
            if (state.currentState == value) return;

            ChangingState();
            state.currentState = value;
            ConfigureCurrentState();
        }
    }

	protected float timeEnteredState;

	private State state = new State();
	private Dictionary<Enum, Dictionary<string, Delegate>> allStateDelegates = new Dictionary<Enum, Dictionary<string, Delegate>>();

	private void Update() {
		PreUpdateState();
		state.DoUpdateState();
		PostUpdateState();
	}

	private void FixedUpdate() {
		PreFixedUpdateState();
		state.DoFixedUpdateState();
		PostFixedUpdateState();
	}

	private void ChangingState() {
        lastState = state.currentState;
        timeEnteredState = Time.time;
    }

	private void ConfigureCurrentState() {
        if (state.ExitState != null) state.ExitState();

		state.DoUpdateState = ConfigureDelegate<Action>("UpdateState", DoNothing);
		state.DoFixedUpdateState = ConfigureDelegate<Action>("FixedUpdateState", DoNothing);
        state.EnterState = ConfigureDelegate<Action>("EnterState", DoNothing);
        state.ExitState = ConfigureDelegate<Action>("ExitState", DoNothing);

        if (state.EnterState != null) state.EnterState();
    }

	private T ConfigureDelegate<T>(string methodRoot, T Default) where T : class {
		Dictionary<string, Delegate> currentStateDelegates;
		Delegate currentStateDelegate;

		// Check if there's already a dictionary of state methods/delegates for the current state.
		// If there isn't one yet, create one.
        if (!allStateDelegates.TryGetValue(state.currentState, out currentStateDelegates)) {
            allStateDelegates[state.currentState] = currentStateDelegates = new Dictionary<string, Delegate>();
        }

		// If the current state already has a delegate defined for this method root, just return it.
		// Otherwise, create a connection to the appropriate method and return it.
        if (!currentStateDelegates.TryGetValue(methodRoot, out currentStateDelegate)) {
			string fullMethodName = state.currentState.ToString() + "_" + methodRoot;
            var method = GetType().GetMethod(fullMethodName,
			                                 System.Reflection.BindingFlags.Instance |
			                                 System.Reflection.BindingFlags.Public |
			                                 System.Reflection.BindingFlags.NonPublic |
			                                 System.Reflection.BindingFlags.InvokeMethod);

            if (method != null) currentStateDelegate = Delegate.CreateDelegate(typeof(T), this, method);
            else currentStateDelegate = Default as Delegate;

            currentStateDelegates[methodRoot] = currentStateDelegate;
        }
        return currentStateDelegate as T;
    }

    protected virtual void PreUpdateState() {}
    protected virtual void PostUpdateState() {}

	protected virtual void PreFixedUpdateState() {}
	protected virtual void PostFixedUpdateState() {}

    static void DoNothing() {}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSM
{
	private Dictionary<string,FSMDelegate> executeDelegates = new Dictionary<string,FSMDelegate>();
	private Dictionary<string,FSMDelegate> enterDelegates = new Dictionary<string,FSMDelegate>();
	private Dictionary<string,FSMDelegate> exitDelegates = new Dictionary<string,FSMDelegate>();
	private string currentState;
	private List<string> stateNames = new List<string>();

	public bool DEBUG = false;
	public delegate void FSMDelegate();

	public void SetState( string newState ){
		// HANDLE EXCEPTIONS ///////////////////////////////////////////////////////////////
		// Ignore switching ot itself
		if( currentState == newState ){
			if( DEBUG )Debug.Log("Already in state \"" + newState + "\"!");
			return;
		}

		// Exit if state does not exist
		if( !stateNames.Contains( newState ) ){
			if( DEBUG )Debug.Log("State \"" + newState + "\" does not exist!");
			return;
		}
		//////////////////////////////////////////////////////////// EO HANDLE EXCEPTIONS //

		// SWITCH STATE ///////////////////////////////////////////////////////////////
		// Exit current state
		if( currentState != null && exitDelegates[currentState] != null ){
			if( DEBUG )Debug.Log( "\t\tFSM: \t   (   " + currentState + "-->   )"	);
			exitDelegates[currentState].Invoke();
		}

		// Set new state as current
		currentState = newState;

		// Enter new state
		if( currentState != null && enterDelegates[currentState] != null ){
			if( DEBUG )Debug.Log( "\t\tFSM: \t-->(   " + currentState + "   )" );
			enterDelegates[currentState].Invoke();
		}
		//////////////////////////////////////////////////////////// EO SWITCH STATE //
	}

	public string GetState(){
		return currentState;
	}

	public void AddState( string stateName, FSMDelegate executeDelegate, FSMDelegate enterDelegate, FSMDelegate exitDelegate ){
		if( executeDelegate == null && enterDelegate == null && exitDelegate == null ){
			if( DEBUG )Debug.Log("Pass at least one delegate!");
			return;
		}

		if( stateNames.Contains( stateName ) ){
			if( DEBUG )Debug.Log("State with this name already exists!");
			return;
		}

		stateNames.Add( stateName );
		executeDelegates.Add( stateName, executeDelegate );
		enterDelegates.Add( stateName, enterDelegate );
		exitDelegates.Add( stateName, exitDelegate );
	}

	public void RemoveState( string stateName ){
		if( currentState == stateName ){
			if( DEBUG )Debug.Log("State currently running!");
			return;
		}

		stateNames.Remove( stateName );
		executeDelegates.Remove( stateName );
		enterDelegates.Remove( stateName );
		exitDelegates.Remove( stateName );
	}

	public void Execute(){
		if( currentState != null && executeDelegates[currentState] != null ){
			if( DEBUG )Debug.Log( "\t\tFSM: \t   (   " + currentState + "   )"	);
			executeDelegates[currentState].Invoke();
		}
		else
			if( DEBUG )Debug.Log("No state set!");
	}
}

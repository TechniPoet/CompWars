using UnityEngine;
using System.Collections;
using GAudio;
using System;

public class PulseScript : MonoBehaviour, IGATPulseClient
{
	public delegate void NotePass(int i);
	public event NotePass PlayNotesEvent;

	PulseModule pulse;
	PulseModule _Pulse
	{
		get
		{
			if (pulse == null)
			{
				pulse = GetComponent<PulseModule>();
			}
			return pulse;
		}
	}


	public void OnPulse(IGATPulseInfo pulseInfo)
	{
		if (PlayNotesEvent != null)
		{
			PlayNotesEvent(pulseInfo.StepIndex);
		}
		
	}

	public void PulseStepsDidChange(bool[] newSteps)
	{
		throw new NotImplementedException();
	}

	// Use this for initialization
	void Start ()
	{
		
	}

	void OnEnable()
	{
		_Pulse.SubscribeToPulse(this);
	}

	void OnDisable()
	{
		_Pulse.UnsubscribeToPulse(this);
	}
}

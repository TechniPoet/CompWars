using UnityEngine;
using System.Collections;
using GAudio;

public class MusicManager : GameMono
{
	public GATActiveSampleBank sampleBank;
	public GATSoundBank toLoad;
	public static bool _PInit = false;

	public PulseScript pulser;

	// Use this for initialization
	void Awake ()
	{
		
		Load();
	}


	void Play(int i)
	{
		GATData mySampleData = sampleBank.GetAudioData("piano_5_B");
		GATManager.DefaultPlayer.PlayData(mySampleData, 0, 1);
	}


	void StartPulse()
	{
		_PInit = true;
		sampleBank.LoadFinished -= StartPulse;
		pulser.PlayNotesEvent += Play;
	}


	void Load()
	{
		sampleBank.LoadFinished += StartPulse;
		sampleBank.SoundBank = toLoad;
		sampleBank.LoadAll();
	}
}

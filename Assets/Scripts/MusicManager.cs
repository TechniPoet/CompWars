using UnityEngine;
using System.Collections;
using GAudio;

public class MusicManager : GameMono
{
	public GATActiveSampleBank sampleBank;
	public GATSoundBank toLoad;
	public static bool _PInit = false;

	public MasterPulseModule mainPulse;
	public PulseScript pulser;

	GATEnvelope wholeNoteEnv;
	GATEnvelope halfNoteEnv;
	GATEnvelope quarterNoteEnv;
	GATEnvelope eighthNoteEnv;
	GATEnvelope sixteenthNoteEnv;

	ConstFile.NOTE key = ConstFile.NOTE.C;

	int[] keyScale;

	int[] majorProgression = new int[] { 0, 3, 4, 0 };
	int[] majorChord = new int[] { 0, 2, 4 };
	int[] scaleSteps = new int[] { 0, 2, 2, 1, 2, 2, 2 };

	// Use this for initialization
	void Awake ()
	{
		Load();
		SetupEnvelopes();
		int baseKey = (int)key;

		keyScale = new int[7];
		for (int i = 0; i < scaleSteps.Length; i++)
		{
			
			baseKey += scaleSteps[i];
			baseKey %= 12;
			keyScale[i] = baseKey;
			Debug.Log(ConstFile.PIANO_NOTES[baseKey] + " " + baseKey);
		}
	}


	void Play(int i)
	{
		//GATData mySampleData = sampleBank.GetAudioData("piano_5_B");
		//GATManager.DefaultPlayer.PlayData(mySampleData, 0, 1);
		//i = i - 1;
		if (i % 16 == 0)
		{
			WholeBeat(i);
		}
		if (i % 8 == 0)
		{
			HalfBeat((i / 8));
		}
		if (i % 4 == 0)
		{
			QuarterBeat((i / 4));
		}
		if (i % 2 == 0)
		{
			EighthBeat((i / 2));
		}
		SixteenthBeat(i);
	}


	void StartPulse()
	{
		_PInit = true;
		sampleBank.LoadFinished -= StartPulse;
		pulser.PlayNotesEvent += Play;
		mainPulse.StartPulsing(15);
	}


	void Load()
	{
		sampleBank.LoadFinished += StartPulse;
		sampleBank.SoundBank = toLoad;
		sampleBank.LoadAll();
	}

	void SetupEnvelopes()
	{
		wholeNoteEnv = MusicUtil.CreateEnvelope(ConstFile.NoteLen.WHOLE);
		halfNoteEnv = MusicUtil.CreateEnvelope(ConstFile.NoteLen.HALF);
		quarterNoteEnv = MusicUtil.CreateEnvelope(ConstFile.NoteLen.QUARTER);
		eighthNoteEnv = MusicUtil.CreateEnvelope(ConstFile.NoteLen.EIGHTH);
		sixteenthNoteEnv = MusicUtil.CreateEnvelope(ConstFile.NoteLen.SIXTEENTH);
	}

	void WholeBeat(int rep)
	{

	}

	void HalfBeat(int rep)
	{

	}

	void QuarterBeat(int rep)
	{
		IGATProcessedSample sample;
		string[] chordArray = MusicUtil.CreateMajorChord(majorProgression[rep], keyScale, ConstFile.PIANO_NOTES, majorChord);
		for (int i = 0; i < chordArray.Length; i++)
		{
			sample = sampleBank.GetProcessedSample(chordArray[i], halfNoteEnv);
			sample.Play(0);
		}
	}

	void EighthBeat(int rep)
	{

	}

	void SixteenthBeat(int rep)
	{

	}
}

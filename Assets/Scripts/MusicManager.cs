using UnityEngine;
using System.Collections.Generic;
using GAudio;
using ROMAN_NUM = ConstFile.ROMAN_NUMBERAL;
using CHORD_TYPE = ConstFile.CHORD_TYPE;
using System.IO;

[System.Serializable]
public class ChordNotation
{
	bool intSet = false;
	public string chordName;
	public bool isOn;
	int baseInt = -1;
	public int BaseInt
	{
		get
		{
			if (!intSet)
			{
				baseInt = (int)chordBase;
				intSet = true;
			}
			return baseInt;
		}
	}
	public ROMAN_NUM chordBase;
	public CHORD_TYPE chordType;
	public ConstFile.NoteLen playOn;
	public ConstFile.NoteLen PlayOn
	{
		get
		{
			return playOn;
		}

		set
		{
			if (value != playOn)
			{
				Counts = null;
			}
			playOn = value;
		}
	}
	public ConstFile.NoteLen noteLen;
	[SerializeField]
	bool[] counts;
	public bool[] Counts
	{
		get
		{
			if (counts == null)
			{
				int num = 1;
				switch (PlayOn)
				{
					case ConstFile.NoteLen.WHOLE:
						num = 1;
						break;
					case ConstFile.NoteLen.HALF:
						num = 2;
						break;
					case ConstFile.NoteLen.QUARTER:
						num = 4;
						break;
					case ConstFile.NoteLen.EIGHTH:
						num = 8;
						break;
					case ConstFile.NoteLen.SIXTEENTH:
						num = 16;
						break;
				}

				counts = new bool[num];
			}
			return counts;
		}

		set
		{
			counts = value;
		}
	}

	public ChordNotation() : this(ROMAN_NUM.I, CHORD_TYPE.TRIAD)
	{
	}

	public ChordNotation(ROMAN_NUM b, CHORD_TYPE t) : this(b,t, ConstFile.NoteLen.QUARTER, -1)
	{
		chordBase = b;
		chordType = t;
		PlayOn = ConstFile.NoteLen.QUARTER;
	}

	public ChordNotation(ROMAN_NUM b, CHORD_TYPE t, ConstFile.NoteLen on, int cnt)
	{
		chordBase = b;
		chordType = t;
		PlayOn = on;

		for (int i = 0; i < Counts.Length; i++)
		{
			Counts[i] = cnt == i ? true : false;
		}
	}

	public string[] GetChord(int b, int[] scaleArray, string[] sampleArray)
	{
		Debug.Log(BaseInt);
		switch (chordType)
		{
			case CHORD_TYPE.TRIAD:
				return MusicUtil.CreateMajorChord(BaseInt, scaleArray, sampleArray);
			case CHORD_TYPE.MINOR:
				return MusicUtil.CreateMinorChord(BaseInt, scaleArray, sampleArray);
			default:
				throw new System.Exception(string.Format("Unknown chord type: {0} {1}", chordType, chordBase));
		}
	}
}

[System.Serializable]
public class Progression : ScriptableObject
{
	public string progName;
	public ChordNotation[] prog;

	List<ChordNotation> toRemove = new List<ChordNotation>();

	public void Init(ChordNotation[] _progression)
	{
		prog = _progression;
	}

	public void Init()
	{
		prog = new ChordNotation[] { };
		progName = "New Progression";
	}

	public void AddChord()
	{
		List<ChordNotation> chords = new List<ChordNotation>(prog);
		chords.Add(new ChordNotation());
		prog = chords.ToArray();
	}

	public void RemoveChord(ChordNotation i)
	{
		toRemove.Add(i);
	}

	public void Clean()
	{
		List<ChordNotation> chords = new List<ChordNotation>(prog);
		foreach (ChordNotation c in toRemove)
		{
			chords.Remove(c);
		}
		prog = chords.ToArray();
	}

	public ChordNotation this[int index]
	{
		get
		{
			return prog[index];
		}
		set
		{
			prog[index] = value;
		}
	}
}

public class MusicManager : GameMono
{
	public GATActiveSampleBank sampleBank;
	public GATSoundBank toLoad;
	public static bool _PInit = false;

	public MasterPulseModule mainPulse;
	public PulseScript pulser;

    [SerializeField]
    public MultiProgression multiProg;
    [SerializeField]
    public string multiProgName;

	GATEnvelope wholeNoteEnv;
	GATEnvelope halfNoteEnv;
	GATEnvelope quarterNoteEnv;
	GATEnvelope eighthNoteEnv;
	GATEnvelope sixteenthNoteEnv;

	public ConstFile.NOTE key = ConstFile.NOTE.C;

	int[] keyScale;

	public List<Progression> progressions = new List<Progression>();
    public Progression currProgression;
	
	public int progressionInd = 0;

	int[] majorProgression = new int[] { 0, 3, 4, 0 };
	int[] scaleSteps = new int[] { 0, 2, 2, 1, 2, 2, 2 };
    
    public int currBeat = 0;
    [SerializeField]
    public Transform playHead;

	// Use this for initialization
	void Awake ()
	{
		Load();
		SetupEnvelopes();
		int baseKey = (int)key;

		keyScale = MusicUtil.GetScaleArray(baseKey);

        multiProg = ScriptableObject.CreateInstance<MultiProgression>() as MultiProgression;
        multiProg.Init();
        Debug.Log("opening "+ Path.Combine("Assets/Resources/MultiProgressions", string.Format("{0}", multiProgName)));
        JsonUtility.FromJsonOverwrite(File.ReadAllText(Path.Combine("Assets/Resources/MultiProgressions", string.Format("{0}", "145 Test.json"))), multiProg);
        multiProg.Load();

        for (int i = 0; i < multiProg.progFiles.Count; i++)
        {
            progressions.Add(multiProg.progFiles[i].p);
            print("Added "+ multiProg.progFiles[i].p.progName);
        }
        currProgression = progressions[progressions.Count - 1];
	}


	void Play(int i)
	{
        currBeat = i;
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
        playHead.transform.localScale = new Vector3(ArenaManager.nodeWidth*4, playHead.transform.localScale.y, playHead.transform.localScale.z);
        playHead.transform.position = new Vector3(ArenaManager.leftSide.x + ((ArenaManager.screenWidth/16)*(i+.5f)), ArenaManager.midPoint.y, 0);
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

	#region beat functions

	void WholeBeat(int rep)
	{
		PlayChord(ConstFile.NoteLen.WHOLE, rep, wholeNoteEnv, ConstFile.PIANO_NOTES);
	}


	void HalfBeat(int rep)
	{
		PlayChord(ConstFile.NoteLen.HALF, rep, halfNoteEnv, ConstFile.PIANO_NOTES);
	}


	void QuarterBeat(int rep)
	{
		PlayChord(ConstFile.NoteLen.QUARTER, rep, quarterNoteEnv, ConstFile.PIANO_NOTES);
	}


	void EighthBeat(int rep)
	{
		PlayChord(ConstFile.NoteLen.EIGHTH, rep, eighthNoteEnv, ConstFile.PIANO_NOTES);
	}


	void SixteenthBeat(int rep)
	{
		PlayChord(ConstFile.NoteLen.SIXTEENTH, rep, sixteenthNoteEnv, ConstFile.PIANO_NOTES);
		if (rep == 15)
		{
            progressionInd = (progressionInd + 1) % progressions.Count;
            this.currProgression = progressions[progressionInd];
		}
	}

	#endregion


	void PlayChord(ConstFile.NoteLen noteLen, int cnt, GATEnvelope env, string[] sampleArray)
	{
		for (int i = 0; i < progressions[progressionInd].prog.Length; i++)
		{
			if (progressions[progressionInd][i].PlayOn == noteLen && progressions[progressionInd][i].Counts[cnt])
			{
				IGATProcessedSample sample;
				string[] chordArray = progressions[progressionInd][i].GetChord((int)key, keyScale, sampleArray);
				for (int j = 0; j < chordArray.Length; j++)
				{
					sample = sampleBank.GetProcessedSample(chordArray[j], env);
					sample.Play(0);
				}
				return;
			}
		}
	}
}

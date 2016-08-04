using UnityEngine;
using System.Collections;
using GAudio;


public static class MusicUtil
{
	public class NoteNotation
	{
		public int accidental;
		public int index;

		public NoteNotation(int ind) : this(ind, 0) { }

		public NoteNotation(int ind, int accident)
		{
			index = ind;
			accidental = accident;
		}

		public string NoteToPlay(int baseIndex, int[] scaleArray, string[] sampleArray, ref int oct)
		{
			int note = scaleArray[(baseIndex + index) % scaleArray.Length];
			if ((baseIndex + index) - scaleArray.Length > 0)
			{
                /*
                 * base 9 +  4
                 */
				oct = oct + 1;
                Debug.LogWarning("Note less than 0 " + note + " " + baseIndex + " " + index + " " + scaleArray.Length);
                for(int i = 0; i < sampleArray.Length; i++)
                {
                    Debug.Log(i + " " + sampleArray[i]);
                }
			}
			else if (accidental > 0)
			{
				if (note >= sampleArray.Length)
				{
					note = note % sampleArray.Length;
					oct++;
				}
			}
			
			return string.Format(sampleArray[note + accidental], oct);
		}
	}


    #region  Chord Notations

    static NoteNotation[] TRIAD_CHORD_NOTATION = new NoteNotation[] 
	{
		new NoteNotation(0),
		new NoteNotation(2),
		new NoteNotation(4)
	};

	static NoteNotation[] MINOR_CHORD_NOTATION = new NoteNotation[]
	{
		new NoteNotation(0),
		new NoteNotation(2),
		new NoteNotation(4)
	};

    static NoteNotation[] SEVEN_CHORD_NOTATION = new NoteNotation[]
    {
        new NoteNotation(0),
        new NoteNotation(2),
        new NoteNotation(4),
        new NoteNotation(6)
    };

    #endregion


    static int[] scaleSteps = new int[] { 0, 2, 2, 1, 2, 2, 2 };
	static int[] minorScaleSteps = new int[] { 0, 2, 1, 2, 2, 3, 1 };
    

    public static GATEnvelope CreateEnvelope(ConstFile.NoteLen note, int offset = 0, bool normalize = false)
	{
        float sampleRate = ConstFile.SAMPLE_RATE;

		int len = Mathf.FloorToInt((ConstFile.NoteBPMCalcs[(int)note] / ConstFile.BPM) * sampleRate);
		int fadeIn = Mathf.FloorToInt((sampleRate * len) / 135000);
		int fadeOut = Mathf.FloorToInt((sampleRate * len) / 4);

		return new GATEnvelope(len, fadeIn, fadeOut, offset, normalize);
	}

    #region Create Chords

    /// <summary>
    /// Returns array of key strings for g-audio to play to generate a major chord.
    /// </summary>
    /// <param name="baseIndex">Index of base note in scale array for desired chord</param>
    /// <param name="scaleArray">Array of indexes to be used for current scale</param>
    /// <param name="sampleArray">Array of key strings for desired instrument</param>
    /// <param name="baseOctave">Octave this chord should be played in</param>
    /// <returns></returns>
    public static string[] CreateMajorChord(int baseIndex, int[] scaleArray, string[] sampleArray, int baseOctave = 2)
	{
		return CreateChord(baseIndex, scaleArray, sampleArray, TRIAD_CHORD_NOTATION, baseOctave);
	}


	/// <summary>
	/// Returns array of key strings for g-audio to play to generate a minor chord.
	/// </summary>
	/// <param name="baseIndex">Index of base note in scale array for desired chord</param>
	/// <param name="scaleArray">Array of indexes to be used for current scale</param>
	/// <param name="sampleArray">Array of key strings for desired instrument</param>
	/// <param name="baseOctave">Octave this chord should be played in</param>
	/// <returns></returns>
	public static string[] CreateMinorChord(int baseIndex, int[] scaleArray, string[] sampleArray, int baseOctave = 3)
	{
		return CreateChord(baseIndex, scaleArray, sampleArray, MINOR_CHORD_NOTATION, baseOctave);
	}

    /// <summary>
	/// Returns array of key strings for g-audio to play to generate a seven chord.
	/// </summary>
	/// <param name="baseIndex">Index of base note in scale array for desired chord</param>
	/// <param name="scaleArray">Array of indexes to be used for current scale</param>
	/// <param name="sampleArray">Array of key strings for desired instrument</param>
	/// <param name="baseOctave">Octave this chord should be played in</param>
	/// <returns></returns>
	public static string[] CreateSevenChord(int baseIndex, int[] scaleArray, string[] sampleArray, int baseOctave = 3)
    {
        return CreateChord(baseIndex, scaleArray, sampleArray, SEVEN_CHORD_NOTATION, baseOctave);
    }

    /// <summary>
    /// Returns array of key strings for g-audio to play to generate chord.
    /// </summary>
    /// <param name="baseIndex">Index of base note in scale array for desired chord</param>
    /// <param name="scaleArray">Array of indexes to be used for current scale</param>
    /// <param name="sampleArray">Array of key strings for desired instrument</param>
    /// <param name="chordArray">Array of NoteNotation specifying how to make chord</param>
    /// <param name="baseOctave">Octave this chord should be played in</param>
    /// <returns></returns>
    public static string[] CreateChord(int baseIndex, int[] scaleArray, string[] sampleArray, NoteNotation[] chordArray, int baseOctave = 3)
	{
		string temp = "";
		string[] ret = new string[chordArray.Length];
		for (int j = 0; j < chordArray.Length; j++)
		{
			ret[j] = chordArray[j].NoteToPlay(baseIndex, scaleArray, sampleArray, ref baseOctave);
			temp += ret[j];
			temp += "\n";
		}
		Debug.Log(temp);
		return ret;
	}

    #endregion

    #region Scales

    public static int[] GetScaleArray(int baseKey)
	{
		int[] keyScale = new int[7];
		for (int i = 0; i < scaleSteps.Length; i++)
		{
			baseKey += scaleSteps[i];
			baseKey %= 12;
			keyScale[i] = baseKey;
		}
		return keyScale;
	}

	public static int[] GetMinorScaleArray(int baseKey)
	{
		int[] keyScale = new int[7];
		for (int i = 0; i < minorScaleSteps.Length; i++)
		{
			baseKey += minorScaleSteps[i];
			baseKey %= 12;
			keyScale[i] = baseKey;
		}
		return keyScale;
	}

    #endregion
}

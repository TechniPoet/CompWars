using UnityEngine;
using System.Collections;
using GAudio;


public static class MusicUtil
{
	public static GATEnvelope CreateEnvelope(ConstFile.NoteLen note, int offset = 0, bool normalize = false)
	{
		float sampleRate = 44100;

		int len = Mathf.FloorToInt((ConstFile.NoteBPMCalcs[(int)note] / ConstFile.BPM) * sampleRate);
		int fadeIn = Mathf.FloorToInt((sampleRate * len) / 135000);
		int fadeOut = Mathf.FloorToInt((sampleRate * len) / 4);

		return new GATEnvelope(len, fadeIn, fadeOut, offset, normalize);
	}

	public static string[] CreateMajorChord(int baseIndex, int[] scaleArray, string[] sampleArray, int[] chordArray, int baseOctave = 3)
	{
		string[] ret = new string[chordArray.Length];
		for (int j = 0; j < chordArray.Length; j++)
		{
			int baseInt = baseIndex;
			baseInt += chordArray[j];
			int noteInt = scaleArray[baseInt % scaleArray.Length];
			int octave = baseOctave + (baseInt / scaleArray.Length);
			ret[j] = string.Format(sampleArray[noteInt], octave);
		}

		return ret;
	}
}

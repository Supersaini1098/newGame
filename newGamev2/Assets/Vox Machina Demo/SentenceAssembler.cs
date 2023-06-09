using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/****************************************************************************************************************
Attatch this script to an object with an audio source. Enter the number of sentences you would like in the 
Custom Sentence Lists field, and then fill those arrays with the audio clips you would like it to play. In order
to play a specific sentence, call the GetSentence(#) function, where "#" is the index of the sentence in 
CustomSentenceLists[]. 
EXAMPLE: To play the first sentence in CustomSentenceLists[] you would use GetSentence(0).

-------
Pitch
-------
The pitch options will change the pitch of each individual word in a sentence depending on the values in the 
Pitch Bend Range field. 
None: This will leave the pitch unaffected.

Random Pitch: This will coose a random value inside of the pitch bend range for each word in a sentence.

Ascending: The first word of the sentence will start at the lowest value of pitch bend range and step up on 
	each successive word.

Descending: The first word of the sentence will start at the highest value of pitch bend range and step down 
	on each successive word.

---------
Question
---------
When Question Ask is toggled on, the last word in a sentence will be pitched up, imitating a questioning tone.
The inflection option indicates how much the pitch is altered, a larger number means a greater change.

*****************************************************************************************************************/

[System.Serializable]
public class CustomSentenceList {
	public AudioClip[] audioFiles;
    

}
	
public class SentenceAssembler : MonoBehaviour {
	private bool currentlyPlaying;

	[Tooltip("The number of specific custom sentences for this Game Object. Fill in each sub-array with the desired sound files.")]
	public CustomSentenceList[] customSentenceLists;
	private AudioSource soundSource;
	private AudioClip[] currentWordQueue;
	private bool startPlayingSentence;

	//Options
	public enum PitchOptions {
		None,
		RandomPitch,
		Ascending,
		Descending
	};
	[Header("Pitch Options", order = 1)]
	[Tooltip("Different pitch options to be selected")]
	public PitchOptions pitchOptions = new PitchOptions();
	[Tooltip("The default pitch used for reference")]
	public float defaultPitch = 1f;
	[Tooltip("The minimum and maximum values that the pitch will be altered")]
	public Vector2 pitchBendRange = new Vector2(0.95f, 1.05f);

	[Header("Question Options", order = 2)]
	//Question
	[Tooltip("Adds a questioning inflection to the last word in a sentence when selected")]
	public bool questionAsk;
	[Tooltip("The amount of inflection added")]
	public float questionInflection = 0.1f;

	// Use this for initialization
	void Start () {
		soundSource = GetComponent<AudioSource> ();

		//Ensure left Value is always smaller than right
		if (pitchBendRange.x > pitchBendRange.y) {
			var tempVal = pitchBendRange.y;
			pitchBendRange.y = pitchBendRange.x;
			pitchBendRange.x = tempVal;
		}
	}

	// Get a list of sound files to play, and trigger the start of the sentence

	public void GetSentence (int sentenceNum) {
		if (!currentlyPlaying) {
			currentWordQueue = customSentenceLists [sentenceNum].audioFiles;
			for (int i = 0; i < currentWordQueue.Length; i++) {
				currentWordQueue [i] = customSentenceLists [sentenceNum].audioFiles [i];
			}
			StartCoroutine (PlaySentence ());
		}
	}

	//Play surrently selected sentence

	IEnumerator PlaySentence() {
		for (int i = 0; i < currentWordQueue.Length; i++) {
			currentlyPlaying = true;
			//Set the pitch bend Options
			//Reset to default Pitch
			if (i == 0) {
				soundSource.pitch = defaultPitch;
			}
			switch(pitchOptions) {
			//None
			case PitchOptions.None:
				break;

			//Random Pitch
			case PitchOptions.RandomPitch:
				soundSource.pitch = Random.Range (pitchBendRange.x, pitchBendRange.y);
				break;

			//Ascending
			case PitchOptions.Ascending:
				var pitchStepUp = (pitchBendRange.y - pitchBendRange.x) / currentWordQueue.Length;
				if (i == 0) {
					soundSource.pitch = pitchBendRange.x;
				}
				if (soundSource.pitch < pitchBendRange.y) {
					soundSource.pitch = soundSource.pitch + pitchStepUp;
				}
				break;

			//Descending
			case PitchOptions.Descending:
				var pitchStepDown = (pitchBendRange.y - pitchBendRange.x) / currentWordQueue.Length;
				if (i == 0) {
					soundSource.pitch = pitchBendRange.y;
				}
				if (soundSource.pitch > pitchBendRange.x) {
					soundSource.pitch = soundSource.pitch - pitchStepDown;
				}
				break;

			}

			soundSource.clip = currentWordQueue [i];
			soundSource.Play ();

			//Ask Question?
			if (questionAsk) {
				if ((currentWordQueue.Length - 1) == i) {
					StartCoroutine (PitchUp(soundSource.clip.length, questionInflection));
				}
			}

			yield return new WaitForSeconds (soundSource.clip.length);
			//Set Status to not playing on last clip
			if ((currentWordQueue.Length - 1) == i) {
				currentlyPlaying = false;
			}
		}
	}

	IEnumerator PitchUp(float clipLength, float pitchChange ) {
		var elapsedTime = 0f;
		var startPitch = soundSource.pitch;

		if (elapsedTime >= clipLength) {
			soundSource.pitch = startPitch;
		}
		while (elapsedTime < clipLength) {
			soundSource.pitch = Mathf.Lerp (startPitch, (startPitch + pitchChange), elapsedTime/clipLength);
			elapsedTime += Time.deltaTime;	
			yield return null;
		}
	}
}

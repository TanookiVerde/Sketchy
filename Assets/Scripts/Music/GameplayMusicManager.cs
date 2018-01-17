using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMusicManager : MonoBehaviour {

	[SerializeField] AudioSource sfxAudioSource;
	[SerializeField] AudioSource myAudioSource;

	[SerializeField] AudioClip turnPage;
	[SerializeField] AudioClip starSound;
	[SerializeField] AudioClip scribble;

	private void Start(){
		myAudioSource = GetComponent<AudioSource>();
	}
	public void PlaySound(Sounds s){
		AudioClip choice = scribble;
		switch(s){
			case Sounds.TURNPAGE:
				choice = turnPage;
				break;
			case Sounds.STAR:
				choice = starSound;
				break;
			case Sounds.SCRIBBLE:
				choice = scribble;
				break;
		}
		sfxAudioSource.clip = choice;
		sfxAudioSource.Play();
	}
	public void StopSound(){
		sfxAudioSource.Stop();
	}
	public void StopAllSound(){
		myAudioSource.Stop();
		sfxAudioSource.Stop();
	}
	public void DecreaseVolume(){
		myAudioSource.volume *= 0.5f;
	}
	public IEnumerator IncreasePitch(){
		myAudioSource.pitch = 1;
		for(int i = 0; i < 20; i++){
			myAudioSource.pitch = Mathf.MoveTowards(myAudioSource.pitch,1.15f,0.01f);
			yield return new WaitForEndOfFrame();
		}		
	}
	public void ResetPitch(){
		StopCoroutine(IncreasePitch());
		myAudioSource.pitch = 1;
	}

}
public enum Sounds{
	TURNPAGE,STAR,SCRIBBLE
}

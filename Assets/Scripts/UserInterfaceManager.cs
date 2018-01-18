using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour {

	[Header("Objects References")]
	public Text time;
	public List<Button> colorButtons;
	public List<Button> brushButtons;

	[Header("Selection")]
	[SerializeField] private int colorButtonIndex;
	[SerializeField] private int brushButtonIndex;

    private PaintManager paintManager;

	private void Start(){
		paintManager = GameObject.Find("PaintManager").GetComponent<PaintManager>();
		ResetSelection();
        SelectBrush(2);
    }
	public void SelectColor(int index){
		colorButtons[colorButtonIndex].transform.GetChild(0).gameObject.SetActive(false);
		colorButtonIndex = index;
		colorButtons[colorButtonIndex].transform.GetChild(0).gameObject.SetActive(true);

		paintManager.SelectColor(index);
	}
	public void SelectBrush(int index){
		if(index > 2) index = 0;
		if(index < 0) index = 2;
		print(index);
		brushButtons[brushButtonIndex].transform.GetChild(0).gameObject.SetActive(false);
		brushButtonIndex = index;
		brushButtons[brushButtonIndex].transform.GetChild(0).gameObject.SetActive(true);

		paintManager.SelectBrush(index);
	}
	private void ResetSelection(){
		foreach(Button b in colorButtons){
			b.transform.GetChild(0).gameObject.SetActive(false);
		}
		foreach(Button b in brushButtons){
			b.transform.GetChild(0).gameObject.SetActive(false);
		}
		SelectColor(0);
		SelectBrush(0);
	}
    public int GetCurrentBrush(){
		return brushButtonIndex;
	}
}

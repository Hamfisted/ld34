using UnityEngine;
using System.Collections;

public class SandwichInventory : MonoBehaviour {

	public enum SandwichPart
	{
		Bread = 0,
		Cheese,
		Meat,
		Lettuce,
		NumParts
	};

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool AcceptSandwichPart(SandwichPart type) {
		if (type >= SandwichPart.NumParts) {
			return(false);
		}
		int max = 1;
		if (type == SandwichPart.Bread) {
			max = 2;
		}
		if (mSandwichParts [(int)type] >= max) {
			return(false);
		}
		mSandwichParts [(int)type] += 1;
		if (HasEnoughPartsForSandwich ()) {
			MakeSandwich ();
		}
		return(true);
	}

	bool HasEnoughPartsForSandwich() {
		if ((mSandwichParts [(int)SandwichPart.Bread] >= 2) &&
			(mSandwichParts [(int)SandwichPart.Cheese] >= 1) &&
			(mSandwichParts [(int)SandwichPart.Meat] >= 1) &&
			(mSandwichParts [(int)SandwichPart.Lettuce] >= 1)) {
			return(true);
		}
		return(false);
	}

	void MakeSandwich() {
		mSandwichParts.Initialize ();
		mNumSandwiches += 1;
	}

	bool UseSandwich() {
		if (mNumSandwiches > 0) {
			mNumSandwiches -= 1;
			return(true);
		}
		return(false);
	}

	int[] mSandwichParts = new int[(int)SandwichPart.NumParts];
	int mNumSandwiches = 0;
}

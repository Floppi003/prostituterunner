using System;
using UnityEngine;

public class Woman {

	public float womanMultiplier = 0.65f;

	public Woman() {
		
	}

	public Woman (float womanMultiplier) {
		this.womanMultiplier = womanMultiplier;
	}
}

public class ProstituteWoman: Woman {
	// public variables
	public int moneyNeeded = 250;

	// private variables
	private int moneyNeededLeft;

	public ProstituteWoman(float womanMultiplier) : base(womanMultiplier) {
		
	}
}

public class FatWoman: Woman {
	// public variables
	public int loveNeeded = 7; // how often you need to press the "make love" button to get rid of her

	// private variables
	private int loveNeededLeft;

	public FatWoman(float womanMultiplier) : base(womanMultiplier) {
		
	}
}

public class GrandmaWoman: Woman {
	// public variables
	public float timeNeeded = 2.5f;

	// private variables
	private float timeNeededLeft;

	public GrandmaWoman(float womanMultiplier) : base (womanMultiplier) {
		
	}
}

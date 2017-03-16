using System;
using UnityEngine;

public class Woman {
	public Woman () {
		
	}
}

public class ProstituteWoman: Woman {
	// public variables
	public int moneyNeeded = 250;

	// private variables
	private int moneyNeededLeft;
}

public class FatWoman: Woman {
	// public variables
	public int loveNeeded = 7; // how often you need to press the "make love" button to get rid of her

	// private variables
	private int loveNeededLeft;
}

public class GrandmaWoman: Woman {
	// public variables
	public float timeNeeded = 2.5f;

	// private variables
	private float timeNeededLeft;
}
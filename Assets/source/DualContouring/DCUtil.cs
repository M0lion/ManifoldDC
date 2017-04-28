using UnityEngine;
using System.Collections;

public static class DCUtil
{
	public static Vector3[] offsets = {
		new Vector3(0,0,0),
		new Vector3(0,0,1),
		new Vector3(1,0,1),
		new Vector3(1,0,0),
		new Vector3(0,1,0),
		new Vector3(0,1,1),
		new Vector3(1,1,1),
		new Vector3(1,1,0)
	};

	public static int[] faceProcs = {
		0,1,
		1,2,
		2,3,
		3,0,
		4,5,
		5,6,
		6,7,
		7,0,
		0,4,
		1,5,
		2,6,
		3,7
	};

	static int[] faForward = {
		1,0,
		2,3,
		5,4,
		6,7
	};

	static int[] faBack = {
		0,1,
		3,2,
		4,5,
		7,6
	};

	static int[] faRight = {
		3,0,
		2,1,
		7,4,
		6,5
	};

	static int[] faLeft = {
		0,3,
		1,2,
		4,7,
		5,6
	};

	static int[] faUp = {
		4,0,
		5,1,
		6,2,
		7,3
	};

	static int[] faDown = {
		0,4
		1,5,
		2,6,
		3,7
	};

	public static int[] faceAdjacents(Vector3 diff)
	{
		diff.Normalize ();
		switch(diff)
		{
		case  Vector3.forward:
			return faForward;
		case Vector3.back:
			return faBack;
		case Vector3.right:
			return faRight;
		case Vector3.left:
			return faLeft;
		case Vector3.up:
			return faUp;
		case Vector3.down:
			return faDown;
		}
	}

	public static int[] edgeProcs = {
		0,1,2,3,
		4,5,6,7,
		0,1,5,4,
		3,2,6,7,
		0,4,7,3,
		1,5,6,3
	};
}

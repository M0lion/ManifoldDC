	using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets
{
	public class DCOCtree
	{
		DCEdge[] edges;
		DCOCtree[] children;

		float[] samples;
		bool[] sampleMap;

		Vector3 origin;
		float size;

		float iso;

		bool homo = false;

		List<Vector3> vertices;
		List<int> indices;

		void init()
		{
			edges = new DCEdge[12];
			children = new DCOCtree[8];
			samples = new float[8];
		}

		public DCOCtree(DerivableScalarField input, float iso, float size = 1.0f, Vector3 start = Vector3.zero)
		{
			this.size = size;
			this.origin = start;
			this.iso = iso;

			init();
		}

		bool isHomo()
		{
			bool b = samples [0];
			for (int i = 1; i < 8; i++) 
			{
				if (sampleMap [i] != b)
					return false;
			}

			return true;
		}

		void sample(DerivableScalarField input)
		{
			for(int i = 0; i < 8; i++)
			{
				samples [i] = input.sample (origin + (size * DCUtil.offsets[i]));
				sampleMap [i] = sample [i] > iso;
			}

			homo = isHomo ();
		}

		void build(DerivableScalarField input)
		{
			sample (input);

			if (isHomo ())
				return;

			for (int i = 0; i < 8; i++) 
			{
				children [i] = new DCOCtree (input, iso, size / 2, origin + (DCUtil.offsets [i] * size / 2));
			}
		}

		public void buildMesh(List<Vector3> vertices, List<int> indices)
		{
			cellProc (this);	
		}

		static void cellProc(DCOCtree q)
		{
			if (homo)
				return;

			for(int i = 0; i < 8; i++)
			{
				cellProc (q.children [i]);
			}

			for (int i = 0; i < 4; i++)
			{
				faceProc (children[i], children[i+1], vertices, indices);
			}
		}

		static void faceProc(DCOCtree q1, DCOCtree q2)
		{
			int[] faceAdjacents = DCUtil.faceAdjacents (q2.origin - q1.origin);
			for(int i = 0; i < 8; i+=2)
			{
				faceProc (q1.children [faceAdjacents [i]], q2.children [faceAdjacents [i + 1]]);
			}
		}

		static void edgeProc(DCOCtree q1, DCOCtree q2, DCOCtree q3, DCOCtree q4)
		{
			
		}
	} 
}
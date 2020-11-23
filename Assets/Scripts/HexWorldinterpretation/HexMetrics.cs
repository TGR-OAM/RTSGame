using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class HexMetrics
    {
        public const float outerRadius = 1f;

        public const float innerRadius = outerRadius * 0.866025404f;

		public static Vector3[] corners = {
			new Vector3(0f, 0f, outerRadius),
			new Vector3(innerRadius, 0f, 0.5f * outerRadius),
			new Vector3(innerRadius, 0f, -0.5f * outerRadius),
			new Vector3(0f, 0f, -outerRadius),
			new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
			new Vector3(-innerRadius, 0f, 0.5f * outerRadius)
		};

		
		public static Vector3 CalcCenterCoordXZFromHexCoordXZ(Vector3 hexCoord,HexGridData UploadedData)
        {
			Vector3 CenterCoord = new Vector3(hexCoord.x * 2 * innerRadius * UploadedData.cellSize + hexCoord.z % 2 * innerRadius * UploadedData.cellSize, 0, hexCoord.z * 1.5f * UploadedData.cellSize * outerRadius);

			return CenterCoord;
        }

		public static Vector3 CalcHexCoordXZFromDefault(Vector3 position, float cellSize)
        {
			position /= cellSize;

			float x = position.x / (HexMetrics.innerRadius * 2f);
			float y = -x;

			float offset = position.z / (HexMetrics.outerRadius * 3f);
			x -= offset;
			y -= offset;

			int iX = Mathf.RoundToInt(x);
			int iY = Mathf.RoundToInt(y);
			int iZ = Mathf.RoundToInt(-x - y);

			if (iX + iY + iZ != 0)
			{
				float dX = Mathf.Abs(x - iX);
				float dY = Mathf.Abs(y - iY);
				float dZ = Mathf.Abs(-x - y - iZ);

				if (dX > dY && dX > dZ)
				{
					iX = -iY - iZ;
				}
				else if (dZ > dY)
				{
					iZ = -iX - iY;
				}
			}

			return new Vector3(iX + iZ/2, 0 ,iZ);
		}

	}
}

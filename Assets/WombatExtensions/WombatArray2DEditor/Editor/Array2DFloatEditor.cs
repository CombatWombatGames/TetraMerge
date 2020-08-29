using UnityEditor;

namespace Array2DEditor
{
    [CustomEditor(typeof(Array2DFloat))]
    public class Array2DFloatEditor : Array2DEditor
    {
        protected override int CellWidth { get { return 32; } }
        protected override int CellHeight { get { return 16; } }

        protected override void SetValue(SerializedProperty cell, int i, int j)
        {
            float[,] previousCells = (target as Array2DFloat).GetCells();

            cell.floatValue = default(float);

            if (i < gridSize.vector2IntValue.y && j < gridSize.vector2IntValue.x)
            {
                cell.floatValue = previousCells[i, j];
            }
        }
    }
}
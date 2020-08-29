using UnityEditor;

namespace Array2DEditor
{
    [CustomEditor(typeof(Array2DInt))]
    public class Array2DIntEditor : Array2DEditor
    {
        protected override int CellWidth { get { return 32; } }
        protected override int CellHeight { get { return 16; } }

        protected override void SetValue(SerializedProperty cell, int i, int j)
        {
            int[,] previousCells = (target as Array2DInt).GetCells();

            cell.intValue = default(int);

            if (i < gridSize.vector2IntValue.y && j < gridSize.vector2IntValue.x)
            {
                cell.intValue = previousCells[i, j];
            }
        }
    }
}
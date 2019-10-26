using UnityEditor;

namespace Array2DEditor
{
    [CustomEditor(typeof(Array2DBool))]
    public class Array2DBoolEditor : Array2DEditor
    {
        protected override int CellWidth { get { return 16; } }
        protected override int CellHeight { get { return 16; } }

        protected override void SetValue(SerializedProperty cell, int i, int j)
        {
            bool[,] previousCells = (target as Array2DBool).GetCells();

            cell.boolValue = default(bool);

            if (i < gridSize.vector2IntValue.y && j < gridSize.vector2IntValue.x)
            {
                cell.boolValue = previousCells[i, j];
            }
        }
    }
}
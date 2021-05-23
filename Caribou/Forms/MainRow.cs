namespace Caribou.Forms
{
    using System;
    using System.Collections.Generic;
    using Eto.Forms;

    /// <summary>The 'main' layout for feature/subfeature selection within the window</summary>
    public static class MainRow
    {
        public static TreeGridView GetLayout(int windowWidth)
        {
            var featureSelect = new TreeGridView()
            {
                Height = 600,
                GridLines = GridLines.Horizontal,
                RowHeight = 30,
            };

            var titleColumn = new GridColumn()
            {
                HeaderText = "Feature",
                DataCell = new TextBoxCell(0),
                Resizable = false,
                Sortable = true,
                AutoSize = true,
            };
            featureSelect.Columns.Add(titleColumn);

            var checkColumn = new GridColumn()
            {
                HeaderText = "Select",
                DataCell = new CheckBoxCell(1),
                Width = 55,
                Resizable = false,
                Sortable = true,
                Editable = true,
            };
            featureSelect.Columns.Add(checkColumn);

            var nodeColumn = new GridColumn()
            {
                HeaderText = "Nodes Count",
                DataCell = new TextBoxCell(2),
                Resizable = false,
                Sortable = true,
                Width = 75,
            };
            featureSelect.Columns.Add(nodeColumn);

            var wayColumn = new GridColumn()
            {
                HeaderText = "Ways Count",
                DataCell = new TextBoxCell(3),
                Resizable = false,
                Sortable = true,
                Width = 75,
            };
            featureSelect.Columns.Add(wayColumn);

            var keyValueColumn = new GridColumn()
            {
                HeaderText = "K:V Format",
                DataCell = new TextBoxCell(4),
                Resizable = false,
                Sortable = true,
                AutoSize = true,
            };
            featureSelect.Columns.Add(keyValueColumn);

            var descriptionColumn = new GridColumn()
            {
                HeaderText = "Description",
                DataCell = new TextBoxCell(5),
                Resizable = false,
                Sortable = true,
                AutoSize = true,
            };
            featureSelect.Columns.Add(descriptionColumn);

            featureSelect.DataStore = SelectionCollection.GetCollection();
            return featureSelect;
        }
    }
}

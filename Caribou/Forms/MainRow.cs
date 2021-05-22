namespace Caribou.Forms
{
    using System;
    using System.Collections.Generic;
    using Eto.Forms;

    /// <summary>The 'main' layout for feature/subfeature selection within the window</summary>
    public static class MainRow
    {
        public static TreeGridView GetLayout(int windowWidth, Dictionary<string, List<string>> testData)
        {
            var featureSelect = new TreeGridView()
            {

            };

            var titleColumn = new GridColumn()
            {
                HeaderText = "Feature",
                DataCell = new TextBoxCell(0),
                Width = 200,
            };
            featureSelect.Columns.Add(titleColumn);

            var checkColumn = new GridColumn()
            {
                HeaderText = "Include",
                DataCell = new CheckBoxCell(2),
                Width = 100
                
            };
            featureSelect.Columns.Add(checkColumn);

            var descriptionColumn = new GridColumn()
            {
                HeaderText = "Description",
                DataCell = new TextBoxCell(1),
                Width = 200,
            };
            featureSelect.Columns.Add(descriptionColumn);

            featureSelect.DataStore = SelectionCollection.GetCollection(testData);
            return featureSelect;
        }
    }
}

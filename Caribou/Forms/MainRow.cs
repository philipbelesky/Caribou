namespace Caribou.Forms
{
    using System;
    using System.Collections.Generic;
    using Caribou.Models;
    using Eto.Forms;

    /// <summary>The 'main' layout for feature/subfeature selection within the window</summary>
    public class MainRow
    {
        private TreeGridItemCollection data;
        public TreeGridView viewForm;

        public MainRow(int tableWidth, int tableHeight)
        {
            this.data = SelectionCollection.GetCollection(false);
            this.viewForm = GetLayout(tableWidth, tableHeight);
        }

        private void ToggleSelectedStatus(TreeGridItem item)
        {
            var newValue = FlipCheckbox(item);
            item.SetValue(1, newValue);

            foreach (TreeGridItem subItem in item.Children)
            {
                subItem.SetValue(1, newValue);
            }
            viewForm.ReloadItem(item); // Also affects children
        }

        private void CellDoubleClickHandler(object sender, GridCellMouseEventArgs e)
        {
            ToggleSelectedStatus(e.Item as TreeGridItem);
        }

        private void CellClickHandler(object sender, GridCellMouseEventArgs e)
        {
            if (e.Column == 1)
            {
                ToggleSelectedStatus(e.Item as TreeGridItem);
            }
            if (e.Column == 5)
            {

            }
            // TODO: toggling if on checkbox
            // TODO: open link if on wiki
        }

        private void HeaderClickHandler(object sender, EventArgs e)
        {
            // TODO: sorting
        }

        private string FlipCheckbox(TreeGridItem item) // to string, flip, and back again
        {
            var currentValue = item.GetValue(1);
            bool currentBool;
            bool.TryParse(currentValue as string, out currentBool);
            bool newBool = !currentBool;
            return newBool.ToString();
        }

        private TreeGridView GetLayout(int tableWidth, int tableHeight)
        {
            var featureSelect = new TreeGridView()
            {
                Height = tableHeight,
                GridLines = GridLines.Horizontal,
                AllowColumnReordering = true,
                RowHeight = 30,
            };
            featureSelect.CellDoubleClick += this.CellDoubleClickHandler;
            featureSelect.CellClick += this.CellClickHandler;
            featureSelect.ColumnHeaderClick += this.HeaderClickHandler;

            var titleColumn = new GridColumn()
            {
                HeaderText = "Feature",
                DataCell = new TextBoxCell(0)
                {
                    //Binding = Binding.Property<OSMSelectableFeature, string>(r => r.Name)
                },
                Resizable = false,
                Sortable = true,
                AutoSize = true,
            };
            featureSelect.Columns.Add(titleColumn);

            var checkColumn = new GridColumn()
            {
                HeaderText = "Select",
                DataCell = new CheckBoxCell(1)
                {
                    //Binding = Binding.Property<OSMSelectableFeature, bool>(r => r.IsSelected)
                },
                Width = 55,
                Resizable = false,
                Sortable = true,
                // Editable = true,
            };
            featureSelect.Columns.Add(checkColumn);

            var nodeColumn = new GridColumn()
            {
                HeaderText = "Nodes Count",
                DataCell = new TextBoxCell(2),
                Resizable = false,
                Sortable = true,
                Width = 85,
            };
            featureSelect.Columns.Add(nodeColumn);

            var wayColumn = new GridColumn()
            {
                HeaderText = "Ways Count",
                DataCell = new TextBoxCell(3),
                Resizable = false,
                Sortable = true,
                Width = 85,
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

            var linkColumn = new GridColumn()
            {
                HeaderText = "Wiki Info",
                DataCell = new TextBoxCell(5),
                Resizable = false,
                Sortable = true,
                AutoSize = true,
            };
            featureSelect.Columns.Add(linkColumn);

            var descriptionColumn = new GridColumn()
            {
                HeaderText = "Description",
                DataCell = new TextBoxCell(6),
                Resizable = false,
                Sortable = true,
                AutoSize = true,
            };
            featureSelect.Columns.Add(descriptionColumn);

            featureSelect.DataStore = this.data;
            return featureSelect;
        }
    }
}

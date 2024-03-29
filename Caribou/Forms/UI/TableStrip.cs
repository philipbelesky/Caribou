﻿namespace Caribou.Forms
{
    using System;
    using Caribou.Forms.Models;
    using Eto.Forms;
    using Eto.Drawing;

    /// <summary>The 'main' layout for feature/subfeature selection within the window</summary>
    public class TableStrip
    {
        public const string TagTypeLabel = "Type";
        private const string SelectTagLabel = "Select";
        private const string TagKVLabel = "Key=Value Format";
        private const string TagWikiLabel = "Wiki Link";

        public TreeGridView viewForm;

        public TableStrip(TreeGridItemCollection selectionState)
        {
            this.viewForm = GetLayout(selectionState);
        }

        public TreeGridItemCollection GetCurrentData()
        {
            return this.viewForm.DataStore as TreeGridItemCollection;
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

        private void OpenWikiLink(TreeGridItem item)
        {
            var url = item.Values[7] as string;
            System.Diagnostics.Process.Start(url);
        }

        private void CellDoubleClickHandler(object sender, GridCellMouseEventArgs e)
        {
            ToggleSelectedStatus(e.Item as TreeGridItem);
        }

        private void CellClickHandler(object sender, GridCellMouseEventArgs e)
        {
            if (e.Column == 1)
                ToggleSelectedStatus(e.Item as TreeGridItem);
            else if (e.Column == 5)
                OpenWikiLink(e.Item as TreeGridItem);
        }

        private void HeaderClickHandler(object sender, EventArgs e)
        {
            // TODO: implement sorting (and change the Sortable properties on GridColumn())
        }

        private string FlipCheckbox(TreeGridItem item) // to string, flip, and back again
        {
            var currentValue = item.GetValue(1);
            bool.TryParse(currentValue as string, out var currentBool);
            bool newBool = !currentBool;
            return newBool.ToString();
        }

        // Styling - set tree grid items to be bold if they or any children are selected
        void _OnFormatCell(object sender, GridCellFormatEventArgs e)
        {
            switch (e.Column.HeaderText)
            {
                case TagTypeLabel:
                    var checkItem = e.Item as OSMTreeGridItem;
                    var isSelectedOrHasChildSelected = false;

                    if (checkItem.IsSelected())
                        isSelectedOrHasChildSelected = true;
                    else if (checkItem.Children.Count > 0)
                        foreach (OSMTreeGridItem child in checkItem.Children)
                            if (child.IsSelected())
                            {
                                isSelectedOrHasChildSelected = true;
                                break;
                            }

                    if (isSelectedOrHasChildSelected)
                        e.Font = new Font(SystemFont.Bold);
                    else
                        e.Font = new Font(SystemFont.Default);
                    break;

                case TagWikiLabel:
                    e.ForegroundColor = new ColorHSL(230f, 1.0f, 0.5f);
                    break;

                case TagKVLabel:
                    e.ForegroundColor = new ColorHSL(0f, 0f, 0.33f);
                    e.Font = new Font(FontFamilies.Sans, 8.0f, FontStyle.None);
                    break;

                case "Description":
                    e.Font = new Font(FontFamilies.Sans, 8.0f, FontStyle.Italic);
                    break;
            }
        }

        private TreeGridView GetLayout(TreeGridItemCollection selectableItems)
        {
            var isMacOS = Eto.Platform.Detect.IsMac;

            var featureSelect = new TreeGridView()
            {
                GridLines = GridLines.Horizontal,
                AllowColumnReordering = true,
                RowHeight = 30,
            };
            featureSelect.CellDoubleClick += this.CellDoubleClickHandler;
            featureSelect.CellClick += this.CellClickHandler;
            featureSelect.ColumnHeaderClick += this.HeaderClickHandler;
            featureSelect.CellFormatting += _OnFormatCell;

            var titleColumn = new GridColumn()
            {
                HeaderText = TagTypeLabel,
                DataCell = new TextBoxCell(0),
                Resizable = false,
                Sortable = false,
                AutoSize = false,
                Width = 220, // Don't autosize; hides the arrow buttons on macOS
            };            
            featureSelect.Columns.Add(titleColumn);

            var checkColumn = new GridColumn()
            {
                HeaderText = SelectTagLabel,
                DataCell = new CheckBoxCell(1),
                Width = isMacOS ? 53 : 44, // 53 minimum macOS without ellipses 
                Resizable = false,
                Sortable = false,
            };
            featureSelect.Columns.Add(checkColumn);

            var nodeColumn = new GridColumn()
            {
                HeaderText = "Nodes Count",
                DataCell = new TextBoxCell(2),
                Resizable = false,
                Sortable = false,
                Width = isMacOS ? 98 : 88,
            };
            featureSelect.Columns.Add(nodeColumn);

            var wayColumn = new GridColumn()
            {
                HeaderText = "Ways Count",
                DataCell = new TextBoxCell(3),
                Resizable = false,
                Sortable = false,
                Width = isMacOS ? 98 : 88,
            };
            featureSelect.Columns.Add(wayColumn);

            var keyValueColumn = new GridColumn()
            {
                HeaderText = TagKVLabel,
                DataCell = new TextBoxCell(4),
                Resizable = false,
                Sortable = false,
                AutoSize = false,
                Width = isMacOS ? 130 : 130,
            };
            featureSelect.Columns.Add(keyValueColumn);

            var linkColumn = new GridColumn()
            {
                HeaderText = TagWikiLabel,
                DataCell = new TextBoxCell(5),
                Resizable = false,
                Sortable = false,
                Width = isMacOS ? 67 : 62, // 67 minimum macOS without ellipses 
            };
            featureSelect.Columns.Add(linkColumn);

            var descriptionColumn = new GridColumn()
            {
                HeaderText = "Description",
                DataCell = new TextBoxCell(6),
                Resizable = false,
                Sortable = false,
            };
            featureSelect.Columns.Add(descriptionColumn);

            featureSelect.DataStore = selectableItems;
            return featureSelect;
        }
    }
}

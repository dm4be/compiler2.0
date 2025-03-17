using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class AnalysisForm : Form
    {
        private DataGridView tokensTable;

        public AnalysisForm(List<Token> tokens)
        {
            InitializeComponent1();
            PopulateTable(tokens);
        }

        private void InitializeComponent1()
        {
            this.Text = "Результаты анализа";
            this.Width = 600;
            this.Height = 400;

            tokensTable = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true };
            tokensTable.Columns.Add("Code", "Код");
            tokensTable.Columns.Add("Type", "Тип");
            tokensTable.Columns.Add("Lexeme", "Лексема");
            tokensTable.Columns.Add("Position", "Позиция");

            this.Controls.Add(tokensTable);
        }

        private void PopulateTable(List<Token> tokens)
        {
            tokensTable.Rows.Clear();
            foreach (var token in tokens)
            {
                tokensTable.Rows.Add(token.Code, token.Type, token.Lexeme, token.Position);
            }
        }
    }
}

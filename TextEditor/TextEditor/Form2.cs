using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class MainForm : Form
    {
        private MenuStrip menuStrip;
        private ToolStrip toolStrip;
        private SplitContainer splitContainer;
        private RichTextBox inputArea;
        private RichTextBox outputArea;
        private FileManager fileManager;
        private EditManager editManager;
        private HelpManager helpManager;

        public MainForm()
        {
            InitializeComponent();
            fileManager = new FileManager();
            editManager = new EditManager();
            helpManager = new HelpManager();
            this.FormClosing += MainForm_FormClosing;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!fileManager.CheckUnsavedChanges(inputArea))
            {
                e.Cancel = true;
            }
        }

        private string GetLineNumbers(RichTextBox textBox)
        {
            int lineCount = textBox.Lines.Length;
            string lineNumbers = "";
            for (int i = 1; i <= lineCount; i++)
            {
                lineNumbers += i + "\n";
            }
            return lineNumbers;
        }

        private void InitializeComponent()
        {
            this.Text = "Compiler";
            this.Width = 800;
            this.Height = 600;

            menuStrip = new MenuStrip();
            menuStrip.Dock = DockStyle.Top;
            menuStrip.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("Файл", null,
                    new ToolStripMenuItem("Создать", null, (s, e) => fileManager.NewFile(inputArea)),
                    new ToolStripMenuItem("Открыть", null, (s, e) => fileManager.OpenFile(inputArea)),
                    new ToolStripMenuItem("Сохранить", null, (s, e) => fileManager.SaveFile(inputArea)),
                    new ToolStripMenuItem("Сохранить как", null, (s, e) => fileManager.SaveFileAs(inputArea)),
                    new ToolStripMenuItem("Выход", null, (s, e) => fileManager.ExitApplication(inputArea))
                ),
                new ToolStripMenuItem("Правка", null,
                    new ToolStripMenuItem("Отменить", null, (s, e) => editManager.Undo(inputArea)),
                    new ToolStripMenuItem("Повторить", null, (s, e) => editManager.Redo(inputArea)),
                    new ToolStripMenuItem("Вырезать", null, (s, e) => editManager.Cut(inputArea)),
                    new ToolStripMenuItem("Копировать", null, (s, e) => editManager.Copy(inputArea)),
                    new ToolStripMenuItem("Вставить", null, (s, e) => editManager.Paste(inputArea)),
                    new ToolStripMenuItem("Удалить", null, (s, e) => editManager.Delete(inputArea)),
                    new ToolStripMenuItem("Выделить все", null, (s, e) => editManager.SelectAll(inputArea))
                ),
                new ToolStripMenuItem("Текст"),
                new ToolStripMenuItem("Пуск"),
                new ToolStripMenuItem("Справка", null,
                    new ToolStripMenuItem("Вызов справки", null, (s, e) => helpManager.ShowHelp()),
                    new ToolStripMenuItem("О программе", null, (s, e) => helpManager.ShowAbout())
                )
            });

            toolStrip = new ToolStrip();
            toolStrip.Dock = DockStyle.Top;

            AddToolStripButton("Создать", "Resources/new.png", (s, e) => fileManager.NewFile(inputArea));
            AddToolStripButton("Открыть", "Resources/open.png", (s, e) => fileManager.OpenFile(inputArea));
            AddToolStripButton("Сохранить", "Resources/save.png", (s, e) => fileManager.SaveFile(inputArea));
            toolStrip.Items.Add(new ToolStripSeparator());
            AddToolStripButton("Отменить", "Resources/undo.png", (s, e) => editManager.Undo(inputArea));
            AddToolStripButton("Повторить", "Resources/redo.png", (s, e) => editManager.Redo(inputArea));
            toolStrip.Items.Add(new ToolStripSeparator());
            AddToolStripButton("Копировать", "Resources/copy.png", (s, e) => editManager.Copy(inputArea));
            AddToolStripButton("Вырезать", "Resources/cut.png", (s, e) => editManager.Cut(inputArea));
            AddToolStripButton("Вставить", "Resources/paste.png", (s, e) => editManager.Paste(inputArea));
            AddToolStripButton("Анализ", "Resources/analyze.png", (s, e) => RunLexer());


            splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal
            };

            inputArea = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = false };
            outputArea = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true };

            var lineNumbers = new RichTextBox
            {
                Padding = new Padding(0, 5, 0, 0),
                Dock = DockStyle.Left,
                Width = 40,
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.None,
                BackColor = SystemColors.ControlLight,
                BorderStyle = BorderStyle.None,
                Enabled = false
            };
            var sharedFont = new Font("Consolas", 10);
            inputArea.Font = sharedFont;
            lineNumbers.Font = sharedFont;
            lineNumbers.SelectionAlignment = HorizontalAlignment.Right;

            inputArea.TextChanged += (s, e) =>
            {
                fileManager.MarkTextChanged();
                lineNumbers.Text = GetLineNumbers(inputArea);
            };
            inputArea.VScroll += (s, e) =>
            {
                int d = inputArea.GetPositionFromCharIndex(0).Y % (int)inputArea.Font.GetHeight();
                lineNumbers.Location = new Point(0, d);
                lineNumbers.Text = GetLineNumbers(inputArea);
            };

            var inputPanel = new Panel { Dock = DockStyle.Fill };
            inputPanel.Controls.Add(inputArea);
            inputPanel.Controls.Add(lineNumbers);
            splitContainer.Panel1.Controls.Add(inputPanel);
            splitContainer.Panel2.Controls.Add(outputArea);

            this.Controls.Add(splitContainer);
            this.Controls.Add(toolStrip);
            this.Controls.Add(menuStrip);
        }

        private void AddToolStripButton(string text, string imagePath, EventHandler onClick)
        {
            var button = new ToolStripButton()
            {
                Text = text,
                Image = File.Exists(imagePath) ? Image.FromFile(imagePath) : null,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                ToolTipText = text
            };
            button.Click += onClick;
            toolStrip.Items.Add(button);
        }
        private void RunLexer()
        {
            Lexer lexer = new Lexer();
            List<Token> tokens = lexer.Analyze(inputArea.Text);

            AnalysisForm analysisForm = new AnalysisForm(tokens);
            analysisForm.Show();
        }
    }
}

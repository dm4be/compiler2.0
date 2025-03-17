using System.Windows.Forms;

namespace TextEditor
{
    public class HelpManager
    {
        public void ShowHelp()
        {
            MessageBox.Show("Это справка по работе с текстовым редактором. Здесь будет информация о функциях и возможностях программы.", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowAbout()
        {
            MessageBox.Show("Текстовый редактор версии 1.0\nРазработан в 2025 году великим и могучим Степаном", "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

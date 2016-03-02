using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compi2_Practica1_1s2016
{

    public partial class EditorTexto : Form
    {
        public String cad1, cad2, cad;
        public int contador = 0;

        private int TabCount = 0;

        public EditorTexto()
        {
            InitializeComponent();
        }

        #region Methods

        #region Tabs

        private void AddTab()
        {

            RichTextBox Body = new RichTextBox();

            Body.Name = "Body";
            Body.Dock = DockStyle.Fill;
            Body.ContextMenuStrip = contextMenuStrip1;

            TabPage NewPage = new TabPage();
            TabCount += 1;

            string DocumentText = "Document " + TabCount;
            NewPage.Name = DocumentText;
            NewPage.Text = DocumentText;
            NewPage.Controls.Add(Body);

            tabControl1.TabPages.Add(NewPage);

        }

        private void RemoveTab()
        {
            if (tabControl1.TabPages.Count != 1)
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
            else
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                AddTab();
            }
        }

        private void RemoveAllTabs()
        {
            foreach (TabPage Page in tabControl1.TabPages)
            {
                tabControl1.TabPages.Remove(Page);
            }

            AddTab();
        }

        private void RemoveAllTabsButThis()
        {
            foreach (TabPage Page in tabControl1.TabPages)
            {
                if (Page.Name != tabControl1.SelectedTab.Name)
                {
                    tabControl1.TabPages.Remove(Page);
                }
            }
        }

        #endregion

        #region SaveAndOpen

        private void Save()
        {
            saveFileDialog1.FileName = tabControl1.SelectedTab.Name;
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog1.Filter = "DX|.dx";
            saveFileDialog1.Title = "Save";

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (saveFileDialog1.FileName.Length > 0)
                {
                    GetCurrentDocument.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                }
            }
        }

        private void SaveAs()
        {
            saveFileDialog1.FileName = tabControl1.SelectedTab.Name;
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog1.Filter = "DX|*.dx|All Files|*.*";
            saveFileDialog1.Title = "Save As";

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (saveFileDialog1.FileName.Length > 0)
                {
                    GetCurrentDocument.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                }
            }
        }

        private void Open()
        {
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog1.Filter = "DX|*.dx|All Files|*.*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (openFileDialog1.FileName.Length > 9)
                {
                    
                    GetCurrentDocument.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.RichText);
                }
            }

        }

        #endregion

        #region TextFunctions

        private void Undo()
        {
            GetCurrentDocument.Undo();
        }

        private void Redo()
        {
            GetCurrentDocument.Redo();
        }

        private void Cut()
        {
            GetCurrentDocument.Cut();
        }

        private void Copy()
        {
            GetCurrentDocument.Copy();
        }

        private void Paste()
        {
            GetCurrentDocument.Paste();
        }

        private void SelectAll()
        {
            GetCurrentDocument.SelectAll();
        }

        #endregion

        #region General

        private void GetFontCollection()
        {
            InstalledFontCollection InsFonts = new InstalledFontCollection();

            foreach (FontFamily item in InsFonts.Families)
            {
                toolStripComboBox1.Items.Add(item.Name);
            }
            toolStripComboBox1.SelectedIndex = 0;
        }

        private void PopulateFontSizes()
        {
            for (int i = 1; i <= 75; i++)
            {
                toolStripComboBox2.Items.Add(i);
            }

            toolStripComboBox2.SelectedIndex = 17;
        }
        #endregion


        #endregion

        #region Properties

        private RichTextBox GetCurrentDocument
        {
            get { return (RichTextBox)tabControl1.SelectedTab.Controls["Body"]; }
        }

        #endregion

        #region EventBindings


        private void AdvancedTextEditor_Load(object sender, EventArgs e)
        {
            AddTab();
            GetFontCollection();
            PopulateFontSizes();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (GetCurrentDocument.Text.Length > 0)
            {
                toolStripStatusLabel1.Text = GetCurrentDocument.Text.Length.ToString();
            }
        }

        #region Menu

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTab();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAll();
        }



        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveTab();
        }

        #endregion

        #region Toolbar


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Font BoldFont = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, FontStyle.Bold);
            Font RegularFont = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (GetCurrentDocument.SelectionFont.Bold)
            {
                GetCurrentDocument.SelectionFont = RegularFont;
            }
            else
            {
                GetCurrentDocument.SelectionFont = BoldFont;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Font ItalicFont = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, FontStyle.Italic);
            Font RegularFont = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (GetCurrentDocument.SelectionFont.Italic)
            {
                GetCurrentDocument.SelectionFont = RegularFont;
            }
            else
            {
                GetCurrentDocument.SelectionFont = ItalicFont;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Font UnderlineFont = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, FontStyle.Underline);
            Font RegularFont = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (GetCurrentDocument.SelectionFont.Underline)
            {
                GetCurrentDocument.SelectionFont = RegularFont;
            }
            else
            {
                GetCurrentDocument.SelectionFont = UnderlineFont;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Font Strikeout = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, FontStyle.Strikeout);
            Font RegularFont = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (GetCurrentDocument.SelectionFont.Strikeout)
            {
                GetCurrentDocument.SelectionFont = RegularFont;
            }
            else
            {
                GetCurrentDocument.SelectionFont = Strikeout;
            }
        }


        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectedText = GetCurrentDocument.SelectedText.ToUpper();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectedText = GetCurrentDocument.SelectedText.ToLower();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {

            float NewFontSize = GetCurrentDocument.SelectionFont.SizeInPoints + 2;

            Font NewSize = new Font(GetCurrentDocument.SelectionFont.Name, NewFontSize, GetCurrentDocument.SelectionFont.Style);

            GetCurrentDocument.SelectionFont = NewSize;
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            float NewFontSize = GetCurrentDocument.SelectionFont.SizeInPoints - 2;

            Font NewSize = new Font(GetCurrentDocument.SelectionFont.Name, NewFontSize, GetCurrentDocument.SelectionFont.Style);

            GetCurrentDocument.SelectionFont = NewSize;
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GetCurrentDocument.SelectionColor = colorDialog1.Color;
            }
        }

        private void HighlighGreen_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectionBackColor = Color.LightGreen;
        }

        private void HighlighOrange_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectionBackColor = Color.Orange;
        }

        private void HighlighYellow_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectionBackColor = Color.Yellow;
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Font NewFont = new Font(toolStripComboBox1.SelectedItem.ToString(), GetCurrentDocument.SelectionFont.Size, GetCurrentDocument.SelectionFont.Style);

            GetCurrentDocument.SelectionFont = NewFont;
        }

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            float NewSize;

            float.TryParse(toolStripComboBox2.SelectedItem.ToString(), out NewSize);

            Font NewFont = new Font(GetCurrentDocument.SelectionFont.Name, NewSize, GetCurrentDocument.SelectionFont.Style);

            GetCurrentDocument.SelectionFont = NewFont;
        }

        #endregion

        #region LeftToolStrip

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            AddTab();
        }

        private void RemoveTabToolStripButton_Click(object sender, EventArgs e)
        {
            RemoveTab();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            int size = -1;
            openFileDialog1.Filter = "DX Files|*.dx|All Files|*.*";
            DialogResult result = openFileDialog1.ShowDialog(); 
            if (result == DialogResult.OK) 
            {
                string file = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(file);
                    size = text.Length;
                    GetCurrentDocument.Text = text;
                }
                catch (IOException)
                {
                }
            }
            
            
            //Open();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "New.txt";
            save.Filter = "DX File | *.dx";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(save.OpenFile());
                writer.Write(GetCurrentDocument.Text);
                writer.Dispose();
                writer.Close();
            }
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            Paste();
        }

        #endregion

        #region ContextMenu

        private void undoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void redoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveAllTabs();
        }

        private void closeAllButThisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveAllTabsButThis();
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            Nodo.todosLosNodos = new List<Nodo>();
            Token.todosTokens = new List<Token>();

            Conjunto.contador = 0;
            Conjunto.todosConjuntosCerraduras.Clear();
            Nodo.resetearCounter();
            Analizador a = new Analizador();
            Gramatica gramatica = new Gramatica();
            if (a.isValid(GetCurrentDocument.Text, gramatica))
            {
                cad2 = "";
                contador = 0;
                dispTree(a.getRoot(GetCurrentDocument.Text, gramatica), 0, "");
                AccionesGramatica actions = new AccionesGramatica();
                actions.hacerAccion(a.getRoot(GetCurrentDocument.Text, gramatica));
                graph("test", cad2);
                MessageBox.Show("Exito");
            }
        }

        #endregion

        public void dispTree(ParseTreeNode node, int level, String strPadre)
        {
            contador++;
            if (!strPadre.Equals(""))
            {
                cad2 = cad2 + "\"" + strPadre + "\" -> \"" + node.ToString() + "_" + contador + "\"";
            }

            strPadre = node.ToString() + "_" + contador;
            foreach (ParseTreeNode child in node.ChildNodes)
            {
                dispTree(child, level + 1, strPadre);
            }


        }

        void generar(string fileName)
        {
            string strCmdText;
            strCmdText = "dot -Tpng C:\\Users\\Mac\\Desktop\\Graphs\\" + fileName + ".txt -o C:\\Users\\Mac\\Desktop\\Graphs\\" + fileName + ".png";
            var proc1 = new ProcessStartInfo();
            proc1.UseShellExecute = true;
            proc1.WorkingDirectory = @"C:\Windows\System32";
            proc1.FileName = @"C:\Windows\System32\cmd.exe";
            proc1.Verb = "runas";
            proc1.Arguments = "/c " + strCmdText;
            proc1.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(proc1);
        }

        public void graph(String fileName, String codigo1)
        {
            String path;
            path = "C:\\Users\\Mac\\Desktop\\Graphs\\" + fileName + ".txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream fs = File.Create(path))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes("digraph G{\n node [shape = circle]; " + codigo1 + "}");
                fs.Write(info, 0, info.Length);
            }
            generar(fileName);
        }

        private void toolStripComboBox2_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

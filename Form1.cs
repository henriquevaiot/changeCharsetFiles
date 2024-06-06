using System.Text;
using System.Windows.Forms;
using UtfUnknown;

namespace mudarCharset
{
    public partial class Form1 : Form
    {

        private FolderBrowserDialog folderBrowserDialog1;
        static List<string> diretorios;
        

        public Form1()
        {
            InitializeComponent();
        }



        static void DirSearch(string sDir)
        {
            diretorios.Add(sDir);

            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    /*foreach (string f in Directory.GetFiles(d))
                    {
                       // Console.WriteLine(f);
                    }*/
                    DirSearch(d);


                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1 = new FolderBrowserDialog();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                diretorios = new List<string>();

                
                foreach (var f in new DirectoryInfo(folderBrowserDialog1.SelectedPath).GetFiles("*.cs", SearchOption.AllDirectories))
                {
                    Encoding encode;
                    string charset = "";
                    var result = CharsetDetector.DetectFromFile(f.FullName);
                    if (result.Detected.Encoding != null)
                        charset = result.Detected.Encoding.WebName;
                    else
                    {
                        if (result.Detected.StatusLog.IndexOf("windows-1252") > -1)
                            charset = "windows-1252";
                        else
                            charset = "nao sei";
                    }

                    string s = File.ReadAllText(f.FullName, Encoding.GetEncoding(charset));
            
                    if (charset != "nao sei" && charset != "utf-8" && charset != "ibm852") { 

                    Encoding de = Encoding.GetEncoding(charset);
                    Encoding para = Encoding.UTF8;
                    byte[] wind1252Bytes = de.GetBytes(s);
                    byte[] utf8Bytes = Encoding.Convert(de, para, wind1252Bytes);
                    string utf8String = Encoding.UTF8.GetString(utf8Bytes);
                   

                    File.WriteAllText(f.FullName, utf8String, Encoding.UTF8);
                   // Console.WriteLine(s);
                    richTextBox1.AppendText(f.FullName + " >> " + charset + "\r\n");

                    }
                    else
                    {

                        richTextBox1.AppendText(f.FullName + " ignorado >> {charset} \r\n");
                    }




                }

            }


        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
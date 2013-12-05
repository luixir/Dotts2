using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace GameTest1
{
    [Serializable]
    public partial class HighScore : Form
    {
        public List<String> name = new List<String>();
        public List<String> score = new List<String>();

        public List<NameAndScore> listscore = new List<NameAndScore>();
     
        private Form1 _form;
        private string filePath = @"..\..\Score.xml";
        //ScoreData _data = new ScoreData();

        public HighScore(Form1 form)
        {
            InitializeComponent();
            LoadScore();
            SortScoreList();
            _form = form;
        }

        // XPATH
        public void AddScore()
        {

        }

        public void LoadScore()
        {
            XPathDocument doc = new XPathDocument(filePath);
            XPathNavigator nav = doc.CreateNavigator();

            // Compile a standard XPath expression
            XPathExpression nameExpr;
            nameExpr = nav.Compile("/HighScores/Score/Name");
            XPathNodeIterator nameIterator = nav.Select(nameExpr);

            XPathExpression scoreExpr;
            scoreExpr = nav.Compile("/HighScores/Score/UserScore");
            XPathNodeIterator scoreIterator = nav.Select(scoreExpr);

            try
            {
                while (scoreIterator.MoveNext())
                {
                    XPathNavigator scoreNav = scoreIterator.Current.Clone();
                    while (nameIterator.MoveNext())
                    {
                        XPathNavigator nameNav = nameIterator.Current.Clone();
                        listscore.Add(new NameAndScore()
                            {
                                PlayerName = nameNav.Value, 
                                Score = TimeSpan.Parse(scoreNav.Value)
                            });
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void AppendScore(TimeSpan time, string userName)
        {
            XmlDocument doc = new XmlDocument();
            // Load XML file from filepath
            doc.Load(this.filePath);
            //TimeSpan time = TimeSpan.Zero;
            // Create new XML element
            XmlNode score = doc.CreateElement("Score");
            XmlNode name = doc.CreateElement("Name");
            XmlNode timescore = doc.CreateElement("UserScore");

            // Add name and timescore element as score's child
            score.AppendChild(name);
            score.AppendChild(timescore);

            // Assign value to name and score
            name.InnerText = userName;
            timescore.InnerText = time.ToString();

            // Document append score as child
            doc.DocumentElement.AppendChild(score);

            // Save the xml file to the filepath
            doc.Save(this.filePath);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                listBox1.Items.Add("Name: " + listscore[i].PlayerName + " Score: " + listscore[i].Score);
            }
        }

        public void SortScoreList()
        {
            listscore.Sort((s1, s2) => -1 * s1.Score.CompareTo(s2.Score));
        }

        public List<NameAndScore> GetScoreList()
        {
            return listscore;
        }
    }
}

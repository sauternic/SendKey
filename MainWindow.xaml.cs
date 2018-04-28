using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Threading;
namespace SendKey
{
    //in Projekt Datei hinzugefügt um neue Implementierung der SendKey Methode zu bekommen.
    //<appSettings>
    //  <add key="SendKeys" value="SendInput"/>
    //</appSettings>


    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string str;
        private int i;
        private int min;
        private int max;
        private bool bolCheckbox1;
        private Thread Th;
        private bool flag1 = false;
        private Random RA = null;


        //Konstruktor
        public MainWindow()
        {
            InitializeComponent();
            //b Siehe Unten es wird nochmals Aktualisiert

            this.str = TextBox1.Text;
            //b Siehe Unten es wird nochmals Aktualisiert
            i = (int)Slider1.Value;

            RA = new Random();
        }


        //#########################Wechsel-Events################################

        private void TextBox_Max(object sender, TextChangedEventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(TextBox_Max1.Text);
                if (i < 1 || i > 1000)
                {
                    throw new Exception();
                }
                else
                {
                    this.max = i;
                }
            }
            catch (Exception) { System.Windows.MessageBox.Show("Falsches Eingabeformat\noder Wertebereich"); TextBox_Max1.Text = "100"; }
        }

        private void TextBox_Min(object sender, TextChangedEventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(TextBox_Min1.Text);
                if (i < 1 || i > 1000)
                {
                    throw new Exception();
                }
                else
                {
                    this.min = i;
                }
            }
            catch (Exception) { System.Windows.MessageBox.Show("Falsches Eingabeformat\noder Wertebereich"); TextBox_Min1.Text = "100"; }
        }

        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox1.Content = "Gleichmässig:";

            TextBlock_mili.Visibility = Visibility.Visible;
            Slider1.Visibility = Visibility.Visible;

            this.bolCheckbox1 = true;

            TextBlock_Max1.Visibility = Visibility.Hidden;
            TextBox_Max1.Visibility = Visibility.Hidden;
            TextBlock_Min1.Visibility = Visibility.Hidden;
            TextBox_Min1.Visibility = Visibility.Hidden;

        }

        private void Checkbox1_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox1.Content = "Ungleichmässig:";

            this.bolCheckbox1 = false;

            TextBlock_mili.Visibility = Visibility.Hidden;
            Slider1.Visibility = Visibility.Hidden;

            TextBlock_Max1.Visibility = Visibility.Visible;
            TextBox_Max1.Visibility = Visibility.Visible;
            TextBlock_Min1.Visibility = Visibility.Visible;
            TextBox_Min1.Visibility = Visibility.Visible;



        }

        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.str = TextBox1.Text;
        }

        private void Slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            i = (int)Slider1.Value;
        }
        //#########################Wechsel-Events-Ende################################


        //%%%%%%%%%%%%%%%%%%%%%%%%%%%% Normale Events %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

        private void Button_Klein(object sender, RoutedEventArgs e)
        {
            Height = 464;
            Width = 749;
        }

        private void Button_Gross(object sender, RoutedEventArgs e)
        {
            Height = 651;
            Width = 1067;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Um Zeit zu haben den Fokus zu der EingabeAnwendung zu Wechseln! ;)
            Thread.Sleep(3000);

            this.Th = new Thread(new ThreadStart(Thread1));
            Th.Start();

        }

        private void Button_Stop(object sender, RoutedEventArgs e)
        {
            flag1 = true;
        }

        private void Button_Weiter(object sender, RoutedEventArgs e)
        {
            //Zeit um den Fokus wieder zu holen!! :/
            Thread.Sleep(3000);
            flag1 = false;
        }

        private void Button_Beenden(object sender, RoutedEventArgs e)
        {
            Th.Abort();
        }

        private void Window_Schliessen(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string strText = TextBox1.Text;

            string strHeight = Convert.ToString(this.Height);
            string strWidth = Convert.ToString(this.Width);
            string strSlider = Convert.ToString(Slider1.Value);
            string strCheckbox1 = Convert.ToString(CheckBox1.IsChecked);
            string strTextBox1_Max1 = Convert.ToString(TextBox_Max1.Text);
            string strTextBox_Min1 = Convert.ToString(TextBox_Min1.Text);


            //TextConkatenieren! :)
            string strConcat = strHeight + "\n" + strWidth + "\n" + strSlider + "\n" + strCheckbox1 + "\n" + strTextBox1_Max1 + "\n" + strTextBox_Min1;

            try
            {
                System.IO.File.WriteAllText("DatenText.config", strText);
                System.IO.File.WriteAllText("DatenFenster.config", strConcat);

            }
            catch (Exception) { }
        }

        private void Window_Oeffnen(object sender, EventArgs e)
        {
            try
            {
                string strText = System.IO.File.ReadAllText("DatenText.config");
                TextBox1.Text = strText;

                string[] strArr = System.IO.File.ReadAllLines("DatenFenster.config");

                this.Height = Convert.ToDouble(strArr[0]);
                this.Width = Convert.ToDouble(strArr[1]);
                Slider1.Value = Convert.ToDouble(strArr[2]);
                CheckBox1.IsChecked = Convert.ToBoolean(strArr[3]);
                TextBox_Max1.Text = strArr[4];
                TextBox_Min1.Text = strArr[5];

                if ((bool)CheckBox1.IsChecked)
                {
                    CheckBox1_Checked(null, null);
                }
                else
                {
                    Checkbox1_Unchecked(null, null);
                }


            }
            catch (Exception) { }
        }

        private void Button_Speichern(object sender, RoutedEventArgs e)
        {
            Window_Schliessen(null, null);
            System.Windows.MessageBox.Show("Fenster-Daten Abgespeichert.\n\n(Wird beim Schliessen des Fensters\nautomatisch gemacht!) :)");
        }
        //%%%%%%%%%%%%%%%%%%%%%%%%%%%% Normale Events-Ende %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%


        //******************************Thread-Methode*********************************
        private void Thread1()
        {
            Char[] chrArr = str.ToCharArray();

            for (int x = 0; x < chrArr.Length; x++)
            {
                // Pausieren bis flag1 wieder auf false! ;)
                while (flag1)
                {
                    Thread.Sleep(200);
                }

                try
                {
                    if (this.bolCheckbox1)
                    {
                        Thread.Sleep(RA.Next(this.i));
                    }
                    else
                    {
                        Thread.Sleep(RA.Next(this.min, this.max));
                    }

                    //Die einzelnen Chars wieder in Strings Umwandeln wegen SendWait(string)

                    string str2 = chrArr[x].ToString();

                    //Zeichen die für Sendkey eine besondere Bedeudung haben mit "{}" Einrahmen. :)
                    if (str2 == "(" || str2 == ")" || str2 == "+" || str2 == "%" ||
                        str2 == "~" || str2 == "[" || str2 == "]")
                        str2 = "{" + str2 + "}";
                    SendKeys.SendWait(str2);
                    str2 = null;
                }
                catch (Exception) { }
            }
        }

        private string strLeerzeichen = " ";
        private void Button_Zeilenumbruch(object sender, RoutedEventArgs e)
        {
            
            string strText = TextBox1.Text;

            string strUmbruch1 = "\u000D" + "\u000A";
            if (strText.IndexOf(strUmbruch1) > -1)
                strText = strText.Replace(strUmbruch1, strLeerzeichen);

            string strUmbruch2 = "\u000A";
            if (strText.IndexOf(strUmbruch2) > -1)
                strText = strText.Replace(strUmbruch2, strLeerzeichen);

            string strUmbruch3 = "\u000A";
            if (strText.IndexOf(strUmbruch3) > -1)
                strText = strText.Replace(strUmbruch3, strLeerzeichen);

            TextBox1.Text = strText;
            strLeerzeichen = " ";//Wieder zurückstellen! :/
        }

        
        private void Button_TypingSpeed(object sender, RoutedEventArgs e)
        {
            strLeerzeichen = "";
            Button_Zeilenumbruch(null,null);
            string strText = TagsRausnehmen(TextBox1.Text);
            
            
            strText = strText.Replace("_", " ");
            strText = strText.Replace("|", "");

            TextBox1.Text = strText;
        }
        private static string TagsRausnehmen(string str)
        {
            string str2 = "";
            bool flag = true;
            char[] chrArr = str.ToCharArray();

            for (int x = 0; x < chrArr.Length; ++x)
            {
                if (chrArr[x] == '<')
                {
                    flag = false;
                    continue;
                }

                if (chrArr[x] == '>')
                {
                    flag = true;
                    continue;
                }
                if (flag)
                {
                    //String Konkatenieren.
                    str2 += chrArr[x];
                }
            }
            return str2;
        }

        private void Button_Loeschen(object sender, RoutedEventArgs e)
        {
            TextBox1.Text = "";
        }

        
    }
}

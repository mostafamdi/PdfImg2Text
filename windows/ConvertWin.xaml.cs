using FileToText_WPF.helpers;
using FileToText_WPF.services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FileToText_WPF.windows
{
    /// <summary>
    /// Interaction logic for ConvertWin.xaml
    /// </summary>
    public partial class ConvertWin : MahApps.Metro.Controls.MetroWindow
    {
        int numberFiles = 0, counter = 0;
        OpenFileDialog ofg;
        List<string> listOfFiles, listLanName;
        ConvertFile convertFile;

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            listOfFiles = new List<string>();
            listLanName = new List<string>();
            ofg = new OpenFileDialog();
            ofg.Multiselect = true;
            ofg.Filter = "Pdf Files (*.pdf)|*.pdf";
            fillComboBox();
        }

        private void selectFileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ofg.ShowDialog() == true)
            {
                listOfFiles.Clear();
                foreach (var path in ofg.FileNames)
                {
                    listOfFiles.Add(path);
                }
            }
        }

        private async void convertFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var resultSelection = Assistant.IsAnyButtonsSelected(pdfFileRadio, imgFileRadio);
            if (resultSelection.success == true)
            {
                string language = Assistant.GetLangName(listCountryNameCombo.SelectedValue.ToString());
                switch (resultSelection.name)
                {
                    case "pdfFileRadio":
                        foreach (var file in listOfFiles)
                        {
                            convertFile = new ConvertFile(file, Properties.Settings.Default.TesseractDataDir, "eng", "300");

                            var resultRun = await Task.Run(() => convertFile.ConvertNormalPdfToText(language));
                            convertFileBtn.IsEnabled = false;
                            if (!resultRun.Success)
                            {
                                convertFileBtn.IsEnabled = true;
                                MessageBox.Show(resultRun.Message);
                            }
                            else
                            {
                                convertFileBtn.IsEnabled = true;
                                MessageBox.Show("Done!");
                            }
                        }
                        break;
                    case "imgFileRadio":
                        foreach (var file in listOfFiles)
                        {
                            convertFile = new ConvertFile(file, Properties.Settings.Default.TesseractDataDir, language, "300");
                            var resultImg2Txt = await Task.Run(() => convertFile.ConvertPic2Text(file, language));
                            if (resultImg2Txt.success == true)
                            {
                                foreach (var item in resultImg2Txt.FileName)
                                {
                                    //richTextBox1.Text = item;
                                }
                                MessageBox.Show("Done!");
                            }
                            else
                            {
                                MessageBox.Show(resultImg2Txt.FileName.First());
                            }
                        }
                        break;


                }
                //convertFile = new ConvertFile(listOfFiles, Properties.Settings.Default.TesseractDataDir, "eng", "300");
                //var res = await Task.Run(() => convertFile.ConvertPic2Text("eng"));
            }
            else
            {
                MessageBox.Show("Please select the file type");
            }
        }

        private void pdfFileRadio_Click(object sender, RoutedEventArgs e)
        {
            ofg.Filter = "Pdf Files (*.pdf)|*.pdf";
        }

        private void imgFileRadio_Click(object sender, RoutedEventArgs e)
        {
            ofg.Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
        }

        void fillComboBox()
        {
            listLanName.Add("English");
            listLanName.Add("Persian(Farsi)");
            listLanName.Add("Arabic");
            listLanName.Add("Chinese(Simplified)");
            listLanName.Add("Hindi(traditional)");
            listLanName.Add("Italian");
            listLanName.Add("Spanish");
            listLanName.Add("German");
            listLanName.Add("French");
            listLanName.Add("Irish");
            listLanName.Add("Croatian");
            listLanName.Add("Indonesian");
            listLanName.Add("Swedish");
            listLanName.Add("Czech");
            listLanName.Add("Ukrainian");
            listLanName.Add("Russian");
            listLanName.Add("Croatian");
            listLanName.Add("Japanese");
            listLanName.Add("Korean");
            listLanName.Add("Turkish");
            listLanName.Sort();
            listCountryNameCombo.ItemsSource= listLanName;
            listCountryNameCombo.SelectedIndex = 0;
        }
        public ConvertWin()
        {
            InitializeComponent();
        }
    }
}

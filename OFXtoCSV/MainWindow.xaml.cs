using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OFXtoCSV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OFXFile selectedOFXFile = null;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "OFX File|*.OFX;*.QFX;*.QIF;*.QBO;*.OFC|All files (*.*)|*.*";
            openFileDialog1.Title = "Load an OFX formatted file";
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {
                try
                {                    
                    string[] RawFile = File.ReadAllLines(openFileDialog1.FileName);
                    selectedOFXFile = new OFXFile(openFileDialog1.FileName, RawFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "File read error", MessageBoxButton.OK, MessageBoxImage.Error);
                }


                if (selectedOFXFile != null)
                {
                    if (selectedOFXFile.Records.Count > 0)
                    {
                        MessageBox.Show("Success! OFX appears to have records in it. Press OK to save a CSV file containing this data.", "File parsed successfully", MessageBoxButton.OK, MessageBoxImage.Information);

                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "CSV Files|*.csv";
                        saveFileDialog1.Title = "Save data in a CSV file";
                        saveFileDialog1.FileName = selectedOFXFile.AccountID + "-" + selectedOFXFile.TimeStampStart + "-" + selectedOFXFile.TimeStampEnd;

                        if (saveFileDialog1.ShowDialog() == true)
                        {
                            try
                            {
                                using (FileStream csvFile = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                                {
                                    using (StreamWriter writer = new StreamWriter(csvFile, Encoding.UTF8))
                                    {
                                        // File meta information
                                        writer.WriteLine("Account: " + selectedOFXFile.AccountID);
                                        writer.WriteLine("Date from: " + selectedOFXFile.TimeStampStart);
                                        writer.WriteLine("Date to: " + selectedOFXFile.TimeStampEnd);
                                        writer.WriteLine("Records: " + selectedOFXFile.Records.Count);
                                        writer.Write(writer.NewLine);

                                        // Headings
                                        writer.WriteLine("FITID,SIC,DTPOSTED,TRNTYPE,NAME,TRNAMT");

                                        // Records
                                        foreach (OFXRecord record in selectedOFXFile.Records)
                                        {
                                            writer.Write("\"" + record.TransactionID + "\",");
                                            writer.Write("\"" + record.SIC + "\",");
                                            writer.Write("\"" + record.DatePosted + "\",");
                                            writer.Write("\"" + record.TransactionTypeString + "\",");
                                            writer.Write("\"" + record.Name + "\",");
                                            writer.Write("\"" + record.TransactionAmount + "\"");
                                            writer.Write(writer.NewLine);
                                        }

                                        writer.Flush();
                                        csvFile.Flush();

                                        MessageBox.Show("CSV file saved successfully to: " + saveFileDialog1.FileName, "File saved successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("An error occurred: " + ex.Message, "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else 
                        {
                            MessageBox.Show("Save cancelled: re-open the original OFX formatted file if you wish to try again.", "Save cancelled", MessageBoxButton.OK, MessageBoxImage.Error);
                        }


                    }
                    else
                    {
                        MessageBox.Show("No transactions found in this file", "OFX parsing error", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Error: Could not load OFX data from file", "OFX parsing error", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OFXtoCSV
{
    class OFXFile
    {
        public string FileName { get; set; }
        public string FinancialInstitutionName { get; set; }
        public string FinancialInstitutionID { get; set; }
        public string AccountID { get; set; }
        public string TimeStampStart { get; set; }
        public string TimeStampEnd { get; set; }
        public List<OFXRecord> Records { get; set; }


        public OFXFile(string filename, string[] FileData)
        {
            this.Records = new List<OFXRecord>();
            
            // Get the metadata from the top of the file
            foreach (string line in FileData)
            {
                if (line.StartsWith("<ORG>"))
                {
                    this.FinancialInstitutionName = line.Replace("<ORG>", "");
                }

                if (line.StartsWith("<FID>"))
                {
                    this.FinancialInstitutionID = line.Replace("<FID>", "");

                }

                if (line.StartsWith("<ACCTID>"))
                {
                    this.AccountID = line.Replace("<ACCTID>", "");

                }

                if (line.StartsWith("<DTSTART>"))
                {
                    this.TimeStampStart = line.Replace("<DTSTART>", "");

                }

                if (line.StartsWith("<DTEND>"))
                {
                    this.TimeStampEnd = line.Replace("<DTEND>", "");

                }
            }

            // Go through the file and find transaction sections, and turn those into OFXRecord objects
            bool captureSection = false;
            List<string> capturedSection = new List<string>();
            foreach (string line in FileData)
            {
                if (line.ToLower().StartsWith("<stmttrn>"))
                {
                    captureSection = true;
                }

                if (line.ToLower().StartsWith("</stmttrn>"))
                {
                    string trans_type = string.Empty;
                    string trans_dateposted = string.Empty;
                    string trans_amnt = string.Empty;
                    string trans_fitid = string.Empty;
                    string trans_sic = string.Empty;
                    string trans_name = string.Empty;

                    foreach (String capturedLine in capturedSection)
                    {
                        if (capturedLine.StartsWith("<TRNTYPE>"))
                        {
                            trans_type = capturedLine.Replace("<TRNTYPE>", "");
                        }

                        if (capturedLine.StartsWith("<DTPOSTED>"))
                        {
                            trans_dateposted = capturedLine.Replace("<DTPOSTED>", "");
                        }

                        if (capturedLine.StartsWith("<TRNAMT>"))
                        {
                            trans_amnt = capturedLine.Replace("<TRNAMT>", "");
                        }

                        if (capturedLine.StartsWith("<FITID>"))
                        {
                            trans_fitid = capturedLine.Replace("<FITID>", "");
                        }

                        if (capturedLine.StartsWith("<SIC>"))
                        {
                            trans_sic = capturedLine.Replace("<SIC>", "");
                        }

                        if (capturedLine.StartsWith("<NAME>"))
                        {
                            trans_name = capturedLine.Replace("<NAME>", "");
                        }
                    }

                    this.Records.Add(new OFXRecord(trans_type, trans_dateposted, trans_amnt, trans_fitid, trans_sic, trans_name));
                    capturedSection.Clear();
                    captureSection = false;
                }

                if (captureSection)
                {
                    capturedSection.Add(line);
                }
            }

        }

        public OFXFile(string filename, string financialinstitutionname, string financialinstitutionid, string accountid, string timestampstart, string timestampend)
        {
            this.FileName = filename;
            this.FinancialInstitutionName = financialinstitutionname;
            this.FinancialInstitutionID = financialinstitutionid;
            this.AccountID = accountid;
            this.TimeStampStart = timestampstart;
            this.TimeStampEnd = timestampend;
        }

    }
}

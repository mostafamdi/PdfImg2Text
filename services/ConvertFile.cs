using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using ImageMagick;
using FileToText_WPF.helpers;
namespace FileToText_WPF.services
{
    public class ConvertFile
    {
        public string FileName { set; get; }
        public string tessDataPath { set; get; }
        public string lang { set; get; }
        public int dpi { set; get; }
        public ConvertFile(string _fileName, string _tesseDataPath, string _lang, string _dpi)
        {
            FileName = _fileName;
            tessDataPath = _tesseDataPath;
            lang = _lang;
            dpi = 300;
        }
        public Result ConvertNormalPdfToText(string lang, string dpi = "300")
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string finalText = "";

                bool isTextBased = IsPdfTextBased(FileName);
                if (isTextBased)
                {
                    string fileName = System.IO.Path.GetFileName(FileName);
                    string filePath = System.IO.Path.GetDirectoryName(FileName);
                    using (PdfReader reader = new PdfReader(FileName))
                    {
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            string text = PdfTextExtractor.GetTextFromPage(reader, i, new SimpleTextExtractionStrategy());
                            //sb.AppendLine($"--- page {i} ---");
                            sb.AppendLine(text);
                            if (Assistant.ContainsPersian(sb.ToString()))
                            {
                                finalText = Assistant.FixPersianText(sb.ToString());
                            }
                            else
                                finalText =  sb.ToString();
                            //writer.WriteLine(text);
                            //writer.WriteLine();
                        }
                        File.WriteAllText(filePath + "\\" + fileName + ".txt", finalText.ToString(), Encoding.UTF8);
                    }
                    return new Result { Success = true, Message = "success" };
                }
                else
                {
                    ConvertPdfToPic(FileName, lang);
                }


                return new Result { Success = true, Message = "success" };


            }
            catch (Exception ex)
            {
                return new Result { Success = false, Message = ex.ToString() };
            }
        }
        public (bool success, List<string> FileName) ConvertPic2Text(string file, string lang)
        {
            var results = new List<string>();
            try
            {
                string pdfNameWithDir = file;
                string fileName = System.IO.Path.GetFileName(pdfNameWithDir);
                string outputDir = System.IO.Path.GetDirectoryName(pdfNameWithDir);
                using (var engine = new TesseractEngine(Properties.Settings.Default.TesseractDataDir, lang, EngineMode.Default))
                {
                    int index = 1;
                    using (var img = Pix.LoadFromFile(file))
                    {
                        using (var page = engine.Process(img))
                        {
                            string text = page.GetText();
                            using (StreamWriter writer = new StreamWriter(outputDir + "\\" + fileName + "-page-" + index + ".txt"))
                            {
                                writer.WriteLine(text);
                                writer.WriteLine();
                                index++;
                            }
                            //richTextBox1.Text += $"📄 Page {System.IO.Path.GetFileName(imagePath)}:\n";
                            //richTextBox1.Text += text;
                            //richTextBox1.Text += "---------------\n";
                        }
                    }

                }

                return (true, results);
            }
            catch (Exception ex)
            {
                results.Add(ex.Message);
                return (true, results);
            }
        }
        public void ConvertPdfToPic(string file, string lang)
        {
            string pdfNameWithDir = file;
            string fileName = System.IO.Path.GetFileName(pdfNameWithDir);
            string outputDir = System.IO.Path.GetDirectoryName(pdfNameWithDir);

            Directory.CreateDirectory(outputDir);
            // ۱. تبدیل PDF به تصویر
            using (var images = new MagickImageCollection())
            {
                var settings = new MagickReadSettings
                {
                    Density = new Density(300, 300) // DPI برای وضوح بهتر OCR
                };

                images.Read(pdfNameWithDir, settings);

                int page = 1;
                foreach (var image in images)
                {
                    string outputPath = System.IO.Path.Combine(outputDir, fileName + $"_page_{page}.png");
                    image.Format = MagickFormat.Png;
                    image.Write(outputPath);
                    ConvertPic2Text(outputPath, lang);
                    page++;
                }
            }
            using (var engine = new TesseractEngine(Properties.Settings.Default.TesseractDataDir, lang, EngineMode.Default))
            {
                int index = 1;
                foreach (string imagePath in Directory.GetFiles(outputDir, "*.png"))
                {
                    using (var img = Pix.LoadFromFile(imagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            string text = page.GetText();
                            using (StreamWriter writer = new StreamWriter(outputDir + "\\" + fileName + "-page-" + index + ".txt"))
                            {
                                writer.WriteLine(text);
                                writer.WriteLine();
                                index++;
                            }
                            //richTextBox1.Text += $"📄 Page {System.IO.Path.GetFileName(imagePath)}:\n";
                            //richTextBox1.Text += text;
                            //richTextBox1.Text += "---------------\n";
                        }
                    }
                }
            }
        }
        private static bool IsPdfTextBased(string pdfPath)
        {
            using (PdfReader reader = new PdfReader(pdfPath))
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    string text = PdfTextExtractor.GetTextFromPage(reader, i, new SimpleTextExtractionStrategy());
                    sb.Append(text.Trim());
                }
                return sb.Length > 10; // اگر متن کافی داشت، PDF متنی است
            }
        }
    }
}

using System.Collections.Generic;
using System.IO;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;

namespace Hotel
{
    public class WordDocumentManager
    {
        public void FillTemplate(string templatePath, string outputPath, Dictionary<string, string> data)
        {
            var saveFileDialog = new SaveFileDialog { Filter = "Word Documents (*.docx)|*.docx|All Files (*.*)|*.*", FileName = outputPath };

            if (saveFileDialog.ShowDialog() == true) {
                outputPath = saveFileDialog.FileName;

                // Копируем шаблон в новый файл
                File.Copy(templatePath, outputPath, true);

                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(outputPath, true))
                {
                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;

                    // Получаем все текстовые элементы документа
                    IEnumerable<Text> texts = mainPart.Document.Body.Descendants<Text>();

                    // Заменяем плейсхолдеры данными
                    foreach (Text text in texts)
                    {
                        foreach (var item in data)
                        {
                            if (text.Text.Contains(item.Key))
                            {
                                text.Text = text.Text.Replace(item.Key, item.Value);
                            }
                        }
                    }

                    mainPart.Document.Save();
                }
            }
        }
    }
}

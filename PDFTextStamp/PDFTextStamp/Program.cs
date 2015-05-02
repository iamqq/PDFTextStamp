using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PDFTextStamp
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length != 6)
            {
                System.Console.WriteLine("Неверное количество аргументов");
                System.Console.WriteLine("используйте PDFTextStamp <вход.файл> <текст> <х> <у> <размер> <выход.файл>");
                return;
            }

            string filename_in = args[0];
            string text_stamp = args[1];
            Int32 x_pos = Int32.Parse(args[2]);
            Int32 y_pos = Int32.Parse(args[3]);
            Int32 font_size = Int32.Parse(args[4]);
            string filename_out = args[5];
            PdfReader reader = new PdfReader(filename_in);
            Rectangle size = reader.GetPageSizeWithRotation(1);
            Document document = new Document(size);

            // open the writer
            FileStream fs = new FileStream(filename_out, FileMode.Create, FileAccess.Write);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            PdfContentByte cb = writer.DirectContent;

            // select the font properties
            BaseFont bf = BaseFont.CreateFont(@"arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);


            // create the new page and add it to the pdf
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                PdfImportedPage page = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page, 0, 0);
                // write the text in the pdf content
                cb.SetFontAndSize(bf, font_size);
                cb.BeginText();
                // put the alignment and coordinates here
                cb.ShowTextAligned(1, text_stamp, x_pos, y_pos, 0);
                cb.EndText();
                document.NewPage();
            }
            // close the streams and voilá the file should be changed :)
            document.Close();
            fs.Close();
            writer.Close();
            reader.Close();

        }
    }
}

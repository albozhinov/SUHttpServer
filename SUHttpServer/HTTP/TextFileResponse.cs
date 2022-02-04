using System.IO;

namespace SUHttpServer.HTTP
{
    public class TextFileResponse : Response
    {
        public string FileName { get; init; }


        public TextFileResponse(string filename) 
            : base(StatusCode.OK)
        {
            this.FileName = filename;

            this.Headers.Add(Header.ContentType, ContentType.Plaintext);
        }

        public override string ToString()
        {
            if (File.Exists(this.FileName))
            {
                this.Body = string.Empty; 
                FileContent = File.ReadAllBytes(this.FileName);

                var fileBytesCount = new FileInfo(this.FileName).Length;
                this.Headers.Add(Header.ContentLength, fileBytesCount.ToString());

                this.Headers.Add(Header.ContentDisposition, $"attachment; filename=\"{this.FileName}\"");

            }

            return base.ToString();
        }
    }
}

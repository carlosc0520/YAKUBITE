using FluentFTP;
using Microsoft.AspNetCore.Http;

namespace CARO.CORE
{
    public class FileUploads
    {
        private string credetencialts = "u551436692.jurissearch.com";
        private string password = "2051CCfirma1091#";

        public async Task<string> UploadFileAsync(string ruta, IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                throw new ArgumentException("El archivo no puede ser nulo o vacío.", nameof(archivo));
            }

            var client = new FtpClient("ccfirma.com", 21)
            {
                Credentials = new System.Net.NetworkCredential(this.credetencialts, this.password) 
            };

            try
            {
                client.Connect();
                if (!client.DirectoryExists(ruta)) client.CreateDirectory(ruta);

                using (var stream = new MemoryStream())
                {
                    await archivo.CopyToAsync(stream);
                    stream.Position = 0;

                    string remoteFilePath = Path.Combine(ruta, archivo.FileName);
                    client.UploadStream(stream, remoteFilePath);

                    return ruta + '/' + archivo.FileName;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (client.IsConnected) client.Disconnect();
            }
        }

        public async Task DeleteDirectoryAsync(string ruta)
        {
            var client = new FtpClient("ccfirma.com", 21)
            {
                Credentials = new System.Net.NetworkCredential(this.credetencialts, this.password)
            };

            try
            {
                client.Connect();
                if (client.FileExists(ruta)) client.DeleteFile(ruta);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (client.IsConnected) client.Disconnect();
            }
        }

        public async Task<byte[]> DownloadFileAsync(string remotePath)
        {
            var client = new FtpClient("ccfirma.com", 21)
            {
                Credentials = new System.Net.NetworkCredential(this.credetencialts, this.password)
            };

            string tempFilePath = Path.GetTempFileName(); 

            try
            {
                client.Connect();
                await Task.Run(() => client.DownloadFile(tempFilePath, remotePath));

                byte[] fileBytes = await File.ReadAllBytesAsync(tempFilePath);
                return fileBytes;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (client.IsConnected) client.Disconnect();
                if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
            }
        }


    }
}

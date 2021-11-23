using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace CargaBloomberg.BE
{
    [MessageContract]
    public class RemoteFileInfo : IDisposable
    {
        [MessageHeader(MustUnderstand = true)]
        public string Filename { get; set; }

        [MessageHeader(MustUnderstand = true)]
        public long Length { get; set; }


        [MessageBodyMember (Order = 1)]
        public Stream FileByteStream { get; set; }

        public void Dispose()
        {
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        }
    }
}

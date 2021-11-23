using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace CargaBloomberg.BE
{
    [MessageContract]
    public class DownloadRequest
    {
        [MessageBodyMember]
        public string Filename { get; set; }
    }
}

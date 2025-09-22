using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBS.ImplementationAutomationConsole.Core.Models
{
    public class AsymetricData
    {
        public string RSAPrivateKeyXML { get; set; }
        public string RSAPublicKeyXML { get; set; }
        public string RSAPublicKeyPEM { get; set; }
        public string RSAPublicKeyPEMEncoded { get; set; }

    }
}
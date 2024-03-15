using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Internal
{
    public interface IInternalEmailService
    {
        public void SendEmail(string toMail, string subject, string body);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases
{
    public interface IEmailVerificationService
    {
        void SendEmail(string toMail, string subject, string body);
    }
}

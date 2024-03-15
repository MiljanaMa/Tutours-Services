using Explorer.Stakeholders.API.Dtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Internal
{
    public interface IInternalNotificationService
    {
        void Generate(int userId, NotificationType type, string actionURL, DateTime date, string additionalMessage);
    }
}

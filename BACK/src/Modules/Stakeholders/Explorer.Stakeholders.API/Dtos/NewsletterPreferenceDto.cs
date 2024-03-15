using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos;

public class NewsletterPreferenceDto
{
    public long UserID { get; set; }
    public uint Frequency { get; set; }
    public DateTime LastSent { get; set; }
}

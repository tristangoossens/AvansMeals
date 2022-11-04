using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Domain
{
    public enum City
    {
        [Display(Name = "Breda")]
        BREDA,

        [Display(Name = "Tilburg")]
        TILBURG,

        [Display(Name = "'s-Hertogenbosch")]
        SHERTOGENBOSCH
    }
}

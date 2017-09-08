using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PdfAutofill.Model
{
    public class PdfViewModel
    {
        public string Url { get; set; }

        public List<string> FieldsData { get; set; }
    }
}

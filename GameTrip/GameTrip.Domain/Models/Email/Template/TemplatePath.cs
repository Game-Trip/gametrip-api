using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTrip.Domain.Models.Email.Template;

public class TemplatePath
{
    private TemplatePath(string value) { Value = value; }

    public string Value { get; private set; }

    public static TemplatePath Register { get { return new TemplatePath("RegisterTemplate.html"); } }

    public override string ToString()
    {
        return Value;
    }
}

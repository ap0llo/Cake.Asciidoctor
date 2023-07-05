namespace Cake.Asciidoctor;

//TODO: Think of a better API to model unset and non-overrideing attribtues
//TODO: A value containing spaces must be enclosed in quotes, in the form NAME="VALUE WITH SPACES".
public record AsciidoctorAttribute(string Name, string? Value, bool Override = true)
{
    private bool m_Unset = false;

    public AsciidoctorAttribute(string name) : this(name, null)
    { }


    public AsciidoctorAttribute(string name, bool unset) : this(name)
    {
        m_Unset = true;
    }

    public override string ToString()
    {
        if (m_Unset)
        {
            return $"{Name}!";
        }

        if (String.IsNullOrEmpty(Value))
        {
            return Name;
        }

        if (Override)
        {

            return $"{Name}={Value}";
        }
        else
        {
            return $"{Name}@={Value}";
        }
    }
}


namespace SqlFileConverter
{
    public interface IConverter<Source,Output>
    {
        Output Convert(Source source);
    }
}

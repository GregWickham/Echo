using System.Xml.Serialization;

namespace SimpleNLG
{
    public partial class NLGSpec
    {
        public string Serialize()
        {
			string serialized;
			using (var stringwriter = new System.IO.StringWriter())
			{
				var serializer = new XmlSerializer(GetType());
				serializer.Serialize(stringwriter, this);
				serialized = stringwriter.ToString();
			}
			return serialized;
		}
	}
}

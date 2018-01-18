/*
 * Created by SharpDevelop.
 * User: rahman
 * Date: 17/01/2018
 * Time: 7:11 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace ValidateXmlByXsd
{
	class Program
	{
		private static string _XmlString = 
@"
<address > 
	<name>John Smith</name>
	<street>109 Abbey Close</street>
	<city>Hayes</city>
	<country>UK</country>
</address>
";
	
		private static string _InvalidXmlString = 
@"
<address > 
	<name>John Smith</name>
	<street>109 Abbey Close</street>
	<city>Hayes</city>
	<Country>UK</Country>
</address>
";
			
		public static void Main(string[] args)
		{			
			XmlStringValidator.ValidateStream(_XmlString, @"xmlschema.xsd");
			
			XmlStringValidator.ValidateStream(_InvalidXmlString, @"xmlschema.xsd");
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
	
	public sealed class XmlStringValidator
	{
		
		public static void Validate(string xmlString, string schemaFilePath)
		{
			var settings = new XmlReaderSettings();
			settings.Schemas.Add(null, schemaFilePath);
			
			settings.ValidationType = ValidationType.Schema;
			settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
			settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
			settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
			settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
			
			//var stream = GetStream(xmlString);
			// Create the XmlReader object.
			XmlReader reader = XmlReader.Create(xmlString, settings);
	
			// Parse the file. 
			while (reader.Read());
		}
		
		public static void ValidateStream(string xmlString, string schemaFilePath)
		{
			var settings = new XmlReaderSettings();
			settings.Schemas.Add(null, schemaFilePath);
			
			settings.ValidationType = ValidationType.Schema;
			settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
			settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
			settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
			settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
			
			var stream = GetStream(xmlString);
			// Create the XmlReader object.
			XmlReader reader = XmlReader.Create(stream, settings);
			// Parse the file. 
			while (reader.Read());
		}
		
		private static Stream GetStream(string str)
		{
		    MemoryStream stream = new MemoryStream();
		    StreamWriter writer = new StreamWriter(stream);
		    writer.Write(str);
		    writer.Flush();
		    stream.Position = 0;
		    return stream;
		}
		
			
		private static void ValidationCallBack(object sender, ValidationEventArgs args)
		{
			if (args.Severity == XmlSeverityType.Warning)
				Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
			else
				Console.WriteLine("\tValidation error: " + args.Message);

		}
	}

}
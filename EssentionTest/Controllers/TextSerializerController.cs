using EssentionTest.Model;
using Essention.Text;
using System.IO;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EssentionTest.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TextSerializerController : ApiController
    {
        [HttpPost()]
        public IHttpActionResult SerializerText([FromBody]TextToSerializer value)
        {
            if (value.FormatType == "cvs")
            {
                var formatText = TextSerializer.SerializeToCvs(value.Text, value.SeparatorCvs);

                var folder = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Files");
                Directory.CreateDirectory(folder);

                using (var fileStream = new FileStream($"{folder}/cvsFile{(DateTime.Now - new DateTime()).Ticks}.cvs", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.Write(formatText);
                    }
                }

                return Ok(new { formatText });
            }
            else if (value.FormatType == "xml")
            {
                var xmlText = TextSerializer.SerializeToXml(value.Text);

                var folder = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Files");
                Directory.CreateDirectory(folder);
                xmlText.Save($"{folder}/xmlFile{(DateTime.Now - new DateTime()).Ticks}.xml");

                var formatText = xmlText.Root.ToString();

                return Ok(new { formatText });
            }
            
            return BadRequest();
        }
    }
}

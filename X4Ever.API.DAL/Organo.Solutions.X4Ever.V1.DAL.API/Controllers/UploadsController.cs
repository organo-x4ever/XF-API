using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Helper;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
using System.Text;
using System.IO;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/files")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UploadsController : ApiControllerOverride
    {
        private IHelper _helper;

        public UploadsController()
        {
            _helper = new Helper.Helper();
        }

        [Route("upload")]
        [POST("upload")]
        public string Post()
        {
            try
            {
                var context = HttpContext.Current;
                var httpRequest = HttpContext.Current.Request;
                try
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        foreach (string file in httpRequest.Files)
                        {
                            var postedFile = httpRequest.Files[file];
                            var fileName = postedFile.FileName.Clean().Split('\\').LastOrDefault().Split('/').LastOrDefault();
                            var filePath = HttpContext.Current.Server.MapPath("~/" + FileUploadPath + "/" + fileName);
                            postedFile.SaveAs(filePath);
                            return HttpConstants.SUCCESS + "#" + FileUploadPath + "/" + fileName;
                        }
                    }

                    return "MessageFileUploadFailed";
                }
                catch (Exception ex)
                {
                    return "MessageErrorOccurred#" + GetExceptionDetail(ex);
                }
            }
            catch (Exception ex)
            {
                return "MAIN#" + GetExceptionDetail(ex);
            }
        }
        
        [Route("upload/web")]
        [POST("upload/web")]
        public string PostWeb(string postedFileName)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                        foreach (string file in httpRequest.Files)
                        {
                            var postedFile = httpRequest.Files[file];
                            if(postedFile!=null) {
                                postedFileName += Path.GetExtension(postedFile.FileName);
                                var fileName = postedFile.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();
                                var filePath = HttpContext.Current.Server.MapPath("~/" + FileUploadPath + "/" + postedFileName);
                                postedFile.SaveAs(filePath);
                                return HttpConstants.SUCCESS + "#" + FileUploadPath + "/" + postedFileName;
                            }
                        }
                    }

                    return "MessageFileUploadFailed";
                }
                catch (Exception ex)
                {
                    return "MessageErrorOccurred#" + GetExceptionDetail(ex);
                }
        }

        [Route("uploadasync")]
        [POST("uploadasync")]
        public HttpResponseMessage PostAsync()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                try
                {
                    if (httpRequest.Files.Count > 0)
                        foreach (string file in httpRequest.Files)
                        {
                            var postedFile = httpRequest.Files[file];
                            if (postedFile != null)
                            {
                                var fileName = postedFile.FileName.Clean().Split('\\').LastOrDefault().Split('/').LastOrDefault();
                                var filePath = HttpContext.Current.Server.MapPath("~/" + FileUploadPath + "/" + fileName);
                                postedFile.SaveAs(filePath);
                                return Request.CreateResponse(HttpStatusCode.OK, FileUploadPath + "/" + fileName);
                            }
                        }

                    return Request.CreateResponse(HttpStatusCode.BadRequest, "MessageFileUploadFailed");
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "MessageFileUploadFailed#" + GetExceptionDetail(ex));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "MAIN#" + GetExceptionDetail(ex));
            }
        }

        [Route("get")]
        [GET("get")]
        public async Task<string> Get(string param1)
        {
            return await Task.Factory.StartNew(() =>
            {
                if (param1 != null)
                    return _helper.GetAppSetting(param1);
                return "";
            });
        }

        private string FileUploadPath
        {
            get
            {
                string fileUploadPath = "Uploads";
                try
                {
                    fileUploadPath = _helper.GetAppSetting("fileUploadDirectoryName");
                }
                catch (Exception)
                {
                    //
                }

                return fileUploadPath;
            }
        }

        public string GetExceptionDetail(Exception exception)
        {
            var stringBuilder = new StringBuilder();
            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);
                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }
    }
}



//string[] headers = new string[number];
//for (var i = 0; i < number; i++)
//{
//    var header = base.GetHeader(HttpConstants.HTTP_HEADER_PARAM + (i + 1).ToString());
//    if (header != null)
//        headers[i] = (string)header;
//}

//if (headers.Length == number)
//{
//    var data = CryptoEngine.Decrypt(headers[0], headers[1] ?? headers[1]);
//    return _helper.GetAppSetting(data);
//}
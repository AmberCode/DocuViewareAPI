using GdPicture14.WEB;
using System;
using System.IO;
using System.Web.Http;
using DocuViewareAPI.Models;

namespace DocuViewareAPI.Controllers
{
    public class DocuViewareController : ApiController
    {
        /// <summary>
        /// This POST request will return the control markup that corresponds to the provided session and configuration.
        /// </summary>
        /// <remarks>InitializeDocuVieware has to be called beforehand to make sure the session exists.</remarks>
        /// <param name="controlConfiguration">A DocuViewareConfiguration object</param>
        /// <returns>A DocuViewareRESTOutputResponse JSONS object that contains all the control HTML to include in the client page.</returns>
        [HttpPost]
        [Route("api/docuvieware/control")]
        public DocuViewareResponse GetDocuViewareControl(DocuViewareConfiguration controlConfiguration)
        {
            if (!DocuViewareManager.IsSessionAlive(controlConfiguration.SessionId))
            {
                if (!string.IsNullOrEmpty(controlConfiguration.SessionId) && !string.IsNullOrEmpty(controlConfiguration.ControlId))
                {
                    DocuViewareManager.CreateDocuViewareSession(controlConfiguration.SessionId, controlConfiguration.ControlId, WebApiApplication.SESSION_TIMEOUT);
                }
                else
                {
                    throw new Exception("Invalid session identifier and/or invalid control identifier.");
                }
            }

            using (var docuViewareInstance = new DocuVieware(controlConfiguration.SessionId)
            {
                AllowPrint = controlConfiguration.AllowPrint,
                EnablePrintButton = controlConfiguration.EnablePrintButton,
                AllowUpload = controlConfiguration.AllowUpload,
                EnableFileUploadButton = controlConfiguration.EnableFileUploadButton,
                CollapsedSnapIn = controlConfiguration.CollapsedSnapIn,
                ShowAnnotationsSnapIn = controlConfiguration.ShowAnnotationsSnapIn,
                EnableRotateButtons = controlConfiguration.EnableRotateButtons,
                EnableZoomButtons = controlConfiguration.EnableZoomButtons,
                EnablePageViewButtons = controlConfiguration.EnablePageViewButtons,
                EnableMultipleThumbnailSelection = controlConfiguration.EnableMultipleThumbnailSelection,
                EnableMouseModeButtons = controlConfiguration.EnableMouseModeButtons,
                EnableFormFieldsEdition = controlConfiguration.EnableFormFieldsEdition,
                EnableTwainAcquisitionButton = controlConfiguration.EnableTwainAcquisitionButton,
                MaxUploadSize = 36700160 // 35MB
            })
            {
                using (var controlOutput = new StringWriter())
                {
                    docuViewareInstance.RenderControl(controlOutput);

                    return new DocuViewareResponse
                    {
                        HtmlContent = controlOutput.ToString()
                    };
                }
            }
        }
    }
}

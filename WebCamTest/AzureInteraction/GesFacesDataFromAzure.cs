using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebCamTest
{
    public static class GesFacesDataFromAzure
    {
        public static async ValueTask<string> GetFacesData()
        {

            string imageFilePath = "image.jpg";
            return await MakeAnalysisRequestAsync(imageFilePath);
        }

        static async ValueTask<string> MakeAnalysisRequestAsync(string imageFilePath)
        {

            string AnalysisJsonString = "";
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add(
                "Ocp-Apim-Subscription-Key", AzureSettings.subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "returnFaceId=true" +
                                      "&returnFaceAttributes=age,gender";

            
            // Assemble the URI for the REST API Call.
            string uri = AzureSettings.uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json"
                // and "multipart/form-data".
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                AnalysisJsonString = await response.Content.ReadAsStringAsync();
                
            }

            return AnalysisJsonString;
        }

        // Returns the contents of the specified file as a byte array.
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}
    


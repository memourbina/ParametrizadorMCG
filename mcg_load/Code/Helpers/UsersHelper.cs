using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace mcg_load.Code.Helpers
{
    public class UsersHelper
    {

        public static bool fs_LogIn(String usuario, String psw, out String msgError, String lenguaje = "esp")
        {
            Boolean boolProcess = true;
            msgError = String.Empty;

            String Dominio = "https://fs.gis.com.mx/";
            String API = "adfs/services/trust/2005/usernamemixed";

            HttpWebResponse response;
            StreamReader readerResponse;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Dominio + API);

            request.ProtocolVersion = HttpVersion.Version11;
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/soap+xml; charset=utf-8";


            if (usuario.Contains("@"))
            {
                usuario = usuario.Split('@')[0].ToString();
            }

            if (usuario.Length > 20)
            {
                usuario = usuario.Substring(0, 20);
            }

            String Content = "<s:Envelope xmlns:s=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:a=\"http://www.w3.org/2005/08/addressing\" xmlns:u=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">" +
                      "<s:Header><a:Action s:mustUnderstand=\"1\">http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue</a:Action><a:ReplyTo><a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address></a:ReplyTo><a:To s:mustUnderstand=\"1\">" +
                      "https://fs.gis.com.mx/adfs/services/trust/2005/usernamemixed</a:To><o:Security s:mustUnderstand=\"1\" xmlns:o=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">" +
                      "<o:UsernameToken><o:Username>gis\\" + usuario + "</o:Username>" +
                      "<o:Password>" + psw + "</o:Password>" +
                      "</o:UsernameToken></o:Security></s:Header><s:Body><t:RequestSecurityToken xmlns:t=\"http://schemas.xmlsoap.org/ws/2005/02/trust\">" +
                      "<wsp:AppliesTo xmlns:wsp=\"http://schemas.xmlsoap.org/ws/2004/09/policy\"><a:EndpointReference><a:Address>http://fs.gis.com.mx/adfs/services/trust</a:Address></a:EndpointReference></wsp:AppliesTo><t:KeyType>http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey</t:KeyType><t:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</t:RequestType><t:TokenType>urn:oasis:names:tc:SAML:1.0:assertion</t:TokenType></t:RequestSecurityToken></s:Body></s:Envelope>";

            try
            {

                var dataPost = Encoding.ASCII.GetBytes(Content);
                Stream newStream = request.GetRequestStream();
                // Send the data.
                newStream.Write(dataPost, 0, dataPost.Length);
                newStream.Close();

                response = (HttpWebResponse)request.GetResponse();

                readerResponse = new StreamReader(response.GetResponseStream());

                StringReader strReader = new StringReader(readerResponse.ReadToEnd());


                String Respuesta = strReader.ReadToEnd();

                DataSet dsResp = new DataSet();

                StringReader theReader = new StringReader(Respuesta);
                dsResp.ReadXml(theReader, XmlReadMode.InferSchema);

                String Token = dsResp.Tables["SecurityContextToken"].Rows[0]["Cookie"].ToString();

                if (Token == String.Empty)
                {
                    boolProcess = false;
                    switch (lenguaje)
                    {
                        case "esp":
                            msgError = "Favor de validar usuario y contraseÃ±a.";
                            break;
                        case "eng":

                            msgError = "Please validate username and password.";
                            break;
                    }
                }

            }
            catch (System.Net.WebException ex)
            {
                boolProcess = false;
                switch (lenguaje)
                {
                    case "esp":
                        msgError = "Favor de validar usuario y contraseÃ±a.";
                        break;
                    case "eng":

                        msgError = "Please validate username and password.";
                        break;
                }
            }

            return boolProcess;
        }

    }
}
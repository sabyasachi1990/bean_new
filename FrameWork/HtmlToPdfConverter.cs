using System;
using System.Collections.Generic;

namespace AppsWorld.Framework
{
    public class HtmlContent
    {
        Dictionary<string, string> masterValues = new Dictionary<string, string>();
        Dictionary<string, string> headerValues = new Dictionary<string, string>();
        Dictionary<string, string>[] itemValues;
        List<string> headerColumns = new List<string>();        //List & Order of selected Table Columns
        string htmlTemplate;
        public string HtmlTemplate
        {
            get
            {
                return htmlTemplate;
            }
            set
            {
                htmlTemplate = value;
                masterValues.Clear();
                headerValues.Clear();
                headerColumns.Clear();
                itemValues = null;
            }
        }

        public HtmlContent(string html)
        {
            this.htmlTemplate = html;
            masterValues = new Dictionary<string, string>();
            headerValues = new Dictionary<string, string>();
        }

        public void ExtractData<T, X>(T content, X child)
        {
            masterValues.Clear();
            headerValues.Clear();
            System.Reflection.PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (System.Reflection.PropertyInfo prop in properties)
            {
                if (prop.GetMethod.ReturnType.IsGenericType)
                {
                    if (!(child is String))
                    {
                        List<X> enumX = (List<X>)prop.GetValue(content);
                        System.Reflection.PropertyInfo[] itemProperties = typeof(X).GetProperties();
                        foreach (System.Reflection.PropertyInfo p in itemProperties)
                            headerValues.Add("{{" + p.Name + "}}", p.Name);

                        itemValues = new Dictionary<string, string>[enumX.Count];
                        for (int i = 0; i < enumX.Count; i++)
                        {
                            X item = enumX[i];
                            Dictionary<string, string> dict = new Dictionary<string, string>();
                            foreach (System.Reflection.PropertyInfo p in itemProperties)
                                dict.Add("{{" + p.Name + "}}", p.GetValue(item) == null ? "" : p.GetValue(item).ToString());
                            itemValues[i] = dict;
                        }
                    }
                }
                else
                {
                    masterValues.Add("{" + prop.Name + "}", prop.GetValue(content) == null ? "" : prop.GetValue(content).ToString());
                }
            }
        }

        public string InsertIntoHtml()
        {
            ReplaceMasterValues();

            ParseHeaderColumns();

            if (itemValues != null)
            {
                ReplaceTableHeader();

                string strTBody = GenerateTableBody();

                InsertTableBody(strTBody);
            }

            return htmlTemplate;
        }

        //Parse the Header Column Names from template before replaced with variable values
        private void ParseHeaderColumns()
        {
            //string strTHead = template.Substring(template.IndexOf("<thead"),(template.IndexOf("</table>") - template.IndexOf("<thead")));
            foreach (String item in headerValues.Keys)
            {
                headerColumns.Add(item);
            }
        }

        private void ReplaceMasterValues()
        {
            foreach (KeyValuePair<string, string> entry in masterValues)
                htmlTemplate = htmlTemplate.Replace(entry.Key, entry.Value);
        }
        private void ReplaceTableHeader()
        {
            foreach (string key in headerColumns)
            {
                String headerName = FormatHeaderName(headerValues[key]);
                htmlTemplate = htmlTemplate.Replace(key, headerName);
            }
        }
        private string FormatHeaderName(string headerName) 
        {
            return headerName.Replace("{","").Replace("}","");
        }
        private string GenerateTableBody()
        {
            string strTBody = "<tbody>";
            foreach (Dictionary<string, string> item in itemValues)
            {
                strTBody += "<tr>";
                foreach (string key in headerColumns)
                    strTBody += "<td>" + item[key] + "</td>";
                strTBody += "</tr>";
            }
            strTBody += "</tbody>";
            return strTBody;
        }
        private void InsertTableBody(string strTBody)
        {
            string html1 = htmlTemplate.Substring(0, htmlTemplate.IndexOf("</table>"));
            string html2 = htmlTemplate.Substring(htmlTemplate.IndexOf("</table>"));
            htmlTemplate = html1 + strTBody + html2;
        }
    }

    public class HtmlToPdfConverter
    {
        public static byte[] HtmlToPDF(string html)
        {
            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            return htmlToPdf.GeneratePdf(html);
        }
    }
}

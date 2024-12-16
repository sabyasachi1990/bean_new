using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Models.Models
{
    public class PreviewModel
    {
        public string From { get; set; }
        public string toEmails { get; set; }
        public string Subject { get; set; }
        public string TemplateContent { get; set; }
        public string EmailBody { get; set; }
        public string Attachments { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        //public string FileName { get; set; }
        //public string FilePath { get; set; }
        //public string URL { get; set; }
        //public string DisplayFileName { get; set; }
        public List<AttachmnetModel> LstAttachmnets { get; set; }
    }

    public class ServiceEntitys
    {
        public string Currency { get; set; }
        public string AccountName { get; set; }
        public string SWIFTCode { get; set; }
        public string BankAddress { get; set; }
        public string AccountNumber { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string Entityaddress { get; set; }
        public string MailingAddress { get; set; }
        public string RegisteredAddress { get; set; }
        public string IdentificationType { get; set; }
        public string RegistrationNo { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string GSTNumber { get; set; }
        public string EntityName { get; set; }
    }
    public class AttachmnetModel
    {
        public string AttachmentFile { get; set; }
        public string AttachmentName { get; set; }
        public string FileName { get; set; }
        public string DisplayFileName { get; set; }
        public string FilePath { get; set; }
        public string URL { get; set; }
    }
}

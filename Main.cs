using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Wox.Plugin;
using Outlook = Microsoft.Office.Interop.Outlook;
using Microsoft.VisualBasic;

namespace Wox.Plugin.OutlookContacts
{
    class Main : IPlugin
    {
        private const string KorNamePattern = "^[가-힣]{2,4}$";
        Regex reg = new Regex(KorNamePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public List<Result> Query(Query query)
        {
            if (!IsKorName(query.Search)) return new List<Result>(0);
            Outlook.ContactItem resultContact = this.FindContactEmailByName(query.Search);

            return new List<Result>
            {
                new Result {
                    Title = $"{query.Search}: {resultContact.MobileTelephoneNumber} / {resultContact.BusinessTelephoneNumber}",
                    SubTitle = $"{resultContact.FullName} 연락처 열기",
                    IcoPath = "Images\\app.png",
                    Score = 100,
                    Action = _ =>
                    {
                        resultContact.Display(true);
                        return true;
                    }
                }
            };
        }

        public void Init(PluginInitContext context)
        {
            
        }
        public bool IsKorName(string text)
        {
            if (reg.Match(text).Value == text) return true;
            return false;
        }

        private Outlook.ContactItem FindContactEmailByName(string fullName)
        {
            Outlook.Application OutlookObj = new Outlook.Application();
            Outlook.NameSpace OutlookNameSpace = OutlookObj.GetNamespace("MAPI");

            Outlook.MAPIFolder contactsFolder = OutlookNameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderContacts);

            Outlook.Items contactItems = contactsFolder.Items;

            try
            {
                Outlook.ContactItem contact =
                    (Outlook.ContactItem)contactItems.
                    Find(String.Format("[FullName]='{0}'", fullName));
                return contact;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

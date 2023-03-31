using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;



namespace Domin.Entity
{
    public class Helper
    {
        public Helper(IHttpContextAccessor httpContextAccessor,ISession session)
        {
           
            this.httpContextAccessor = httpContextAccessor;
            this.session = session;
        }
        public const string PathImageuser = "/Images/Users/";
        public const string PathSaveImageuser = "Images/Users";


        //public const string MsgType = "msgType";
        //public const string Seccess = "secess";
        //public const string Error = "error";


        public const string Success = "success";
        public const string Error = "error";

        public const string MsgType = "msgType";
        public const string Title = "title";
        public const string Msg = "msg";

        public const string Save = "Save";
        public const string Update = "Update";
        public const string Delete = "Delete";
        private readonly HttpContext httpContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ISession session;


        //Data Default SuperAdmin User

        public const string Email = "superadmin@domin.com";
        public const string UserName = "superadmin@domin.com";
        public const string Name = "superadmin";
        public const string Password = "superadmin@P@$$w0rd123456";


        public const string Permission = "Permission";

        //Data Default Basic User

        public const string EmailBasic = "basicuser@domin.com";
        public const string UserNameBasic = "basicuser@domin.com";
        public const string NameBasic = "basicuser";
        public const string PasswordBasic = "basicuser@P@$$w0rd123456";


        public enum eCurrentState
        {
            Active = 1,
            Delete =0
        }


        public enum eRoles
        {
            SuperAdmin,
            Admin,
            Basic
        }

        public enum PermissionModuleName
        {
            Home,
            Accounts,
            Roles,
            Registers,
            Categories
        }

        //private void SessionMsg(string Type, string Title, string Msg)
        //{
        //    HttpContext.Session.SetString(Helper.MsgType, Type);
        //    HttpContext.Session.SetString(Helper.Title, Title);
        //    HttpContext.Session.SetString(Helper.Msg, Msg);
        //}


    }
}

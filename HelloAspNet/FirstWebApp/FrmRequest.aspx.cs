using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FirstWebApp
{
    public partial class FrmRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strUserID = "";
            string strPassWord = string.Empty;
            string strName = "";
            string strAge = "";

            strUserID = Request.Params["TxtUserID"]; //Get 형식으로 가져올 때 (데이터를 넘길 때)
            strPassWord = Request.Params["TxtPassword"]; // 1. Get/Post 뭐든지 불러옴
            strName = Request.Form["TxtName"]; // Post형식
            strAge = Request["TxtAge"]; // 2. Get/post 뭐든지 불러옴

            var result = $"입력하신 아이디는 {strUserID}이고 <br /> 암호는 {strPassWord}입니다." +
                $"<br /> 이름은 {strName}이고, <br /> 나이는 {strAge}입니다.<br />";
            LblResult.Text = result;
            LblIpAddress.Text = Request.UserHostAddress;
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {

        }
    }
}